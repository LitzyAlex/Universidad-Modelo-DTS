
using UnityEngine;
using SimMarket;

public class ClienteSeleccionable : MonoBehaviour
{
    private Cliente cliente;

    public void Configurar(Cliente datosCliente)
    {
        cliente = datosCliente;
    }

    public void Seleccionar()
    {
        if (cliente == null)
        {
            Debug.LogWarning("Este cliente no tiene sus datos asignados.");
            return;
        }

        if (PanelClienteUI.Instance != null)
        {
            PanelClienteUI.Instance.Mostrar(cliente);
        }
        else
        {
            Debug.LogWarning("No se encontró PanelClienteUI en la escena.");
        }
    }
}
