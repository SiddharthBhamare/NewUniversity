
/* Set the width of the sidebar to 250px and the left margin of the page content to 250px */
var isOpen=true;
function openNav() {
    if (isOpen) {
        document.getElementById("mySidebar").style.width = "200px";
        document.getElementById("main").style.marginLeft = "200px";
        isOpen = false;
    }
    else {
        document.getElementById("mySidebar").style.width = "0";
        document.getElementById("main").style.marginLeft = "0";
        isOpen = true;
    }
}