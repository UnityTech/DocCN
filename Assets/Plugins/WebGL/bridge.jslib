mergeInto(LibraryManager.library, {
    Hello: function () {
        console.log('Hello, world!');
    },
    
    ChangeCursor: function (cursor) {
        document.getElementById('#canvas').style.cursor = Pointer_stringify(cursor);
    },
});