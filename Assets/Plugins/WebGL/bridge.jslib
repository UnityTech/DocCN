mergeInto(LibraryManager.library, {
    Initialize: function () {
        window.addEventListener('popstate', function () {
            setTimeout(function () {
                SendMessage('Panel', 'LocationChange', window.location.pathname);
            }, 0);
        });
        SendMessage('Panel', 'LocationChange', window.location.pathname);
    },

    LocationPush: function (url) {
        window.history.pushState(null, null, Pointer_stringify(url));
    },

    LocationBack: function () {

    },

    LocationReplace: function (url) {
        window.history.replaceState(null, null, Pointer_stringify(url));
    },

    ChangeCursor: function (cursor) {
        document.getElementById('#canvas').style.cursor = Pointer_stringify(cursor);
    },

    HrefTo: function (url) {
        var a = document.createElement('a');
        a.href = Pointer_stringify(url);
        a.target = '_blank';
        a.click();
    },

    Download: function (url, filename) {
        var a = document.createElement('a');
        a.href = Pointer_stringify(url);
        a.download = Pointer_stringify(filename);
        a.target = '_blank';
        a.click();
    },

    CopyText: function (text) {
        var el = document.createElement('textarea');
        el.value = Pointer_stringify(text);
        el.setAttribute('readonly', '');
        el.style = {
            position: 'fixed', 
            left: '-9999px',
        };
        document.body.appendChild(el);
        el.select();
        document.execCommand('copy');
        document.body.removeChild(el);
    },
});