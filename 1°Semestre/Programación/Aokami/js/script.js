// Elementos del menú lateral
const drawer = document.getElementById("drawer");
const overlay = document.getElementById("overlay");
const hamburger = document.getElementById("hamburger");
const closeDrawer = document.getElementById("closeDrawer");
const menuIconTop = document.getElementById("menuIconTop");

// Elementos del panel de filtros
const filtersPanel = document.getElementById("filtersPanel");
const filterToggle = document.getElementById("filterToggle");
const closeFilters = document.getElementById("closeFilters");
const applyFilters = document.getElementById("applyFilters");
const resetFilters = document.getElementById("resetFilters");

// Precios
const priceRange = document.getElementById("priceRange");
const minPriceInput = document.getElementById("minPrice");
const maxPriceInput = document.getElementById("maxPrice");

// Productos
const products = document.querySelectorAll('.product');
const searchInput = document.getElementById("searchInput");

// ===== FUNCIONES PARA EL MENÚ LATERAL =====
function openMenu() {
    drawer.classList.add("open");
    overlay.classList.add("active");
    document.body.style.overflow = "hidden";
}

function closeMenu() {
    drawer.classList.remove("open");
    overlay.classList.remove("active");
    document.body.style.overflow = "";
}

// ===== FUNCIONES PARA EL PANEL DE FILTROS =====
function openFilters() {
    filtersPanel.classList.add("open");
    overlay.classList.add("active");
    document.body.style.overflow = "hidden";
}

function closeFiltersPanel() {
    filtersPanel.classList.remove("open");
    overlay.classList.remove("active");
    document.body.style.overflow = "";
}

// ===== EVENT LISTENERS =====
// Menú lateral
hamburger.addEventListener("click", openMenu);
menuIconTop.addEventListener("click", openMenu);
closeDrawer.addEventListener("click", closeMenu);

// Filtros
filterToggle.addEventListener("click", openFilters);
closeFilters.addEventListener("click", closeFiltersPanel);
overlay.addEventListener("click", function() {
    closeMenu();
    closeFiltersPanel();
});

// Cerrar con tecla ESC
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        closeMenu();
        closeFiltersPanel();
    }
});

// ===== LÓGICA DE PRECIOS =====
priceRange.addEventListener("input", function() {
    minPriceInput.value = this.value;
});

minPriceInput.addEventListener("change", function() {
    priceRange.value = this.value;
});

maxPriceInput.addEventListener("change", function() {
    // Validar que el máximo sea mayor que el mínimo
    if (parseInt(this.value) < parseInt(minPriceInput.value)) {
        this.value = parseInt(minPriceInput.value) + 10;
    }
});

// ===== FILTRADO DE PRODUCTOS =====
function filterProducts() {
    const searchTerm = searchInput.value.toLowerCase().trim();
    const minPrice = parseInt(minPriceInput.value) || 0;
    const maxPrice = parseInt(maxPriceInput.value) || 1000;
    
    // Obtener categorías seleccionadas
    const selectedCategories = [];
    document.querySelectorAll('.filter-section:nth-child(2) input[type="checkbox"]:checked').forEach(cb => {
        selectedCategories.push(cb.nextElementSibling.textContent.toLowerCase());
    });
    
    // Obtener marcas seleccionadas
    const selectedBrands = [];
    document.querySelectorAll('.filter-section:nth-child(5) input[type="checkbox"]:checked').forEach(cb => {
        selectedBrands.push(cb.nextElementSibling.textContent.toLowerCase());
    });

    products.forEach(product => {
        const title = product.querySelector('h3').textContent.toLowerCase();
        const brand = product.querySelector('.brand').textContent.toLowerCase();
        const price = parseInt(product.getAttribute('data-price'));
        const category = product.getAttribute('data-category');
        
        let showProduct = true;
        
        // Filtrar por búsqueda
        if (searchTerm && !title.includes(searchTerm) && !brand.includes(searchTerm)) {
            showProduct = false;
        }
        
        // Filtrar por precio
        if (price < minPrice || price > maxPrice) {
            showProduct = false;
        }
        
        // Filtrar por categoría (si hay alguna seleccionada)
        if (selectedCategories.length > 0 && !selectedCategories.some(cat => 
            category && category.includes(cat.replace(/[^a-z]/g, '')))) {
            showProduct = false;
        }
        
        // Filtrar por marca (si hay alguna seleccionada)
        if (selectedBrands.length > 0 && !selectedBrands.some(selectedBrand => 
            brand.includes(selectedBrand))) {
            showProduct = false;
        }
        
        product.style.display = showProduct ? 'block' : 'none';
    });
}

// Aplicar filtros
applyFilters.addEventListener("click", function() {
    filterProducts();
    closeFiltersPanel();
    
    // Mostrar notificación
    showNotification('Filtros aplicados correctamente');
});

// Resetear filtros
resetFilters.addEventListener("click", function() {
    // Limpiar todos los checkboxes
    document.querySelectorAll('input[type="checkbox"]').forEach(cb => {
        cb.checked = false;
    });
    
    // Resetear precios
    priceRange.value = 250;
    minPriceInput.value = 0;
    maxPriceInput.value = 500;
    
    // Resetear búsqueda
    searchInput.value = '';
    
    // Mostrar todos los productos
    products.forEach(product => {
        product.style.display = 'block';
    });
    
    showNotification('Filtros limpiados');
});

// Búsqueda en tiempo real
searchInput.addEventListener("input", filterProducts);

// ===== FUNCIONALIDAD DEL CARRITO =====
document.querySelectorAll('.btn-add').forEach(button => {
    button.addEventListener('click', function() {
        const productName = this.parentElement.querySelector('h3').textContent;
        const productPrice = this.parentElement.querySelector('.price').textContent;
        
        console.log(`Producto agregado: ${productName} - ${productPrice}`);
        
        // Efecto visual
        const originalText = this.textContent;
        this.textContent = '✓ Agregado';
        this.style.backgroundColor = '#7c9a57';
        
        setTimeout(() => {
            this.textContent = originalText;
            this.style.backgroundColor = '';
        }, 1500);
    });
});

// ===== FUNCIÓN DE NOTIFICACIÓN =====
function showNotification(message) {
    // Crear notificación si no existe
    let notification = document.querySelector('.notification');
    if (!notification) {
        notification = document.createElement('div');
        notification.className = 'notification';
        document.body.appendChild(notification);
        
        // Estilos para la notificación
        notification.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background: #8dab64;
            color: white;
            padding: 15px 25px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
            z-index: 10000;
            transform: translateX(150%);
            transition: transform 0.3s ease;
            font-weight: 500;
        `;
    }
    
    notification.textContent = message;
    notification.style.transform = 'translateX(0)';
    
    // Ocultar después de 3 segundos
    setTimeout(() => {
        notification.style.transform = 'translateX(150%)';
    }, 3000);
}

// ===== INICIALIZACIÓN =====
// Asegurar que los filtros estén sincronizados al cargar
document.addEventListener('DOMContentLoaded', function() {
    // Colores seleccionables
    document.querySelectorAll('.color-option').forEach(color => {
        color.addEventListener('click', function() {
            this.classList.toggle('selected');
        });
    });
    
    // Click en enlaces del menú
    document.querySelectorAll('.drawer-menu a').forEach(link => {
        link.addEventListener('click', function(e) {
            if (!this.classList.contains('active')) {
                document.querySelectorAll('.drawer-menu a').forEach(l => l.classList.remove('active'));
                this.classList.add('active');
                closeMenu();
            }
        });
    });
});