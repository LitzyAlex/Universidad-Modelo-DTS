
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimMarket;

public class PanelClienteUI : MonoBehaviour
{
    public static PanelClienteUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text tituloCliente;
    [SerializeField] private TMP_Text textoDatos;
    [SerializeField] private Button botonCerrar;

    // Guardamos al cliente que estamos consultando.
    private Cliente clienteSeleccionado;

    private void Awake()
    {
        Instance = this;

        botonCerrar.onClick.AddListener(Cerrar);

        panel.SetActive(false);
    }

    private void Update()
    {
        // Actualizamos los datos solamente si el panel está abierto.
        if (panel.activeSelf && clienteSeleccionado != null)
        {
            ActualizarDatos();
        }
    }

    public void Mostrar(Cliente cliente)
    {
        if (cliente == null)
            return;

        // Guardamos el cliente seleccionado.
        clienteSeleccionado = cliente;

        panel.SetActive(true);

        ActualizarDatos();
    }

    private void ActualizarDatos()
    {
        tituloCliente.text = "Cliente #" + clienteSeleccionado.Id;

        textoDatos.text =
            "Tipo: " + (clienteSeleccionado.EsPreference
                ? "Preference"
                : "Normal") + "\n" +

            "Estado: " + clienteSeleccionado.Estado + "\n" +

            "Artículos totales: " +
            clienteSeleccionado.ArticulosTotales + "\n" +

            "Artículos recolectados: " +
            clienteSeleccionado.ArticulosRecolectados + "\n" +

            "Artículos por recolección: " +
            clienteSeleccionado.ArticulosPorRecoleccion + "\n" +

            "Iteraciones por recolección: " +
            clienteSeleccionado.IteracionesPorRecoleccion;

    }

    public void Cerrar()
    {
        panel.SetActive(false);

        // Dejamos de consultar al cliente seleccionado.
        clienteSeleccionado = null;
    }
}
