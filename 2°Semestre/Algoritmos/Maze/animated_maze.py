
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image as mpimg
from matplotlib.animation import FuncAnimation

def animate_maze(maze_generator, interval=250):
    textures = {
        'W': mpimg.imread('wall.png'),
        ' ': mpimg.imread('path.png'),
        'S': mpimg.imread('start.png'),
        'E': mpimg.imread('end.png'),
        'A': mpimg.imread('agent.png'),
        'V': mpimg.imread('visited.png'),
        'P': mpimg.imread('pinchos.png'),
        'L': mpimg.imread('vida.png'),
        'M': mpimg.imread('veneno.png')
    }

    # Obtener primer estado
    maze = next(maze_generator)
    rows, cols = maze.shape

    fig, ax = plt.subplots()

    # Dibujar estado inicial
    ims = []
    for i in range(rows):
        row_imgs = []
        for j in range(cols):
            cell = maze[i, j]
            img = textures.get(cell, textures[' '])

            im = ax.imshow(img, extent=(j, j+1, rows-i-1, rows-i))
            row_imgs.append(im)
        ims.append(row_imgs)

    ax.set_xticks([])
    ax.set_yticks([])
    ax.set_xlim(0, cols)
    ax.set_ylim(0, rows)
    ax.set_aspect('equal')

    # Función de actualización
    def update(frame):    
        maze = frame
        for i in range(rows):
            for j in range(cols):
                cell = maze[i, j]
                img = textures.get(cell, textures[' '])
                ims[i][j].set_data(img)
        return []

    ani = FuncAnimation(
        fig,
        update,
        frames=maze_generator,
        interval=interval,
        repeat=False,
        cache_frame_data=False
    )

    plt.show()
