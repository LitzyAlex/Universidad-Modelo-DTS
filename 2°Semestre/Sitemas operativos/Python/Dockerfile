# Usar una imagen oficial de Python como base
FROM python:3.11-slim

# Evitar que Python genere archivos .pyc y permitir que la salida llegue a la terminal sin buffer
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

# Directorio de trabajo
#Esta cambiando la carpeta donde va a trabajar
WORKDIR /app

# Instalar dependencias del sistema necesarias
#Actualiza el sistema para que la imagen tenga todo
RUN apt-get update && apt-get install -y --no-install-recommends \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# Instalar dependencias de Python
#librerias
COPY requirements.txt /app/
RUN pip install --upgrade pip && pip install -r requirements.txt

# Copiar el resto del código
COPY . /app/

# Exponer el puerto
EXPOSE 8000

# Comando para ejecutar la aplicación
CMD ["python", "manage.py", "runserver", "0.0.0.0:8000"]
