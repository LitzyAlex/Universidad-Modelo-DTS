
def merge_sort(arr):
    if len(arr) <= 1:   #Ya esta ordenada
        return arr
    
    n=len(arr)
    mid = n // 2   #Va dividiendo a la mitad

    izquierda = merge_sort(arr[:mid])  # : -> se llama slicing aqui agarra del primer numero hasta mid-1
    derecha = merge_sort(arr[mid:])    # aqui agarra de mid hasta n-1

    return merge(izquierda, derecha)  #Vuelve a la anterior accion que se interrumpio


def merge(izquierda, derecha):
    resultado = []
    i = 0
    j = 0

    while i < len(izquierda) and j < len(derecha):
        if izquierda[i] < derecha[j]:
            resultado.append(izquierda[i])
            i += 1
        else:
            resultado.append(derecha[j])
            j += 1

    # agregar lo que sobra
    while i < len(izquierda):
        resultado.append(izquierda[i])
        i += 1

    while j < len(derecha):
        resultado.append(derecha[j])
        j += 1

    return resultado


arr=[12,1,34,5,3]
print(merge_sort(arr))
