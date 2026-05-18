#Backtracking
import numpy as np
def print_digits_impl(n,array,pos):
    if pos == n :
       print(array)
    else:
       array[pos] = 0
       print_digits_impl(n,array,pos+1) #Abandona el método

       array[pos] = 1
       print_digits_impl(n,array,pos+1)
    

def print_digits(n):
     array=np.zeros(n,dtype=int) #Crea un arreglo de 0 de n digitos de tipo entero
     print_digits_impl(n,array,0)


n = 3
print_digits(n)