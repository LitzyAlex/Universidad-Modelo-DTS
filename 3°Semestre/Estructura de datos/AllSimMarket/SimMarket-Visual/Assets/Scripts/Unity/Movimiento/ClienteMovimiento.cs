
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClienteMovimiento : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float distanciaLlegada = 0.05f;

    [Header("Pausas al explorar")]
    [SerializeField] private float esperaMinima = 1f;
    [SerializeField] private float esperaMaxima = 3f;

    private bool esperandoDestino;
    private float temporizadorEspera;

    private Rigidbody2D cuerpo;
    private NavegacionSupermercado navegacion;

    private List<Vector3> ruta = new List<Vector3>();
    private int indicePunto;

    private bool moviendose;
    private bool explorando;

    public bool EstaMoviendose => moviendose;
    // Distancia que recorrió el cliente durante el último frame.
    public float DistanciaRecorridaEsteFrame { get; private set; }
    public float DistanciaRecorridaTotal { get; private set; }

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        cuerpo.gravityScale = 0f;
        cuerpo.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        DistanciaRecorridaEsteFrame = 0f;
        if (!moviendose || indicePunto >= ruta.Count)
            return;

        Vector2 destino = ruta[indicePunto];

        Vector2 nuevaPosicion = Vector2.MoveTowards(
            cuerpo.position,
            destino,
            velocidad * Time.fixedDeltaTime
        );

        float distancia = Vector2.Distance(cuerpo.position, nuevaPosicion);

        DistanciaRecorridaEsteFrame = distancia;
        DistanciaRecorridaTotal += distancia;

        cuerpo.MovePosition(nuevaPosicion);

        if (Vector2.Distance(nuevaPosicion, destino) <= distanciaLlegada)
        {
            cuerpo.position = destino;
            indicePunto++;

            if (indicePunto >= ruta.Count)
                moviendose = false;
        }
    }

    // Activa la exploración durante las compras.
    public void IniciarExploracion(NavegacionSupermercado mapa)
    {
        navegacion = mapa;
        explorando = true;

        ElegirDestinoExploracion();
    }

    // Se llama mientras el cliente está comprando.
    public void ActualizarExploracion()
    {
        // Solo exploramos si el cliente está en modo exploración.
        if (!explorando || moviendose || navegacion == null)
            return;

        // Si todavía no comenzó a esperar, iniciamos una pausa.
        if (!esperandoDestino)
        {
            esperandoDestino = true;
            temporizadorEspera = Random.Range(esperaMinima,esperaMaxima);

            return;
        }

        // Reducimos el tiempo de espera.
        temporizadorEspera -= Time.deltaTime;

        // Cuando termina la pausa, elegimos otro destino.
        if (temporizadorEspera <= 0f)
        {
            esperandoDestino = false;
            ElegirDestinoExploracion();
        }
    }

    private void ElegirDestinoExploracion()
    {
        if (!navegacion.IntentarObtenerDestinoAleatorio(
            out Vector2 destino))
        {
            return;
        }

        List<Vector3> nuevaRuta = navegacion.ObtenerRuta(
            cuerpo.position,
            destino
        );

        if (nuevaRuta.Count > 0)
            SeguirRuta(nuevaRuta);
    }

    // Se utiliza para explorar o desplazarse con una ruta calculada.
    public void SeguirRuta(List<Vector3> nuevaRuta)
    {
        if (nuevaRuta == null || nuevaRuta.Count == 0)
        {
            Detener();
            return;
        }

        ruta = new List<Vector3>(nuevaRuta);
        indicePunto = 0;
        moviendose = true;
    }

    // Movimiento normal: necesita que el destino esté dentro
    // de la cuadrícula de navegación.
    public void IrA(Vector2 destino)
    {
        explorando = false;

        if (navegacion == null)
        {
            Detener();
            return;
        }

        SeguirRuta(
            navegacion.ObtenerRuta(cuerpo.position, destino)
        );
    }

    // Movimiento hacia una caja:
    // 1. Usa A* hasta el punto de conexión.
    // 2. Continúa directamente hasta el lugar de la fila.
    public void IrACola(Vector2 destino, Vector2 puntoConexion)
    {
        explorando = false;

        if (navegacion == null)
        {
            Detener();
            return;
        }

        // Calculamos una ruta que termina en el área de conexión.
        List<Vector3> nuevaRuta = navegacion.ObtenerRuta(
            cuerpo.position,
            puntoConexion
        );

        if (nuevaRuta == null || nuevaRuta.Count == 0)
        {
            Detener();
            return;
        }

        // Añadimos la posición de la fila al final.
        // El tramo entre estos dos últimos puntos será directo.
        Vector3 ultimoPunto = nuevaRuta[nuevaRuta.Count - 1];

        if (Vector2.Distance(ultimoPunto, destino) > distanciaLlegada)
        {
            nuevaRuta.Add(new Vector3(destino.x, destino.y, 0f));
        }

        SeguirRuta(nuevaRuta);
    }

    // Cancela la exploración y cualquier ruta actual.
    public void Detener()
    {
        moviendose = false;
        ruta.Clear();
        indicePunto = 0;
    }

    public void IrDirectoA(Vector2 destino)
    {
        explorando = false;

        ruta.Clear();
        ruta.Add(new Vector3(destino.x, destino.y, 0f));

        indicePunto = 0;
        moviendose = true;
    }
}
