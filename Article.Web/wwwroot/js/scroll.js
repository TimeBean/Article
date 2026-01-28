window.addEventListener("scroll", () => {
    const el = document.getElementById("progress");
    const h = document.documentElement;
    el.value = (h.scrollTop / (h.scrollHeight - h.clientHeight)) * 100;
});