function myFunction() {



    let x = document.getElementById("bmenu");
    if (x.className === "menu") {
        x.className += " responsive";

    } else {
        x.className = "menu";
    }
}