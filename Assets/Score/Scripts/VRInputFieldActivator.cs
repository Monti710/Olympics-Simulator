using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VRInputFieldKeyboardController : MonoBehaviour, ISelectHandler
{
    [Tooltip("Referencia al teclado en pantalla (OnScreenKeyboard GameObject)")]
    public GameObject onScreenKeyboard;

    [Tooltip("Input Action para cerrar teclado, asignar en inspector")]
    public InputActionReference closeKeyboardAction;

    private TMP_InputField inputField;
    private bool keyboardActive = true;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        if (onScreenKeyboard != null)
            onScreenKeyboard.SetActive(false); // Apagado por defecto

        if (closeKeyboardAction != null)
            closeKeyboardAction.action.performed += ctx => CloseKeyboard();
    }

    private void OnDestroy()
    {
        if (closeKeyboardAction != null)
            closeKeyboardAction.action.performed -= ctx => CloseKeyboard();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (onScreenKeyboard == null) return;

        keyboardActive = !keyboardActive; // Alternar estado
        onScreenKeyboard.SetActive(keyboardActive);

        if (keyboardActive)
        {
            var keyboardScript = onScreenKeyboard.GetComponent<PrabdeepDhaliwal.OnScreenKeyboard.OnScreenKeyboard>();
            if (keyboardScript != null)
                keyboardScript.Setup(inputField);
        }
    }

    private void Update()
    {
        // Si el teclado está activo pero no hay un InputField seleccionado en la escena, cerrar teclado.
        if (keyboardActive && EventSystem.current.currentSelectedGameObject == null)
        {
            CloseKeyboard();
        }
    }

    private void CloseKeyboard()
    {
        if (!keyboardActive) return;

        keyboardActive = false;
        if (onScreenKeyboard != null)
            onScreenKeyboard.SetActive(false);

        // Opcional: deseleccionar el campo de texto para evitar problemas de foco
        if (EventSystem.current.currentSelectedGameObject == gameObject)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
