$(document).ready(function () {
    var categoryId = $("#CategoryId").val();
    var photoId = $("#PhotoId").val();
    $('.mygallery2').galleryView({
        enterIntoCategory: categoryId,
        enterIntoPhoto: photoId
    });
});