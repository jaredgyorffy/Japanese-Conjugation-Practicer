using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class QuizMenu : MonoBehaviour
{
    [SerializeField] private UIDocument quizMenu;
    private VisualElement quizMenuRoot;
    private Button submitButton;
    private Button restartButton;
    private Button hintButton;
    private TextField textField;
    private KanaRomajiTranslator textConverter;
    private VisualElement informationBox;
    private VisualElement number;

    [CreateProperty] public string PreviousAnswer => informationText;
    private string informationText;

    [CreateProperty] public string CurrentKanji => currentKanji;
    private string currentKanji;

    [CreateProperty] public string CurrentKana => currentKana;
    private string currentKana;
    [CreateProperty] public string QuestionType => question;
    private string question;

    private bool hintVisible = false;

    private bool useKanaKeyboard = true;
    private bool confirmAnswer = false;
    private bool useEnglishCharacters = false;

    public Action<bool> HintPressed;
    public Action<string> SubmitButtonPressed;
    public bool Initialized { get; private set; }

    private void Awake()
    {
        TryInitialize();
    }

    public void TryInitialize()
    {
        if (Initialized) return;
        quizMenuRoot = quizMenu.rootVisualElement;
        quizMenuRoot.dataSource = this;
        textField = quizMenuRoot.MQ<TextField>("TextField");
        submitButton = quizMenuRoot.MQ<Button>("Submit");
        submitButton.clicked += OnPressSubmit;
        number = quizMenuRoot.MQ<VisualElement>("Number");

        hintButton = quizMenuRoot.MQ<Button>("Hint");
        hintButton.clicked += ToggleHint;

        restartButton = quizMenuRoot.MQ<Button>("Restart");

        textConverter = quizMenu.GetComponent<KanaRomajiTranslator>();
        textConverter.InputChanged += OnInputChanged;
        informationBox = quizMenuRoot.MQ<VisualElement>("InformationBox");
        textField.RegisterCallback<NavigationSubmitEvent>(OnPressEnterToSubmit, TrickleDown.TrickleDown);

        restartButton.SetEnabled(true);
        restartButton.visible = false;

        SetInformationText("");
        Initialized = true;
    }

    public void Unsubscribe()
    {
        if (Initialized)
        {
            submitButton.clicked -= OnPressSubmit;
            hintButton.clicked -= ToggleHint;
            textConverter.InputChanged -= OnInputChanged;
            textField.UnregisterCallback<NavigationSubmitEvent>(OnPressEnterToSubmit, TrickleDown.TrickleDown);
            Initialized = false;
        }
    }

    private void OnPressEnterToSubmit(NavigationSubmitEvent evt)
    {
        evt.StopImmediatePropagation();
        OnPressSubmit();
    }

    private void Update()
    {
        if (Initialized == false)
        {
            return;
        }

        var keyboard = textField.touchScreenKeyboard;

        if (keyboard == null)
        {
            return;
        }

        if (keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            OnPressSubmit();
        }
    }

    public void SetNumberVisible(bool visible)
    {
        number.visible = visible;
    }

    public void SetInformationText(string text)
    {
        informationText = text;
        informationBox.style.visibility = string.IsNullOrEmpty(text)
            ? Visibility.Hidden
            : Visibility.Visible;
    }

    private void OnInputChanged()
    {
        confirmAnswer = false;
        textField.style.color = Color.black;
    }

    private void OnPressSubmit()
    {
        if (hintVisible)
        {
            ToggleHint();
        }

        if (textField.value == "")
        {
            SetInformationText("Please submit an Answer");
            textField.style.color = Color.maroon;
            textField.Focus();
            return;
        }
        else if (useEnglishCharacters)
        {
            if (textField.value.ContainsEnglishCharacters() == false)
            {
                SetInformationText("please answer in english");
                textField.style.color = Color.maroon;
                textField.Focus();
                return;
            }
        }

        SubmitButtonPressed?.Invoke(textField.value);
        textField.value = "";
        confirmAnswer = false;
    }

    public void SetKanaKeyboard(bool useKanaKeyboard)
    {
        textConverter.SetEnabled(useKanaKeyboard);
    }

    public void PrepareNextQuestion()
    {


        textField.Focus();
    }

    public void SetKana(string kana, string Kanji)
    {
        if (string.IsNullOrEmpty(Kanji))
        {
            currentKanji = kana;
            currentKana = "";
        }
        else
        {
            currentKanji = Kanji;
            currentKana = kana;
        }
    }
    public void SetQuestion(string question)
    {
        this.question = question;
    }

    public void ToggleHint()
    {

        if (hintVisible)
        {
            hintVisible = false;
            hintButton.RemoveFromClassList("Pressed");
            SetInformationText("");
            HintPressed?.Invoke(true);
        }
        else
        {
            hintVisible = true;
            hintButton.AddToClassList("Pressed");
            hintButton.Focus();
            HintPressed?.Invoke(false);
        }
    }

    private void OnDestroy()
    {
        if (Initialized)
        {
            Unsubscribe();
        }
    }
}
