(function () {
  const THRESHOLDS = [20, 10, 5]; // ordered high -> low doesn't matter, we’ll pick min crossed
  const POLL_MS = 30000; // 30s
    let done = false;
  function lastKey(containerId) {
    return `wc:${containerId}:lastPercent`;
  }

  function getLastPercent(containerId) {
    const v = localStorage.getItem(lastKey(containerId));
    if (v === null) return null;
    const n = Number(v);
    return Number.isFinite(n) ? n : null;
  }

  function setLastPercent(containerId, percent) {
    localStorage.setItem(lastKey(containerId), String(percent));
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
        if (done)
            return;
    const id = container.id;
    const name = container.name;
    const curr = Number(container.percent);

    if (!Number.isFinite(curr)) return;

    const prev = getLastPercent(id);

    // ✅ First time seeing this container in this browser: DO NOT notify
    // just store baseline to avoid refresh spam.
    if (prev === null) {
      setLastPercent(id, curr);
      return;
    }

    // Find all crossed thresholds: prev > T && curr <= T
    const crossed = THRESHOLDS.filter((t) => prev > t && curr <= t);

    if (crossed.length > 0) {
      // ✅ Only notify once: the MOST critical threshold (smallest T)
      const t = Math.min(...crossed);

      notify(
        `⚠️ ${name}: Low water`,
        `Water level is ${curr.toFixed(1)}% (≤ ${t}%).`
        );
        done = true;
    }

    // Update last seen percent
    setLastPercent(id, curr);
  }

    async function tick() {
        done = false
    try {
      const containers = await fetchStatus();
      containers.forEach(checkThresholds);
    } catch {
      // silent
    }
  }

  async function init() {
    if (!("Notification" in window)) return;

    if (Notification.permission === "default") {
      await Notification.requestPermission();
    }
    if (Notification.permission !== "granted") return;

    await tick();
    setInterval(tick, POLL_MS);
  }

  document.addEventListener("DOMContentLoaded", init);
})();
