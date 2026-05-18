# Usar la imagen oficial de PHP con Apache (versión 8.3)
FROM php:8.3-apache

# Instalar dependencias del sistema necesarias para Composer
RUN apt-get update && apt-get install -y \
    git \
    unzip \
    libzip-dev \
    && rm -rf /var/www/html/*

# Habilitar el módulo mod_rewrite de Apache para mejores rutas
RUN a2enmod rewrite

# Copiar el ejecutable de Composer desde la imagen oficial, instalarlo y las librerias que ocupa
COPY --from=composer:latest /usr/bin/composer /usr/bin/composer

# Configurar el directorio de trabajo
WORKDIR /var/www/html

# Copiar el contenido de la carpeta src al directorio raíz de Apache
COPY ./src /var/www/html

# Ajustar los permisos para que Apache pueda acceder a los archivos, a2enmod rewrite
RUN chown -R www-data:www-data /var/www/html

# Exponer el puerto 80 para el servidor web
EXPOSE 80

#No ocupa CMD porque php ya trae dentro esto
