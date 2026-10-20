# SimMarket 

> Esta guia explica **como funciona el simulador** para que todos podamos entenderlo(incluyendome porque luego me revuelvo)
> Al final hay una seccion de **Teoria nueva** con cosas que no hemos visto en clase pero que se utilizaron aqui
>Todas las flechitas fueron hechas con ayuda de chat c:  (no se hacer readme)


## 1. Que es 

Es una **simulacion de un supermercado**. Imaginen una tienda con varias cajas de cobro. Llegan clientes, recorren la tienda juntando productos, se forman en una caja, les cobran y se van.

La simulacion avanza por **iteraciones** (o turnos por decirlo de otra manera). En cada iteracion pasan varias cosas en un orden fijo, y despues podemos ver como quedo todo: cuantos clientes hay, que hace cada caja, quien esta formado, entre otras cosas mas.


## 2. Clases principales

|   Clase   | Archivo | Que es |
|---|---|---|
| **Cliente** | `Cliente.cs` | Una persona que llega, junta productos y luego paga |
| **Caja** | `Caja.cs` | Una caja de cobro con su propia fila de clientes |
| **Simulador** | `Simulador.cs` | El el que controla todo: tiene las reglas, hace que pase cada iteracion y decide a que caja va cada cliente |

Otros que igual existen ahi:

| Archivo | Para que sirve |
|---|---|
| `SimConfig.cs` | La configuracion de todo: todos los numeros que se pueden cambiar (cuantas cajas, cuantos clientes llegan, etc) |
| `ArrayList.cs` | Nuestra **lista** (la del proyecto anterior) |
| `ArrayQueue.cs` | Nuestra **cola** (la de proyecto anterior igual) |
| `Program.cs` | La version de **consola** para probar la logica sin Unity |
| `MarketManager.cs` | El conector con **Unity** (ver seccion 10). Esta comentado con `/* ... */` |

### Como se usan entre si (con flechas de chat)

```
Program.cs (consola)  ─┐
                       ├──►  Simulador  ──►  tiene una ArrayList de Cliente (los que andan en la tienda)
MarketManager (Unity) ─┘          │
                                  └──►  tiene una ArrayList de Caja
                                                   │
                                                   └──►  cada Caja tiene una ArrayQueue de Cliente (su fila)
```

La idea clave: **la logica vive solo en `Simulador`, `Cliente` y `Caja`.** Consola y Unity solo le piden "avanza una iteracion" y luego **leen** como quedo todo.
Entonces se deberia poder editar estos sin cambiar la logica principal.

---

## 3. Las estructuras de datos que usamos (requisito del PDF)

| Estructura | Donde | Para que |
|---|---|---|
| `ArrayList<Cliente>` | `Simulador.clientesEnMercado` | Lista de clientes que estan **dentro de la tienda y aun no estan en una fila** (comprando, o ya listos para formarse) |
| `ArrayList<Caja>` | `Simulador.cajas` | Lista con todas las cajas de cobro. |
| `ArrayQueue<Cliente>` | `Caja.fila` (una por caja) | La **fila** de cada caja |

---

## 4. Iteracion paso a paso

Cada vez que se llama a `EjecutarIteracion()` pasan **4 pasos, siempre en este orden**:

```

PASO 1  Cajas cobran        -> cada caja cobra a su 1er cliente 
PASO 2  Formar en cajas     -> los "ListoParaCaja" eligen caja  
PASO 3  Recolectar          -> los que compran agarran productos
PASO 4  Llegadas            -> entran clientes nuevos (si aplica)

```

### Paso 1 — Las cajas cobran (`ProcesarCajas`)
Cada caja le cobra **solo al cliente que esta al frente de su fila**. Cada caja tiene una capacidad fija (por ejemplo, 3 productos por iteracion).
- Si al cliente todavia le faltan productos por cobrar, se queda en la caja.
- Si ya termino, **sale de la fila y del supermercado**, y se suma 1 a `TotalAtendidos`.
- El siguiente cliente de la fila empieza a ser atendido hasta la **siguiente** iteracion.

> **Ejemplo:** caja con capacidad 3 y un cliente con 7 productos por cobrar -> iteracion A: quedan 4 -> iteracion B: queda 1 -> iteracion C: queda 0 y se va. (3 iteraciones = el techo de 7 / 3)

### Paso 2 — Formarse en una caja (`AsignarClientesACajas`)
Se recorre la lista de clientes de la tienda **en orden de llegada** (los que llegaron antes eligen primero). Solo importan los que estan en estado `ListoParaCaja`. Para cada uno:
1. Se busca la mejor caja (`SeleccionarCaja`, ver seccion 6).
2. Si hay caja -> se forma al final de su fila y **sale de la lista de la tienda** (ya esta en una fila).
3. Si **no** hay ninguna caja disponible -> el cliente **abandona** la compra, sube el contador `ClientesAbandonaron` y tambien sale de la lista.

