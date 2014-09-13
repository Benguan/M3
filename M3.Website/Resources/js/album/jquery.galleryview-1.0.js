
(function ($) {
    $.fn.galleryView = function (options) {
        var opts = $.extend($.fn.galleryView.defaults, options);
        var j_gallery;
        var j_thumb;
        var j_thumb_warp;
        var j_thumb_ul;
        var j_navNext;
        var j_navPrev;
        var j_navPage;
        var j_ins;
        var j_insFullImage;
        var j_controlbar;
        var j_fullscreen;
        var j_play;
        var j_rotate;
        var angle = 90;

        var d_albums = {};
        var currentPage = 1;
        var currentAlbumId = 1;
        var iterator = 0;
        var d_thumb = [];
        var d_thumbIndex = {};

        //d_albums["1"] = {
        //    id: 1, name: "2014春游", page: 1, year: 2014,
        //    cover: { id: 1, title: "第1页第1张", description: "第1张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/1.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/1.jpg" },
        //    photos: [
        //         [{ id: 1, title: "第1页第1张", description: "第1张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/1.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/1.jpg" },
        //         { id: 2, title: "第1页第2张", description: "第2张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/2.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/2.jpg" },
        //         { id: 3, title: "第1页第2张", description: "第3张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/3.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/3.jpg" },
        //         { id: 4, title: "第1页第2张", description: "第4张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/4.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/4.jpg" },
        //         { id: 5, title: "第1页第2张", description: "第5张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/5.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/5.jpg" },
        //         { id: 6, title: "第1页第2张", description: "第6张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/6.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/6.jpg" },
        //         { id: 7, title: "第1页第2张", description: "第7张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/7.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/7.jpg" },
        //         { id: 8, title: "第1页第2张", description: "第8张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/8.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/8.jpg" },
        //         { id: 9, title: "第1页第2张", description: "第9张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/9.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/9.jpg" },
        //         { id: 10, title: "第1页第2张", description: "第10张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/10.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/10.jpg" },
        //         { id: 11, title: "第1页第2张", description: "第11张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/11.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/11.jpg" },
        //         { id: 12, title: "第1页第2张", description: "第12张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/12.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/12.jpg" },
        //         { id: 13, title: "第1页第2张", description: "第13张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/13.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/13.jpg" },
        //         { id: 14, title: "第1页第2张", description: "第14张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/14.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/14.jpg" },
        //         { id: 15, title: "第1页第2张", description: "第15张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/15.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/15.jpg" },
        //         { id: 16, title: "第1页第2张", description: "第16张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/16.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/16.jpg" },
        //         { id: 17, title: "第1页第2张", description: "第17张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/17.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/17.jpg" },
        //         { id: 18, title: "第1页第2张", description: "第18张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/18.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/18.jpg" },
        //         { id: 19, title: "第1页第2张", description: "第19张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/19.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/19.jpg" },
        //         { id: 20, title: "第1页第2张", description: "第20张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/20.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/20.jpg" }],
        //    ]
        //};

        function loadImages(handler) {

            currentPage = currentPage ? currentPage : 1;

            if (d_albums[currentAlbumId] && d_albums[currentAlbumId].photos[currentPage - 1]) {
                var albums = d_albums[currentAlbumId];
                callback({
                    id: albums.id,
                    name: albums.name,
                    cover: albums.cover,
                    photos: albums.photos[currentPage - 1]
                });
            } else {
                var param = currentAlbumId + '/' + currentPage;
                function proxy(d) {
                    return d;
                };

                $.ajax({
                    url: opts.baseUrl + "api/gallery/detail/" + param,
                    processData: false,
                    dataType: "jsonp",
                    jsonpCallback: "proxy",
                    success: function (response) {
                        if (response) {
                            callback(response);
                            handler && handler(response);
                            console.log("ajax success!");
                        } else {
                            console.log("ajax data error! pls checked!");
                        }
                    }
                });
            }
        }

        function callback(albums) {
            if (d_albums[albums.id] != null) {
                d_albums[albums.id] = {
                    id: albums.id, name: albums.name, cover: albums.cover,
                    photos: albums.photos
                };
            } else {
                if (d_albums.photos == null) {
                    d_albums.photos = {};
                }
                if (d_albums.photos[albums.page] == null) {
                    d_albums.photos[albums.page] = [];
                }
                d_albums.photos[albums.page].push(albums.photos);
            }
            pushThumbs(albums.photos);
        }

        function showItem(id, param) {
            angle = 90;
            //Disable next/prev buttons until transition is complete
            j_navNext.off('click');
            j_navPrev.off('click');
            //j_frames.off('click');
            //Fade out all panels and fade in target panel
            var tempThumbIndex = d_thumbIndex[id];
            var thumb = tempThumbIndex.data;
            var i = tempThumbIndex.index;
            var img = j_ins.find(".tn3e-full-image img");

            var showImmediately = param && param.showImmediately;
            if (!showImmediately) {
                j_ins.animate({ opacity: "-=0.8" }, 200, function () {
                    img.attr("src", thumb.normalUrl);
                    j_ins.animate({ opacity: "+=0.8" }, 600, function () {
                    });
                });
            } else {
                img.attr("src", thumb.normalUrl);
            }

            if (d_thumb.length > 0) {
                j_navPage.html(i + 1 + " / " + (d_thumb.length));
            }

            iterator = i;

            j_thumb_ul.find("li").removeClass("tn3e-thumb-over");
            j_thumb_ul.find("li").find("div").css("opacity", "0.5");
            var thumb_li = j_thumb_ul.find("li[data-photo-id=" + id + "]");
            thumb_li.addClass("tn3e-thumb-over");
            thumb_li.find("div").css("opacity", "0");

            var warpOffset = j_thumb_warp.offset();
            var liOffset = thumb_li.offset();
            var theThumbOffset = j_thumb.offset();
            var theThumbLiOffset = thumb_li.offset();

            j_thumb_ul.animate({
                top: "-" + i * 75 + "px"
            }, 200);

            j_navPrev.click(showPrevItem);
            j_navNext.click(showNextItem);
        };
        function showNextItem() {
            $(document).stopTime("transition");
            if (++iterator >= d_thumb.length) { iterator = 0; }
            showItem(d_thumb[iterator].id);
            if (opts.autoPlay) {
                $(document).everyTime(opts.transition_interval, "transition", function () {
                    showNextItem();
                });
            }
        };
        function showPrevItem() {
            $(document).stopTime("transition");
            if (--iterator < 0) { iterator = d_thumb.length - 1; }
            showItem(d_thumb[iterator].id);
            if (opts.autoPlay) {
                $(document).everyTime(opts.transition_interval, "transition", function () {
                    showNextItem();
                });
            }
        };
        function getPos(el) {
            var left = 0, top = 0;
            var el_id = el.id;
            if (el.offsetParent) {
                do {
                    left += el.offsetLeft;
                    top += el.offsetTop;
                } while (el = el.offsetParent);
            }
            //If we want the position of the gallery itself, return it
            if (el_id == id) { return { 'left': left, 'top': top }; }
                //Otherwise, get position of element relative to gallery
            else {
                var gPos = getPos(j_gallery[0]);
                var gLeft = gPos.left;
                var gTop = gPos.top;

                return { 'left': left - gLeft, 'top': top - gTop };
            }
        };

        function buildThumbs() {
            var thum = '<div class="tn3e-thumbs">\
                            <div style="position: absolute; overflow: hidden; width: 123px; height: 297px;">\
                                <ul style="position: relative; margin: 0px; padding: 0px; border-width: 0px; width: 1520px; list-style: none;">\
                                </ul>\
                            </div>\
                        </div>';
            j_thumb = $(thum);
            j_thumb.css({
                "position": "absolute",
                "cursor": "auto",
                "height": "300px"
            });
            j_thumb_warp = j_thumb.find("div");
            j_thumb_ul = j_thumb.find("ul");
            j_gallery.append(j_thumb);
        };

        function pushThumbs(photos) {
            for (var i = 0; i < photos.length; i++) {
                var thumb = $('<li class="tn3e-thumb" style="float: none; opacity: 1;" data-photo-id="' + photos[i].id + '">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="' + photos[i].thumbnailUrl + '"></li>');
                thumb.click(function () {
                    var pid = $(this).data("photo-id");
                    showItem(pid);
                });
                j_thumb_ul.append(thumb);

                d_thumb.push(photos[i]);
                if (d_thumbIndex[photos[i].id] == null) {
                    d_thumbIndex[photos[i].id] = { index: d_thumb.length - 1, data: d_thumb[d_thumb.length - 1] };
                }
            }
        };

        function buildFilmstrip() {
            var strip = $('<div class="tn3e-image" style="width: 770px; height: 300.885416666667px; overflow: hidden; position: relative;">\
                            <div class="tn3e-image-ins" style="position: absolute; width: 100%; height: 100%;">\
                                <div class="tn3e-image-in" style="position: absolute; overflow: hidden; visibility: visible; width: 770px; height: 300.885416666667px; left: 0px;">\
                                    <div class="tn3e-full-image" style="position: absolute; width: 770px; height: 301px; left: 0px; top: 0px;text-align:center;marin:0 auto">\
                                        <img src="/Resources/images/album/grad.jpg" alt="Abstract lights" style="max-width: 770px;max-height:300px">\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="tn3-in-image" style="position: absolute; width: 770px; height: 301px; left: 0px; top: 0px; display: block;">\
                                <div class="tn3e-control-bar tn3_v tn3_h tn3_o" style="top: 110.5px; left: 263.5px; display: block; opacity: 0;">\
                                    <div class="tn3e-fullscreen" title="Maximize"></div>\
                                    <div class="tn3e-play" title="Start Slideshow"></div>\
                                    <div class="tn3e-show-albums" title="Album List"><i class="fa fa-rotate-right"></i></div>\
                                </div>\
                                <div class="tn3e-preloader" style="display: none;">\
                                    <img src="/Resources/images/album/preload.gif">\
                                </div>\
                                <div class="tn3e-timer" style="display: none;"></div>\
                            </div>\
                        </div>');
            j_ins = strip.find(".tn3e-image-in");
            j_controlbar = strip.find(".tn3e-control-bar");
            j_fullscreen = strip.find(".tn3e-fullscreen");
            j_insFullImage = strip.find(".tn3e-full-image");
            j_rotate = strip.find(".tn3e-show-albums");
            
            j_rotate.click(function () {
                var transformAngle = "rotate(" + angle + "deg)";
                j_ins.find("img").css("-webkit-transform", transformAngle);
                angle = angle + 90;

            });
            j_play = strip.find(".tn3e-play");
            if (opts.autoPlay) {
                j_play.addClass("tn3e-play-active");
            } else {
                j_play.removeClass("tn3e-play-active");
            }
            j_play.click(function () {
                opts.autoPlay = !opts.autoPlay;
                if (opts.autoPlay) {
                    $(this).addClass("tn3e-play-active");
                    $(document).everyTime(opts.transition_interval, "transition", function () {
                        showNextItem();
                    });
                } else {
                    $(this).removeClass("tn3e-play-active");
                    $(document).stopTime("transition");
                }
            });

            var showControllBar = function(control) {
                controllBarStatus = true;
                console.log("over:" + controllBarStatus);
                control.animate({ opacity: "+=1" }, 100);
            };
            var hideControllBar = function (control) {
                controllBarStatus = false;
                setTimeout(function () {
                    console.log("out:" + controllBarStatus);
                    if (!controllBarStatus) {
                        control.animate({ opacity: "-=1" }, 100);
                    }
                }, 400);
            };
            j_controlbar.mouseover(function () {
                showControllBar($(this));
            });
            j_controlbar.mouseout(function () {
                hideControllBar($(this));
            });
            j_fullscreen.click(function () {
                opts.fullscreen = !opts.fullscreen;
                var escHandle = function (e) {
                    if (e.keyCode == 27 || e.which == 27) {
                        exitFullscreen(j_gallery.get(0));
                        exitToFullscreen();
                    }
                };

                var toFullscreen = function () {
                    $(this).addClass("tn3e-fullscreen-active");
                    var docWidth = $(document).width();
                    var docHeight = $(document).height();
                    j_gallery.css({
                        width: docWidth,
                        height: docHeight
                    });
                    strip.css({
                        width: docWidth - 180,
                        height: docHeight - 95,
                    });

                    j_ins.css({
                        width: docWidth - 180,
                        height: docHeight - 95,
                    });

                    var fullImg = j_insFullImage.find("img");
                    var thumb = d_thumb[iterator];

                    var lwidth = docWidth - 95;
                    var lheight = docHeight - 180;

                    fullImg.css({ "max-width": lwidth });
                    fullImg.css({ "max-height": lheight });


                    j_insFullImage.css({
                        width: docWidth - 180,
                        height: docHeight - 95,
                        top: (docHeight - 95 - fullImg.height()) / 2
                    });
                    j_thumb_warp.css({
                        height: docHeight - 95,
                    });
                    j_thumb.css({
                        height: docHeight - 95,
                    });
                  
                    j_controlbar.css({
                        top: (j_gallery.height() / 2) - j_controlbar.height() + "px",
                        left: (j_gallery.width() / 2) - j_controlbar.width() + "px",
                    });

                  
                    j_controlbar.css({
                        top: (j_gallery.height() / 2) - j_controlbar.height() + "px",
                        left: (j_gallery.width() / 2) - j_controlbar.width() + "px",
                    });

                    launchFullscreen(j_gallery.get(0));

                    $(document).on('keyup', escHandle);
                };
                var exitToFullscreen = function () {
                    $(this).removeClass("tn3e-fullscreen-active");
                    j_gallery.css({
                        width: 950,
                        height: 395,
                    });
                    strip.css({
                        width: 770,
                        height: 300,
                    });
                    j_ins.css({
                        width: 770,
                        height: 300,
                    });

                    var fullImg = j_insFullImage.find("img");
                    var lheight = 300;
                    var lwidth = 770;

                    var thumb = d_thumb[iterator];
                    fullImg.css({ "max-width": lwidth });
                    fullImg.css({ "max-height": lheight });

                    j_insFullImage.css({
                        width: 770,
                        height: 300,
                        top:0
                    });
                    j_thumb_warp.css({
                        height: 297,
                    });
                    j_thumb.css({
                        height: 297,
                    });
                    j_controlbar.css({
                        top : "110.5px",
                        left: "263.5px"
                    });
                    exitFullscreen(j_gallery.get(0));
                    $(document).off('keyup', escHandle);
                };
                if (opts.fullscreen) {
                    toFullscreen();
                } else {
                    exitToFullscreen();
                }
            });
            j_gallery.append(strip);
        };

        var controllBarStatus;

        function launchFullscreen(element) {
            if (element.requestFullscreen) {
                element.requestFullscreen();
            } else if (element.mozRequestFullScreen) {
                element.mozRequestFullScreen();
            } else if (element.webkitRequestFullscreen) {
                element.webkitRequestFullscreen();
            } else if (element.msRequestFullscreen) {
                element.msRequestFullscreen();
            }
        }

        function exitFullscreen() {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            }
        }

        function buildBar() {
            var next = '<div class="tn3e-next" title="Next Image"></div>';
            var prev = '<div class="tn3e-prev" title="Previous Image"></div>';
            var page = '<div class="tn3e-page" title="Page"></div>';
            j_navNext = $(next);
            j_navPrev = $(prev);
            j_navPage = $(page);
            j_gallery.append(j_navNext);
            j_gallery.append(j_navPrev);
            j_gallery.append(j_navPage);
        };

        function mouseIsOverPanels(x, y) {
            var pos = getPos(j_gallery[0]);
            var top = pos.top;
            var left = pos.left;
            return x > left && x < left + opts.panel_width && y > top && y < top + opts.panel_height;
        };

        return this.each(function () {
            var containter = $(this);
            j_gallery = $('<div class="tn3e-gallery"></div>');
            j_gallery.css('visibility', 'hidden');
            j_gallery.css({
                'float': 'none',
                'width': '950px',
                'height': opts.height + 'px'
            });

            containter.empty();
            containter.append(j_gallery);

            currentAlbumId = opts.enterIntoCategory;

            buildFilmstrip();
            buildBar();
            buildThumbs();

            var firstShowItem = function () {
                if (d_thumb.length > 0) {
                    if (opts.enterIntoPhoto && d_thumbIndex[opts.enterIntoPhoto]) {
                        showItem(opts.enterIntoPhoto, { showImmediately: true });
                    } else {
                        showItem(d_thumb[0].id, { showImmediately: true });
                    }
                }
            };

            loadImages(firstShowItem);

            // show the enter into photo.
            if (d_thumb.length > 0) {
                if (opts.enterIntoPhoto && d_thumbIndex[opts.enterIntoPhoto]) {
                    showItem(opts.enterIntoPhoto, { showImmediately: true });
                } else {
                    showItem(d_thumb[0].id, { showImmediately: true });
                }
            }

            if (opts.autoPlay) {
                $(document).everyTime(opts.transition_interval, "transition", function () {
                    showNextItem();
                });
            }

            j_gallery.css('visibility', 'visible');
        });
    };

    $.fn.galleryView.defaults = {
        baseUrl: "/",
        transition_speed: 500,
        transition_interval: 5000,
        overlay_opacity: 0.6,
        show_captions: false,
        pause: false,
        autoPlay: true,
        fullscreen: false,
        height: 395,
        enterIntoCategory: 1,
        enterIntoPhoto: 1
    };
})(jQuery);