import matplotlib.pyplot as plt
import matplotlib.animation as animation
import random
 

#*********************************************************************
def bubble_sort(data):
    n = len(data)
 
    for i in range(n-1): # pasadas
        for j in range(n-1-i): # recorrido de intercambios
            if (data[j] > data[j+1]):
                data[j], data[j+1] = data[j+1], data[j] # intercambio basado en tuplas
                yield data   #Generador de secuencias, la funcion recuerda donde se quedo, continua donde se habia quedado

            
#*********************************************************************
def selection_sort(data):    #O(n^2)
   n = len(data)
   for i in range(n-1):
      mini = i
      for j in range(i+1,n):
         if data[j] < data[mini]:
            mini= j

      data[i], data[mini]= data[mini], data[i]
      yield data

#*********************************************************************     
def insertion_sort(data):  #O(n^2), mejor caso O(n)
   n = len(data)
   for i in range(1,n):
      e=data[i]
      j = i-1
      while j>= 0 and data[j] > e:
         data[j+1]=data[j]
         yield data
         j-=1

      data[j+1]=e
      yield data

#*********************************************************************   

def quicksort(data):
   def partition(data,low,high):
    pivote=data[high]
    i= low-1
    for j in range(low,high):
      if data[j] <= pivote:
         i+=1
         data[i],data[j]=data[j],data[i]
         yield data

    data[i+1],data[high]=data[high],data[i+1]
    yield data
    return i+1
   
   def quicksort_impl(data,low,high):      #O(n log n)
    if low<high:                         #Para que no se intercambien i y j
      pivote= yield from partition(data,low,high)  #Busca el pivote, la mitad de la lista(idealmente), y divide los numero en menores y mayores, ya esta ordenado

      yield from quicksort_impl(data,low,pivote-1)
      yield from quicksort_impl(data,pivote+1,high)

   yield from quicksort_impl(data,0,len(data)-1)
 
 #********************************************************************* 

data = list(range(1, 51))
random.shuffle(data)
 
fig, ax = plt.subplots()
bars = ax.bar(range(len(data)), data)
 
ax.set_title("Sort Visualization")
 
def update(data):
    for bar, val in zip(bars, data):
        bar.set_height(val)
 
ani = animation.FuncAnimation(
    fig,
    update,
    #frames=bubble_sort(data),
    #frames=selection_sort(data),
    #frames=insertion_sort(data),
    frames=quicksort(data),
    repeat=False,
    interval=100 #Que tan rapido va la animación
)
 
plt.show()