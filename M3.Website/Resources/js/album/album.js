$(document).ready(function () {
    $('.mygallery2').galleryView({
        skinDir: "/skins",
        responsive: "width",
        mouseWheel: false,
        image: { crop: true },
        skin: ["tn3e", "vertical"],
        height: 395,
        touch: {
            fsMode: "/tn3_touch_fullscreen.html"
        }
    });
});