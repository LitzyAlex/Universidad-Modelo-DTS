#Dadas dos cadenas de texto, implementa una funcion que determine si la segunda cadena es una 
#anagrama de la primera. Un anagrama es una palabra o frase que resulta de la transposicion
#de letras de otra palabra o frase. Dicho de otra forma, una palabra es anagrama de otra si las
#dos tienen las mismas letras, con el mismo numero de apariciones, pero en un orden diferente.

def valid_anagram(palabra1,palabra2):
 palabra2_lista=list(palabra2)                #convierte la palabra en una lista
 for i in range(len(palabra1)):               #Lee cada letra de la palabra
    for j in range(len(palabra2_lista)):       #Por cada letra de mi lista
      if palabra1[i]==palabra2_lista[j]:        #Si la letra de mi primera palabra coincide
        palabra2_lista[j]=None                 #Quito esa letra de la palabra2                      
        break                                  #Rompe el ciclo
    else:                                      #Este solo se ejecuta si el for no sale con el break
      return False                             #No se encuentra la letra
 return True                                   #Si se encuentran todas las letras se envia verdadero


print(valid_anagram("", "")) # true
print(valid_anagram("aaz", "zza")) # false
print(valid_anagram("anagram", "nagaram")) # true
print(valid_anagram("rat", "car")) # false
print(valid_anagram("awesome", "awesom")) # false
print(valid_anagram("qwerty", "qeywrt")) # true
print(valid_anagram("texttwisttime", "timetwisttext")) # true

#****************************
print('\n****************\n')
#****************************

#Implementa una funcion llamada count unique values(), que acepte una lista ordenada, y
#realice el conteo de los valores unicos en la lista. La lista puede contener numeros negativos,
#sin embargo, siempre estara ordenado.

def count_unique_values(unicos):
  if len(unicos)==0:                    #Si el tamaño de la lista es 0(vacia), me devuelve 0
    return 0
  cont=1                                #Contador para los numeros unicos
  for i in range(1,len(unicos)):        #Para i en un rango empezando por 1 hasta el tamaño de mi lista
      if unicos[i]!=unicos[i-1]:         #Si el numero anterior es diferente al que sigue ya cambio
        cont=cont+1                      #Entonces el contador aumenta en 1 porque es otro numero
  return cont                            #Devuelvo el numero de numeros unicos

print(count_unique_values([1,1,1,1,1,2])) # 2
print(count_unique_values([1,2,3,4,4,4,7,7,12,12,13])) # 7
print(count_unique_values([])) # 0
print(count_unique_values([-2,-1,-1,0,1])) # 4

#****************************
print('\n****************\n')
#****************************

#Implementa una funcion llamada max subarray sum(), que acepte una lista de enteros y un
#numero n. La funcion debera calcular la maxima suma de n elementos consecutivos de la lista.


def max_subarray_sum(lista,n):
  if len(lista)==0:
    return None
  suma_max=float('-inf')              #Se pone como si fuera un - infinito, o un numero muy pequeño
  for i in range(len(lista)-n+1):    #esto calcula las posiciones iniciales que se pueden tomar sin pasarse de lo que se tiene, el +1 es para que agarre igual el numero final
    suma=0                           #La suma interna empieza en 0
    for j in range(n):               #En un rango de n(numeros consecutivos)
      suma=suma+lista[i+j]            #Se suma el numero que este en el indice indicado
    if suma>suma_max:                 #Si esa suma supera la suma maxima anterior
      suma_max=suma                   #Se guarda como la suma maxima
  return suma_max     

print(max_subarray_sum([1,2,5,2,8,1,5], 2)) # 10
print(max_subarray_sum([1,2,5,2,8,1,5], 4)) # 17
print(max_subarray_sum([4,2,1,6], 1)) # 6
print(max_subarray_sum([4,2,1,6,2], 4)) # 13
print(max_subarray_sum([], 4)) # None

#****************************
print('\n****************\n')
#****************************

#Implementa una funcion llamada procesar() que reciba como parametro una lista de enteros,
#y que devuelva una lista que contenga: a) el promedio de los numeros positivos, b) el promedio
#de los numeros negativos, c) el conteo de ceros, y d) una lista con los numeros primos presentes
#en la lista original.

def procesar(enteros):
 suma_pos=0
 cont_pos=0
 suma_neg=0
 cont_neg=0
 cont_ceros=0
 primos=[]
 for i in range(len(enteros)):                  #Saber si es positivo y sumarlos, contarlos
   if enteros[i]>0:
     suma_pos=suma_pos+enteros[i]
     cont_pos=cont_pos+1
   if enteros[i]<0:                             #Saber si es negativo y sumarlos, contarlos
     suma_neg=suma_neg+enteros[i]
     cont_neg=cont_neg+1
   if enteros[i]==0:                            #Saber si es 0 y contarlo
     cont_ceros=cont_ceros+1
   if enteros[i]>1:                             #Si es mayor a 1 para empezar a saber si es primo
    primo=True
    for j in range(2,enteros[i]):               #rango de 2 al numero
      if (enteros[i]%j==0):                     #Dividirlo entre los numeros y si es divisible no es primo
        primo=False
        break
    if primo:                                   #Si es primo guardarlo en una lista
      primos.append(enteros[i])
 prom_pos=suma_pos/cont_pos if cont_pos> 0 else 0     #Sacar promedios si el conteo es mayor a 0
 prom_neg=suma_neg/cont_neg if cont_neg> 0 else 0

 return[prom_pos,prom_neg,cont_ceros,primos]       #Devolver todos los valores

print(procesar([-5, 3, 0, 7, 14, -6, 3, 0, -2, 3, 8]))
# [ 6.333333333333333, -4.333333333333333, 2, [ 3, 7, 3, 3 ] ]
print(procesar([2, 0, 0, 1, -4, -3, 0, 5, 11, -7, 9]))
# [ 5.6, -4.666666666666667, 3, [ 2, 5, 11 ] ]



