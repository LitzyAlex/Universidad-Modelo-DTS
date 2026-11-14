
using UnityEngine;

public class AnimacionCliente : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ClienteMovimiento movimiento;

    [Header("Pasitos")]
    [SerializeField] private float distanciaPorPaso = 1.2f;
    [SerializeField] private float anguloBalanceo = 12f;

    [Header("Saltitos")]
    [SerializeField] private float alturaRebote = 0.18f;
    [SerializeField] private float intensidadEstiramiento = 0.10f;

    [Header("Suavidad")]
    [SerializeField] private float velocidadRecuperacion = 8f;

    private Vector3 escalaOriginal;
    private Vector3 posicionOriginal;

    private float distanciaAnterior;
    private float fasePasos;
    private bool caminandoAntes;

    private void Awake()
    {
        escalaOriginal = transform.localScale;
        posicionOriginal = transform.localPosition;

        if (movimiento == null)
            movimiento = GetComponentInParent<ClienteMovimiento>();

        if (movimiento != null)
            distanciaAnterior = movimiento.DistanciaRecorridaTotal;
    }

    private void Update()
    {
        if (movimiento == null)
            return;

        float distanciaActual = movimiento.DistanciaRecorridaTotal;

        // Calculamos cuánto avanzó desde la última actualización.
        float distancia = Mathf.Max(
            0f,
            distanciaActual - distanciaAnterior
        );

        distanciaAnterior = distanciaActual;

        if (movimiento.EstaMoviendose)
        {
            // Si acaba de empezar a caminar, comenzamos un ciclo nuevo.
            if (!caminandoAntes)
                fasePasos = 0f;

            // Un paso completo se basa en la distancia recorrida.
            fasePasos += distancia * Mathf.PI / distanciaPorPaso;

            AnimarCaminata();
            caminandoAntes = true;
        }
        else
        {
            VolverANormal();
            caminandoAntes = false;
        }
    }

    private void AnimarCaminata()
    {
        // El cuerpo sube y baja con cada paso.
        float salto = Mathf.Abs(Mathf.Sin(fasePasos));

        transform.localPosition = posicionOriginal
            + Vector3.up * salto * alturaRebote;

        // El óvalo alterna su inclinación.
        float balanceo = Mathf.Sin(fasePasos * 0.5f);

        transform.localRotation = Quaternion.Euler(
            0f,
            0f,
            balanceo * anguloBalanceo
        );

        // Se aplasta al aterrizar y recupera su forma al despegar.
        float aplastamiento = (1f - salto)
            * intensidadEstiramiento;

        transform.localScale = new Vector3(
            escalaOriginal.x * (1f + aplastamiento),
            escalaOriginal.y * (1f - aplastamiento),
            escalaOriginal.z
        );
    }

    private void VolverANormal()
    {
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.identity,
            Time.deltaTime * velocidadRecuperacion
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            posicionOriginal,
            Time.deltaTime * velocidadRecuperacion
        );

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            escalaOriginal,
            Time.deltaTime * velocidadRecuperacion
        );
    }
}
