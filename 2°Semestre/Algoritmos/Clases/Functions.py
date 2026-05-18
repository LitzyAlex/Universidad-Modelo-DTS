#****************************
print('\n****************\n')
#****************************

def suma(a,b,c):
    return a+b+c

print(type(suma)) #Function, son tipos de contenido

print(suma(2,3,5)) #Poner nombre la funcion y luego los valores

#****************************
print('\n****************\n')
#****************************

def printName(firstName='John',lastName='Doe'):  #Parametros por omision
    print(f'{lastName}, {firstName}') #String

printName()
printName('Jaime')
printName('Jaime', 'Estrella') #Parametros por posicion, se respeta el orden de como se toman los valores
printName(lastName='Estrella', firstName='Jaime') #Parametros por nombre
printName(lastName='Estrella') #Solo se puede cambiar por nombre si quieres cambiar otros que no sean el primero
print(printName())#Imprime lo que devuelve

#****************************
print('\n****************\n')
#****************************

x=10 #Variable global

def fx():
    x=15 #Variable local
    print(x)

fx()
print(x)

#****************************
print('\n****************\n')
#****************************

x=10 

def fx():
    global x #Para igual cambiar la variable global
    x=15 
    print(x)

fx()
print(x)

#****************************
print('\n****************\n')
#****************************

def hyp(a,b): 
    return (a**2 + b**2)**(1/2)

print(type(hyp))
print(hyp(3,4))

fx= lambda a,b: (a**2 + b**2)**(1/2) #Funciones sin nombre, forma breve de escribir un metodo
print(type(fx))
print(fx(3,4))

#****************************
print('\n****************\n')
#****************************

def apply_op(a,b,op): #Este metodo tiene como objetivo aplicar la operacion op en los operandos a y b
    return op(a,b)

print(apply_op(4,6, lambda a,b:a+b))
print(apply_op(4,6, lambda a,b:a**b))
print(apply_op(4,6, hyp)) #Puede aceptar cualquier metodo que cumpla con los valores que necesita

#****************************
print('\n****************\n')
#****************************


