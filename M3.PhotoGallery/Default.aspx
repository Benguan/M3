<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html>
<head>
<title>M3 Photo Gallery</title>
<meta name="Description" content="Snow Stack is a demo of WebKit CSS 3D visual effects with latest WebKit nightly on Mac OS X Snow Leopard" />
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>

<style type="text/css">

body
{
    overflow: hidden;
	background-color: black;
	color: white;
	margin: 0;
	padding: 0;
}

.view
{
	position: absolute;
	display: block;
    -webkit-transform-style: preserve-3d;
}

#title .view{ float: left; font-size:32px; font-weight: bold;}

.viewflat
{
	position: absolute;
	display: block;
    -webkit-transform-style: preserve-3d;
}

.fader
{
	-webkit-transition-property: opacity;
	-webkit-transition-duration: 550ms;
	-webkit-transition-timing-function: ease-in-out;
}

.page
{
    -webkit-perspective: 600;
	width: 100%;
    height: 100%;
	margin: 0 auto;
	text-align: center;
}

div.origin
{
	left: 50%;
	top: 50%;
}

div#camera
{
	-webkit-transition-property: -webkit-transform;
	-webkit-transition-duration: 5s;
	-webkit-transition-timing-function: cubic-bezier(0.2, 0.6, 0.6, 0.9);
	-webkit-transform: translate3d(0, 0, 0);
}

div#dolly
{
	-webkit-transition-property: -webkit-transform;
	-webkit-transition-duration: 550ms;
	-webkit-transition-timing-function: ease-out;
	-webkit-transform: translate3d(0, 0, 0);
}

div.cell.reflection img
{
	-webkit-mask-image: -webkit-gradient(linear, left top, left bottom, color-stop(0.25, transparent), color-stop(1.0, rgba(255, 255, 255, 0.5)));
}

div.cell
{
	-webkit-transition-property: -webkit-transform opacity;
	-webkit-transition-duration: 550ms;
	-webkit-transform: translate3d(0, 0, 0);
}

div.cell img
{
	display: block;
	position: absolute;

	-webkit-transition-property: -webkit-transform /* PERF -webkit-box-shadow */ /* border-color */;
	-webkit-transition-duration: 550ms;
	-webkit-transform: translate3d(0, 0, 0);
}

div.original img
{
	border: 1px solid transparent;
}

div.original.selected img
{
/* PERF
	-webkit-box-shadow: 0px 0px 35px #000;
*/
	border-color: #000;
}

.mover
{
	-webkit-transition-property: -webkit-transform;
	-webkit-transition-duration: 550ms;
}

div.original.selected .mover
{
	-webkit-transform: translate3d(0, 0, 40px);
}

div.original.selected.magnify .mover
{
	-webkit-transform: translate3d(0, 0, 40px);
}

div.original.selected.magnify img
{
	-webkit-transform: scale(2.0);
}

div.reflection.selected .mover
{
	-webkit-transform: translate3d(0, 0, 40px);
}

#stack.view{
	margin-top:50px;	
}

</style>

</head>

<body>

<div class="page view">

<div class="origin view">
	<div id="camera" class="view">
		<div id="dolly" class="view">
			<div id="title" class="view">

			</div>	
			<div id="stack" class="view">
			</div>
			<div id="mirror" class="view">
				<div id="rstack" class="view">
				</div>
			</div>
		</div>
	</div>
</div>

</div>

