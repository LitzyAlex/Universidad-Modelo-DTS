
const cartItems = [
    {
        id: 1,
        name: 'Lip Divine Liquid Lipstick',
        brand: 'Moira',
        price: 80.00,
        quantity: 1,
        image: null
    },
    {
        id: 2,
        name: 'Eyeshadow Palette Pro',
        brand: 'Moira',
        price: 120.00,
        quantity: 2,
        image: null
    },
    {
        id: 3,
        name: 'Foundation Perfect Match',
        brand: 'Moira',
        price: 95.00,
        quantity: 1,
        image: null
    }
];


const SHIPPING_COST = 0;
const TAX_RATE = 0.16;

document.addEventListener('DOMContentLoaded', function() {
    renderCart();
    updateCartTotals();
});

function renderCart() {
    const cartContainer = document.getElementById('cartItemsContainer');
    const emptyCart = document.getElementById('emptyCart');

    if (cartItems.length === 0) {
        cartContainer.style.display = 'none';
        emptyCart.style.display = 'block';
        return;
    }

    cartContainer.style.display = 'flex';
    emptyCart.style.display = 'none';
    cartContainer.innerHTML = '';

    cartItems.forEach(item => {
        const itemElement = createCartItemElement(item);
        cartContainer.appendChild(itemElement);
    });
}

function createCartItemElement(item) {
    const div = document.createElement('div');
    div.className = 'cart-item';
    div.id = `item-${item.id}`;

    const itemTotal = item.price * item.quantity;

    div.innerHTML = `
        <div class="cart-item-image">
            ${item.image ? `<img src="${item.image}" alt="${item.name}">` : 'Imagen'}
        </div>
        <div class="cart-item-info">
            <div class="cart-item-name">${item.name}</div>
            <div class="cart-item-brand">${item.brand}</div>
            <div class="cart-item-price">$${item.price.toFixed(2)}</div>
        </div>
        <div class="cart-item-quantity">
            <button class="qty-btn" onclick="changeQuantity(${item.id}, -1)">−</button>
            <input type="number" value="${item.quantity}" min="1" onchange="updateQuantity(${item.id}, this.value)">
            <button class="qty-btn" onclick="changeQuantity(${item.id}, 1)">+</button>
        </div>
        <div class="cart-item-total">$${itemTotal.toFixed(2)}</div>
        <button class="cart-item-remove" onclick="removeItem(${item.id})">Eliminar</button>
    `;

    return div;
}

function changeQuantity(itemId, change) {
    const item = cartItems.find(i => i.id === itemId);
    if (item) {
        item.quantity += change;
        if (item.quantity < 1) {
            item.quantity = 1;
        }
        renderCart();
        updateCartTotals();
    }
}

function updateQuantity(itemId, newQuantity) {
    const item = cartItems.find(i => i.id === itemId);
    const quantity = parseInt(newQuantity);
    
    if (item && quantity >= 1) {
        item.quantity = quantity;
        renderCart();
        updateCartTotals();
    }
}

function removeItem(itemId) {
    const index = cartItems.findIndex(i => i.id === itemId);
    if (index > -1) {
        const item = cartItems[index];
        if (confirm(`¿Deseas eliminar "${item.name}" del carrito?`)) {
            cartItems.splice(index, 1);
            renderCart();
            updateCartTotals();
        }
    }
}

function updateCartTotals() {

    const subtotal = cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
  
    const taxes = subtotal * TAX_RATE;
    
    const total = subtotal + SHIPPING_COST + taxes;

    const itemCount = cartItems.reduce((sum, item) => sum + item.quantity, 0);

    document.getElementById('subtotal').textContent = `$${subtotal.toFixed(2)}`;
    document.getElementById('taxes').textContent = `$${taxes.toFixed(2)}`;
    document.getElementById('shipping').textContent = `$${SHIPPING_COST.toFixed(2)}`;
    document.getElementById('total').textContent = `$${total.toFixed(2)}`;
    document.getElementById('itemCount').textContent = `${itemCount} ${itemCount === 1 ? 'artículo' : 'artículos'} en el carrito`;

    const checkoutBtn = document.getElementById('checkoutBtn');
    checkoutBtn.disabled = cartItems.length === 0;
    if (cartItems.length === 0) {
        checkoutBtn.style.opacity = '0.5';
        checkoutBtn.style.cursor = 'not-allowed';
    } else {
        checkoutBtn.style.opacity = '1';
        checkoutBtn.style.cursor = 'pointer';
    }
}

function proceedToCheckout() {
    if (cartItems.length === 0) {
        alert('Tu carrito está vacío. Agrega productos antes de continuar.');
        return false;
    }
    return true;
}

function saveCartToLocalStorage() {
    localStorage.setItem('cart', JSON.stringify(cartItems));
}

function loadCartFromLocalStorage() {
    const saved = localStorage.getItem('cart');
    if (saved) {
        const loaded = JSON.parse(saved);
        // Actualizar el array de cartItems
        cartItems.length = 0;
        cartItems.push(...loaded);
    }
}

function clearCart() {
    if (confirm('¿Deseas vaciar todo el carrito?')) {
        cartItems.length = 0;
        renderCart();
        updateCartTotals();
        saveCartToLocalStorage();
    }
}

console.log('Carrito cargado correctamente');
