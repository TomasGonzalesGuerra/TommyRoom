window.scrollToElement = function (elementId) {
    const el = document.getElementById(elementId);
    if (el) {
        const offset = 80; // altura del nav sticky
        const top = el.getBoundingClientRect().top + window.scrollY - offset;
        window.scrollTo({ top, behavior: 'smooth' });
    }
};
