function resize() {
    var canvas = document.getElementById('#canvas');
    canvas.setAttribute('width', (window.innerWidth * window.devicePixelRatio).toString());
    canvas.setAttribute('height', (window.innerHeight * window.devicePixelRatio).toString());
}

document.addEventListener('DOMContentLoaded', function() {
    var originOnResize = window.onresize;
    if (originOnResize) {
        window.onresize = function (ev) {
            originOnResize(ev);
            resize();
        };
    } else {
        window.onresize = resize;
    }
    
    document.getElementById('gameContainer').addEventListener('DOMSubtreeModified', resize);
});