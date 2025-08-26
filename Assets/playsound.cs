using UnityEngine;

public class PlaySoundController : MonoBehaviour
{
    [Header("Audio Source to play")]
    public AudioSource audioSource;

    // Llama este m�todo desde el bot�n UI OnClick
    public void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource no asignado.");
            return;
        }

        // Detener todos los audios y subt�tulos activos en la escena (ejemplo con 'Sub' script)
        Sub[] allSubs = FindObjectsOfType<Sub>();
        foreach (var sub in allSubs)
        {
            // Detener el audio y subt�tulos limpiamente
            sub.StopAllCoroutines();
            if (sub.audioSource.isPlaying)
                sub.audioSource.Stop();

            sub.subtitleText.text = "";
            sub.subtitleText.alpha = 0f;
        }

        // Detener este audio si ya estaba reproduci�ndose
        if (audioSource.isPlaying)
            audioSource.Stop();

        // Reproducir audio asignado
        audioSource.Play();
    }
}
