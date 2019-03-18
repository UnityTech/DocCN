mergeInto(LibraryManager.library, {
    Initialize: function() {
        window.addEventListener('popstate', function(e) {
            
        });
        SendMessage('Panel', 'LocationChange', window.location.pathname);
    },
    
    LocationPush: function(title, url) {
        window.history.pushState(null, Pointer_stringify(title), Pointer_stringify(url));
    },
    
    LocationBack: function() {
        
    },
    
    LocationReplace: function(title, url) {
        window.history.replaceState(null, Pointer_stringify(title), Pointer_stringify(url));
    },
    
    ChangeCursor: function (cursor) {
        document.getElementById('#canvas').style.cursor = Pointer_stringify(cursor);
    },
});