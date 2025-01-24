// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
    const spanElement = document.querySelector('.num');
    const targetNumber = parseInt(spanElement.getAttribute('data-to'));
    const duration = parseInt(spanElement.getAttribute('data-time'));
    const increment = 1;

    let currentNumber = 0;
    const interval = setInterval(() => {
        currentNumber += increment;
        if (currentNumber >= targetNumber) {
            currentNumber = targetNumber;
            clearInterval(interval);
        }
        spanElement.textContent = Math.floor(currentNumber);
    }, 200);
});
