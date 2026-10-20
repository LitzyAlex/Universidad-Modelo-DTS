/*
using UnityEngine;
using SimMarket;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class MarketManager : MonoBehaviour
{
    // Clase de Unity. Conecta el Simulador con la escena
    // Solo llama a EjecutarIteracion() y dibuja el estado
        [Header("Parámetros")]
        [SerializeField] private int numCajas = 5;
        [SerializeField] private int numCajasRapidas = 2;
        [SerializeField] private int limiteRapida = 5;
        [SerializeField] private int maxClientesPorCaja = 6;
        [SerializeField] private int iteracionesLlegada = 30;
        [SerializeField] private float segundosPorIteracion = 1f;

        [Header("Interfaz")]
        [SerializeField] private TMP_Text textoInformacion;

        [Header("Visuales de las cajas")]
        [SerializeField] private GameObject prefabCaja;
        [SerializeField] private float separacionVertical = 1.3f;
        [SerializeField] private float posicionX = -5f;

        [Header("Visuales de clientes")]
        [SerializeField] private GameObject prefabCliente;
        [SerializeField] private Vector2 posicionEntrada = new Vector2(-7f, 0f);
        [SerializeField] private float separacionClientes = 0.6f;

        [Header("Navegación")]
        [SerializeField] private NavegacionSupermercado navegacion;

        private readonly HashSet<int> clientesYendoACaja = new();
        // El índice 0 corresponde al lugar más cercano a la caja.
        private readonly Dictionary<int, List<int>> filasPorCaja = new();

        private readonly Dictionary<int, GameObject> clientesVisuales = new();
        private Transform[] visualesCajas;

        private Simulador sim;
        private bool automatico;
        private float temporizador;
        private TextMeshPro[] textosCajas;
        private bool esperandoMovimiento;

    private void Start()
        {
            var config = new SimConfig
            {
                NumCajas = numCajas,
                NumCajasRapidas = numCajasRapidas,
                LimiteArticulosRapida = limiteRapida,
                MaxClientesPorCaja = maxClientesPorCaja,
                IteracionesLlegada = iteracionesLlegada
            };
            sim = new Simulador(config);
            CrearCajasVisuales();

        //Conrola clientes
            sim.ClienteLlego += CrearClienteVisual;
            sim.ClienteSeFormo += FormarClienteVisual;
            sim.ClienteSalio += RetirarClienteVisual;
            sim.ClienteAbandono += RetirarClienteAbandonoVisual;
    }


    private void Update()
    {
        MoverClientesVisuales();

        // No iniciar otra iteracion mientras haya movimientos pendientes
        if (esperandoMovimiento)
        {
            return;
        }

        if (!automatico)
        {
            return;
        }

        temporizador += Time.deltaTime;

        if (temporizador >= segundosPorIteracion)
        {
            temporizador = 0f;
            SiguienteIteracion();
        }
    }

    public void SiguienteIteracion()
    {
        // Evita avanzar mientras los clientes se están moviendo.
        if (esperandoMovimiento || sim.Terminada)
            return;

        esperandoMovimiento = true;

        sim.EjecutarIteracion();
        ActualizarPanel();

        // Cuando la simulación termina se detiene el modo automático
        if (sim.Terminada)
            automatico = false;

        StartCoroutine(EsperarMovimiento());
    }

    public void Reproducir() => automatico = true;
    public void Pausar() => automatico = false;

    //Refresca textos del panel: iteracion, en mercado, atendidos, estado de cada caja
    private void ActualizarPanel()
    {
        textoInformacion.text =
            $"Iteración: {sim.Iteracion}\n" +
            $"Clientes en tienda: {sim.ClientesEnTienda}\n" +
            $"Clientes comprando: {sim.ClientesComprando}\n" +
            $"Clientes en filas: {sim.ClientesEnFilas}\n" +
            $"Clientes atendidos: {sim.TotalAtendidos}\n" +
            $"Clientes que abandonaron: {sim.ClientesAbandonaron}" +
            (sim.Terminada ? "\nSimulación terminada" : "");

        Debug.Log($"Iter {sim.Iteracion} | comprando {sim.ClientesComprando} | filas {sim.ClientesEnFilas} | atendidos {sim.TotalAtendidos}");

        for (int i = 0; i < sim.NumCajas; i++)
        {
            Debug.Log(sim.ObtenerCaja(i));
        }
        ActualizarCajasVisuales();
    }

    //Cajas
    private void CrearCajasVisuales()
    {
        float posicionInicialY =(sim.NumCajas - 1) * separacionVertical / 2f;
        textosCajas = new TextMeshPro[sim.NumCajas];
        visualesCajas = new Transform[sim.NumCajas];

        for (int i = 0; i < sim.NumCajas; i++)
        {
            Caja caja = sim.ObtenerCaja(i);

            float posicionY = posicionInicialY - i * separacionVertical;

            Vector3 posicion = new Vector3(posicionX,posicionY,0f);

            GameObject visual = Instantiate(prefabCaja,posicion,Quaternion.identity);

            visual.name = $"CajaVisual_{caja.Id}";
            //Para el texto de las cajas
            GameObject etiqueta = new GameObject("Etiqueta");
            etiqueta.transform.SetParent(visual.transform, false);
            etiqueta.transform.localPosition = Vector3.zero;
            etiqueta.transform.localScale = new Vector3(1f / visual.transform.localScale.x,1f / visual.transform.localScale.y,1f / visual.transform.localScale.z);


            TextMeshPro texto = etiqueta.AddComponent<TextMeshPro>();
            texto.text = $"Caja {caja.Id}\n{(caja.EsRapida ? "RAPIDA" : "NORMAL")}";
            texto.alignment = TextAlignmentOptions.Center;
            texto.fontSize = 2;
            texto.color = Color.black;

            textosCajas[i] = texto;
            visualesCajas[i] = visual.transform;

            SpriteRenderer sprite = visual.GetComponent<SpriteRenderer>();

            if (sprite != null)
            {
                sprite.color = caja.EsRapida? new Color(0.3f, 0.8f, 0.5f): new Color(0.3f, 0.55f, 0.9f);
            }
        }
    }

    private void ActualizarCajasVisuales()
    {
        for (int i = 0; i < sim.NumCajas; i++)
        {
            Caja caja = sim.ObtenerCaja(i);

            string tipo = caja.EsRapida ? "RAPIDA" : "NORMAL";
            string estado;

            if (caja.ClienteAlFrente == null)
            {
                estado = "Sin clientes";
            }
            else
            {
                estado = $"Articulos restantes: {caja.ClienteAlFrente.ArticulosPorCobrar}";
            }

            textosCajas[i].text =
                $"Caja {caja.Id} · {tipo}\n" +
                $"Capacidad: {caja.CapacidadCobro} artículos/iteración\n" +
                $"Clientes en fila: {caja.ClientesEnEspera}\n" +
                estado;
        }
    }

    //Clientes
    private void CrearClienteVisual(Cliente cliente)
    {
        GameObject visual = Instantiate(prefabCliente,new Vector3(posicionEntrada.x, posicionEntrada.y, 0f),Quaternion.identity);

        visual.name = $"ClienteVisual_{cliente.Id}";
        clientesVisuales[cliente.Id] = visual;

        ClienteMovimiento movimiento = visual.GetComponent<ClienteMovimiento>();

        if (movimiento != null && navegacion != null)
        {
            movimiento.IniciarExploracion(navegacion);
        }

        SpriteRenderer sprite = visual.GetComponent<SpriteRenderer>();

        if (sprite != null && cliente.EsPreference)
        {
            sprite.color = Color.yellow;
        }
    }


    private void FormarClienteVisual(Cliente cliente, Caja caja)
    {
        if (!clientesVisuales.TryGetValue(cliente.Id, out GameObject visual))
            return;

        int indiceCaja = caja.Id - 1;

        if (indiceCaja < 0 || indiceCaja >= visualesCajas.Length)
            return;

        // Creamos la lista de la caja si todavía no existe
        if (!filasPorCaja.ContainsKey(caja.Id))
            filasPorCaja[caja.Id] = new List<int>();

        List<int> fila = filasPorCaja[caja.Id];

        // Evitamos agregar al mismo cliente dos veces
        if (!fila.Contains(cliente.Id))
            fila.Add(cliente.Id);

        // Su posición depende de cuántos clientes tiene delante
        int posicionEnFila = fila.IndexOf(cliente.Id);

        Vector3 posicionCaja = visualesCajas[indiceCaja].position;

        Vector2 destino = posicionCaja + new Vector3(1.7f + posicionEnFila * separacionClientes,0f,0f);

        ClienteMovimiento movimiento = visual.GetComponent<ClienteMovimiento>();

        if (movimiento != null)
        {
            movimiento.IrA(destino);
            clientesYendoACaja.Add(cliente.Id);
        }
    }


    private void ReorganizarFila(Caja caja)
    {
        if (!filasPorCaja.TryGetValue(caja.Id, out List<int> fila))
            return;

        int indiceCaja = caja.Id - 1;

        if (indiceCaja < 0 || indiceCaja >= visualesCajas.Length)
            return;

        Vector3 posicionCaja = visualesCajas[indiceCaja].position;

        // Recorremos los clientes en su orden actual
        for (int i = 0; i < fila.Count; i++)
        {
            int idCliente = fila[i];

            if (!clientesVisuales.TryGetValue(idCliente, out GameObject visual))
                continue;

            if (visual == null)
                continue;

            ClienteMovimiento movimiento =visual.GetComponent<ClienteMovimiento>();

            if (movimiento == null)
                continue;

            // Cada cliente avanza al lugar que le corresponde
            Vector2 destino = posicionCaja + new Vector3(1.7f + i * separacionClientes,0f, 0f );

            movimiento.IrA(destino);
            clientesYendoACaja.Add(idCliente);
        }
    }


    private void RetirarClienteVisual(Cliente cliente, Caja caja)
    {
        // Quitamos al cliente que terminó de pagar
        if (clientesVisuales.TryGetValue(cliente.Id, out GameObject visual))
        {
            if (visual != null)
                Destroy(visual);

            clientesVisuales.Remove(cliente.Id);
        }

        clientesYendoACaja.Remove(cliente.Id);

        // Lo quitamos de la fila de su caja
        if (filasPorCaja.TryGetValue(caja.Id, out List<int> fila))
        {
            fila.Remove(cliente.Id);

            // Los clientes restantes avanzan un lugar
            ReorganizarFila(caja);
        }
    }


    private void RetirarClienteAbandonoVisual(Cliente cliente)
    {
        // El cliente se fue sin comprar: solo se quita su visual
        if (clientesVisuales.TryGetValue(cliente.Id, out GameObject visual))
        {
            if (visual != null)
                Destroy(visual);

            clientesVisuales.Remove(cliente.Id);
        }

        clientesYendoACaja.Remove(cliente.Id);
    }


    private void MoverClientesVisuales()
    {
        foreach (var par in clientesVisuales)
        {
            if (par.Value == null)
                continue;

            ClienteMovimiento movimiento =
                par.Value.GetComponent<ClienteMovimiento>();

            if (movimiento != null)
            {
                movimiento.ActualizarExploracion();
            }
        }
    }

    private IEnumerator EsperarMovimiento()
    {
        while (HayClientesEnMovimiento())
        {
            yield return null;
        }

        clientesYendoACaja.Clear();
        esperandoMovimiento = false;
    }

    private bool HayClientesEnMovimiento()
    {
        foreach (int id in clientesYendoACaja)
        {
            if (!clientesVisuales.TryGetValue(id, out GameObject visual))
                continue;

            if (visual == null)
                continue;

            ClienteMovimiento movimiento = visual.GetComponent<ClienteMovimiento>();

            if (movimiento != null && movimiento.EstaMoviendose)
                return true;
        }

        return false;
    }


}
*/