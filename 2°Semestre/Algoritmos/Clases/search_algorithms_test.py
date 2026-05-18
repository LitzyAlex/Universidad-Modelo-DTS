#Busqueda lineal
#import dtslib
#index= dtslib.linear_serch(data,element)     El mismo nombre

#import dtslib as dts
#index= dts.linear_serch(data,element)        Se puede abreviar

#from dtslib import linear_serch as ls        Cambia el nombre del metodo
#index= ls(data,element) 

#from dtslib import linear_serch              #Solo importo el linear_serach porque no se repite
#index= linear_serch(data,element) 

from dtslib import *                          #Importa todas las funciones

data=[1,0,-4,5,11,2,-9,3,17,6]
element=3;

index= linear_serch(data,element)      #Se llama a la funcion que esta en mi otro archivo
if(index >=0):
    print(f'Valor {element} encontrado en el indice {index}')
else:
    print('Elemento no encontrado')

#****************************
print('\n****************\n')
#****************************

class Student:
    def __init__(self,name,age,id):   #forma en la que se crea(inicializa) el objeto, aqui el estudiante. Siempre se pone self al inicio
        self.name=name              #self significa yo, es el objeto
        self.age=age
        self.id= id
    
    def __str__(self):
        return f'{self.name}, {self.age}, {self.id}'
    

#x=Student('Hola', 78, '793848')
#print(type(x))
#print(x.name)
#print(x.age)
#print(x.id)
    
data = [
    Student('Pedro Alvarez', 20, '816505'),
    Student('Juan Perez', 32, '563188'),
    Student('Martha Montejo', 45, '235474'),
    Student('Laura Cisneros', 37, '937125'),
    Student('Erick Parra', 18, '754638'),
]

#element= Student('Laura Cisneros', 37, '937125')
element=data[3]

index= linear_serch(data,element)      
if(index >=0):
    print(f'Valor {element} encontrado en el indice {index}')
else:
    print('Elemento no encontrado')

#****************************
print('\n****************\n')
#****************************

#element= linear_serch_obj(data,lambda s: s.id=='816505')     
element= linear_serch_obj(data,lambda s: s.age==32)  
if element!=None:
    print(f'Elemento encontrado: {element}')
else:
    print('Elemento no encontrado')

#****************************
print('\n****************\n')
#****************************
   
result= linear_serch_obj_all(data,lambda s: 20 <= s.age <=40)  
if len(result)>0:
    print(f'Elementos encontrado: ')
    for e in result:
     print(f'\t{e}')  #Imprimir cada uno de los elementos, el salto de linea es con \t
else:
    print('Elemento no encontrado')

#****************************
print('\n****************\n')
#****************************

data_ints=[1,0,-4,5,11,2,-9,3,17,6]

result= linear_serch_obj_all(data_ints,lambda e: e>=0)  
print(result)

#****************************
print('\n****************\n')
#****************************

result= test_if_exists(data_ints,lambda e: e==0)  
print(result)

#****************************
print('\n****************\n')
#****************************

result= test_if_all(data_ints,lambda e: e<20)  
print(result)

#****************************
print('\n****************\n')
#****************************

data=[1,3,7,8,11,12,18,21,25,29,31,42,55,78]
element=28

index= binary_search(data,element)      #Se llama a la funcion que esta en mi otro archivo
if(index >=0):
    print(f'Valor {element} encontrado en el indice {index}')
else:
    print('Elemento no encontrado')

#****************************
print('\n****************\n')
#****************************

data=[1,3,7,8,11,12,18,21,25,29,31,42,55,78]
element=78

index= binary_search_R(data,element)      #Se llama a la funcion que esta en mi otro archivo
if(index >=0):
    print(f'Valor {element} encontrado en el indice {index}')
else:
    print('Elemento no encontrado')

#****************************
print('\n****************\n')
#****************************
