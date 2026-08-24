using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour
{
    private UIDocument titleScreen;
    private VisualElement root;
    private VisualElement tapToContinue;
    private void Start()
    {
        titleScreen = GetComponent<UIDocument>();
        root = titleScreen.rootVisualElement;
        root.MQ<Button>("Continue").clicked += OnTouched;
        SetupYoyo();
    }

    private void SetupYoyo()
    {
        tapToContinue = titleScreen.rootVisualElement.MQ<VisualElement>("Prompt");
        // When the animation ends, the callback toggles a class to set the scale to 1.3 
        // or back to 1.0 when it's removed.
        tapToContinue.RegisterCallback<TransitionEndEvent>(evt => tapToContinue.ToggleInClassList("Visible"));
        // Schedule the first transition 100 milliseconds after the root.schedule.Execute method is called.
        root.schedule.Execute(() => tapToContinue.ToggleInClassList("Hidden")).StartingIn(100);
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
