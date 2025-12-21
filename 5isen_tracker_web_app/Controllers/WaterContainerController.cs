using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using _5isen_tracker_dll.Services;
using _5isen_tracker_web_app.Models.Ui;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;

namespace _5isen_tracker_web_app.Controllers;

[Authorize]
public class WaterContainerController : Controller
{
    private readonly MyApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public WaterContainerController(MyApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var uid = user!.Id;

        var containers = await _db.WaterContainers
            .Include(w => w.Device)
                .ThenInclude(d => d.Logs)
            .Where(w => w.Device.UserId == uid)
            .OrderBy(w => w.Name)
            .ToListAsync();

        var vm = new WaterContainerIndexModel();

        foreach (var c in containers)
        {
            var logs = c.Device?.Logs ?? new List<Log>();
            var lastLog = logs.OrderByDescending(l => l.CreatedAt).FirstOrDefault();

            var distance = lastLog?.DistanceCm ?? 0m;
            var (_, percent, liters) = WaterCalcService.Compute(c, distance);

            vm.Containers.Add(new WaterContainerCardModel
            {
                Id = c.Id,
                Name = c.Name,
                QrCode = c.QrCode,
                Percent = percent,
                Liters = liters,
                LastDistanceCm = distance,
                LastUpdateUtc = lastLog?.CreatedAt
            });
        }

        return View(vm);
    }

    [HttpGet]
    public IActionResult Pair() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pair(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            TempData["Err"] = "Invalid QR code.";
            return RedirectToAction(nameof(Pair));
        }

        var user = await _userManager.GetUserAsync(User);
        var uid = user!.Id;

        var container = await _db.WaterContainers
            .Include(w => w.Device)
            .FirstOrDefaultAsync(w => w.QrCode == code);

        if (container == null)
        {
            TempData["Err"] = "No container found for this QR code.";
            return RedirectToAction(nameof(Pair));
        }

        if (!string.IsNullOrEmpty(container.Device.UserId) && container.Device.UserId != uid)
        {
            TempData["Err"] = "This container is already paired to another user.";
            return RedirectToAction(nameof(Pair));
        }

        container.Device.UserId = uid;
        await _db.SaveChangesAsync();

        // After pairing, go configure it (professional flow)
        return RedirectToAction(nameof(Edit), new { id = container.Id });
    }

    // -------- EDIT --------

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var uid = user!.Id;

        var c = await _db.WaterContainers
            .Include(x => x.Device)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (c == null) return NotFound();
        if (c.Device.UserId != uid) return Forbid();

        var vm = new WaterContainerEditVm
        {
            Id = c.Id,
            Name = c.Name,
            QrCode = c.QrCode,
            HeightCm = c.HeightCm,
            Shape = c.Shape,
            RadiusCm = c.RadiusCm,
            LengthCm = c.LengthCm,
            WidthCm = c.WidthCm,
            MaxLiters = c.MaxLiters
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(WaterContainerEditVm vm)
    {
        var user = await _userManager.GetUserAsync(User);
        var uid = user!.Id;

        var c = await _db.WaterContainers
            .Include(x => x.Device)
            .FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (c == null) return NotFound();
        if (c.Device.UserId != uid) return Forbid();

        if (!ModelState.IsValid) return View(vm);

        c.Name = vm.Name;
        c.HeightCm = vm.HeightCm;
        c.Shape = vm.Shape;

        // Clear shape fields then re-apply
        c.RadiusCm = null;
        c.LengthCm = null;
        c.WidthCm = null;
        c.MaxLiters = null;

        if (vm.Shape == ContainerShape.Cylinder)
            c.RadiusCm = vm.RadiusCm;

        if (vm.Shape == ContainerShape.Rectangular)
        {
            c.LengthCm = vm.LengthCm;
            c.WidthCm = vm.WidthCm;
        }

        if (vm.Shape == ContainerShape.CapacityOnly)
            c.MaxLiters = vm.MaxLiters;

        await _db.SaveChangesAsync();

        TempData["Ok"] = "Container updated.";
        return RedirectToAction(nameof(Index));
    }
}
