#Los metodos en python pueden ser objetos, y estos tienen atributos(variables miembro)
def factorialI(n):
    if not hasattr(factorialI,"counter"):  #Si factorialI no tiene la variable counter
        factorialI.counter = 0  #Se crea la variable counter, asignandole un valor

    factorialI.counter += 1   #FactorialI es un objeto que tiene asociado una variable contador

    factor = 1
    for i in range(1, n+1):
        factor *= i

    return factor

for i in range(7):
    print(f'!{i} = {factorialI(i)}')
print(f'\n# Llamadas -> {factorialI.counter}')

# ************************************************
print('\n*************************************\n')
# ************************************************

def factorialR(n):
    if not hasattr(factorialR,"counter"):  
        factorialR.counter = 0  

    factorialR.counter += 1

    if n == 0:
        return 1
    else:
        return n * factorialR(n-1)

for i in range(7):
    print(f'!{i} = {factorialR(i)}')
print(f'\n# Llamadas -> {factorialR.counter}')

# ************************************************
print('\n*************************************\n')
# ************************************************


def factorialDP(n):
    if not hasattr(factorialDP,"counter"):  
        factorialDP.counter = 0  

    factorialDP.counter += 1

    if not hasattr(factorialDP,"mem"):  
        factorialDP.mem={}            #Guarda un conjunto de parejas de datos(clave,valor), es un diccionario

    if n == 0:
        return 1
    elif n in factorialDP.mem:
        return factorialDP.mem[n]
    else:
        aux= n * factorialDP(n-1)
        factorialDP.mem[n] = aux
        return aux

for i in range(7):
    print(f'!{i} = {factorialDP(i)}')
print(f'\n# Llamadas -> {factorialDP.counter}')


# ************************************************
print('\n*************************************\n')
# ************************************************

def FibonacciI(n):
    if not hasattr(FibonacciI,"counter"):  
        FibonacciI.counter = 0  

    FibonacciI.counter += 1

    if n == 0 or n == 1:
        return 1
    
    f_2 = 1
    f_1 = 1
    for i in range(2, n+1):
        f = f_1 + f_2
        
        f_2 = f_1
        f_1 = f
    
    return f

for i in range(12):
    print(f'fib({i}) = {FibonacciI(i)}')

print(f'\n# Llamadas -> {FibonacciI.counter}')

# ************************************************
print('\n*************************************\n')
# ************************************************


def FibonacciR(n):
    if not hasattr(FibonacciR,"counter"):  
        FibonacciR.counter = 0  

    FibonacciR.counter += 1

    if n == 0 or n == 1:
        return 1

    return FibonacciR(n-1) + FibonacciR(n-2)

for i in range(12):
    print(f'fib({i}) = {FibonacciR(i)}')

print(f'\n# Llamadas -> {FibonacciR.counter}')

# ************************************************
print('\n*************************************\n')
# ************************************************

def FibonacciDP(n):
    if not hasattr(FibonacciDP,"counter"):  
        FibonacciDP.counter = 0  

    FibonacciDP.counter += 1

    if not hasattr(FibonacciDP,"mem"):  
        FibonacciDP.mem = {}

    if n == 0 or n == 1:
        return 1
    elif n in FibonacciDP.mem:
        return FibonacciDP.mem[n]

    aux = FibonacciDP(n-1) + FibonacciDP(n-2)
    FibonacciDP.mem[n] = aux
    return aux

for i in range(12):
    print(f'fib({i}) = {FibonacciDP(i)}')

print(f'\n# Llamadas -> {FibonacciDP.counter}')
