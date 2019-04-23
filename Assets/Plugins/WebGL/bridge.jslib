mergeInto(LibraryManager.library, {
    Initialize: function () {
        window.addEventListener('popstate', function () {
            setTimeout(function() {
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
        window.location.href = Pointer_stringify(url);
    },
    
    Download: function (url, filename) {
        var a = document.createElement('a');
        a.href = Pointer_stringify(url);
        a.download = Pointer_stringify(filename);
        a.click();
    },
});