using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleSyncChunks : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text subtitleText;
    public float fadeDuration = 1f;
    public float typingSpeed = 0.05f;      // Tiempo entre letras
    public float delayBetweenChunks = 1.30f;  // Tiempo que permanece cada bloque antes de pasar al siguiente

    // Fragmentos pensados para ocupar 1–2 líneas cada uno
    private string[] subtitleChunks = new string[]
    {
        "Bienvenido a este sistema de adiestramiento de armas.",
        "Aquí podrás entrenar para mejorar tu tiro.",
        "Enfrente tuyo hay un arma.",
        "Tu objetivo es disparar a la diana que está en tu dirección.",
        "Mientras más al centro le des, más puntaje vas a obtener.",
        "¡Vamos! Es hora de comenzar."
    };

    void Start()
    {
        subtitleText.text = "";
        subtitleText.alpha = 0f;
        audioSource.Play();

        StartCoroutine(FadeIn());
        StartCoroutine(PlaySubtitles());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleText.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        subtitleText.alpha = 1f;
    }

    IEnumerator PlaySubtitles()
    {
        foreach (string chunk in subtitleChunks)
        {
            subtitleText.text = "";
            foreach (char letter in chunk.ToCharArray())
            {
                subtitleText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(delayBetweenChunks);
        }

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleText.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        subtitleText.alpha = 0f;
        subtitleText.text = "";
    }
}
