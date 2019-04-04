mergeInto(LibraryManager.library, {
    Initialize: function () {
        window.addEventListener('popstate', function (e) {

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
});