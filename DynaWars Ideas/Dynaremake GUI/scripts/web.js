var menuPart = ""; //Inventory, Skills, options, etc
function blur_elem(classname, restart, selector, add) {
	if(selector === undefined) selector = "body";
	if(restart === undefined || (restart != undefined && restart == false)) {
		if(add === undefined) {
			document.getElementById("css").innerHTML += "<style>"+selector+" > div:not(#"+classname+") {-webkit-filter: blur(3px);-moz-filter: blur(3px);-o-filter: blur(3px);-ms-filter: blur(3px);filter: blur(3px);}</style>";
		} else {
			document.getElementById("css").innerHTML = "<style>"+selector+" > div:not(#"+classname+") {-webkit-filter: blur(3px);-moz-filter: blur(3px);-o-filter: blur(3px);-ms-filter: blur(3px);filter: blur(3px);}</style>";
		}
	} else if(restart != undefined && restart == true) {
		document.getElementById("css").innerHTML = "";
	}
}
function blur_id(idname, restart) {
	if(restart === undefined) {
		document.getElementById("css").innerHTML += "<style>#"+idname+" {-webkit-filter: blur(3px);-moz-filter: blur(3px);-o-filter: blur(3px);-ms-filter: blur(3px);filter: blur(3px);}</style>";
	} else {
		document.getElementById("css").innerHTML = "";
	}
}
function back(elem, id, restore_blur) {
	if(restore_blur === undefined) {
		blur_elem("", true);
	} else {
		blur_elem(id);
	}
	elem.parentNode.style.display = "none";
	showElem(id);
}
function switchTemplate(id, id2, blurred) {
	if(blurred != undefined) {
		blur_elem(id2, undefined, undefined, false);
	} else {
		blur_elem("", true);
	}
	hideElem(id);
	showElem(id2);
}
function showElem(id, state) {
	if(state === undefined) state = "block";
    document.getElementById(id).style.display = state;
}
function hideElem(id) {
    document.getElementById(id).style.display = "none";
}
function show(elem, state) {
	if(state === undefined) state = "block";
    elem.style.display = state;
}
function hide(elem) {
    elem.style.display = "none";
}
function mainMenuBackground() {
	document.getElementById("background").style.background = "url('images/main-background.png')";
}
function gameBackground() {
	document.getElementById("background").style.background = "url('images/game-background.png')";
}
document.onkeyup = function(e) {
	if (!e) e = window.event;
	if(document.getElementById("mainGame").style.display == "block") {
		/*console.log(e.keyCode);*/
		if(e.keyCode == 69) {
			(document.getElementById("gameMenu").style.display == "none") ? showElem("gameMenu")+blur_elem("gameMenu", false, "#mainGame")+blur_id("background") : hideElem("gameMenu")+blur_id("", true);
		} else if(e.keyCode == 27) {
			(document.getElementById("pauseMenu").style.display == "none") ? showElem("pauseMenu")+blur_elem("pauseMenu", false, "#mainGame")+blur_id("background") : hideElem("pauseMenu")+blur_id("", true);
		}
	}
	
}
function followPos(elem) {
	elem.style.top = event.clientY;
	elem.style.left = event.clientX;
}