import numpy as np
from animated_maze import animate_maze
 
#Segun google tener constantes es buena practica, pero se puden cambiar si es necesario
INITIAL_LIVES   = 5      # vidas iniciales
POISON_INTERVAL = 3      # cada cuantos movimientos hace daño el veneno
LIFE_CELL_HEAL  = 2      # vida que recupera una celda 'L'
DAMAGE_CELLS    = {'P', 'M'}   # celdas que quitan vida al entrar
LIFE_CELLS      = {'L'}        # celdas que dan vida
 
 
def find_start(maze):
    #Valida el inicio y final
    rows, cols = maze.shape
    start_pos  = None
    start_count = end_count = 0
 
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
 
    #Encontamos la meta
    if original[row, col] == 'E':
        print(f"Solucion encontrada: pasos={steps}, vidas={lives}")
        solutions.append({'steps': steps, 'lives': lives})
        return
 
    #Aqui es para saber en que celda se encuentra y que hacer
    original_cell = original[row, col]
 
    if original_cell == 'P':          #Los pinchos por eso P
        lives -= 1
    elif original_cell == 'M':        #Veneno es M
        lives -= 1
        poisoned = True
        poison_steps = 0
    elif original_cell == 'L':        #Vida es lives
        lives = min(lives + LIFE_CELL_HEAL, INITIAL_LIVES)
        poisoned = False          # la vida cura el veneno
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
 
    #Sirve, no le muevan
    if last_row != -1 and original[last_row, last_col] not in ('S', 'E'):
        maze[last_row, last_col] = 'V'
    if original_cell not in ('S', 'E'):
        maze[row, col] = 'A'
    yield maze
 
    #Tener las celdas vecinas guardadas
    neighbors = [
        (row, col- 1),   # izquierda
        (row,  col+ 1),   # derecha
        (row+ 1, col),   # abajo
        (row- 1, col),   # arriba
    ]
 
    life_moves   = []   # prioridad máxima
    safe_moves   = []
    danger_moves = []   #tratar de no pasar aca
 
    for r, c in neighbors:
        if not (0 <= r < maze.shape[0] and 0 <= c < maze.shape[1]):
            continue
        orig = original[r, c]
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
        yield from maze_solver_impl(maze, original, r, c, lives, poisoned, poison_steps, steps, visited, solutions, row, col)
 
    #Pone la celda de antes al retroceder
    if last_row != -1 and original[last_row, last_col] not in ('S', 'E'):
        maze[last_row, last_col] = 'V'
    if original_cell not in ('S', 'E'):
        maze[row, col] = original_cell
        yield maze
 
 

def maze_solver(maze):
    si, sj = find_start(maze)
    if si == -1:
        return
    original = maze.copy() #Copiar el laberinto para saber como es
    solutions = []
    visited = {} 
 
    yield from maze_solver_impl(maze, original, si, sj, lives= INITIAL_LIVES, poisoned= False, poison_steps = 0, steps = 0, visited= visited, solutions= solutions,)
 
    if not solutions:
        print("\nNo se encontró ninguna solucion.")
    else:
        print(f"\nTotal de soluciones encontradas: {len(solutions)}")
        for s in solutions:
            print(f" pasos={s['steps']}  vidas={s['lives']}")
 
        best = max(solutions, key=lambda x: (x['lives'], -x['steps']))
        print(f"\nMejor solucion: vidas={best['lives']}, pasos={best['steps']}")
 
 
maze_str = [
    "WWWWWWWWWWWWWWWWWWWW",   # row 0
    "WS  W     W   P   WW",   # row 1
    "W W W WWW W WWW W WW",   # row 2
    "W W   W   W W   W  W",   # row 3
    "W WWWWW W W W W WW W",   # row 4
    "W W   W W   W W L  W",   # row 5
    "W W W WWWWWWW WW WWW",   # row 6
    "W W W   M   W W    W",   # row 7
    "W W WWWWWWW W W WWWW",   # row 8
    "W   W L   W W   P  W",   # row 9
    "W WWW WWWWW W W WWWW",   # row 10
    "W     W   W W W    W",   # row 11
    "W WWWWW M W W WWW WW",   # row 12
    "W W     W W W      W",   # row 13
    "W W WWWWW W W WWWWWW",   # row 14
    "W W W   M W W     MW",   # row 15
    "W W W W W   W WWWW W",   # row 16
    "W     W WWWWW W    W",   # row 17
    "WWWWW W       W  E W",   # row 18
    "WWWWWWWWWWWWWWWWWWWW",   # row 19
]
 
maze = np.array([list(row) for row in maze_str])
 
animate_maze(maze_solver(maze), interval=80)