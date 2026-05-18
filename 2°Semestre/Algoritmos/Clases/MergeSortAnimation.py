import matplotlib.pyplot as plt
import matplotlib.animation as animation
import random

def merge_sort(data, inicio=0):
    
    if len(data) <= 1:
        return data
    
    mid = len(data) // 2
    izquierda = merge_sort(data[:mid], inicio)
    derecha = merge_sort(data[mid:], inicio + mid)
    
    return list(merge(izquierda, derecha, inicio))

def merge(izquierda, derecha, inicio):
    
    resultado = []
    i = j = 0
    
    while i < len(izquierda) and j < len(derecha):
        if izquierda[i] <= derecha[j]:
            resultado.append(izquierda[i])
            i += 1
        else:
            resultado.append(derecha[j])
            j += 1
        
        # Animacion
        temp = resultado + izquierda[i:] + derecha[j:]
        for k, val in enumerate(temp):
            arreglo_global[inicio + k] = val
        frames.append((list(arreglo_global), inicio, inicio + len(temp) - 1))
    
    # Agregar lo que sobra
    resto = izquierda[i:] + derecha[j:]
    resultado.extend(resto)
    
    for k, val in enumerate(resultado):
        arreglo_global[inicio + k] = val
    frames.append((list(arreglo_global), inicio, inicio + len(resultado) - 1))
    
    return resultado

# Configuración de datos
n = 50
arreglo_global = list(range(1, n + 1))
random.shuffle(arreglo_global)
frames = [(list(arreglo_global), -1, -1)]  # Frame inicial

# Ejecutar el sort para capturar todos los frames
merge_sort(list(arreglo_global))

# Configuración de la gráfica
plt.style.use('dark_background')
fig, ax = plt.subplots(figsize=(12, 6))
fig.patch.set_facecolor('#1a1a2e')
ax.set_facecolor('#1a1a2e')

# Crear barras iniciales
colores_iniciales = ['#4361ee'] * n
bars = ax.bar(range(n), frames[0][0], color=colores_iniciales, edgecolor='#7209b7', linewidth=0.5)

# Estilo del gráfico
ax.set_xlim(-1, n)
ax.set_ylim(0, n + 5)
ax.set_title('Merge Sort', fontsize=18, fontweight='bold', color='#f8f8f2', pad=15)
ax.set_xlabel('Índice', fontsize=12, color='#888888')
ax.set_ylabel('Valor', fontsize=12, color='#888888')
ax.tick_params(colors='#888888')
for spine in ax.spines.values():
    spine.set_color('#333333')

# Texto de información
texto_frame = ax.text(0.02, 0.95, '', transform=ax.transAxes, fontsize=11, 
                       color='#f8f8f2', verticalalignment='top',
                       fontfamily='monospace')

def update(frame_data):
    data, inicio, fin = frame_data
    
    for i, (bar, val) in enumerate(zip(bars, data)):
        bar.set_height(val)
        
        # Colorear según estado
        if inicio <= i <= fin:
            # Sección activa: gradiente según valor
            intensidad = val / n
            bar.set_color(plt.cm.plasma(intensidad))
            bar.set_edgecolor('#ffffff')
            bar.set_linewidth(1)
        else:
            bar.set_color('#4361ee')
            bar.set_edgecolor('#7209b7')
            bar.set_linewidth(0.5)
    
    texto_frame.set_text(f'Operaciones: {frames.index(frame_data)}/{len(frames)-1}')
    return list(bars) + [texto_frame]

ani = animation.FuncAnimation(
    fig,
    update,
    frames=frames,
    repeat=False,
    interval=50,
    blit=True
)

plt.tight_layout()
plt.show()
