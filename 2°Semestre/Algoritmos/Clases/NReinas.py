import numpy as np
import subprocess

subprocess.run('cls', shell=True)  #Para limpiar la pantalla al inicio

solution_counter=0

#Va imprimiendo las soluciones
def print_solution(n,board):
    global solution_counter
    solution_counter +=1
    print(f'\n-----Solucion #{solution_counter}-----\n{board}\n')  #Para debugear


def validation_board(n,board):
    #Validate rows
    for i in range(n):
        #print(f'\nValidando fila {i}...')
        sum=0
        for j in range(n):
            sum+=board[i,j]
            #print(f'({i},{j})')
        if sum > 1:
            return False
        
    #Validate L-R diagonals
    for i in range(n):
       # print(f'\nValidando diagonal inferior L-R {i}...')
        sum=0
        for j in range (n-i):
            sum += board[i+j,j]
            #print(f'({i+j},{j})')
        if sum > 1:
            return False

    for i in range(n):
        #print(f'\nValidando diagonal superior L-R {i}...')
        sum=0
        for j in range (n-i):
            sum += board[j,i+j]
           # print(f'({j},{i+j})')
        if sum > 1:
            return False
        

    #Validate R-L diagonals
    for i in range(n):
       # print(f'\nValidando diagonal inferior R-L {i}...')
        sum=0
        for j in range (n-i):
            sum += board[i+j,n-1-j]
           # print(f'({i+j},{n-1-j})')
        if sum > 1:
            return False
        
    for i in range(n):
       # print(f'\nValidando diagonal superior R-L {i}...')
        sum=0
        for j in range (n-i):
            sum += board[j,n-1-j-i]
           # print(f'({j},{n-1-i-j})')
        if sum > 1:
            return False

    return True








def n_queen_solver_impl(n,board,col):
    if col==n:
        print_solution(n,board)
        return   #Para que regrese la siguiente posición
    
    for row in range(n):
        board[row,col] = 1  #Las reinas seran 1, empieza intentando en el (0,0)
        if validation_board(n,board):      #Si la validacion sale bien se vuelve a llamar
            n_queen_solver_impl(n,board,col+1)

        board[row,col]=0    #No se puede poner la reina ahi
    


def n_queen_solver(n):
    board = np.zeros((n,n), dtype=int)
    col=0
    n_queen_solver_impl(n,board,col)



n=8
n_queen_solver(n)

#board = np.zeros((n,n), dtype=int)
#validation_board(n,board)