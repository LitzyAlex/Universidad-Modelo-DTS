
using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraMovimiento : MonoBehaviour
{
    [Header("Movimiento de cámara")]
    [SerializeField] private float velocidad = 8f;

    [Header("Zoom de cámara")]
    [SerializeField] private float velocidadZoom = 5f;
    [SerializeField] private float zoomMinimo = 3f;
    [SerializeField] private float zoomMaximo = 12f;

    private Camera camara;

    private void Start()
    {
        camara = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        MoverCamara();
        ZoomCamara();
    }

    private void MoverCamara()
    {
        Vector2 direccion = Vector2.zero;

        if (Keyboard.current.leftArrowKey.isPressed)
            direccion.x -= 1f;

        if (Keyboard.current.rightArrowKey.isPressed)
            direccion.x += 1f;

        if (Keyboard.current.upArrowKey.isPressed)
            direccion.y += 1f;

        if (Keyboard.current.downArrowKey.isPressed)
            direccion.y -= 1f;

        transform.position += (Vector3)(
            direccion.normalized * velocidad * Time.deltaTime
        );
    }

    private void ZoomCamara()
    {
        // W acerca la cámara.
        if (Keyboard.current.wKey.isPressed)
        {
            camara.orthographicSize -=
                velocidadZoom * Time.deltaTime;
        }

        // S aleja la cámara.
        if (Keyboard.current.sKey.isPressed)
        {
            camara.orthographicSize +=
                velocidadZoom * Time.deltaTime;
        }

        // Limitar el zoom mínimo y máximo.
        camara.orthographicSize = Mathf.Clamp(
            camara.orthographicSize,
            zoomMinimo,
            zoomMaximo
        );
    }
}
