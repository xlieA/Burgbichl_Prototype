// https://stackoverflow.com/questions/77123254/how-to-make-vibration-in-webgl

mergeInto(LibraryManager.library, {
    Vibrate: function(duration) {
        if (typeof navigator.vibrate === "function") {
            navigator.vibrate(duration);
        }
    }
});
