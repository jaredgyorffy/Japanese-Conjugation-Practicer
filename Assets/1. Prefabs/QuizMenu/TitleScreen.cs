using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour
{
    private UIDocument titleScreen;
    private void Start()
    {
        titleScreen = GetComponent<UIDocument>();
        titleScreen.rootVisualElement.MQ<Button>("Continue").clicked += OnTouched;
    }

    private void OnTouched()
    {
        titleScreen.rootVisualElement.MandatoryQ<VisualElement>("Background").AddToClassList("Hidden");
        Invoke("TurnOff", 1);
    }

    private void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}
