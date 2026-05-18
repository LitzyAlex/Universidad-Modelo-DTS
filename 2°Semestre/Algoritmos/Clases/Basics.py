#Comentarios en python

#****************************
print('\n****************\n')
#****************************

x=7
print(x)
print(type(x))

x='Hola Python!'
print(x)
print(type(x))

#****************************
print('\n****************\n')
#****************************

print(type(7))
print(type(45.63))
print(type(7+3j))
print(type('texto'))
print(type(True))
print(type(None))

#****************************
print('\n****************\n')
#****************************

m=35
n=14
print(f'm={m}, n={n}') #F string, permite crear un string para imprimir e incrustar valores

m=35.3853845
n=14.0245842
print(f'm={m:.3f}, n={n}') #El :.3f define la cantidad de decimales que se imprimen

#****************************
print('\n****************\n')
#****************************
#Operadores aritmeticos, resultado numerico
x=19
y=6
print(x+y)
print(x-y)
print(x*y)
print(x/y)
print(x//y) #Resultado division sin decimales
print(x%y) #Residuo
print(x**y) #Exponente 19^6

#****************************
print('\n****************\n')
#****************************
#operadores relacionales, resultado boleano
x=19
y=6
print(x>y)
print(x<y)
print(x>=y)
print(x<=y)
print(x==y)
print(x!=y)

#****************************
print('\n****************\n')
#****************************
#Operadores logicos, resultado boleano 
x=True
y=False
print(x and y)
print(x or y)
print(not x)

#****************************
print('\n****************\n')
#****************************

x=15
y=24
print(x>10 and y<50)
print((x>10) and (y<50))

#****************************
print('\n****************\n')
#****************************

x=9
if x>15:
    print('Se cumple la 1ra condicion')

if x>=5 and x<=10:
    print('Se cumple la 2da condicion')

if 5<= x <=10:
    print('Se cumple la 3ra condicion')

#****************************
print('\n****************\n')
#****************************

x=15
y=7

if x>y:
    mayor=x
else:
    mayor=y

print(mayor)

mayor=x if x>y else y
print(mayor)

#****************************
print('\n****************\n')
#****************************

x=15
y=73
z=25
mayor=x if x>y and x>z else (y if y>z else z )
print(mayor)

#****************************
print('\n****************\n')
#****************************

x=0

if x>0:
    print('x es positivo')
elif x==0:
    print('x es cero')
else:
    print('x es negativo')

#****************************
print('\n****************\n')
#****************************

day=6
match day: #Switch
    case 1:
        print('Lunes')
    case 2:
        print('Martes')
    case 3:
        print('Miercoles')
    case 4:
        print('Jueves')
    case 5:
        print('Viernes')
    case 6:
        print('Sabado')
    case 7:
        print('Domingo')
    case _:
        print('Dia invalido')

#****************************
print('\n****************\n')
#****************************

day=6
match day: #Switch
    case 1|2|3|4|5:
        print('Dia de trabajo')
    case 6|7:
        print('Dia de descanso')
    case _:
        print('Dia invalido')

#****************************
print('\n****************\n')
#****************************

day=6
month=11
match day: #Switch
    case 1|2|3|4|5 if 1<= month <=11:
        print('Dia de trabajo')
    case 6|7 if 1<= month <=11:
        print('Dia de descanso')
    case 1|2|3|4|5|6|7 if month==12:
        print('Vacaciones')
    case _:
        print('Dia invalido')

#****************************
print('\n****************\n')
#****************************

i=1
while i<8:
    print(i)
    i+=1

#****************************
print('\n****************\n')
#****************************

i=1
while i<8:
    if i==5:
        break
    print(i)
    i+=1

#****************************
print('\n****************\n')
#****************************

i=1
while i<8:
    if i==5:
        i+=1
        continue
    print(i)
    i+=1

#****************************
print('\n****************\n')
#****************************

for c in 'Hola python':
    print(c)

#****************************
print('\n****************\n')
#****************************

texto='Hola mundo'
for i in range(len(texto)): #len es para saber la longitud, range es un secuenciador: objeto capaz de generar una secuencia numerica
    print(texto[i])

#****************************
print('\n****************\n')
#****************************

for i in range(8):
    print(i)

#****************************
print('\n****************\n')
#****************************

for i in range(5,15): #Numero donde empieza y termina
    print(i)

#****************************
print('\n****************\n')
#****************************

for i in range(5,15,3): #Numero donde empieza, termina y paso
    print(i)

#****************************
print('\n****************\n')
#****************************

for i in range(5,15):
    if i==10:
        break
    print(i)
else:
    print("Ciclo finalizado") #Si el ciclo del for se completa se escribe el else, no cuenta si se rompe

#****************************
print('\n****************\n')
#****************************

for i in range(5,15):
    print(i)
else:
    print("Ciclo finalizado")

#****************************
print('\n****************\n')
#****************************

for i in range(5,15):
    if i==10:
        continue #Reinicia la iteración, aqui por ejemplo no se escribe el 10
    print(i)

 #****************************
print('\n****************\n')
#****************************

lista=[5,4,3,6,1,2]
n=3
for i in range(len(lista)-n+1):
    print(i)


for i in range(n):
    print(i)
