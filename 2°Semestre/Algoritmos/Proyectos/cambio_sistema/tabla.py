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
#Cantidad es el cambio que se quiere entregar, denominacion es lista, cantidad diccionario con cantidad de monedas que hay
def cambio_voraz(cantidad,denominaciones,cantidades):
    denoms = sorted(denominaciones, reverse=True) #Para que este de mayor a menor
    resultado = {} #Diccionario vacio
    restante = cantidad #guarda el dinero que falta

    for moneda in denoms: #Recorre cada denominacion
        #El str convierte la moneda a texto
        disponibles = cantidades.get(str(moneda), 0) #Obtiene cuantas monedas de ese valor existen
        necesarias = restante // moneda #Calcula cuantas monedas de ese valor serían necesarias para cubrir el restante
        usar = min(necesarias, disponibles) #Se toma el minimo entre las monedas que necesito y las que tengo
        #De las necesarias solo puedo agarrar el minimo de las que tengo, si ocupo 2 y tengo 1 solo agarro 1
        if usar > 0: #i hay algo que usar
            resultado[moneda] = usar #Guarda las monedas de esa denominación que se utilizaron
            restante -= usar * moneda #REsta del cambio pendiente lo que ya se utilizo para tener un nuevo restante
    return resultado, restante

def cambio_backtracking(cantidad, denominaciones, cantidades,matriz):
    candidatos_por_columna = {} #crea un diccionario donde se guardan las mejores monedas para cada cantidad posible
    #Algo como=   1:[1], 2:[2,1], 3:[2,1]
    for cambio in range(1, cantidad + 1): #Recorrre todas las cantidades dependiendo de la cantidad que se solicite
        candidatos = [] #Lista temporal para guardar los candidatos 
        for i, moneda in enumerate(denominaciones): #recorre las denominaciones
            if moneda > cambio: #Si la moneda es mayor al cambio que ocupamos no nos sirve
                continue

            valor = matriz[i][cambio] #Recupera el valor de la fila. fila=0 columna=1 : 1
            if valor == float('inf'): #Si esta inf en la matriz se ignora ahi
                continue

            candidatos.append((valor, moneda))  #Guarda el candidato (numero de monedas minima,denominacion)

        candidatos.sort(key=lambda x: x[0]) #Ordena los candidatos de menor a mayor segun el primer valor de la tupla
        candidatos_por_columna[cambio] = [moneda for _, moneda in candidatos] #Guarda solo el valor de la moneda de candidatos

    resultado = {} #Se guarda la solución encontrada
    #BACKTRACKING
    def bt(restante):
        # Caso base c:
        if restante == 0: #Si ya no queda cambio
            return True
        
        print(f"\nCambio restante: {restante}") #Para decir cuanto cambio me queda
        candidatos = candidatos_por_columna.get(restante, []) #Busca las mejores monedas para el valor restante
        for moneda in candidatos: #Se intenta cada posibilidad
            disponibles = cantidades.get(str(moneda), 0) #Obtiene cuantas monedas de esa existen
            usadas = resultado.get(moneda, 0) #Cuantas de esta moneda ya se usaron en la rama
            print(f"Intentando {moneda}. Restante={restante}") #Mostrar la moneda de ahora y el retsante de cambio que me falta

            if usadas >= disponibles: #Si ya gaste todas las monedas de esa que existian
                print(f"No hay suficientes monedas de {moneda}. Probando otra opcion.\n")
                continue

            resultado[moneda] = usadas + 1 #Se agrega 1 moneda mas que es la que se va a ocupar agorita mismo
            nuevo_restante = restante - moneda #Para saber cuanto me queda despues de ocupar la moneda
            print(f"Tomo {moneda}. Nuevo restante = {nuevo_restante}")

            if bt(nuevo_restante): #Volver a llamar a la funcion
                return True

            print(f"Fallo la rama con {moneda}. Hacia atraaas") #Fallo, debe retroceder a la anterior "rama"
 
            # Retroceder
            resultado[moneda] -= 1 #Quita la moneda que habia agregado
            if resultado[moneda] == 0: #Si se queda en 0 eliminar ese resultado completamente
                del resultado[moneda]

        return False #No funciono nada
    exito = bt(cantidad) #Otra vez exito xd
    if exito:
        return resultado, 0

    return {}, cantidad #NO ENCONTRO UNA SOLUCION

#config contiene la información actual, resultado indica las monedas que se usaron
def aplicar_cambio(config, resultado):
    for moneda, usadas in resultado.items(): #moneda ("5"), usada(cantidad)
        config['cantidades'][str(moneda)] -= usadas   #Resta las usadas 
    guardar_config(config) #Configura el json con las nuevas cantidades

#FORMATOS
#Lo agarre de lo del horario
def format_tabla_cambio(denominaciones, max_cambio):
    TAM_BLOQUE = 30 #Es que me di cuenta que si pongo cambio muy grande explota el cli
    denoms, matriz = construir_tabla(denominaciones, max_cambio)
    lineas = [] #Aqui se guarda la tabla
    for inicio in range(1, max_cambio + 1, TAM_BLOQUE):
         #Llamando a la funcion de arriba
        fin = min(inicio + TAM_BLOQUE - 1, max_cambio)
        columnas = list(range(inicio, fin + 1))

        ANCHO_DENOM = max(len('Denominacion'), max(len(str(d)) for d in denoms)) #El ancho de la primera columna
        ANCHO_COL = max(len(str(max_cambio)), 3) #Ancho de las demas columnas

        anchos = [ANCHO_DENOM] + [ANCHO_COL] * len(columnas) #Guarda todos los anchos en una lista
        total = sum(anchos) + len(anchos) + 1  #Es el total de la tabla, su tamaño
        separador = '+' + '+'.join('-' * a for a in anchos) + '+' #Hace el separador de los anchos

    #Solo es funcion para centrar el texto segun el ancho
        def c(texto, ancho): 
            return str(texto).center(ancho)

        
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
                piezas.append(c(str(val) if val != float('inf') else '?', anchos[j + 1])) #Aqui solo checa si el valor es infinito, y lo pone si no lo es, se agrega
            #Complicado, recorre columnas y solo toma las validas, escoge la mayor
            max_pos = max((col for col in columnas if matriz[i][col] != float('inf')), default=0)
            lineas.append('|' + '|'.join(piezas) + '|') #Pone las paredes/separadores

        lineas.append(separador)
        lineas.append('Denominacion'.center(anchos[0])) #Agrega el texto centrado al final, listoooo

    return '\n'.join(lineas)
