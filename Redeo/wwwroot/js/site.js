const toggleMenu = () => document.body.classList.toggle("open");

$('a.jt').cluetip({
    showTitle: false,
    dropShadow: false,
    cluetipClass: 'custom',
    positionBy: 'fixed',
    leftOffset: -13,
    hoverIntent: false,
    sticky: true,
    mouseOutClose: true,
    closePosition: '',
});