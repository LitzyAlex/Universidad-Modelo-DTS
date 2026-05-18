
#Busqueda lineal
def linear_serch(data,element):   #O(n)
    for i in range(len(data)):  #range(len(data)) es para el tamaño de la lista
        if data[i]== element:   #Si el elemento del indice es igual al elemento que se busca
            return i            #Se devuelve el indice
    return -1                   #Debe ser -1 y no 0 para que no haya confuciones


#Se pasa una lista y una condicion, el primer elemento que cumpla con esa condicion es el elemento qe se devuelve
def linear_serch_obj(data,condition):   #bool condition(element) le dices al programa el metodo para buscar
    for i in range(len(data)):  
        if condition(data[i]):          #Si la condicion es verdadera se decuelve el elemento que cumple esa condicion
            return data[i]            
    return None  

def linear_serch_obj_all(data,condition):  #Devuelve una lista de todos los elementos que cumplen con la condicion
    result=[]                              #lista vacia
    for i in range(len(data)):  
        if condition(data[i]):         
            result.append(data[i])         #append es para agregar el objeto a la lista
    return result  


#bool test_if_exists(...)
def test_if_exists(data, condition):  #Solo me dice si el elemento existe o no segun una condicion
    for i in range(len(data)):  
        if condition(data[i]):         
         return True            
    return False 

#def test_if_all(data, condition):   #Checa que todos lo elementos cumplan una condicion
 #   for i in range(len(data)):  
  #      if not condition(data[i]):         
   #      return False            
    #return True 

def test_if_all(data, condition):   #Checa que todos lo elementos cumplan una condicion
    for e in data:  
        if not condition(e):         
         return False            
    return True 

#Busqueda binaria (binary search)

def binary_search(data,element): #O(log n)
    i=0
    j=len(data)-1
    while i<= j:
        medio=(i+j)//2                      #Divide el array 
        if data[medio]==element:
            return medio
        elif data[medio]>element:
            j=medio-1
        else:
            i=medio+1
    return -1




def binary_search_R(data,element):  #Para hacerlo mas amigable, recursividad indirecta
    def _binary_search_R(data,element,i,j): #O(log n) recursivo, una función interna, solo funcina aca
     if i>j:
        return -1
     medio=(i+j)//2                      #Divide el array 
     if data[medio]==element:
        return medio
     elif data[medio]>element:
       return _binary_search_R(data,element,i,medio-1)
     else:
        return _binary_search_R(data,element,medio+1,j)
    
    i=0
    j=len(data)-1
    return _binary_search_R(data,element,i,j)

#*************************************************************************

def bubble_sort(data):      #O(n^2)
   n=len(data)
   for i in range(n-1):
      sorted = True
      for j in range(n-1-i): #Para que llegue al ultimo valor
         if (data[j]>data[j+1]):
            data[j],data[j+1]= data[j+1],data[j]  #intercambio de tuplas
            sorted= False

      if sorted:
         print(f'Pasada #{i} => se detecto ya ordenado')
         return

      print(f'Pasada #{i} => {data}')


#Selection sort
def selection_sort(data):    #O(n^2)
   n = len(data)
   for i in range(n-1):
      mini = i
      for j in range(i+1,n):
         if data[j]< data[mini]:
            mini= j

      data[i], data[mini]= data[mini], data[i]

      print(f'Pasada #{i} => {data}')

#Insertion sort
def insertion_sort(data):  #O(n^2), mejor caso O(n)
   n = len(data)
   for i in range(1,n):
      e=data[i]
      j = i-1
      while j>= 0 and data[j] > e:
         data[j+1]=data[j]
         j-=1

      data[j+1]=e
      print(f'Pasada #{i} => {data}')

#Quick sort

def quicksort(data):
   def partition(data,low,high):
    pivote=data[high]
    i= low-1
    for j in range(low,high):
      if data[j] <= pivote:
         i+=1
         data[i],data[j]=data[j],data[i]

    data[i+1],data[high]=data[high],data[i+1]
    return i+1
   
   def quicksort_impl(data,low,high):      #O(n log n)
    if low<high:                         #Para que no se intercambien i y j
      pivote= partition(data,low,high)  #Busca el pivote, la mitad de la lista(idealmente), y divide los numero en menores y mayores, ya esta ordenado

      quicksort_impl(data,low,pivote-1)
      quicksort_impl(data,pivote+1,high)

   quicksort_impl(data,0,len(data)-1)