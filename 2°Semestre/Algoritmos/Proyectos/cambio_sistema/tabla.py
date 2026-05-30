import json
import os

#JSON
#Constante para la ruta del json, mas facil, copie la ruta de tareas anteriores xd
CONFIG_FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'cambio_config.json')

#Gracias chati por, y octavio
def cargar_config(): #Lee el json
    with open(CONFIG_FILE, 'r', encoding='utf-8') as f:  #Abre el json en modo lectura
        return json.load(f)  #Y lo convierte en diccionario

def guardar_config(config): #Guarda edicion del json
    with open(CONFIG_FILE, 'w', encoding='utf-8') as f: #Abre json en modo escritura
        json.dump(config, f, indent=2, ensure_ascii=False)  #convierte el diccionario a json


# Las filas son las denominaciones disponibles (1, 2, 5, 10)
# Las columnas son los valores de cambio del 1 al max_cambio
# Cada celda dice cuantas monedas minimas se necesita para dar ese cambio
# usando las denominaciones de esa fila y las anteriores
# Logica de cada celda matriz[i][c]:
# c == 0  -> 0 monedas (dar cambio de cero no cuesta nada)
# c < denominacion -> heredamos el valor de la fila de arriba (no podemos usar esta moneda)
# c >= denominacion -> min(fila_arriba,1+matriz[fila_actual][c-denominacion] )

def construir_tabla(denominaciones, max_cambio):
    denoms = sorted(denominaciones) #Ordenamos las denominaciones
    INF = float('inf') #Es para representar el infinito
    n = len(denoms) #Cantidad de monedas que hay de cambio(no cuantas hay de cada una, solo las que hay)

    #Se hace una matriz(como dice la variable) llena de inf de max + 1 porque ocupamos uno mas
    matriz = [[INF] * (max_cambio+1) for _ in range(n)]
    #Esto se elimina despues pero a todos los cambios de 0 les pone 0
    for i in range(n):
        matriz[i][0] = 0

    #El enumerate da un indice y un valor
    #Un ejemplo para dejarlo claro:
    #i=0 denom=1
    #i=1 denom=2
    #i=2 denom=5
    for i, denom in enumerate(denoms):
        for c in range(1, max_cambio+1): #Recorre todos los cambios posibles
            if i > 0: #practicamente todas 
                #Copio el valor de la fila de arriba
                matriz[i][c] = matriz[i-1][c] 
            #Primero checamos si la moneda cabe(osea si el cambio es de 3 y la moneda es 5 por ejemplo)
            #El otro es para saber si puedo formar el cambio restante, ejemplo rapido:
            #c=6  y denom=5,   6-5=1, y revisa esa parte de la matriz[i][1], si eso existe si puedo ocupar la moneda
            if c >= denom and matriz[i][c-denom] != INF:
                #Esto calcula cuantas monedas ocupo si decidio agarrar este cambio
                #Otro ejemplo bonito: como arriba dijimos tengo c=6  y denom=5
                #usar = 1+ matriz[i][1]   y matriz[i][1]=1, entonces usar=1+1=2
                #Representa que ocupas 2 monedas, una de 5, luego una de 1
                usar = 1+ matriz[i][c-denom]
                if usar < matriz[i][c]: #Si usar es mejor que la solución que anda ahi(que deberia porque solo tenia monedas de 1)
                    matriz[i][c] = usar #Guarda la nueva solución ahi

    return denoms, matriz

#Algoritmooooos
#El voraz=
def cambio_voraz(cantidad,denominaciones,cantidades):
    denoms = sorted(denominaciones, reverse=True) #Para que este de mayor a menor
    resultado = {}
    restante = cantidad

    for moneda in denoms:
        disponibles = cantidades.get(str(moneda), 0)
        necesarias = restante // moneda
        usar = min(necesarias, disponibles)
        if usar > 0:
            resultado[moneda] = usar
            restante -= usar * moneda
    return resultado, restante

def aplicar_cambio(config, resultado):
    for moneda, usadas in resultado.items():
        config['cantidades'][str(moneda)] -= usadas

    guardar_config(config)

#FORMATOS
#Lo agarre de lo del horario
def format_tabla_cambio(denominaciones, max_cambio):
    denoms, matriz = construir_tabla(denominaciones, max_cambio) #Llamando a la funcion de arriba
    columnas = list(range(1, max_cambio+1)) #Encabezados de las columnas

    ANCHO_DENOM = max(len('Denominacion'), max(len(str(d)) for d in denoms)) #El ancho de la primera columna
    ANCHO_COL = max(len(str(max_cambio)), 3) #Ancho de las demas columnas
    ANCHO_MAX = len('Max. Cambio') #La comuna final

    anchos = [ANCHO_DENOM] + [ANCHO_COL] * len(columnas) + [ANCHO_MAX] #Guarda todos los anchos en una lista
    total = sum(anchos) + len(anchos) + 1  #Es el total de la tabla, su tamaño
    separador = '+' + '+'.join('-' * a for a in anchos) + '+' #Hace el separador de los anchos

    #Solo es funcion para centrar el texto segun el ancho
    def c(texto, ancho): 
        return str(texto).center(ancho)

    lineas = [] #Aqui se guarda la tabla
    lineas.append('=' * total)
    lineas.append('TABLA DE CAMBIO MINIMO'.center(total))
    lineas.append('=' * total)

    enc = [''] + [str(col) for col in columnas] + ['Max. Cambio'] #Crea los encabezados de las columnas
    lineas.append('|' + '|'.join(c(e, a) for e, a in zip(enc, anchos)) + '|') #Las agrega con las lineas de separación
    lineas.append(separador) #Y luego el separador

    for i, denom in enumerate(denoms):  #Recorre cada fila de la matriz, i numero de fila, denom moneda actual
        piezas = [c(denom, anchos[0])] #Se crea lista, se guarda el denom centrado
        for j, col in enumerate(columnas): #Recorre las columnas
            val = matriz[i][col] #Guarda el valor de esa celda
            piezas.append(c(str(val) if val != float('inf') else '?', anchos[j + 1])) #Aqui solo checa si ek valor es infinito, y lo pone si no lo es, se agrega
        #Complicado, recorre columnas y solo toma las validas, escoge la mayor
        max_pos = max((col for col in columnas if matriz[i][col] != float('inf')), default=0)
        #y agrega el valor máximo alcanzable de la fila 
        piezas.append(c(max_pos, anchos[-1]))
        lineas.append('|' + '|'.join(piezas) + '|') #Pone las paredes/separadores

    lineas.append(separador)
    lineas.append('Denominacion'.center(anchos[0])) #Agrega el texto centrado al final, listoooo

    return '\n'.join(lineas)
