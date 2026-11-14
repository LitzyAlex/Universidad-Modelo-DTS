
using UnityEngine;
using TMPro;
using SimMarket;
using UnityEngine.SceneManagement;

public class ConfiguracionUI : MonoBehaviour
{
    [Header("Campos de configuración")]
    public TMP_InputField inputNumCajas;
    public TMP_InputField inputNumCajasRapidas;
    public TMP_InputField inputLimiteArticulosRapida;
    public TMP_InputField inputMinCapacidadCobro;
    public TMP_InputField inputMaxCapacidadCobro;
    public TMP_InputField inputMaxClientesPorCaja;

    public TMP_InputField inputMaxLlegadasPorIteracion;
    public TMP_InputField inputMinArticulos;
    public TMP_InputField inputMaxArticulos;
    public TMP_InputField inputMinIteracionesPorRecoleccion;
    public TMP_InputField inputMaxIteracionesPorRecoleccion;
    public TMP_InputField inputMinArticulosPorRecoleccion;
    public TMP_InputField inputMaxArticulosPorRecoleccion;

    public TMP_InputField inputProbPreference;
    public TMP_InputField inputIteracionesLlegada;
    public TMP_InputField inputSemilla;

    [Header("Mensajes")]
    public TMP_Text textoError;

    [Header("Botones")]
    public UnityEngine.UI.Button botonIniciar;

    private SimConfig configPredeterminada;
    public SimConfig ConfiguracionElegida { get; private set; }

    private void Start()
    {
        configPredeterminada = new SimConfig();
        CargarValoresPredeterminados();
    }

    private void CargarValoresPredeterminados()
    {
        inputNumCajas.text = configPredeterminada.NumCajas.ToString();
        inputNumCajasRapidas.text = configPredeterminada.NumCajasRapidas.ToString();
        inputLimiteArticulosRapida.text = configPredeterminada.LimiteArticulosRapida.ToString();
        inputMinCapacidadCobro.text = configPredeterminada.MinCapacidadCobro.ToString();
        inputMaxCapacidadCobro.text = configPredeterminada.MaxCapacidadCobro.ToString();
        inputMaxClientesPorCaja.text = configPredeterminada.MaxClientesPorCaja.ToString();

        inputMaxLlegadasPorIteracion.text = configPredeterminada.MaxLlegadasPorIteracion.ToString();
        inputMinArticulos.text = configPredeterminada.MinArticulos.ToString();
        inputMaxArticulos.text = configPredeterminada.MaxArticulos.ToString();
        inputMinIteracionesPorRecoleccion.text = configPredeterminada.MinIteracionesPorRecoleccion.ToString();
        inputMaxIteracionesPorRecoleccion.text = configPredeterminada.MaxIteracionesPorRecoleccion.ToString();
        inputMinArticulosPorRecoleccion.text = configPredeterminada.MinArticulosPorRecoleccion.ToString();
        inputMaxArticulosPorRecoleccion.text = configPredeterminada.MaxArticulosPorRecoleccion.ToString();

        inputProbPreference.text = configPredeterminada.ProbPreference.ToString();
        inputIteracionesLlegada.text = configPredeterminada.IteracionesLlegada.ToString();

        inputSemilla.text = configPredeterminada.Semilla.HasValue
            ? configPredeterminada.Semilla.Value.ToString()
            : "";

        textoError.text = "";
    }


