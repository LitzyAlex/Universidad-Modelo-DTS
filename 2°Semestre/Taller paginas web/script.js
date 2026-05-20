//variable para likes
let likes = 0;

function cambiarTexto(){
    document.getElementById("titulo").innerHTML="Tobias Fox :p";
    document.getElementById("descripcion").innerHTML="Un perro muy molesto";
}

function modoOscuro(){
    document.body.classList.toggle("dark");
}

function darLike(){
    likes++;
    document.getElementById("likes").innerHTML=likes;
}