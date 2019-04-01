function resize() {
    var canvas = document.getElementById('#canvas');
    canvas.setAttribute('width', window.innerWidth.toString());
    canvas.setAttribute('height', window.innerHeight.toString());
}

document.addEventListener('DOMContentLoaded', function() {
    window.onresize = resize;
    document.getElementById('gameContainer').addEventListener('DOMSubtreeModified', resize);
});