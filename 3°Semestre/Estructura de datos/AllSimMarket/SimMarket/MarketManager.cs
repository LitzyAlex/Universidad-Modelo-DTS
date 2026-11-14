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
        [SerializeField] private float segundosPorIteracion = 1f;

        [Header("Interfaz")]
        [SerializeField] private TMP_Text textoInformacion;

        [Header("Visuales de las cajas")]
        [SerializeField] private GameObject prefabCaja;
        [SerializeField] private Sprite imagenCajaNormal;
        [SerializeField] private Sprite imagenCajaRapida;
        [SerializeField] private float separacionVertical = 1.3f;
        [SerializeField] private float posicionX = -5f;

        [Header("Visuales de clientes")]
        [SerializeField] private GameObject prefabCliente;
        [SerializeField] private Sprite[] imagenesClientesNormales;
        [SerializeField] private Sprite imagenClientePreference;
        [SerializeField] private Vector2 posicionEntrada = new Vector2(-7f, 0f);
        [SerializeField] private float separacionClientes = 0.6f;

        [Header("Navegación")]
        [SerializeField] private NavegacionSupermercado navegacion;

        // Punto donde termina la ruta A* y empieza el recorrido directo.
        [SerializeField] private Vector2 puntoConexionCajas = new Vector2(-2.5f, 0f);

        private readonly HashSet<int> clientesYendoACaja = new();
        // Clientes que están regresando a la salida para desaparecer.
        private readonly HashSet<int> clientesAbandonando = new();
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

        SimConfig config;

        if (ConfiguracionCompartida.ConfiguracionElegida != null)
        {
            config = ConfiguracionCompartida.ConfiguracionElegida;
        }
        else
        {
            config = new SimConfig
            {
                NumCajas = numCajas,
                NumCajasRapidas = numCajasRapidas,
                LimiteArticulosRapida = limiteRapida
            };
        }

        sim = new Simulador(config);
        Debug.Log("Cajas configuradas: " + config.NumCajas);
        Debug.Log("Cajas rápidas: " + config.NumCajasRapidas);
        Debug.Log("Límite de artículos: " + config.LimiteArticulosRapida);

        CrearCajasVisuales();

        //Conrola clientes
            sim.ClienteLlego += CrearClienteVisual;
            sim.ClienteSeFormo += FormarClienteVisual;
            sim.ClienteSalio += RetirarClienteVisual;
            sim.ClienteAbandono += RetirarClienteAbandonadoVisual;
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
        if (sim == null || sim.Terminada)
        {
            automatico = false;
            return;
        }

        // Evita avanzar mientras los clientes se están moviendo.
        if (esperandoMovimiento)
            return;

        esperandoMovimiento = true;

        sim.EjecutarIteracion();
        ActualizarPanel();

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
            $"Clientes en fila: {sim.ClientesEnFilas}\n" +
            $"Clientes atendidos: {sim.TotalAtendidos}\n" +
            $"Clientes que abandonaron: {sim.ClientesAbandonaron}";

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
            etiqueta.transform.localPosition = new Vector3(0f, -0.11f, 0f);
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
                if (caja.EsRapida)
                {
                    sprite.sprite = imagenCajaRapida;
                }
                else
                {
                    sprite.sprite = imagenCajaNormal;
                }

                // Conservamos los colores originales de las imágenes.
                sprite.color = Color.white;
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
        // Añadimos el componente que permite seleccionar al cliente.
        ClienteSeleccionable seleccionable =
            visual.GetComponent<ClienteSeleccionable>();

        if (seleccionable == null)
        {
            seleccionable = visual.AddComponent<ClienteSeleccionable>();
        }

        // Asociamos el objeto visual con su cliente real de SimMarket.
        seleccionable.Configurar(cliente);

        visual.name = $"ClienteVisual_{cliente.Id}";
        clientesVisuales[cliente.Id] = visual;

        ClienteMovimiento movimiento = visual.GetComponent<ClienteMovimiento>();

        if (movimiento != null && navegacion != null)
        {
            movimiento.IniciarExploracion(navegacion);
        }



        SpriteRenderer sprite = visual.GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
            if (cliente.EsPreference)
            {
                // Los clientes Preference siempre usan su imagen.
                sprite.sprite = imagenClientePreference;
            }
            else
            {
                // Elegimos una imagen aleatoria para los clientes normales.
                if (imagenesClientesNormales.Length > 0)
                {
                    int indiceAleatorio = Random.Range(
                        0,
                        imagenesClientesNormales.Length
                    );

                    sprite.sprite = imagenesClientesNormales[indiceAleatorio];
                }
            }

            // Conservamos los colores originales de las imágenes.
            sprite.color = Color.white;
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

        Vector2 destino = posicionCaja + new Vector3(3f + posicionEnFila * separacionClientes,-0.5f,0f);

        ClienteMovimiento movimiento = visual.GetComponent<ClienteMovimiento>();

        if (movimiento != null)
        {
            movimiento.IrACola(destino, puntoConexionCajas);
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
            Vector2 destino = posicionCaja + new Vector3(3f + i * separacionClientes,-0.5f, 0f );

            movimiento.IrACola(destino, puntoConexionCajas);
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



    private void MoverClientesVisuales()
    {
        // Guardamos los clientes que llegaron para eliminarlos
        // después del recorrido del diccionario.
        List<int> clientesQueLlegaron = new List<int>();

        foreach (var par in clientesVisuales)
        {
            if (par.Value == null)
            {
                if (clientesAbandonando.Contains(par.Key))
                    clientesQueLlegaron.Add(par.Key);

                continue;
            }

            ClienteMovimiento movimiento =
                par.Value.GetComponent<ClienteMovimiento>();

            if (movimiento == null)
                continue;

            movimiento.ActualizarExploracion();

            // Solo eliminamos a los clientes que abandonaron
            // y terminaron de regresar a la salida.
            if (clientesAbandonando.Contains(par.Key) &&
                !movimiento.EstaMoviendose)
            {
                clientesQueLlegaron.Add(par.Key);
            }
        }

        // Eliminamos los objetos después del foreach para
        // evitar modificar el diccionario mientras lo recorremos.
        foreach (int id in clientesQueLlegaron)
        {
            if (clientesVisuales.TryGetValue(id, out GameObject visual))
            {
                if (visual != null)
                    Destroy(visual);

                clientesVisuales.Remove(id);
            }

            // Quitamos al cliente de los registros de movimiento
            // cuando ya llegó a la salida.
            clientesAbandonando.Remove(id);
            clientesYendoACaja.Remove(id);
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

        // Si ya no quedan clientes y terminaron las llegadas,
        // detenemos la reproducción automática.
        if (sim.Terminada)
        {
            automatico = false;
            ActualizarPanel();
            Debug.Log("La simulación terminó.");
        }
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


    private void RetirarClienteAbandonadoVisual(Cliente cliente)
    {
        // Buscamos el objeto visual del cliente.
        if (!clientesVisuales.TryGetValue(cliente.Id, out GameObject visual))
            return;

        if (visual == null)
        {
            clientesVisuales.Remove(cliente.Id);
            return;
        }

        // Lo mantenemos registrado para que la simulación
        // espere hasta que llegue a la salida.
        clientesYendoACaja.Add(cliente.Id);

        // Marcamos al cliente para eliminarlo cuando llegue a la salida.
        clientesAbandonando.Add(cliente.Id);

        ClienteMovimiento movimiento =
            visual.GetComponent<ClienteMovimiento>();

        if (movimiento != null)
        {
            if (navegacion != null)
            {
                movimiento.IrA(posicionEntrada);
            }
            else
            {
                movimiento.IrDirectoA(posicionEntrada);
            }
        }
        else
        {
            // Si no tiene el componente de movimiento, lo eliminamos.
            clientesAbandonando.Remove(cliente.Id);
            clientesYendoACaja.Remove(cliente.Id);
            clientesVisuales.Remove(cliente.Id);
            Destroy(visual);
        }
    }



}

*/