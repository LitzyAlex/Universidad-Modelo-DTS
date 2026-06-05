import os
from tabla import cargar_config, guardar_config, format_tabla_cambio, cambio_voraz, aplicar_cambio, cambio_backtracking, construir_tabla

def limpiar_pantalla():
    #Esto para que no pase lo de la otra vez porque no todos ocupamos linux(octavio)
    os.system('cls' if os.name == 'nt' else 'clear')

def menu1():
    print("\n --- Sistema de cambio :D --- \n")
    print(" 1) Mostrar tabla")
    print(" 2) Editar datos")
    print(" 3) Cambio")
    print(" 4) Salir")

#El menu pues para elegir obviamente
#Hay dos menus porque el 2 ya lo tenia antes del 1 y no queria hacer todo otra vez
def menu2():
    print(" 1) Agregar/Modificar denominacion")
    print(" 2) Eliminar denominacion")
    print(" 3) Cambiar maximo de cambio")
    print(" 4) Volver\n")

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
    print("\n --- Agregar/Modificar denominacion --- \n")
    #Muestra las denominaciones actuales ordenandolas con sorted para que se vean bien bonitas
    print(f" Denominaciones actuales: {sorted(config['denominaciones'])}\n")
    
    
    #El int es para convertir el texto a numero   
    nueva = int(input(" Nueva denominacion: "))
    cantidad = int(input(" Cantidad disponible: "))
    if nueva <= 0:
        print(" La denominacion debe ser mayor a 0.")
    elif nueva in config['denominaciones']:
        #print(f" La denominacion {nueva} ya existe.")
        config['cantidades'][str(nueva)] = cantidad
        guardar_config(config)
        print(f" Cantidad {nueva} agregada.")
    else:
        config['denominaciones'].append(nueva) #Se agrega si es correcta
        config['cantidades'][str(nueva)] = cantidad
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
        del config['cantidades'][str(eli)]
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

def dar_cambio():
    limpiar_pantalla()
    config = cargar_config()
    print("--- Monedas disponibles ---")
    for moneda, cantidad in config["cantidades"].items():
        print(f'- {moneda} -> {cantidad}')
    cantidad = int(input("Cantidad de cambio: "))
    if cantidad > config['max_cambio']:
        print(f"\nLa cantidad debe ser menor o igual al cambio maximo: {config['max_cambio']}")
        input("\nPresiona una tecla para continuar...")
        return
    
    _, matriz = construir_tabla(config['denominaciones'],config['max_cambio'])
    #voraz primero
    print("\n**** Algoritmo voraz ****\n")
    resultado_vo, restante_vo = cambio_voraz(cantidad,config['denominaciones'],config['cantidades'])
    if resultado_vo:
        for moneda, usadas in sorted(resultado_vo.items(),reverse=True):
            print(f"{usadas} x {moneda}")

    if restante_vo == 0:
        print("\nCambio completado.")
    else:
        print(f"\nNo fue posible completar el cambio. \nFaltan {restante_vo} pesos.")

    print("\n**** Backtraking ****\n")
    resultado_bt, restante_bt = cambio_backtracking(cantidad,config['denominaciones'],config['cantidades'],matriz)
    if resultado_bt:
        print("\nSolucion encontrada:\n")
        for moneda, usadas in sorted(resultado_bt.items(),reverse=True):
            print(f"{usadas} x {moneda}")
    
    if restante_bt == 0:
        print("\nCambio completado.")
        aplicar_cambio(config, resultado_bt)
    else:
        print("\nNo existe solución valida.")

    input("\nPresiona una tecla para continuar...")

#Para las opciones
def seleccion():
    limpiar_pantalla()
    menu1()
    opcion = input(" Selecciona una opcion: ")

    match opcion:
        case '1':
            mostrar_tabla()
        case '2':
            limpiar_pantalla()
            menu2()
            opcion = input(" Selecciona una opcion: ")
            match opcion:
                case '1':
                    agregar_denominacion()
                case '2':
                    eliminar_deniminacion()
                case '3':
                    cambiar_max()
                case '4':
                    seleccion()
        case '3':
            dar_cambio()
        case '4':
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
