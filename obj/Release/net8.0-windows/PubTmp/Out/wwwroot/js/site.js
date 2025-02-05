﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/* "Animación" áumento de números */
document.addEventListener("DOMContentLoaded", function () {
    const spanEl = document.getElementById("clientes");
    const numFinal = parseInt(spanEl.getAttribute('data-to'));
    const duracion = parseInt(spanEl.getAttribute('data-time'));
    const incremento = 1;

    let numActual = 0;
    const intervalo = setInterval(() => {
        numActual += incremento;
        if (numActual >= numFinal) {
            numActual = numFinal;
            clearInterval(intervalo);
        }
        spanEl.textContent = Math.floor(numActual);
    }, duracion);
});
