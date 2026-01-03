
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Actions.IQuizActions
{
    private Actions actions;
    public delegate void InputDelegate(InputAction.CallbackContext obj);
    public event InputDelegate Submit_Performed;
    public event InputDelegate Submit_Cancelled;
    private void Awake()
    {
       actions = new Actions();
       actions.Quiz.SetCallbacks(this);
       actions.Enable();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Submit_Performed?.Invoke(context);
        }

        if (context.canceled)
        {
            Submit_Cancelled?.Invoke(context);
        }
    }
}
