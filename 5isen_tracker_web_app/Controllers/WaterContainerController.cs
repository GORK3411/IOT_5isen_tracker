using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using _5isen_tracker_dll.Services;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using _5isen_tracker_dll.Repositories.Interfaces;

using _5isen_tracker_web_app.Models.Ui;

namespace _5isen_tracker_web_app.Controllers;

[Authorize]
public class WaterContainerController : Controller
{
    private readonly MyApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IWaterContainer _waterContainerRepositories;

    public WaterContainerController(
        MyApplicationDbContext db,
        UserManager<IdentityUser> userManager,
        IWaterContainer waterContainer)
    {
        _db = db;
        _userManager = userManager;
        _waterContainerRepositories = waterContainer;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var uid = user!.Id;

        // ✅ Recommended: fetch only the LAST log per container (DB does the work)
        var items = await _db.WaterContainers
            .Where(w => w.UserId == uid)
            .OrderBy(w => w.Name)
            .Select(w => new
            {
                Container = w,
                LastLog = w.Device.Logs
                    .OrderByDescending(l => l.CreatedAt)
                    .FirstOrDefault()
            })
            .ToListAsync();

        var vm = new WaterContainerIndexModel();

        foreach (var item in items)
        {
            var c = item.Container;
            var lastLog = item.LastLog;

            var distance = lastLog?.DistanceCm ?? 0m;
            var (_, percent, liters) = WaterCalcService.Compute(c, distance);

            vm.Containers.Add(new WaterContainerCardModel
            {
                Id = c.Id,
                Name = c.Name,
                Percent = percent,
                Liters = liters,
                LastDistanceCm = distance,
                LastUpdateUtc = lastLog?.CreatedAt
            });
        }

        return View(vm);
    }

    // ✅ NEW: History page (last 7 days chart)
    [HttpGet]
    public async Task<IActionResult> History(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var uid = user!.Id;

        // Verify ownership + get the container
        var container = await _db.WaterContainers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == uid);

        if (container == null)
            return NotFound();

        var fromUtc = DateTime.UtcNow.AddDays(-7);

        // Get logs for last 7 days
        var logs = await _db.Logs
            .AsNoTracking()
            .Where(l => l.DeviceId == container.DeviceId && l.CreatedAt >= fromUtc)
            .OrderBy(l => l.CreatedAt)
            .ToListAsync();

        var vm = new HistoryViewModel
        {
            ContainerId = container.Id,
            ContainerName = container.Name,
            LastUpdateUtc = logs.LastOrDefault()?.CreatedAt
        };

        foreach (var log in logs)
        {
            var (_, percent, liters) = WaterCalcService.Compute(container, log.DistanceCm);

            // label example: "Sun 21:30"
            vm.Labels.Add(log.CreatedAt.ToLocalTime().ToString("ddd HH:mm"));
            vm.Percents.Add(percent);
            vm.Liters.Add(liters);
        }

        return View(vm);
    }

    // -------- PAIR --------

    [HttpGet]
    public IActionResult Pair() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pair(string nodeId)
    {
        try
        {
            var waterContainer = new WaterContainer();
            waterContainer.Device = GetDeviceByNodeId(nodeId);
            waterContainer.User = await _userManager.GetUserAsync(User);
            waterContainer.Name = "A";

            await _waterContainerRepositories.AddAsync(waterContainer);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
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
        if (c.UserId != uid) return Forbid();

        var vm = new WaterContainerEditVm
        {
            Id = c.Id,
            Name = c.Name,
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
        if (c.UserId != uid) return Forbid();

        if (!ModelState.IsValid) return View(vm);

        c.Name = vm.Name;
        c.HeightCm = vm.HeightCm;
        c.Shape = vm.Shape;

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

    private Device GetDeviceByNodeId(string nodeId)
    {
        return _db.Devices.FirstOrDefault(x => x.NodeId == nodeId);
    }
}