    private bool LeerConfiguracion()
    {
        SimConfig nuevaConfig = new SimConfig();

        int numCajas;
        int numCajasRapidas;
        int limiteArticulosRapida;
        int minCapacidadCobro;
        int maxCapacidadCobro;
        int maxClientesPorCaja;

        int maxLlegadasPorIteracion;
        int minArticulos;
        int maxArticulos;
        int minIteracionesPorRecoleccion;
        int maxIteracionesPorRecoleccion;
        int minArticulosPorRecoleccion;
        int maxArticulosPorRecoleccion;

        double probPreference;
        int iteracionesLlegada;

        if (!int.TryParse(inputNumCajas.text, out numCajas) ||
            !int.TryParse(inputNumCajasRapidas.text, out numCajasRapidas) ||
            !int.TryParse(inputLimiteArticulosRapida.text, out limiteArticulosRapida) ||
            !int.TryParse(inputMinCapacidadCobro.text, out minCapacidadCobro) ||
            !int.TryParse(inputMaxCapacidadCobro.text, out maxCapacidadCobro) ||
            !int.TryParse(inputMaxClientesPorCaja.text, out maxClientesPorCaja) ||
            !int.TryParse(inputMaxLlegadasPorIteracion.text, out maxLlegadasPorIteracion) ||
            !int.TryParse(inputMinArticulos.text, out minArticulos) ||
            !int.TryParse(inputMaxArticulos.text, out maxArticulos) ||
            !int.TryParse(inputMinIteracionesPorRecoleccion.text, out minIteracionesPorRecoleccion) ||
            !int.TryParse(inputMaxIteracionesPorRecoleccion.text, out maxIteracionesPorRecoleccion) ||
            !int.TryParse(inputMinArticulosPorRecoleccion.text, out minArticulosPorRecoleccion) ||
            !int.TryParse(inputMaxArticulosPorRecoleccion.text, out maxArticulosPorRecoleccion) ||
            !double.TryParse(inputProbPreference.text,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out probPreference) ||
            !int.TryParse(inputIteracionesLlegada.text, out iteracionesLlegada))
        {
            textoError.text = "Revisa los campos: todos deben contener números válidos.";
            return false;
        }

        nuevaConfig.NumCajas = numCajas;
        nuevaConfig.NumCajasRapidas = numCajasRapidas;
        nuevaConfig.LimiteArticulosRapida = limiteArticulosRapida;
        nuevaConfig.MinCapacidadCobro = minCapacidadCobro;
        nuevaConfig.MaxCapacidadCobro = maxCapacidadCobro;
        nuevaConfig.MaxClientesPorCaja = maxClientesPorCaja;

        nuevaConfig.MaxLlegadasPorIteracion = maxLlegadasPorIteracion;
        nuevaConfig.MinArticulos = minArticulos;
        nuevaConfig.MaxArticulos = maxArticulos;
        nuevaConfig.MinIteracionesPorRecoleccion = minIteracionesPorRecoleccion;
        nuevaConfig.MaxIteracionesPorRecoleccion = maxIteracionesPorRecoleccion;
        nuevaConfig.MinArticulosPorRecoleccion = minArticulosPorRecoleccion;
        nuevaConfig.MaxArticulosPorRecoleccion = maxArticulosPorRecoleccion;

        nuevaConfig.ProbPreference = probPreference;
        nuevaConfig.IteracionesLlegada = iteracionesLlegada;

        if (string.IsNullOrWhiteSpace(inputSemilla.text))
        {
            nuevaConfig.Semilla = null;
        }
        else
        {
            int semilla;

            if (!int.TryParse(inputSemilla.text, out semilla))
            {
                textoError.text = "La semilla debe ser un número entero o quedar vacía.";
                return false;
            }

            nuevaConfig.Semilla = semilla;
        }

        try
        {
            nuevaConfig.Validar();
        }
        catch (System.Exception error)
        {
            textoError.text = error.Message;
            return false;
        }

        textoError.text = "";

        ConfiguracionElegida = nuevaConfig;
        ConfiguracionCompartida.ConfiguracionElegida = nuevaConfig;

        return true;
    }


    public void AlPresionarIniciar()
    {
        if (!LeerConfiguracion())
        {
            return;
        }

        SceneManager.LoadScene("Market");
    }


    public void RestablecerConfiguracion()
    {
        configPredeterminada = new SimConfig();
        CargarValoresPredeterminados();

        // Limpiar la configuración guardada anteriormente.
        ConfiguracionElegida = null;
        ConfiguracionCompartida.ConfiguracionElegida = null;
    }


    public void SeleccionarTiendaChica()
    {
        configPredeterminada = SimConfig.TiendaChica();
        CargarValoresPredeterminados();
    }

    public void SeleccionarHoraPico()
    {
        configPredeterminada = SimConfig.HoraPico();
        CargarValoresPredeterminados();
    }

    public void SeleccionarSupermercado()
    {
        configPredeterminada = SimConfig.Supermercado();
        CargarValoresPredeterminados();
    }





}
