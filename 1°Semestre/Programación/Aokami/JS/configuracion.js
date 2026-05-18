function cerrarSesion() {
    if (confirm('¿Estás seguro de que deseas cerrar sesión?')) {
        alert('Sesión cerrada correctamente');
        location.href = '../index.html';
    }
}

document.addEventListener('DOMContentLoaded', function() {
    const settingsLinks = document.querySelectorAll('.settings-link');
    
    settingsLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const settingText = this.previousElementSibling.textContent;
            handleSettingClick(settingText);
        });
    });
});

function handleSettingClick(setting) {
    switch(setting) {
        case 'Cambiar nombre de usuario':
            openModal('Cambiar nombre de usuario', 'Ingresa tu nuevo nombre de usuario');
            break;
        case 'Cambiar mail':
            openModal('Cambiar correo electrónico', 'Ingresa tu nuevo correo electrónico');
            break;
        case 'Cambiar contraseña':
            openModal('Cambiar contraseña', 'Ingresa tu nueva contraseña');
            break;
        case 'Compras':
            alert('Visualizando tu historial de compras...');
            break;
        case 'Comprar otra vez':
            alert('Mostrando productos de compras anteriores...');
            break;
        case 'Historial':
            alert('Mostrando el historial completo...');
            break;
        case 'Favoritos':
            alert('Abriendo tu lista de favoritos...');
            break;
        case 'Notificaciones':
            alert('Configurando notificaciones...');
            break;
        case 'Cuenta':
            alert('Detalles de tu cuenta...');
            break;
        default:
            console.log('Opción no reconocida');
    }
}

function openModal(title, message) {
    const userInput = prompt(title + '\n\n' + message);
    if (userInput !== null) {
        alert('Cambio realizado: ' + userInput);
    }
}

function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function validateUsername(username) {
    return username.length >= 3 && username.length <= 20;
}

console.log('Script de configuración cargado correctamente');
