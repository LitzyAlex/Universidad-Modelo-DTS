
using System.Collections.Generic;
using UnityEngine;

public class NavegacionSupermercado : MonoBehaviour
{
    [Header("Área del supermercado")]
    [SerializeField] private Vector2 origen = new Vector2(-7f, -4f);
    [SerializeField] private Vector2 tamaño = new Vector2(12f, 8f);

    [Header("Cuadrícula")]
    [SerializeField] private float tamañoCelda = 0.2f;
    [SerializeField] private LayerMask capaEstantes;
    [SerializeField] private float radioCliente = 0.25f;

    [Header("Zona de compras")]
    [SerializeField] private float limiteIzquierdoCompras = -2.5f;

    private int columnas;
    private int filas;
    private bool[,] celdasBloqueadas;

    // Movimientos posibles: cuatro direcciones y cuatro diagonales.
    private readonly Vector2Int[] direcciones =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

    private void Awake()
    {
        ConstruirCuadricula();
    }

    private void ConstruirCuadricula()
    {
        columnas = Mathf.CeilToInt(tamaño.x / tamañoCelda);
        filas = Mathf.CeilToInt(tamaño.y / tamañoCelda);

        celdasBloqueadas = new bool[columnas, filas];

        for (int x = 0; x < columnas; x++)
        {
            for (int y = 0; y < filas; y++)
            {
                Vector2 centro = origen + new Vector2((x + 0.5f) * tamañoCelda,(y + 0.5f) * tamañoCelda);

                float radioDeteccion = Mathf.Max(tamañoCelda * 0.45f,radioCliente);

                celdasBloqueadas[x, y] =Physics2D.OverlapCircle(centro,radioDeteccion,capaEstantes) != null;
            }
        }
    }

    // Convierte una posición del mundo en una celda.
    private Vector2Int PosicionACelda(Vector2 posicion)
    {
        Vector2 relativa = posicion - origen;

        return new Vector2Int(Mathf.FloorToInt(relativa.x / tamañoCelda),Mathf.FloorToInt(relativa.y / tamañoCelda));
    }

    // Comprueba que una celda esté dentro de la cuadrícula.
    private bool DentroCuadricula(Vector2Int celda)
    {
        return celda.x >= 0 && celda.x < columnas && celda.y >= 0 && celda.y < filas;
    }

    // Comprueba si una celda está libre.
    private bool CeldaTransitable(Vector2Int celda)
    {
        return DentroCuadricula(celda) && !celdasBloqueadas[celda.x, celda.y];
    }

    // Devuelve el centro de una celda en coordenadas del mundo.
    private Vector3 CeldaAPosicion(Vector2Int celda)
    {
        return new Vector3(origen.x + (celda.x + 0.5f) * tamañoCelda,origen.y + (celda.y + 0.5f) * tamañoCelda,0f);
    }

    public bool EsPosicionTransitable(Vector2 posicion)
    {
        if (celdasBloqueadas == null)
        {
            return false;
        }

        return CeldaTransitable(PosicionACelda(posicion));
    }

    // Busca una celda libre cercana si el punto está bloqueado.
    private Vector2Int BuscarCeldaLibre(Vector2Int inicial)
    {
        if (CeldaTransitable(inicial))
        {
            return inicial;
        }

        float menorDistancia = float.MaxValue;
        Vector2Int mejorCelda = new Vector2Int(-1, -1);

        for (int x = 0; x < columnas; x++)
        {
            for (int y = 0; y < filas; y++)
            {
                Vector2Int candidata = new Vector2Int(x, y);

                if (!CeldaTransitable(candidata))
                {
                    continue;
                }

                float distancia = (candidata - inicial).sqrMagnitude;

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    mejorCelda = candidata;
                }
            }
        }

