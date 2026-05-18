 

def factorial_I(n):
 factor=1
 for i in range(1,n+1):
  factor=factor*i
 return factor 

for i in range(6):
 print(f'!{i}={factorial_I(i)}')  #Todo lo que esta con {} se escribe su valor

 #****************************
print('\n****************\n')
#****************************

def factorial_R(n):
 if n==0:
  return 1
 else:
  return n*factorial_R(n-1)
 
for i in range(6):
 print(f'!{i}={factorial_I(i)}') 

 #****************************
print('\n****************\n')
#****************************

def Fibonacci(n):
 if n==0 or n==1:
  return 1
 f_1=1
 f_2=1
 for i in range(2,n+1):
   f=f_1+f_2
   f_2=f_1
   f_1=f
 return f

for i in range(8):
 print(f'fib{i}={Fibonacci(i)}') 

 #****************************
print('\n****************\n')
#****************************

def FibonacciR(n):
 if n==0 or n==1:
  return 1
 return FibonacciR(n-1)+FibonacciR(n-2)

for i in range(8):
 print(f'fib{i}={FibonacciR(i)}') 
 