using UnityEngine;
using UnityEngine.UIElements;

public class KanaRomajiTranslator : MonoBehaviour
{
    [SerializeField] private int tickRate = 1;
    private Observable<string> input;

    public delegate void InputChangedEventHandler();
    public event InputChangedEventHandler InputChanged;

    private UIDocument uiDocument;
    private VisualElement root;
    private TextField textField;

    [SerializeField] KanaRomajiList KanaRomajiList;

    private int tickCount = 0;


    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        textField = root.MQ<TextField>("TextField");
        input = new Observable<string>();
        BindInputChanged(input);
    }
    private void Update()
    {
        tickCount++;
        if (tickCount >= tickRate)
        {
            input.Value = textField.value;
            tickCount = 0;
        }
    }

    private void InvokeStateChangedEvent()
    {
        InputChanged?.Invoke();
        foreach (var pair in KanaRomajiList.ThreeLetterPairs)
        {
            FindAndReplaceRomaji(pair);
        }

        foreach (var pair in KanaRomajiList.TwoLetterPairs)
        {
            FindAndReplaceRomaji(pair);
        }

        foreach (var pair in KanaRomajiList.SingleLetterPairs)
        {
            FindAndReplaceRomaji(pair);
        }
    }

    private void FindAndReplaceRomaji(KanaRomajiPair pair)
    {
        if (FindRomaji(pair))
        {
            ReplaceRomaji(pair);
        }
    }

    private bool FindRomaji(KanaRomajiPair pair)
    {
        if (input.Value.Contains(pair.Romaji))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ReplaceRomaji(KanaRomajiPair pair)
    {
        string value = input.Value;
        input.Value = value.Replace(pair.Romaji, pair.Kana);
        textField.value = input.Value;
    }

    public void BindInputChanged(Observable<string> input)
    {
        this.input = input;
        input.ValueChanged += (_) => InvokeStateChangedEvent();
    }
}