>**Detalle del codigo:** se usa un `while` con un indice `i` en vez de un `for`. Cuando se quita a un cliente de la lista (`RemoveAt`), todos los demas se recorren una posicion hacia atras, asi que **no** hay que avanzar `i`, si no, nos saltariamos a un cliente. (No se si hay una mejor forma de hacerlo).

### Paso 3 — Recolectar productos (`RecolectarArticulos`)
Todos los clientes que siguen en la lista llaman a su metodo `Recolectar()` (los que no estan "Comprando" simplemente lo ignoran).

### Paso 4 — Llegan clientes nuevos (`GenerarLlegadas`)
- Si ya paso el periodo de llegadas (`IteracionesLlegada`), **no entra nadie** y se termina el metodo.
- Si no, llegan **de 0 a `MaxLlegadasPorIteracion`** clientes (numero aleatorio). A cada uno se le generan sus datos al azar (ver seccion 5) y se agrega a la lista.

---

## 5. Clientes

### Datos que se generan al azar al llegar (y ya no cambian otra vez)
| Dato | Que significa | Rango |
|---|---|---|
| `ArticulosTotales` | Cuantos productos va a juntar | 1 a 20 |
| `IteracionesPorRecoleccion` | Cada cuantas iteraciones agarra productos | 1 a 3 |
| `ArticulosPorRecoleccion` | Cuantos agarra cada vez | 1 a 4 |
| `EsPreference` | ¿Es cliente **Preferente**? (10 % de probabilidad) | si / no |

### Ejemplo de recoleccion
Cliente con **10 productos**, que agarra **3 productos cada 2 iteraciones**, y llega (se crea) en la iteracion 1:

| Iteracion | Que pasa | Productos juntados |
|---|---|---|
| 2 | Cuenta 1 de 2… todavia no agarra | 0 |
| 3 | Cuenta 2 de 2 -> agarra 3 | 3 |
| 5 | Agarra 3 | 6 |
| 7 | Agarra 3 | 9 |
| 9 | Le tocan 3 pero solo falta 1 -> agarra 1 (nunca se pasa) | 10 |
| 10 | Ya esta `ListoParaCaja` -> se forma en una caja (paso 2) | — |

### Los estados del cliente
El cliente siempre esta en uno de estos estados (es un **Enum**, ver teoria para saber que es):

```
 Comprando ──(junta todo)──► ListoParaCaja ──(hay caja)──► EnFila ──(le cobran todo)──► Terminado
                                   │
                                   └──(todas las cajas elegibles llenas)──► Abandono
```

### Marcas en la clase `Cliente`
- `ArticulosRecolectados`: lo que lleva juntado.
- `ArticulosPorCobrar`: lo que le falta pagar en caja (empieza igual que el total y baja cuando la caja le cobra).
- Los datos fijos solo se pueden **leer** desde afuera (`{ get; }`); los que cambian solo los modifica el propio cliente (`{ get; private set; }`).

---

## 6. Cajas y como se elige a donde formarse

### Tipos de caja
- Se crean `NumCajas` cajas en total. Las **primeras `NumCajasRapidas`** son **rapidas**; el resto son **normales**.
- A cada caja se le asigna **una sola vez, al azar**, cuantos productos cobra por iteracion (de 1 a `MaxCapacidadCobro`).

### ¿Quien puede formarse en cual? (`Caja.PuedeAtender`)
- **Caja normal:** cualquier cliente.
- **Caja rapida:** solo si el cliente es **Preferente** *o* si lleva `LimiteArticulosRapida` productos o menos.

### ¿Cual caja elige? (`Simulador.SeleccionarCaja`)
De entre las cajas que **puede usar** y que **no estan llenas**, elige la que tenga **menor tiempo de espera estimado**.

La formula (la que pide el profe) suma, por cada cliente que ya esta en la fila (**incluyendo al que se esta atendiendo**), cuantas iteraciones necesita para cobrarle lo que le falta:

```
Tiempo de espera = suma de productos pendientes del cliente / capacidad de la caja (redondeado hacia arriba)
```

El `⌈ ⌉` significa **redondear hacia arriba**

> **Ejemplo:** una caja de capacidad 3 tiene dos clientes en fila: uno con 7 pendientes y otro con 3.
> ⌈7÷3⌉ + ⌈3÷3⌉ = 3 + 1 = **4 iteraciones de espera**.

**Desempates (en este orden):**
1. Si hay empate y el cliente puede usar caja rapida -> se prefiere la **rapida**.
2. Si el empate sigue -> gana la caja con **menor numero** (la primera que se revisa).

