
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelDesplegable : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button botonPestana;
    [SerializeField] private float duracion = 0.3f;

    private Vector2 posicionAbierta;
    private Vector2 posicionCerrada;
    private bool abierto = false;
    private Coroutine animacion;


    private void Start()
    {
        posicionAbierta = panel.anchoredPosition;

        // Subir el panel hasta dejar visible la pestaña.
        float alturaOculta = panel.rect.height - 40f;

        posicionCerrada = posicionAbierta +
            new Vector2(0, alturaOculta);

        panel.anchoredPosition = posicionCerrada;

        botonPestana.onClick.AddListener(AlternarPanel);
    }


    public void AlternarPanel()
    {
        abierto = !abierto;

        if (animacion != null)
            StopCoroutine(animacion);

        Vector2 destino = abierto
            ? posicionAbierta
            : posicionCerrada;

        animacion = StartCoroutine(MoverPanel(destino));
    }

    private IEnumerator MoverPanel(Vector2 destino)
    {
        Vector2 inicio = panel.anchoredPosition;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float progreso = Mathf.Clamp01(tiempo / duracion);
            progreso = progreso * progreso * (3f - 2f * progreso);

            panel.anchoredPosition =
                Vector2.Lerp(inicio, destino, progreso);

            yield return null;
        }

        panel.anchoredPosition = destino;
        animacion = null;
    }
}
