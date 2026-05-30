import os
from tabla import cargar_config, guardar_config, format_tabla_cambio

def limpiar_pantalla():
    #Esto para que no pase lo de la otra vez porque no todos ocupamos linux(octavio)
    os.system('cls' if os.name == 'nt' else 'clear')


#El menu pues para elegir obviamente
def menu():
    print("\n --- Sistema de cambio minimo :D --- \n")
    print(" 1) Mostrar tabla")
    print(" 2) Agregar denominacion")
    print(" 3) Eliminar denominacion")
    print(" 4) Cambiar maximo de cambio")
    print(" 5) Salir\n")

#Para mostrar la tabla ya hecha
def mostrar_tabla():
    limpiar_pantalla()   #Limpia pantalla como dice
    config = cargar_config() #Guarda los datos del json en config
    #llama la funcion que construye la tabla y le pasa los datos del json
    print(format_tabla_cambio(config['denominaciones'], config['max_cambio']))
    input("\nPresiona una tecla para continuar...")

#Agregar otra denominación(dinero para el cambio)
def agregar_denominacion():
    limpiar_pantalla()
    config = cargar_config()
    print("\n --- Agregar denominacion --- \n")
    #Muestra las denominaciones actuales ordenandolas con sorted para que se vean bien bonitas
    print(f" Denominaciones actuales: {sorted(config['denominaciones'])}\n")
    
    #El int es para convertir el texto a numero   
    nueva = int(input(" Nueva denominacion: "))
    if nueva <= 0:
        print(" La denominacion debe ser mayor a 0.")
    elif nueva in config['denominaciones']:
        print(f" La denominacion {nueva} ya existe.")
    else:
        config['denominaciones'].append(nueva) #Se agrega si es correcta
        guardar_config(config) #Guarda el json ya actualizado
        print(f" Denominacion {nueva} agregada.")
    

    input("\nPresiona una tecla para continuar...")

def eliminar_deniminacion():
    limpiar_pantalla()
    config = cargar_config()
    print("\n --- Eliminar denominacion --- \n")
    print(f" Denominaciones actuales: {sorted(config['denominaciones'])}\n")

    eli = int(input(" Denominacion a eliminar: "))
    if eli <= 0:
        print(" La denominacion debe ser mayor a 0.")
    elif eli not in config['denominaciones']:
        print(f" La denominacion {eli} no existe.")
    else:
        config['denominaciones'].remove(eli) #Se agrega si es correcta
        guardar_config(config) #Guarda el json ya actualizado
        print(f" Denominacion {eli} eliminada.")
    
    input("\nPresiona una tecla para continuar...")

#Cambiar el maximo de cambio(osea lo del precio mayor)
def cambiar_max():
    limpiar_pantalla()
    config = cargar_config() #EL JSON
    print("\n --- Cambiar maximo de cambio --- \n")
    print(f" Maximo actual: {config['max_cambio']}\n") #Te muestra el max actual

    
    nuevo = int(input(" Nuevo maximo: "))
    if nuevo <= 0:
        print(" El maximo debe ser mayor a 0.")
    else:
        config['max_cambio'] = nuevo   #Reescribir el maximo
        guardar_config(config) #Actualizar el json
        print(f" Maximo de cambio actualizado a {nuevo}.") #Ya ta

    input("\nPresiona una tecla para continuar...")

#Para las opciones
def seleccion():
    limpiar_pantalla()
    menu()
    opcion = input(" Selecciona una opcion: ")

    match opcion:
        case '1':
            mostrar_tabla()
        case '2':
            agregar_denominacion()
        case '3':
            eliminar_deniminacion()
        case '4':
            cambiar_max()
        case '5':
            limpiar_pantalla()
            print("\n Bye bye\n")
            exit()
        case _:
            print(" Opcion no valida")
            input("\nPresiona una tecla para continuar...")

#Solo verifica que este es el archivo principal que quizas luego lo cambies :c
if __name__ == "__main__":   
    while True:   #Bucle infinito
        seleccion()
