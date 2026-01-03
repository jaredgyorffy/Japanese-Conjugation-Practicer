using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class KanaRomajiTranslator : MonoBehaviour
{
    string input;

    //public delegate void InputChangedEventHandler();
    //public event InputChangedEventHandler InputChanged;

    UIDocument uiDocument;
    VisualElement root;
    TextField textField;

    [SerializeField] KanaRomajiList KanaRomajiList;


    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        textField = root.MQ<TextField>("TextField");
        input = new Observable<string>();
        //BindInputChanged(input);
    }

    private void Update()
    {
        input = textField.value;
        InvokeStateChangedEvent();
    }

    private void InvokeStateChangedEvent()
    {
        foreach (var pair in KanaRomajiList.TwoLetterPairs)
        {
            FindAndReplaceRomaji(pair);
        }

        if (input.Contains("na"))
        {
            input = input.Replace("na", "な");
            textField.value = input;
        }
        //InputChanged?.Invoke();
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
        if (input.Contains(pair.Romaji))
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
        input = input.Replace(pair.Romaji, pair.Kana);
        textField.value = input;
    }

    /*public void BindInputChanged(Observable<string> input)
    {
        this.input = input;
        this.input.ValueChanged += (_) => InvokeStateChangedEvent();
    }*/
}
