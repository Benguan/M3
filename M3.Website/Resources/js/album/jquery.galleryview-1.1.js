
(function ($) {
    $.fn.galleryView = function (options) {
        var opts = $.extend($.fn.galleryView.defaults, options);

        var id;
        var iterator = 0;
        var gallery_width;
        var gallery_height;
        var frame_margin = 10;
        var strip_width;
        var wrapper_width;
        var item_count = 0;
        var slide_method;
        var img_path;
        var paused = false;
        var frame_caption_size = 20;
        var frame_margin_top = 5;
        var pointer_width = 2;

        var j_gallery;
        var j_filmstrip;
        var j_frames;
        var j_panels;
        var j_pointer;
        var j_thumb;

        // { album: "2014春游", title: "春游", description : "2014春游",  cover: "1.gif", 
        //   photos : [   [{title: "第1页第1张", description : "第1张", thumbnailUrl: "1.thumb.gif", normalUrl : "1.gif"},{title: "第1页第2张", description : "第2张", thumbnailUrl: "2.thumb.gif", normalUrl : "2.gif"}],
        //                [{title: "第2页第1张", description : "第1张", thumbnailUrl: "1.thumb.gif", normalUrl : "1.gif"},{title: "第2页第2张", description : "第2张", thumbnailUrl: "2.thumb.gif", normalUrl : "2.gif"}]
        //            ]
        // }
        var d_images;
        var currentpage;
        var currentphoto;

        function showItem(i) {
            //Disable next/prev buttons until transition is complete
            $('img.nav-next').unbind('click');
            $('img.nav-prev').unbind('click');
            j_frames.unbind('click');
            if (has_panels) {
                if (opts.fade_panels) {
                    //Fade out all panels and fade in target panel
                    j_panels.fadeOut(opts.transition_speed).eq(i % item_count).fadeIn(opts.transition_speed, function () {
                        if (!has_filmstrip) {
                            $('img.nav-prev').click(showPrevItem);
                            $('img.nav-next').click(showNextItem);
                        }
                    });
                }
            }

            if (has_filmstrip) {
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
                        $('img.nav-prev').click(showPrevItem);
                        $('img.nav-next').click(showNextItem);
                        enableFrameClicking();
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
                    j_pointer.unbind('click').click(function () {
                        var a = $('a', j_frames[i]).eq(0);
                        if (a.attr('target') == '_blank') { window.open(a.attr('href')); }
                        else { location.href = a.attr('href'); }
                    });
                }
            }
        };
        function showNextItem() {
            $(document).stopTime("transition");
            if (++iterator == j_frames.length) { iterator = 0; }
            showItem(iterator);
            $(document).everyTime(opts.transition_interval, "transition", function () {
                showNextItem();
            });
        };
        function showPrevItem() {
            $(document).stopTime("transition");
            if (--iterator < 0) { iterator = item_count - 1; }
            //alert(iterator);
            showItem(iterator);
            $(document).everyTime(opts.transition_interval, "transition", function () {
                showNextItem();
            });
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
        function enableFrameClicking() {
            j_frames.each(function (i) {
                //If there isn't a link in this frame, set up frame to slide on click
                //Frames with links will handle themselves
                if ($('a', this).length == 0) {
                    $(this).click(function () {
                        $(document).stopTime("transition");
                        showItem(i);
                        iterator = i;
                        $(document).everyTime(opts.transition_interval, "transition", function () {
                            showNextItem();
                        });
                    });
                }
            });
        };

        function buildThumbs() {
            var thum = '<div class="tn3e-thumbs" style="height: 296.885416666667px; position: absolute; cursor: auto;">\
                            <div style="position: absolute; overflow: hidden; width: 123px; height: 297px;">\
                                <ul style="position: relative; margin: 0px; padding: 0px; border-width: 0px; width: 1520px; list-style: none; top: -533.529805252305px;">\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/1.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/2.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/3.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/4.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/5.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/6.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/7.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/8.jpg"></li>\
                                    <li class="tn3e-thumb tn3e-thumb-selected" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/9.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/10.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/11.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/12.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/13.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/14.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/15.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/16.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/17.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/18.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/19.jpg"></li>\
                                    <li class="tn3e-thumb" style="float: none; opacity: 1;">\
                                        <div style="width: 114px; height: 72px; position: absolute; opacity: 0.5; background-color: rgb(0, 0, 0);"></div>\
                                        <img src="http://www.tn3gallery.com/images/114x72/20.jpg"></li>\
                                </ul>\
                            </div>\
                        </div>';
            j_gallery.append(thum);
        };

        function buildFilmstrip() {
            var strip = '<div class="tn3e-image" style="width: 770px; height: 300.885416666667px; overflow: hidden; position: relative;">\
                            <div class="tn3e-image-ins" style="position: absolute; width: 100%; height: 100%;">\
                                <div class="tn3e-image-in" style="position: absolute; overflow: hidden; visibility: visible; width: 770px; height: 300.885416666667px; left: 0px;">\
                                    <div class="tn3e-full-image" style="position: absolute; width: 770px; height: 301px; left: 0px; top: 0px;">\
                                        <img src="css/grad.jpg" alt="Abstract lights" width="770" height="301" style="width: 770px; height: 301px;">\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="tn3-in-image" style="position: absolute; width: 770px; height: 301px; left: 0px; top: 0px; display: block;">\
                                <div class="tn3e-control-bar tn3_v tn3_h tn3_o" style="top: 110.5px; left: 263.5px; display: none; opacity: 0;">\
                                    <div class="tn3e-fullscreen" title="Maximize"></div>\
                                    <div class="tn3e-play" title="Start Slideshow"></div>\
                                    <div class="tn3e-show-albums" title="Album List"></div>\
                                </div>\
                                <div class="tn3e-preloader" style="display: none;">\
                                    <img src="css/preload.gif">\
                                </div>\
                                <div class="tn3e-timer" style="display: none;"></div>\
                            </div>\
                        </div>'
            j_gallery.append(strip);
        };

        function buildBar() {
            var next = '<div class="tn3e-next" title="Next Image"></div>';
            var prev = '<div class="tn3e-prev" title="Previous Image"></div>';
            j_gallery.append(next);
            j_gallery.append(prev);
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

            /*
            $().mousemove(function (e) {
                if (mouseIsOverPanels(e.pageX, e.pageY)) {
                    if (opts.pause_on_hover) {
                        $(document).oneTime(500, "animation_pause", function () {
                            $(document).stopTime("transition");
                            paused = true;
                        });
                    }
                    if (has_panels && !has_filmstrip) {
                        $('.nav-overlay').fadeIn('fast');
                        $('.nav-next').fadeIn('fast');
                        $('.nav-prev').fadeIn('fast');
                    }
                } else {
                    if (opts.pause_on_hover) {
                        $(document).stopTime("animation_pause");
                        if (paused) {
                            $(document).everyTime(opts.transition_interval, "transition", function () {
                                showNextItem();
                            });
                            paused = false;
                        }
                    }
                    if (has_panels && !has_filmstrip) {
                        $('.nav-overlay').fadeOut('fast');
                        $('.nav-next').fadeOut('fast');
                        $('.nav-prev').fadeOut('fast');
                    }
                }
            });

    

            j_panels.eq(0).show();

            if (item_count > 1) {
                $(document).everyTime(opts.transition_interval, "transition", function () {
                    showNextItem();
                });
            }
                    */

            j_gallery.css('visibility', 'visible');
        });
    };

    $.fn.galleryView.defaults = {
        panel_width: 400,
        panel_height: 300,
        frame_width: 80,
        frame_height: 80,
        filmstrip_size: 3,
        overlay_height: 70,
        overlay_font_size: '1em',
        transition_speed: 400,
        transition_interval: 6000,
        overlay_opacity: 0.6,
        overlay_color: 'black',
        background_color: 'black',
        overlay_text_color: 'white',
        caption_text_color: 'white',
        border: '1px solid black',
        nav_theme: 'light',
        easing: 'swing',
        filmstrip_position: 'bottom',
        overlay_position: 'bottom',
        show_captions: false,
        fade_panels: true,
        pause_on_hover: false,
        height: 395
    };
})(jQuery);