import numpy as np
import random
from animated_maze import animate_maze
 
#Segun google tener constantes es buena practica, pero se puden cambiar si es necesario
N               = 20     # <--- CAMBIA ESTE VALOR PARA EL TAMAÑO N x N
INITIAL_LIVES   = 5      # vidas iniciales
POISON_INTERVAL = 3      # cada cuantos movimientos hace daño el veneno
LIFE_CELL_HEAL  = 2      # vida que recupera una celda 'L'
DAMAGE_CELLS    = {'P', 'M'}   # celdas que quitan vida al entrar
LIFE_CELLS      = {'L'}        # celdas que dan vida
 

#------------------------------------------------------------------------------------
# Esta funcion enuentra y se asegura de que haya solo 1 start y solo 1 exit.
def find_start(maze):
    #Valida el inicio y final
    rows, cols = maze.shape
    start_pos  = None
    start_count = end_count = 0

    # No hay mucho pierde, recorre todo el arrelgo en busca del start (S) y del exit (E)
    for i in range(rows):
        for j in range(cols):
            cell = maze[i, j]
            if cell == 'S':
                start_count += 1
                start_pos = (i, j)
            elif cell == 'E':
                end_count += 1
 
    if start_count == 0:
        print("Error: No existe celda de inicio (S)")
        return -1, -1
    if start_count > 1:
        print("Error: Hay múltiples celdas de inicio (S)")
        return -1, -1
    if end_count == 0:
        print("Error: No existe celda de salida (E)")
        return -1, -1
    if end_count > 1:
        print("Error: Hay múltiples celdas de salida (E)")
        return -1, -1
 
    return start_pos
#-------------------------------------------------------------------------------------- 
 

#--------------------------------------------------------------------------------------
# Esta funcion es la encargada de resolver el maze
# Ya explique lo que recibe mas abajo, solo que de principio usamos last_row y last_col =- 1. 
def maze_solver_impl(maze, original, row, col, lives, poisoned, poison_steps, steps, visited, solutions, last_row=-1, last_col=-1):
    #El backtracking
 
    #Primero comprobar si viene envenenado
    if poisoned:
        poison_steps += 1
        if poison_steps % POISON_INTERVAL == 0:
            lives -= 1
 
    #Si ya no tiene vida la ruta no sirve
    if lives <= 0:
        return
 
    steps += 1  #Avanzar si tiene vida aun
 
    # Encontramos la meta
    # Si en el maze[row del start, col del start] es == exit, agregamos a la lista de soluciones
    if original[row, col] == 'E':
        print(f"Solucion encontrada: pasos={steps}, vidas={lives}")
        solutions.append({'steps': steps, 'lives': lives})
        return
 
    #Aqui es para saber en que celda se encuentra y que hacer
    original_cell = original[row, col]
 
    if original_cell == 'P':          # Los pinchos por eso P
        lives -= 1
    elif original_cell == 'M':        # Veneno es M (que toc XDXDXD)
        poisoned = True
        poison_steps = 0
    elif original_cell == 'L':        #Vida es lives
        lives = min(lives + LIFE_CELL_HEAL, INITIAL_LIVES)
        poisoned = False              # La vida cura el veneno
        poison_steps = 0
 
    if lives <= 0:    #Igual si en este momento donde esta se queda sin vida para que no siga
        return
 
    # Solo seguir si tenemos mas vidas que antes en esa celda, igual depende de cuantos pasos y vidas tengamos en ese momento
    prev = visited.get((row, col))
    if prev is not None:
        prev_lives, prev_steps = prev
        if prev_lives >= lives and prev_steps <= steps:
            return
    visited[(row, col)] = (lives, steps)
 
    # Limpiar la celda anterior: si era S o E se restaura, si no se marca como visitada
    if last_row != -1 and original[last_row, last_col] not in ('S', 'E'):
        maze[last_row, last_col] = 'V'
    if original[last_row, last_col] in ('S'):
        maze[last_row, last_col] = 'S'
    if original_cell not in ('S', 'E'):
        maze[row, col] = 'A'
    yield maze, lives, steps

    # Marcar siempre la posición actual con el agente
    maze[row, col] = 'A'
    yield maze, lives, steps

    #Tener las celdas vecinas guardadas
    neighbors = [
        (row, col- 1),   # izquierda
        (row,  col+ 1),  # derecha
        (row+ 1, col),   # abajo
        (row- 1, col),   # arriba
    ]
 
    life_moves   = []   # prioridad máxima
    safe_moves   = []
    danger_moves = []   #tratar de no pasar aca

    # Aqui vamos a revisar a los vecinos para decidir a donde ir. 
    for r, c in neighbors:
        if not (0 <= r < maze.shape[0] and 0 <= c < maze.shape[1]):
            continue
        
        # Si la celda está marcada como visitada ('V'), no la puede volver a visitar
        if maze[r, c] == 'V':
            continue

        # Excelente asignacion en el nombre de las vairables litzy, completamente comprensible. 
        orig = original[r, c] # Son las coordenadas de los vecinos actuales

        if orig == 'W':
            continue
        if orig in LIFE_CELLS:
            life_moves.append((r, c))
        elif orig in DAMAGE_CELLS:
            danger_moves.append((r, c))
        else:
            safe_moves.append((r, c))
 
    #Aqui se suman para que se junten en uno mismo y ese sea el orden que seguira
    for r, c in life_moves + safe_moves + danger_moves:
        # RECURSIVO GENTE
        yield from maze_solver_impl(maze, original, r, c, lives, poisoned, poison_steps, steps, visited, solutions, row, col)
        # Estamos llamando a la funcion que resuelve el laverinto con las coordenadas actualizadas.
 
    #Pone la celda de antes al retroceder
    if last_row != -1 and original[last_row, last_col] not in ('S', 'E'):
        maze[last_row, last_col] = 'V'
    if original_cell not in ('S', 'E'):
        maze[row, col] = original_cell
        yield maze, lives, steps
