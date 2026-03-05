// https://www.digitalocean.com/community/tutorials/front-and-rear-camera-access-with-javascripts-getusermedia
// https://stackoverflow.com/questions/52812091/getusermedia-selecting-rear-camera-on-mobile
// https://www.webdevdrops.com/en/how-to-access-device-cameras-with-javascript
// https://bobbyhadz.com/blog/javascript-create-video-element

mergeInto(LibraryManager.library, {
    RequestCamera: function () {
        if ('mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices) {
            // Create the video element dynamically
            let videoElement = document.createElement("video");
            videoElement.id = "cameraFeed";

            videoElement.autoplay = true;
            videoElement.playsInline = true; // Prevents fullscreen mode on iOS
            videoElement.muted = true; // Prevents issues with autoplay

            // Make the video fullscreen
            videoElement.style.position = "absolute";
            videoElement.style.top = "0";
            videoElement.style.left = "0";
            videoElement.style.width = "100vw";  // Viewport width
            videoElement.style.height = "100vh"; // Viewport height
            videoElement.style.objectFit = "cover"; // Cover entire screen
            videoElement.style.zIndex = "-1"; // Behind Unity canvas

            // Append the video element to the body
            document.body.appendChild(videoElement);

            let unityCanvas = document.getElementById("unity-canvas");
            if (unityCanvas) {
                unityCanvas.style.zIndex = "1";  // Ensure it's above the video
                unityCanvas.style.backgroundColor = "transparent";  // Transparent canvas
            }

            // Request camera access
            navigator.mediaDevices.getUserMedia({
                audio: false,
                video: {
                    facingMode: { exact: "environment" }, // Force back camera
                    width: { ideal: window.innerWidth },  // Set video width to screen width
                    height: { ideal: window.innerHeight } // Set video height to screen height
                }
            })
            .then(stream => {
                videoElement.srcObject = stream;
            })
            .catch(error => console.error("Camera access error:", error));
        } else {
            console.warn("Camera access not supported.");
        }
    }
});