### Cajas llenas y abandono
Cada caja admite maximo `MaxClientesPorCaja` clientes **contando al que se esta atendiendo**. Una caja llena **no se puede elegir**. Si **todas** las cajas que el cliente podia usar estan llenas, el cliente **se va sin comprar** (abandona como en los juegos).

---

## 7. Cuando termina la simulacion

La propiedad `Terminada` es verdadera cuando se cumplen las **tres** condiciones:
1. Ya paso el periodo de llegadas (`Iteracion >= IteracionesLlegada`),
2. no queda nadie en la lista de la tienda (`clientesEnMercado.Empty`), y
3. no queda nadie en las filas de las cajas.

Despues del periodo de llegadas ya no entra nadie, pero la simulacion **sigue** hasta que todos terminen o abandonen. (asi dice el PDF)

---

## 8. Configuracion (`SimConfig`)

El PDF usa nombres de constantes en ingles; en el codigo estan en spanish(perdon por eso) y se llaman asi:

| Nombre en el PDF | Nombre en el codigo (`SimConfig`) | Valor por defecto |
|---|---|---|
| `NEW_CLIENTS_PER_ITERATION` | `MaxLlegadasPorIteracion` | 2 |
| `PRODUCTS_TO_COLLECT` | `MaxArticulos` (minimo `MinArticulos` = 1) | 20 |
| `ITERATIONS_TO_COLLECT` | `MaxIteracionesPorRecoleccion` | 3 |
| `ITERATIONS_TO_COLLECT_PRODUCTS` | `MaxArticulosPorRecoleccion` | 4 |
| `TOTAL_CASHIER_LANES` | `NumCajas` | 3 |
| `EXPRESS_CASHIER_LANES` | `NumCajasRapidas` | 2 |
| `EXPRESS_LANE_MAX_PRODUCTS` | `LimiteArticulosRapida` | 5 |
| `MAX_CUSTOMERS_PER_CASHIER` | `MaxClientesPorCaja` | 2 |
| `CHECKOUT_PRODUCTS` | `MaxCapacidadCobro` (minimo `MinCapacidadCobro` = 1) | 5 |
| `CUSTOMER_ARRIVAL_ITERATIONS` | `IteracionesLlegada` | 10 |
| Probabilidad de Preferente (10 %) | `ProbPreference` | 0.10 |
| — (extra) | `Semilla` | `null` (azar real) |

`Validar()` revisa que los valores tengan sentido (por ejemplo, que no haya 0 cajas) y lanza un error si no. 

---

## 9. Como se conecta Unity (`MarketManager`)

No hace falta entender su codigo por dentro; **no tiene reglas del simulador**, solo dibuja. Lo que hace:

1. **Crea el simulador** con los valores que se escriben en el Inspector de Unity (numero de cajas, limite de caja rapida, etc).
2. **Avanza la simulacion** llamando a `sim.EjecutarIteracion()` (con un boton o automaticamente cada cierto numero de segundos).
3. **Se ocupa a los 4 eventos** del simulador (`ClienteLlego`, `ClienteSeFormo`, `ClienteSalio`, `ClienteAbandono`) para dibujar: cuando llega un cliente crea su "cliente", cuando se forma lo manda caminando a su caja, cuando sale o abandona lo borra.
4. **Lee las propiedades** del simulador (`Iteracion`, `ClientesEnFilas`, `TotalAtendidos`, etc.) para escribir el panel de texto.

El archivo esta entre `/* ... */` para que el proyecto de consola compile sin tener Unity instalado.

---

## 10. Como probar en consola

Ejecutar el proyecto (`Program.cs`). Despues de cada iteracion se imprime:
- el resumen (iteracion, clientes en mercado / comprando / en filas, atendidos, abandonaron),
- cada cliente que sigue comprando,
- cada caja con su fila (`[atendiendo]` es el primero),
- y mensajes de los eventos (`->` se formo, `<-` salio, `xx` abandono).

**Enter** = siguiente iteracion, **e** = salir.

`Program.cs` usa `Semilla = 1`, asi que **siempre sale la misma simulacion** (sirve para pruebas). Para que cada vez sea distinta, quiten la semilla: `new SimConfig()`.

---

# Teoria nueva

## A) Enum (`EstadoCliente`)

**Que es:** una lista de **opciones con nombre**. Es como decirle a C#: esta variable solo puede valer una de estas cosas

```csharp
public enum EstadoCliente
{
    Comprando, ListoParaCaja, EnFila, Terminado, Abandono
}
```

**Analogia:** el estado de un pedido de comida a domicilio: *Preparando -> En camino -> Entregado*. Solo hay unos cuantos estados posibles y tienen nombre.