#-------------------------------------------------------------------------------------------------------------------
 
#-------------------------------------------------------------------------------------------------------------------
# La funcion encargada de llamar a la funciones que resuelven el maze y guardar la mejor solucion
def maze_solver(maze):
    si, sj = find_start(maze) # Guardamos los valores de la tupla de manera individual (creo que es para la interfaz)

    if si == -1:
        return
    
    original = maze.copy() # Copiar el laberinto para saber como es
    solutions = []
    visited = {} 

    # Aqui se resuelve llamando a maze_solver, se usa yield from para pasarlo a la interfaz
    # Le pasamos a maze_solver_impl(El maze original que recivimos en la funcion, una copia de este mismo ?? XDXDXD, 
    # las coordenadas del start, las vidas actuales, si no encontramos envenenados, los pasos de veneno que ya recorrimos, el total de pasos que ya recorrimos,
    # la lista que soluciones y la mejor solucion actual)
    yield from maze_solver_impl(maze, original, si, sj, lives= INITIAL_LIVES, poisoned= False, poison_steps = 0, steps = 0, visited= visited, solutions= solutions,)
 
    if not solutions:
        print("\nNo se encontró ninguna solucion.")
    else:
        print(f"\nTotal de soluciones encontradas: {len(solutions)}")
        for s in solutions:
            print(f" pasos={s['steps']}  vidas={s['lives']}")
 
        best = max(solutions, key=lambda x: (x['lives'], -x['steps']))
        print(f"\nMejor solucion: vidas={best['lives']}, pasos={best['steps']}")
#------------------------------------------------------------------------------------------------------------------
 

#------------------------------------------------------------------------------------------------------------------
# Funcion para generar un laberinto aleatorio de n x n usando el Algoritmo de Prim Simplificado.
def generate_random_maze(n=20):
    # Inicializar todo con paredes ('W')
    maze = [['W' for _ in range(n)] for _ in range(n)]
    
    # Lista de paredes candidatas (celda_dentro, celda_nueva)
    walls = []
    
    # Empezar en (1, 1) y marcarlo como camino
    start_r, start_c = 1, 1
    maze[start_r][start_c] = ' '
    
    # Agregar paredes iniciales candidatas alrededor del inicio (saltando de 2 en 2)
    for dr, dc in [(0, 2), (0, -2), (2, 0), (-2, 0)]:
        nr, nc = start_r + dr, start_c + dc
        if 1 <= nr < n-1 and 1 <= nc < n-1:
            walls.append((start_r, start_c, nr, nc))
            
    # Algoritmo de Prim simplificado para conectar todas las celdas
    while walls:
        # Elegir una pared al azar para que el laberinto sea aleatorio
        idx = random.randint(0, len(walls) - 1)
        wr, wc, nr, nc = walls.pop(idx)
        
        if maze[nr][nc] == 'W':
            # Conectar la celda nueva rompiendo la pared intermedia
            maze[wr + (nr-wr)//2][wc + (nc-wc)//2] = ' '
            maze[nr][nc] = ' '
            
            # Agregar nuevas paredes candidatas desde la celda recién "excavada"
            for dr, dc in [(0, 2), (0, -2), (2, 0), (-2, 0)]:
                nnr, nnc = nr + dr, nc + dc
                if 1 <= nnr < n-1 and 1 <= nnc < n-1 and maze[nnr][nnc] == 'W':
                    walls.append((nr, nc, nnr, nnc))

    # Si N es par, el algoritmo no llega al borde.
    if n % 2 == 0:
        for i in range(1, n-1):
            if maze[n-3][i] == ' ': maze[n-2][i] = ' ' # Conectar hacia abajo
            if maze[i][n-3] == ' ': maze[i][n-2] = ' ' # Conectar hacia la derecha

    # Rompemos algunas paredes extra para que existan multiples rutas posibles.
    for _ in range(n // 2): 
        r = random.randint(1, n-2)
        c = random.randint(1, n-2)
        if maze[r][c] == 'W':
            # Solo si ayuda a conectar dos pasillos (evita espacios vacios grandes)
            if (maze[r-1][c] == ' ' and maze[r+1][c] == ' ') or \
               (maze[r][c-1] == ' ' and maze[r][c+1] == ' '):
                maze[r][c] = ' '

    # Colocar Inicio 'S' y buscar el Fin 'E' en la parte inferior/derecha
    maze[1][1] = 'S'
    found_e = False
    for r in range(n-2, 0, -1):
        for c in range(n-2, 0, -1):
            if maze[r][c] == ' ':
                maze[r][c] = 'E'
                found_e = True
                break
        if found_e: break

    # Agregar elementos aleatorios (Pinchos 'P', Veneno 'M', Vida 'L')
    for r in range(1, n-1):
        for c in range(1, n-1):
            if maze[r][c] == ' ':
                rand = random.random()
                if rand < 0.05: maze[r][c] = 'P'
                elif rand < 0.10: maze[r][c] = 'M'
                elif rand < 0.15: maze[r][c] = 'L'

    # Retornar como lista de strings (formato maze_str)
    return ["".join(row) for row in maze]
#------------------------------------------------------------------------------------------------------------------
 
 
#------------------------------------------------------------------------------------------------------------------ 
# 1. Generar laberinto aleatorio con el N configurado arriba
maze_str_random = generate_random_maze(N)
 
# 2. Convertir a numpy array
maze = np.array([list(row) for row in maze_str_random])
 
# 3. Ejecutar la animación
animate_maze(maze_solver(maze), interval=0.01)
