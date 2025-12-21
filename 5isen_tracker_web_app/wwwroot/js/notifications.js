(function () {
  const THRESHOLDS = [20, 10, 5];
  const POLL_MS = 30000; // 30s (change to 60000 for 1 min)

  function key(containerId, threshold) {
    return `wc:${containerId}:armed:${threshold}`;
  }

  function isArmed(containerId, threshold) {
    const v = localStorage.getItem(key(containerId, threshold));
    // default = armed (true) so first time it can notify when it drops
    return v === null ? true : v === "true";
  }

  function setArmed(containerId, threshold, val) {
    localStorage.setItem(key(containerId, threshold), val ? "true" : "false");
  }

  function notify(title, body) {
    new Notification(title, { body });
  }

  async function fetchStatus() {
    const res = await fetch("/WaterContainer/StatusJson", { cache: "no-store" });
    if (!res.ok) return [];
    return await res.json();
  }

  function checkThresholds(container) {
    const id = container.id;
    const name = container.name;
    const percent = Number(container.percent);

    THRESHOLDS.forEach((t) => {
      // re-arm when back above threshold
      if (percent > t) {
        setArmed(id, t, true);
        return;
      }

      // crossing detected (armed and now <= threshold)
      if (isArmed(id, t) && percent <= t) {
        notify(
          `⚠️ ${name}: Low water`,
          `Water level is ${percent.toFixed(1)}% (≤ ${t}%).`
        );
        setArmed(id, t, false);
      }
    });
  }

  async function tick() {
    try {
      const containers = await fetchStatus();
      containers.forEach(checkThresholds);
    } catch (e) {
      // silent fail (no console spam)
    }
  }

  async function init() {
    if (!("Notification" in window)) return;

    // Ask permission once
    if (Notification.permission === "default") {
      await Notification.requestPermission();
    }

    // Only run if allowed
    if (Notification.permission !== "granted") return;

    // Run immediately + then every POLL_MS
    await tick();
    setInterval(tick, POLL_MS);
  }

  document.addEventListener("DOMContentLoaded", init);
})();
