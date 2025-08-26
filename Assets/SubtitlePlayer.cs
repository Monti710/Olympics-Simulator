using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;  // Necesario para usar Button

public class SubtitlePlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Header("Text Settings")]
    public TMP_Text subtitleText;
    public float fadeDuration = 1f;
    public float typingSpeed = 0.05f;
    public float delayBetweenChunks = 1.30f;

    [Header("Subtitles Content")]
    [TextArea(3, 10)]
    public string[] subtitleChunks = new string[]
    {
        "Bienvenido a este sistema de adiestramiento de armas.",
        "Aquí podrás entrenar para mejorar tu tiro.",
        "Enfrente tuyo hay un arma.",
        "Tu objetivo es disparar a la diana que está en tu dirección.",
        "Mientras más al centro le des, más puntaje vas a obtener.",
        "¡Vamos! Es hora de comenzar."
    };

    [Header("UI Button (to disable while playing)")]
    public Button playButton;

    private Coroutine currentFadeInCoroutine;
    private Coroutine currentSubtitlesCoroutine;
    public bool isPlaying = false;

    // Método público para asignar al botón OnClick
    public void PlayAudioAndSubtitles()
    {
        // Primero detener cualquier otro SubtitlePlayer activo en la escena para evitar interferencias
        SubtitlePlayer[] allSubtitlePlayers = FindObjectsOfType<SubtitlePlayer>();
        foreach (SubtitlePlayer sp in allSubtitlePlayers)
        {
            if (sp != this && sp.isPlaying)
            {
                sp.StopSubtitles();
            }
        }

        if (isPlaying)
        {
            StopAllSubtitles();
            StartCoroutine(DelayedPlay());
        }
        else
        {
            PlaySubtitlesSequence();
        }
    }

    private IEnumerator DelayedPlay()
    {
        yield return null; // espera un frame para asegurarse que las corrutinas se detengan
        PlaySubtitlesSequence();
    }

    private void StopAllSubtitles()
    {
        if (currentFadeInCoroutine != null) StopCoroutine(currentFadeInCoroutine);
        if (currentSubtitlesCoroutine != null) StopCoroutine(currentSubtitlesCoroutine);

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void ResetSubtitlesState()
    {
        subtitleText.alpha = 0f;
        subtitleText.text = "";
        audioSource.time = 0; // Reinicia el audio al inicio
    }

    private void PlaySubtitlesSequence()
    {
        isPlaying = true;
        if (playButton != null)
            playButton.interactable = false;

        ResetSubtitlesState();

        audioSource.Play();
        currentFadeInCoroutine = StartCoroutine(FadeIn());
        currentSubtitlesCoroutine = StartCoroutine(PlaySubtitles());
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
            subtitleText.text = "";  // limpiar justo antes de escribir cada línea

            foreach (char letter in chunk.ToCharArray())
            {
                subtitleText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(delayBetweenChunks);
        }

        yield return StartCoroutine(FadeOut());

        isPlaying = false;
        if (playButton != null)
            playButton.interactable = true;
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

    // Método para detener manualmente los subtítulos
    public void StopSubtitles()
    {
        StopAllSubtitles();
        isPlaying = false;
        if (playButton != null)
            playButton.interactable = true;

        subtitleText.alpha = 0f;
        subtitleText.text = "";
    }
}