        return mejorCelda;
    }

    // Heurística: estima la distancia hasta el destino.
    private float CalcularHeuristica(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        // Distancia que permite diagonales.
        return Mathf.Max(dx, dy) + (1.41421356f - 1f) * Mathf.Min(dx, dy);
    }

    // Calcula una ruta usando A*.
    // Devuelve posiciones del mundo que el cliente puede seguir.
    public List<Vector3> ObtenerRuta(Vector2 inicio, Vector2 destino)
    {
        List<Vector3> ruta = new List<Vector3>();

        if (celdasBloqueadas == null || tamañoCelda <= 0f)
        {
            return ruta;
        }

        Vector2Int celdaInicio = BuscarCeldaLibre(PosicionACelda(inicio));

        Vector2Int celdaDestino = BuscarCeldaLibre(PosicionACelda(destino));

        if (celdaInicio.x < 0 || celdaDestino.x < 0)
        {
            return ruta;
        }

        float[,] costos = new float[columnas, filas];
        Vector2Int[,] anteriores = new Vector2Int[columnas, filas];
        bool[,] tieneAnterior = new bool[columnas, filas];
        bool[,] cerradas = new bool[columnas, filas];

        for (int x = 0; x < columnas; x++)
        {
            for (int y = 0; y < filas; y++)
            {
                costos[x, y] = float.MaxValue;
            }
        }

        List<Vector2Int> abiertas = new List<Vector2Int>();
        costos[celdaInicio.x, celdaInicio.y] = 0f;
        abiertas.Add(celdaInicio);

        bool encontrada = false;

        while (abiertas.Count > 0)
        {
            // Elegimos la celda con menor costo estimado.
            int mejorIndice = 0;
            float mejorCosto = float.MaxValue;

            for (int i = 0; i < abiertas.Count; i++)
            {
                Vector2Int candidata = abiertas[i];

                float costoEstimado =costos[candidata.x, candidata.y] + CalcularHeuristica(candidata, celdaDestino);

                if (costoEstimado < mejorCosto)
                {
                    mejorCosto = costoEstimado;
                    mejorIndice = i;
                }
            }

            Vector2Int actual = abiertas[mejorIndice];
            abiertas.RemoveAt(mejorIndice);

            if (actual == celdaDestino)
            {
                encontrada = true;
                break;
            }

            cerradas[actual.x, actual.y] = true;

            foreach (Vector2Int direccion in direcciones)
            {
                Vector2Int vecino = actual + direccion;

                if (!CeldaTransitable(vecino))
                {
                    continue;
                }

                if (cerradas[vecino.x, vecino.y])
                {
                    continue;
                }

                // Evita cortar las esquinas de los estantes.
                if (direccion.x != 0 && direccion.y != 0)
                {
                    Vector2Int lateralX = actual + new Vector2Int(direccion.x, 0);

                    Vector2Int lateralY = actual + new Vector2Int(0, direccion.y);

                    if (!CeldaTransitable(lateralX) || !CeldaTransitable(lateralY))
                    {
                        continue;
                    }
                }

                float costoMovimiento =direccion.x != 0 && direccion.y != 0 ? 1.41421356f : 1f;

                float nuevoCosto = costos[actual.x, actual.y] + costoMovimiento;

                if (nuevoCosto >= costos[vecino.x, vecino.y])
                {
                    continue;
                }

                costos[vecino.x, vecino.y] = nuevoCosto;
                anteriores[vecino.x, vecino.y] = actual;
                tieneAnterior[vecino.x, vecino.y] = true;

                if (!abiertas.Contains(vecino))
                    abiertas.Add(vecino);
            }
        }

        if (!encontrada)
            return ruta;

        // Reconstruimos el camino desde el destino hasta el inicio.
        Vector2Int celda = celdaDestino;
        ruta.Add(CeldaAPosicion(celda));

        while (celda != celdaInicio)
        {
            if (!tieneAnterior[celda.x, celda.y])
            {
                ruta.Clear();
                return ruta;
            }

            celda = anteriores[celda.x, celda.y];
            ruta.Add(CeldaAPosicion(celda));
        }

        ruta.Reverse();
        return ruta;
    }

    private void OnDrawGizmosSelected()
    {
        if (celdasBloqueadas == null)
            return;

        for (int x = 0; x < columnas; x++)
        {
            for (int y = 0; y < filas; y++)
            {
                Vector2 centro = origen + new Vector2((x + 0.5f) * tamañoCelda,(y + 0.5f) * tamañoCelda);

                Gizmos.color = celdasBloqueadas[x, y] ? Color.red : Color.green;

                Gizmos.DrawWireCube(centro,Vector3.one * tamañoCelda);
            }
        }
    }


    public bool IntentarObtenerDestinoAleatorio(out Vector2 destino)
    {
        destino = Vector2.zero;

        if (celdasBloqueadas == null || columnas == 0 || filas == 0)
            return false;

        // Calculamos la primera columna que pertenece
        // a la zona de compras
        int primeraColumna = Mathf.CeilToInt(
            (limiteIzquierdoCompras - origen.x) / tamañoCelda);

        primeraColumna = Mathf.Clamp(primeraColumna, 0, columnas);

        // Si no queda espacio para explorar, no hay destino.
        if (primeraColumna >= columnas)
            return false;

        // Buscamos una celda libre únicamente dentro
        // de la zona de compras.
        for (int intento = 0; intento < 100; intento++)
        {
            int x = Random.Range(primeraColumna, columnas);
            int y = Random.Range(0, filas);

            Vector2Int celda = new Vector2Int(x, y);

            if (!CeldaTransitable(celda))
                continue;

            destino = CeldaAPosicion(celda);
            return true;
        }

        return false;
    }

}