<script type="text/javascript">

    var CWIDTH;
    var CHEIGHT;
    var CGAP = 10;
    var CXSPACING;
    var CYSPACING;

    function translate3d(x, y, z) {
        return "translate3d(" + x + "px, " + y + "px, " + z + "px)";
    }

    function cameraTransformForCell(n) {
        var x = Math.floor(n / 3);
        var y = n - x * 3;
        var cx = (x + 0.5) * CXSPACING;
        var cy = (y + 0.5) * CYSPACING;

        if (magnifyMode) {
            return translate3d(-cx, -cy, 180);
        }
        else {
            return translate3d(-cx, -cy, 0);
        }
    }

    var currentCell = -1;

    var cells = [];

    var currentTimer = null;

    var dolly = jQuery("#dolly")[0];
    var camera = jQuery("#camera")[0];

    var magnifyMode = false;

    var zoomTimer = null;

    function refreshImage(elem, cell) {
        if (cell.iszoomed) {
            return;
        }

        if (zoomTimer) {
            clearTimeout(zoomTimer);
        }

        var zoomImage = jQuery('<img class="zoom"></img>');

        zoomTimer = setTimeout(function () {
            zoomImage.load(function () {
                layoutImageInCell(zoomImage[0], cell.div[0]);
                jQuery(elem).replaceWith(zoomImage);
                cell.iszoomed = true;
            });

            zoomImage.attr("src", cell.info.zoom);

            zoomTimer = null;
        }, 2000);
    }

    function layoutImageInCell(image, cell) {
        var iwidth = image.width;
        var iheight = image.height;
        var cwidth = jQuery(cell).width();
        var cheight = jQuery(cell).height();
        var ratio = Math.min(cheight / iheight, cwidth / iwidth);

        iwidth *= ratio;
        iheight *= ratio;

        image.style.width = Math.round(iwidth) + "px";
        image.style.height = Math.round(iheight) + "px";

        image.style.left = Math.round((cwidth - iwidth) / 2) + "px";
        image.style.top = Math.round((cheight - iheight) / 2) + "px";
    }

    function updateStack(newIndex, newmagnifymode) {
        if (currentCell == newIndex && magnifyMode == newmagnifymode) {
            return;
        }

        var oldIndex = currentCell;
        newIndex = Math.min(Math.max(newIndex, 0), cells.length - 1);
        currentCell = newIndex;

        if (oldIndex != -1) {
            var oldCell = cells[oldIndex];
            oldCell.div.attr("class", "cell fader view original");
            if (oldCell.reflection) {
                oldCell.reflection.attr("class", "cell fader view reflection");
            }
        }

        var cell = cells[newIndex];
        cell.div.addClass("selected");

        if (cell.reflection) {
            cell.reflection.addClass("selected");
        }

        magnifyMode = newmagnifymode;

        if (magnifyMode) {
            cell.div.addClass("magnify");
            refreshImage(cell.div.find("img")[0], cell);
        }

        dolly.style.webkitTransform = cameraTransformForCell(newIndex);

        var currentMatrix = new WebKitCSSMatrix(document.defaultView.getComputedStyle(dolly, null).webkitTransform);
        var targetMatrix = new WebKitCSSMatrix(dolly.style.webkitTransform);

        var dx = currentMatrix.e - targetMatrix.e;
        //var angle = Math.min(Math.max(dx / (CXSPACING * 3.0), -1), 1) * 45;

        //var angle = dx>0?30:-30;
        var angle = 0;

        camera.style.webkitTransform = "rotateY(" + angle + "deg)";
        camera.style.webkitTransitionDuration = "0ms";

        /*
        if (currentTimer)
        {
            clearTimeout(currentTimer);
        }
        
        currentTimer = setTimeout(function ()
        {
            camera.style.webkitTransform = "rotateY(0)";
            camera.style.webkitTransitionDuration = "1s";
        }, 330);*/
    }

    function snowstack_addimage(reln, info) {
        var cell = {};
        var realn = cells.length;
        cells.push(cell);

        var x = Math.floor(realn / 3);
        var y = realn - x * 3;



        cell.info = info;

        cell.div = jQuery('<div class="cell fader view original" style="opacity: 0"></div>').width(CWIDTH).height(CHEIGHT);
        cell.title = jQuery('<div class="cell fader view original" style="opacity: 1"></div>').width(CWIDTH * 3).height(50);

        cell.div[0].style.webkitTransform = translate3d(x * CXSPACING, y * CYSPACING, 0);
        cell.title[0].style.webkitTransform = translate3d(x * CXSPACING, y * CYSPACING, 0);

        var img = document.createElement("img");

        jQuery(img).load(function () {
            layoutImageInCell(img, cell.div[0]);
            cell.div.append(jQuery('<a class="mover viewflat" href="' + cell.info.link + '" target="_blank"></a>').append(img));

            cell.div.css("opacity", 1);
        });

        img.src = info.thumb;


        jQuery("#stack").append(cell.div);
        if (y === 0 && (x % 3 === 0)) {
            cell.title[0].innerHTML = "上海旅游节";
            jQuery("#title").append(cell.title);
        }

        if (y == 2) {
            console.log({ "x": x, "y": y });
            cell.reflection = jQuery('<div class="cell fader view reflection" style="opacity: 0"></div>').width(CWIDTH).height(CHEIGHT);
            cell.reflection[0].style.webkitTransform = translate3d(x * CXSPACING, y * CYSPACING, 0);

            var rimg = document.createElement("img");

            jQuery(rimg).load(function () {
                layoutImageInCell(rimg, cell.reflection[0]);
                cell.reflection.append(jQuery('<div class="mover viewflat"></div>').append(rimg));
                cell.reflection.css("opacity", 1);
            });

            rimg.src = info.thumb;

            jQuery("#rstack").append(cell.reflection);
        }
    }

    function snowstack_init() {
        CHEIGHT = Math.round(window.innerHeight / 5);
        CWIDTH = Math.round(CHEIGHT * 300 / 180);
        CXSPACING = CWIDTH + CGAP;
        CYSPACING = CHEIGHT + CGAP;

        jQuery("#mirror")[0].style.webkitTransform = "scaleY(-1.0) " + translate3d(0, -CYSPACING * 6 - 1, 0);
    }


    jQuery(window).load(function () {
        var page = 1;
        var loading = true;

        snowstack_init();

        flickr(function (images) {
            jQuery.each(images, snowstack_addimage);
            updateStack(1);
            loading = false;
        }, page);

        var keys = { left: false, right: false, up: false, down: false };

        var keymap = { 37: "left", 38: "up", 39: "right", 40: "down" };

        var keytimer = null;

        function updatekeys() {
            var newcell = currentCell;
            if (keys.left) {
                /* Left Arrow */
                if (newcell >= 3) {
                    newcell -= 3;
                }
            }
            if (keys.right) {
                /* Right Arrow */
                if ((newcell + 3) < cells.length) {
                    newcell += 3;
                }
                else if (!loading) {
                    /* We hit the right wall, add some more */
                    page = page + 1;
                    loading = true;
                    flickr(function (images) {
                        jQuery.each(images, snowstack_addimage);
                        loading = false;
                    }, page);
                }
            }
            if (keys.up) {
                /* Up Arrow */
                newcell -= 1;
            }
            if (keys.down) {
                /* Down Arrow */
                newcell += 1;
            }

            updateStack(newcell, magnifyMode);
        }

        var delay = 100;

        function keycheck() {
            if (keys.left || keys.right || keys.up || keys.down) {
                if (keytimer === null) {
                    var doTimer = function () {
                        updatekeys();
                        keytimer = setTimeout(doTimer, delay);
                    };
                    doTimer();
                }
            }
            else {
                clearTimeout(keytimer);
                keytimer = null;
            }
        }

        /* Limited keyboard support for now */
        window.addEventListener('keydown', function (e) {
            if (e.keyCode == 32) {
                /* Magnify toggle with spacebar */
                updateStack(currentCell, !magnifyMode);
            }
            else {
                keys[keymap[e.keyCode]] = true;
            }

            keycheck();
        });

        window.addEventListener('keyup', function (e) {
            keys[keymap[e.keyCode]] = false;
            keycheck();
        });
    });

    function flickr(callback, page) {
        var host = "http://localhost:43926/";
        var api = "api/Gallery/Detail/" + page + "/1";
        var url = host + api;

        jQuery.ajax({
            url: url,
            processData: false,
            cache: true,
            dataType: "jsonp",
            jsonpCallback: "receive",
            success: function (item) {
                console.log(item);
            }
        })
        /*
        jQuery.getJSON(url, function(data) 
        {
            var images = jQuery.map(data, function (item)
            {
    
                return {
                    categoryID : item.Id,
                    year:item.Year,
                    categoryName: item.Name,
                    page: item.Page
                }
                return {
                    thumb: item.url_s,
                    zoom: 'http://farm' + item.farm + '.static.flickr.com/' + item.server + '/' + item.id + '_' + item.secret + '.jpg',
                    link: 'http://www.flickr.com/photos/' + item.owner + '/' + item.id,
                    categoryName:"上海旅游节"
    
    
            });
            console.log(images);
            //callback(images);
        });
    */
    }

</script>



</body>

</html>