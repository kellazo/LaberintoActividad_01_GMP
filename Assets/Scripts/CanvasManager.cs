using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Vida del Player")]
    [SerializeField]
    private Image healthBar;
    //[SerializeField] private float maxLife = 100f;

    //Encapsular para que des de otro script se pueda acceder
    public Image HealthBar { get => healthBar; }

    /// Actualiza la barra de vida en base a la vida actual y máxima.

    // =========== Overlay de Daño ===========
    [Header("Pantalla de daño")]
    [SerializeField] private Image damageOverlay;      // Una UI Image roja que ocupe toda la pantalla
    [SerializeField] private float damageOverlayAlpha = 0.3f; // nivel de transparencia
    [SerializeField] private float damageOverlayTime = 1.0f; // cuánto tiempo mostrarlo

    // =========== Sonido de Daño ============
    [Header("Sonido de daño")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hurtClip;

    [Header("Hint UI")]
    [SerializeField] private TextMeshProUGUI hintText;

    [SerializeField] private GameObject gameOverPanel; // Panel que muestra "Game Over"

    private void Start()
    {
        // Asegúrate de que el panel esté oculto al inicio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false); // Oculto al inicio
        }
    }

    public void UpdateHealthBar(float currentLife, float maxLife)
    {
        // El fillAmount va de 0 a 1
        float fillValue = currentLife / maxLife;
        healthBar.fillAmount = fillValue;
    }

    // Muestra un overlay rojo y reproduce sonido de dolor (si está asignado),
    // luego quita el overlay tras 'damageOverlayTime'.
    public void ShowDamageFeedback()
    {
        // Reproduce sonido
        if (audioSource != null && hurtClip != null)
            audioSource.PlayOneShot(hurtClip);

        // Muestra overlay
        if (damageOverlay != null)
            StartCoroutine(ShowOverlayCoroutine());
    }
    private IEnumerator ShowOverlayCoroutine()
    {
        // Subir alpha a damageOverlayAlpha
        SetOverlayAlpha(damageOverlayAlpha);

        // Esperar
        yield return new WaitForSeconds(damageOverlayTime);

        // Bajar alpha a 0
        SetOverlayAlpha(0f);
    }
    private void SetOverlayAlpha(float alpha)
    {
        if (damageOverlay == null) return;
        Color c = damageOverlay.color;
        c.a = alpha;
        damageOverlay.color = c;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Muestra el texto de hint (ej: "Pulsa E...").
    public void ShowHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
            hintText.gameObject.SetActive(true);
        }
    }
    // Oculta el texto de hint.
    public void HideHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
    }
    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
