def jump_search(data, element):
    n = int(len(data) ** 0.5)  # tamaño del salto
    Ibloque = 0   #Inicio del bloque
    Fbloque = n   #Fin del bloque

    # Si elemento se encuentra en el primer indice, retornar indice 0
    if data[0] == element: return 0

    # Saltos 
    #Mientras Fbloque sea menor al array y el valor en el indice Fbloque-1 sea menor al valor objetivo
    while Fbloque < len(data) and data[Fbloque] < element:  
        Ibloque = Fbloque
        Fbloque += n

    # Búsqueda lineal en el bloque
    # Cuando una de las condiciones del while no se cumpla, entonces sabremos que el bloque donde se hara la busqueda lineal
    # inicia en Ibloque y acaba en Fbloque
    for i in range(Ibloque,Fbloque):
        if data[i] == element:
            return i
    return -1


data = [2,5,8,12,16,23,38,56,72,91]
element = 91
resultado = jump_search(data, element)
if (resultado >= 0):
    print(f'Valor {element} encontrado en el indice {resultado}')
else:
    print('Elemento no encontrado')

#****************************
print('\n**********************************************************************************************************************\n')
#****************************

def interpolation_search(data,element):
    i = 0
    j = len(data) - 1

    # Por si por si
    while i <= j:
        medio = int(i + ((j - i) * (element - data[i])) / (data[j] - data[i]))   #Forumula para sacar el medio               
        if data[medio] == element:
            return medio
        elif data[medio] > element:
            j = medio - 1
        else:
            i = medio + 1
    return -1 # Si no encunetra nada retornar -1

data = [10,14,19,26,27,31,33,35,42,44]
element = 44
resultado = interpolation_search(data, element)
if (resultado >= 0):
    print(f'Valor {element} encontrado en el indice {resultado}')
else:
    print('Elemento no encontrado')


data = [1,3,5,6,8,10,11,14]
element = 11
resultado = interpolation_search(data, element)
if (resultado >= 0):
    print(f'Valor {element} encontrado en el indice {resultado}')
else:
    print('Elemento no encontrado')


#****************************
print('\n**********************************************************************************************************************\n')
#****************************

def exponential_search(data,element):
    k = 0
    i = 0

    # K crece exponencialmente teniendo como limite la cantidad de elementos del array
    for k in range(len(data)):
        if i >= len(data): break # i al ser el valor que crece de manera muy rapida, primero verificamos si no es mayor a los elementos del arr
        if data[i] == element: return i # Se encontro el valor objetivo
        if data[i] > element: break # Si el valor en el indice i es mayor al elementos, significa que ya nos pasamos
        i = pow(2,k)     #Es para sacar la potencia, pow(base,exponente)
    
    e = i // 2
    j = i - 1
    if j >= len(data): j = len(data)-1 # Si j es muy grande (se sale del arreglo) entonces tomaremos el valor len(data)-1
    while e <= j:
        medio = (e + j) // 2 # El nuevo valor medio                      
        if data[medio] == element:
            return medio
        elif data[medio] > element:
            j = medio - 1
        else:
            e = medio + 1
    return -1

data = [6,11,19,24,33,54,67,81,94,99]
element = 99
resultado = exponential_search(data, element)
if (resultado >= 0):
    print(f'Valor {element} encontrado en el indice {resultado}')
else:
    print('Elemento no encontrado')
