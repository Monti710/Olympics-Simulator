using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;

    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        // Leer el nombre de la escena guardado antes
        string sceneToLoad = PlayerPrefs.GetString("NextScene");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Evita que se cargue autom�ticamente al 100%
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }

        // Llega al 90% y esperamos un segundo extra (opcional)
        loadingBar.value = 1f;
        yield return new WaitForSeconds(1f);

        // Activar la siguiente escena
        operation.allowSceneActivation = true;
    }
}
