
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorClientes : MonoBehaviour
{
    [SerializeField] private Camera camara;

    private void Awake()
    {
        if (camara == null)
            camara = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (camara == null)
            return;

        // Convertimos la posición del mouse a coordenadas del mundo.
        Vector3 posicionMouse = Mouse.current.position.ReadValue();

        Vector3 posicionMundo = camara.ScreenToWorldPoint(
            new Vector3(
                posicionMouse.x,
                posicionMouse.y,
                -camara.transform.position.z
            )
        );

        // Buscamos el collider que está debajo del cursor.
        Collider2D colliderCliente = Physics2D.OverlapPoint(
            new Vector2(posicionMundo.x, posicionMundo.y)
        );

        if (colliderCliente == null)
            return;

        // El collider puede estar en el objeto principal o en un hijo.
        ClienteSeleccionable seleccionable =
            colliderCliente.GetComponentInParent<ClienteSeleccionable>();

        if (seleccionable != null)
        {
            Debug.Log("¡Cliente seleccionado!");
            seleccionable.Seleccionar();
        }
    }
}
