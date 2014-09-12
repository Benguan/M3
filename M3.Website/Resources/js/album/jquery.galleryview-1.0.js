
(function ($) {
    $.fn.galleryView = function (options) {
        var opts = $.extend($.fn.galleryView.defaults, options);
        var paused = false;
        var j_gallery;
        var j_filmstrip;
        var j_frames;
        var j_panels;
        var j_pointer;
        var j_thumb;
        var j_thumb_ul;
        var j_navNext;
        var j_navPrev;
        var j_ins;
        var j_controlbar;
        var j_fullscreen;
        var j_play;

        // { album: "2014春游", id : 1, title: "春游", description : "2014春游",  cover: "1.gif", page: 1,
        //   photos : [   [{id: 1, title: "第1页第1张", description : "第1张", thumbnailUrl: "1.thumb.gif", normalUrl : "1.gif"},{id: 2, title: "第1页第2张", description : "第2张", thumbnailUrl: "2.thumb.gif", normalUrl : "2.gif"}],
        //                [{id: 3,title: "第2页第1张", description : "第1张", thumbnailUrl: "1.thumb.gif", normalUrl : "1.gif"}, {id: 4, title: "第2页第2张", description : "第2张", thumbnailUrl: "2.thumb.gif", normalUrl : "2.gif"}]
        //            ]
        // }
        var d_albums = {};
        var currentPage = 1;
        var currentAlbumId = 1;
        var iterator = 0;
        var d_prev_album;
        var d_thumb = [];
        var d_thumbIndex = {};

        d_albums["1"] = {
            id: 1, name: "2014春游", page: 1, year: 2014,
            cover: { id: 1, title: "第1页第1张", description: "第1张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/1.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/1.jpg" },
            photos: [
                 [{ id: 1, title: "第1页第1张", description: "第1张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/1.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/1.jpg" },
                 { id: 2, title: "第1页第2张", description: "第2张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/2.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/2.jpg" },
                 { id: 3, title: "第1页第2张", description: "第3张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/3.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/3.jpg" },
                 { id: 4, title: "第1页第2张", description: "第4张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/4.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/4.jpg" },
                 { id: 5, title: "第1页第2张", description: "第5张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/5.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/5.jpg" },
                 { id: 6, title: "第1页第2张", description: "第6张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/6.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/6.jpg" },
                 { id: 7, title: "第1页第2张", description: "第7张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/7.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/7.jpg" },
                 { id: 8, title: "第1页第2张", description: "第8张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/8.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/8.jpg" },
                 { id: 9, title: "第1页第2张", description: "第9张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/9.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/9.jpg" },
                 { id: 10, title: "第1页第2张", description: "第10张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/10.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/10.jpg" },
                 { id: 11, title: "第1页第2张", description: "第11张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/11.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/11.jpg" },
                 { id: 12, title: "第1页第2张", description: "第12张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/12.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/12.jpg" },
                 { id: 13, title: "第1页第2张", description: "第13张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/13.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/13.jpg" },
                 { id: 14, title: "第1页第2张", description: "第14张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/14.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/14.jpg" },
                 { id: 15, title: "第1页第2张", description: "第15张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/15.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/15.jpg" },
                 { id: 16, title: "第1页第2张", description: "第16张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/16.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/16.jpg" },
                 { id: 17, title: "第1页第2张", description: "第17张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/17.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/17.jpg" },
                 { id: 18, title: "第1页第2张", description: "第18张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/18.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/18.jpg" },
                 { id: 19, title: "第1页第2张", description: "第19张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/19.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/19.jpg" },
                 { id: 20, title: "第1页第2张", description: "第20张", thumbnailUrl: "http://www.tn3gallery.com/images/114x72/20.jpg", normalUrl: "http://www.tn3gallery.com/images/920x360/20.jpg" }],
            ]
        };

        function loadImages() {

            currentPage = currentPage ? currentPage : 1;

            if (d_albums[currentAlbumId] && d_albums[currentAlbumId].photos[currentPage - 1]) {
                var albums = d_albums[currentAlbumId];
                callback({
                    id: albums.id, name: albums.name, cover: albums.cover,
                    photos: albums.photos[currentPage - 1]
                })
            } else {
                var param = currentpage + "/" + currentAlbumId;
                $.ajax({ url: opts.baseUrl + "api/gallery/detail/" + param, processData: false })
                    .done(function (response) {
                        if (response) {
                            callback(response);
                            console.log("ajax success!");
                        }
                        else {
                            console.log("ajax data error! pls checked!");
                        }
                    }.fail(function () {
                        console.log("ajax error! pls checked!");
                    }));
            }
        }

        function callback(albums) {
            if (d_albums[albums.id] != null) {
                d_albums[albums.id] = {
                    id: albums.id, name: albums.name, cover: albums.cover,
                    photos: albums.photos
                };
            } else {
                d_albums.photos[albums.page].push(albums.photos);
            }
            pushThumbs(albums.photos);
        }

        function showAlbum(album) {
        }

        function showItem(id, param) {
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

            iterator = i;

            j_thumb_ul.animate({
                top: "-" + i * 72 + "px"
            }, 200)

            j_navPrev.click(showPrevItem);
            j_navNext.click(showNextItem);

            /*
            //Slide either pointer or filmstrip, depending on transition method
            if (slide_method == 'strip') {
                //Stop filmstrip if it's currently in motion
                j_filmstrip.stop();

                //Determine distance between pointer (eventual destination) and target frame
                var distance = getPos(j_frames[i]).left - (getPos(j_pointer[0]).left + 2);
                var leftstr = (distance >= 0 ? '-=' : '+=') + Math.abs(distance) + 'px';

                //Animate filmstrip and slide target frame under pointer
                //If target frame is a duplicate, jump back to 'original' frame
                j_filmstrip.animate({
                    'left': leftstr
                }, opts.transition_speed, opts.easing, function () {
                    //Always ensure that there are a sufficient number of hidden frames on either
                    //side of the filmstrip to avoid empty frames
                    if (i > item_count) {
                        i = i % item_count;
                        iterator = i;
                        j_filmstrip.css('left', '-' + ((opts.frame_width + frame_margin) * i) + 'px');
                    } else if (i <= (item_count - strip_size)) {
                        i = (i % item_count) + item_count;
                        iterator = i;
                        j_filmstrip.css('left', '-' + ((opts.frame_width + frame_margin) * i) + 'px');
                    }

                    if (!opts.fade_panels) {
                        j_panels.hide().eq(i % item_count).show();
                    }
                    j_navPrev.click(showPrevItem);
                    j_navNext.click(showNextItem);
                });
            } else if (slide_method == 'pointer') {
                //Stop pointer if it's currently in motion
                j_pointer.stop();
                //Get position of target frame
                var pos = getPos(j_frames[i]);
                //Slide the pointer over the target frame
                j_pointer.animate({
                    'left': (pos.left - 2 + 'px')
                }, opts.transition_speed, opts.easing, function () {
                    if (!opts.fade_panels) {
                        j_panels.hide().eq(i % item_count).show();
                    }
                    $('img.nav-prev').click(showPrevItem);
                    $('img.nav-next').click(showNextItem);
                    enableFrameClicking();
                });
            }

          

            if ($('a', j_frames[i])[0]) {
                j_pointer.off('click').click(function () {
                    var a = $('a', j_frames[i]).eq(0);
                    if (a.attr('target') == '_blank') { window.open(a.attr('href')); }
                    else { location.href = a.attr('href'); }
                });
            }
              */
        };
        function showNextItem() {
            $(document).stopTime("transition");
            if (++iterator >= d_thumb.length - 1) { iterator = 0; }
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
                                    <div class="tn3e-full-image" style="position: absolute; width: 770px; height: 301px; left: 0px; top: 0px;">\
                                        <img src="css/grad.jpg" alt="Abstract lights" width="770" height="301" style="width: 770px; height: 301px;">\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="tn3-in-image" style="position: absolute; width: 770px; height: 301px; left: 0px; top: 0px; display: block;">\
                                <div class="tn3e-control-bar tn3_v tn3_h tn3_o" style="top: 110.5px; left: 263.5px; display: block; opacity: 1;">\
                                    <div class="tn3e-fullscreen" title="Maximize"></div>\
                                    <div class="tn3e-play" title="Start Slideshow"></div>\
                                    <div class="tn3e-show-albums" title="Album List"></div>\
                                </div>\
                                <div class="tn3e-preloader" style="display: none;">\
                                    <img src="css/preload.gif">\
                                </div>\
                                <div class="tn3e-timer" style="display: none;"></div>\
                            </div>\
                        </div>');
            j_ins = strip.find(".tn3e-image-in");
            j_controlbar = strip.find(".tn3e-control-bar");
            j_fullscreen = strip.find(".tn3e-fullscreen");
            j_play = strip.find(".tn3e-play");
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

            j_gallery.append(strip);
        };

        function buildBar() {
            var next = '<div class="tn3e-next" title="Next Image"></div>';
            var prev = '<div class="tn3e-prev" title="Previous Image"></div>';
            j_navNext = $(next);
            j_navPrev = $(prev);
            j_gallery.append(j_navNext);
            j_gallery.append(j_navPrev);
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

            buildFilmstrip();
            buildBar();
            buildThumbs();
            loadImages();

            if (d_thumb.length > 0) {
                showItem(d_thumb[0].id, { showImmediately: true });
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
        baseUrl: "http://localhost:43926/",
        transition_speed: 500,
        transition_interval: 2000,
        overlay_opacity: 0.6,
        show_captions: false,
        pause: false,
        autoPlay: false,
        height: 395
    };
})(jQuery);