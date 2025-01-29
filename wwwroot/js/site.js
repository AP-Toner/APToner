// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/* "Animación" áumento de números */
document.addEventListener("DOMContentLoaded", function () {
    let paginaActual = 1;
    const productosPorPagina = 21;

    //console.log("Página actual: ", paginaActual);

    const spanEl = document.getElementById("clientes");
    const prodTitulo = document.querySelectorAll('.producto-title');
    const mostrarMasBtn = document.getElementById('mostrar-mas');
    const listaProductos = document.getElementById('lista-productos');

    if (spanEl) {
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
    }

    if (prodTitulo) {
        prodTitulo.forEach(title => {
            let text = title.textContent.toLowerCase();

            //text = text.replace(/\b\w/g, char => char.toUpperCase());

            title.textContent = text;
        });
    }

    if (mostrarMasBtn && listaProductos) {

        //console.log("Página actual: ", paginaActual);

        mostrarMasBtn.addEventListener('click', function () {
            paginaActual++;

            console.log("Página actual: ", paginaActual);

            fetch(`/Store?pagina=${paginaActual}`)
                .then(response => response.text())
                .then(html => {
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(html, 'text/html');
                    const masProductos = doc.querySelectorAll('#lista-productos .col-12');
                    masProductos.forEach(producto => listaProductos.appendChild(producto));
                    if (masProductos.length < productosPorPagina) {
                        mostrarMasBtn.style.display = 'none';
                    }
                })
                .catch(error => console.error('Error mostrando más productos:', error));

            //console.log("Página actual: ", paginaActual);
        });
    }
});
