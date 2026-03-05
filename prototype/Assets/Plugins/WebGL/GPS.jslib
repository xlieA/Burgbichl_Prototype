// https://www.w3schools.com/js/js_api_geolocation.asp
// https://developer.mozilla.org/de/docs/Web/API/Geolocation_API/Using_the_Geolocation_API
// https://hellocoding.de/blog/coding-language/javascript/geolocation-api

mergeInto(LibraryManager.library, {
    GetLocation: function () {
        if (navigator.geolocation) {
            navigator.geolocation.watchPosition(position => {
                let latitude = position.coords.latitude;
                let longitude = position.coords.longitude;

                // Format the coordinates as a string
                let locationString = latitude + "," + longitude;

                // Send the location data to Unity's C# script
                SendMessage('PlayerLocation', 'ReceiveLocation', locationString);
            }, error => {
                console.warn("Geolocation error: ", error.message);
            }, {
                enableHighAccuracy: true,
                maximumAge: 1000,
                timeout: 5000
            });
        } else {
            console.warn("Geolocation is not supported by this browser.");
        }
    }
});