**Por que lo usamos:** la alternativa seria usar numeros (0, 1, 2…) o textos ("comprando"). Con numeros nadie(yo) se acuerda de que significa cada uno, con textos es facil escribir mal una palabra y el programa no avisa. Con un enum, **si te equivocas el programa no compila**.

**Como se usa:**
```csharp
Estado = EstadoCliente.Comprando;                    // asignar
if (c.Estado == Cliente.EstadoCliente.ListoParaCaja) // comparar
```
(Desde fuera de `Cliente` se escribe `Cliente.EstadoCliente.X` porque el enum esta definido *dentro* de la clase `Cliente`.)

---

## B) `override` (sobreescribir `ToString`) 

**Que es:** en C# **todos** los objetos heredan un metodo llamado `ToString()` que lo convierte a texto. Por defecto solo devuelve el nombre de la clase (algo como `SimMarket.Cliente`), que no sirve de nada.

`override` significa **"reemplazo la version original por la mia"**. (Algo que acabamos de ver con Balam)

```csharp
public override string ToString()
{
    return $"Cliente #{Id} | {Estado} | recolectados {ArticulosRecolectados}/{ArticulosTotales} ...";
}
```

**Por que lo usamos:** gracias a esto, en `Program.cs` podemos escribir simplemente `Console.WriteLine(caja)` o `Console.WriteLine(cliente)` y sale la informacion completa. Lo hacemos en `Cliente` y en `Caja`.

---

## C) Eventos (`event`), `+=` e `Invoke`

Son 3 piezas de **una misma idea**: que una clase pueda **avisar** a otras que "algo paso" sin tener que conocerlas.

**El problema:** `Simulador` decide que un cliente se formo en una caja, pero **Unity** necesita enterarse para mover el "cliente". Y el Simulador no deberia saber nada de Unity (si no, ya no podriamos probarlo en consola).

**La solucion(con analogia)**

| Pieza | Codigo | Analogia |
|---|---|---|
| **Declarar el evento** | `public event Action<Cliente, Caja> ClienteSeFormo;` | Instalar un timbre en la puerta |
| **Suscribirse** | `sim.ClienteSeFormo += MiMetodo;` | Alguien dice "avisame cuando suene el timbre" |
| **Invoke (disparar)** | `ClienteSeFormo?.Invoke(c, destino);` | Tocar el timbre. A todos los suscritos se les ejecuta su metodo. |

**Detalles:**
- `Action<Cliente, Caja>` es el "tipo" del aviso: indica que **datos** viajan con el (aqui, el cliente y la caja). `Action<Cliente>` viaja solo con un cliente.
- `?.Invoke(...)` — el `?.` significa **"solo si hay alguien suscrito"**. Si nadie escucha, no pasa nada (sin ese `?`, el programa se caeria, no lo quiten, gracias).
- `+=` es **suscribirse** (agregar un oyente). El Simulador no sabe quien escucha: solo toca el timbre.

**Los 4 eventos del simulador:**

| Evento | ¿Cuando suena? |
|---|---|
| `ClienteLlego` | Entro un cliente nuevo |
| `ClienteSeFormo` | Un cliente se formo en una caja |
| `ClienteSalio` | Un cliente termino de pagar y salio |
| `ClienteAbandono` | Un cliente se fue porque todas las cajas estaban llenas |

**Donde se usan:** `Program.cs` los usa para imprimir los mensajes con flechas; `MarketManager` los usa para dibujar.

---

## D) `readonly`

```csharp
private readonly ArrayList<Caja> cajas = new ArrayList<Caja>();
```

Significa que **la variable se asigna una vez y ya no se puede reemplazar** por otra lista. (si se le pueden agregar elementos a la lista, lo que no se puede es cambiarla por una lista distinta.)

---

## E) `int?` (numero que puede ser "nada") y la `Semilla`

`int?` es un entero que ademas puede valer `null` (nada). Se usa en `Semilla`:
- `null` -> no hay semilla, los numeros aleatorios son distintos cada vez.
- `1` (u otro numero) -> **siempre** se genera la misma secuencia "aleatoria".

```csharp
rnd = config.Semilla.HasValue ? new Random(config.Semilla.Value) : new Random();
```
`HasValue` pregunta "¿trae un numero?" y `.Value` saca ese numero.

---

## F) `Math.Ceiling` y `Math.Min`

- **`Math.Ceiling`** redondea **hacia arriba** (es el `⌈ ⌉` de la formula). Como dividir dos enteros en C# corta los decimales, se convierte a `double` antes (`(double)pendientes / CapacidadCobro`) y luego se regresa a `int`.
- **`Math.Min(a, b)`** devuelve **el menor** de los dos. Lo usamos para que un cliente **nunca agarre mas productos de los que faltan** y para que a una caja **nunca le cobre mas de lo que le queda al cliente** (asi nada queda negativo).

