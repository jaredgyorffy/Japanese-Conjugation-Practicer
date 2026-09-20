using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Properties;
using Hieki.Search;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class AdventureMode : MonoBehaviour
{
    [SerializeField] private UIDocument testScreen;
    private VisualElement testScreenRoot;

    [SerializeField] private QuizMediator quizMediator;

    [SerializeField] private UIDocument battleMenu;
    private VisualElement battleMenuRoot;

    [CreateProperty] public string BattleText => battleText;
    private string battleText = "";
    [CreateProperty] public string EnemyName => enemyName;
    private string enemyName = "Test Enemy";
    [CreateProperty] public float EnemyCurrentHP => enemyCurrentHP;
    private float enemyCurrentHP = 100;
    [CreateProperty] public float PlayerCurrentHP => playerCurrentHP;
    private float playerCurrentHP = 100;
    [CreateProperty] public float EnemyMaxHP => enemyMaxHP;
    private float enemyMaxHP = 100;
    [CreateProperty] public float PlayerMaxHP => playerMaxHP;
    private float playerMaxHP = 100;

    [CreateProperty] public string LeftText => leftText;
    private string leftText;

    [CreateProperty] public string RightText => rightText;
    private string rightText;

    [CreateProperty] public string ForwardText => forwardText;
    private string forwardText;

    private Action restartAction;
    private bool debugEnemyHealth = false;

    [SerializeField] private float inputDelay = 5;
    private int playerDefaultMaxHP;
    private QuizConfiguration config;
    private Monster currentMonster;
    [SerializeField] private GameObject monster;
    private Animator animator;
    private SpriteRenderer monsterSprite;
    private Image uiMonsterSprite;
    private Queue<MonsterDifficulty> campaignOrder;
    private VisualElement enemy;
    private VisualElement player;
    private VisualElement dialogueBox;
    [SerializeField] MonsterLibrary monsterLibrary;
    [SerializeField] private DungeonGenerator dungeonGenerator;
    private Button leftButton;
    private Button rightButton;
    private Button centerButton;
    private DungeonDirection currentDirectionInput;
    private QuestionCategory currentCategory;

    public Action leftButtonPressed;
    public Action rightButtonPressed;
    public Action centerButtonPressed;
    public bool Initialized { get; private set; }

    void Start()
    {
        battleMenuRoot = battleMenu.rootVisualElement;
        testScreenRoot = testScreen.rootVisualElement.MQ<VisualElement>("Panel");
        uiMonsterSprite = battleMenuRoot.MQ<Image>("EnemySprite");
        enemy = battleMenuRoot.MQ<VisualElement>("Enemy");
        player = battleMenuRoot.MQ<VisualElement>("Player");
        dialogueBox = battleMenuRoot.MQ<VisualElement>("DialogueBox");
        animator = monster.GetComponent<Animator>();
        monsterSprite = monster.GetComponent<SpriteRenderer>();
        battleMenuRoot.dataSource = this;
        SetBattleScreenVisible(false);
        monsterSprite.enabled = false;
        dungeonGenerator.TileSequenceComplete += DeployMonster;
        leftButton = battleMenuRoot.MQ<Button>("Left");
        rightButton = battleMenuRoot.MQ<Button>("Right");
        centerButton = battleMenuRoot.MQ<Button>("Forward");
    }

    private void SetDialogueBoxText(string text)
    {
        if (text == "")
        {
            battleText = "";
            dialogueBox.RemoveFromClassList("Visible");
            dialogueBox.AddToClassList("Hidden");
        }
        else
        {
            battleText = text;
            dialogueBox.RemoveFromClassList("Hidden");
            dialogueBox.AddToClassList("Visible");
        }
    }

    private void Update()
    {
        uiMonsterSprite.style.backgroundImage = new StyleBackground(monsterSprite.sprite);
        if (currentMonster != null)
        {
            if (campaignOrder.Count <= 0)
            {
                uiMonsterSprite.style.unityBackgroundImageTintColor = Color.HSVToRGB((Time.time/2) % 1, 1, 1);
            }
        }
    }

    public void InitializeAdventure(float playerMaxHP, int enemies, QuizConfiguration config, Action restartAction = null)
    {
        SetTestVisible(false);
        SetBattleScreenVisible(true);
        this.config = config;
        quizMediator.SetQuizType(QuestionCategory.Vocab);
        quizMediator.InitializeQuiz(config);
        debugEnemyHealth = config.DebugEnemyHealth;
        battleMenu.enabled = true;
        this.playerMaxHP = playerMaxHP;
        playerCurrentHP = playerMaxHP;

        leftButton.clicked += leftButtonPressed;
        rightButton.clicked += rightButtonPressed;
        centerButton.clicked += centerButtonPressed;
        leftButton.clicked += () => SetNextDirection(DungeonDirection.Left);
        centerButton.clicked += () => SetNextDirection(DungeonDirection.Forward);
        rightButton.clicked += () => SetNextDirection(DungeonDirection.Right);

        SetQuizQuestionType(GetRandomQuestionType(config.questionTypes));
        SetDirectionButtonVisibility(false);
        campaignOrder = GenerateMonsterOrder(enemies);
        TryGenerateRandomMonster();
        DeployMonster();
        monsterSprite.enabled = true;
        animator.SetBool("Spawned", true);
        this.restartAction = restartAction;
        if (Initialized)
        {
            return;
        }
        quizMediator.AnswerSubmitted += ResolveBattle;
        Initialized = true;

    }

    private void SetQuizQuestionType(QuestionType questionType)
    {
        quizMediator.CurrentQuiz.SetQuestionType(questionType);
        currentCategory = questionType.Category;
    }

    private void SetDirectionButtonVisibility(bool visibility)
    {
        if (visibility)
        {
            leftButton.AddToClassList("Visible");
            rightButton.AddToClassList("Visible");
            centerButton.AddToClassList("Visible");
            leftButton.RemoveFromClassList("Hidden");
            rightButton.RemoveFromClassList("Hidden");
            centerButton.RemoveFromClassList("Hidden");
        }
        else
        {
            leftButton.AddToClassList("Hidden");
            rightButton.AddToClassList("Hidden");
            centerButton.AddToClassList("Hidden");
            leftButton.RemoveFromClassList("Visible");
            rightButton.RemoveFromClassList("Visible");
            centerButton.RemoveFromClassList("Visible");
            leftText = "";
            forwardText = "";
            rightText = "";
        }
    }

    private Queue<MonsterDifficulty> GenerateMonsterOrder(int monsterCount)
    {
        Queue<MonsterDifficulty> monsters = new Queue<MonsterDifficulty>();
        for (int i = 1; i <= monsterCount; i++)
        {
            if (i <= monsterCount * 0.33f)
            {
                monsters.Enqueue(MonsterDifficulty.Easy);
            }
            else if (i <= monsterCount * 0.66f)
            {
                monsters.Enqueue(MonsterDifficulty.Medium);
            }
            else if (i < monsterCount)
            {
                monsters.Enqueue(MonsterDifficulty.Hard);
            }
            else if (i == monsterCount)
            {
                monsters.Enqueue(MonsterDifficulty.Boss);
            }
        }

        return monsters;
    }

    public void Unsubscribe()
    {
        if (Initialized)
        {
            quizMediator.AnswerSubmitted -= ResolveBattle;
            SetBattleScreenVisible(false);
            SetTestVisible(true);
        }
        Initialized = false;
    }

    private void SetBattleScreenVisible(bool visible)
    {
        if (visible)
        {
            battleMenuRoot.SetEnabled(true);
            battleMenuRoot.visible = true;
            testScreen.sortingOrder = 1;
        }
        else
        {
            battleMenuRoot.SetEnabled(false);
            battleMenuRoot.visible = false;
            testScreen.sortingOrder = 0;
        }
    }
    private void SetTestVisible(bool visible)
    {
        if (visible)
        {
            testScreenRoot.style.opacity = 1;
            testScreen.sortingOrder = 3;
        }
        else
        {
            testScreen.sortingOrder = 0;
            testScreenRoot.style.opacity = 0;
        }
    }

    private void NextQuestionOrQuiz()
    {
        if (enemyCurrentHP <= 0)
        {
            if (dungeonGenerator.CurrentTile.Endpoints.Count > 1)
            {
                if (campaignOrder.TryPeek(out _) == false)
                {
                    StartVictorySequence();
                    return;
                }
                dungeonGenerator.MoveToDecisionPoint(PathChoosingSequence);
                return;
            }
            else
            {
                SetDialogueBoxText("");
                SetQuizQuestionType(GetRandomQuestionType(config.questionTypes));
                if (TryGenerateRandomMonster() == false)
                {
                    return;
                }
                dungeonGenerator.GenerateNextTile(DungeonDirection.None);
                return;
            }
        }

        if (playerCurrentHP <= 0)
        {
            restartAction.Invoke();
            return;
        }
        //Final boss uses a randomized question type
        if (campaignOrder.TryPeek(out _) == false)
        {
            quizMediator.CurrentQuiz.SetQuestionType(GetRandomQuestionType(config.questionTypes));
        }
        quizMediator.CurrentQuiz.PrepareNextQuestion();
        SetTestVisible(true);
    }

    private void StartVictorySequence()
    {
        SetBattleUIVisibility(false);
        SetDialogueBoxText($"You are the Conjugation Master!");
        Invoke("VictoryCondition", inputDelay);
        return;
    }

    private void PathChoosingSequence()
    {
        
        SetDialogueBoxText("");
        StartCoroutine(ChooseDungeonPath(null));
        SetBattleUIVisibility(false);
    }

    IEnumerator ChooseDungeonPath(Action action)
    {
        List<QuestionType> questionTypes = GenerateQuestionTypes(dungeonGenerator.CurrentTile.Endpoints);
        InitializeDirectionButtons(dungeonGenerator.CurrentTile.Endpoints, questionTypes);
        SetDialogueBoxText ($"Waiting for Input");

        currentDirectionInput = DungeonDirection.None;
        yield return new WaitUntil(() => currentDirectionInput != DungeonDirection.None);
        SetDialogueBoxText($"");
        SetDirectionButtonVisibility(false);
        int currentQuestionTypeIndex = GetSelectedDirectionIndex(currentDirectionInput, dungeonGenerator.CurrentTile.Endpoints);
        SetQuizQuestionType(questionTypes[currentQuestionTypeIndex]);
        TryGenerateRandomMonster();
        DungeonTile nextTile = dungeonGenerator.GenerateNextTile(currentDirectionInput);
    }

    private List<QuestionType> GenerateQuestionTypes(List<DungeonEndPoint> endpoints)
    {
        List<QuestionType> questionLibrary = new();
        questionLibrary.AddRange(config.questionTypes);
        List <QuestionType> choices = new();

        for (int i = 0; i < endpoints.Count; i++)
        {
            QuestionType questionType = GetRandomQuestionType(questionLibrary);
            if (questionLibrary.Count > endpoints.Count - i)
            {
                questionLibrary.Remove(questionType);
            }
            choices.Add(questionType);
        }
        return choices;
    }

    private QuestionType GetRandomQuestionType(List<QuestionType> questions)
    {
        List<QuestionType> questionTypes = new();
        if (questions.Count <= 1)
        {
            return questions[0];
        }
        foreach (QuestionType questionType in questions)
        {
            if (questionType.Category != currentCategory)
            {
                questionTypes.Add(questionType);
            }
        }
        return questionTypes[UnityEngine.Random.Range(0, questionTypes.Count)];
    }

    private int GetSelectedDirectionIndex(DungeonDirection currentDirectionInput, List<DungeonEndPoint> Endpoints)
    {
        for (int i = 0; i < Endpoints.Count; i++)
        {
            if (currentDirectionInput == Endpoints[i].Direction)
            {
                return i;
            }
        }
        return 0;
    }

    private void InitializeDirectionButtons(List<DungeonEndPoint> Endpoints, List<QuestionType> monsters)
    {
        SetDirectionButtonVisibility(false);
        for (int i = 0; i < Endpoints.Count; i++)
        {
            if (Endpoints[i].Direction == DungeonDirection.Left)
            {
                leftButton.AddToClassList("Visible");
                leftButton.RemoveFromClassList("Hidden");
                leftText = monsters[i].Title;
            }
            else if (Endpoints[i].Direction == DungeonDirection.Forward)
            {
                centerButton.AddToClassList("Visible");
                centerButton.RemoveFromClassList("Hidden");
                forwardText = monsters[i].Title;
            }
            else if (Endpoints[i].Direction == DungeonDirection.Right)
            {
                rightButton.AddToClassList("Visible");
                rightButton.RemoveFromClassList("Hidden");
                rightText = monsters[i].Title;
            }
        }
    }

    private void SetNextDirection(DungeonDirection direction)
    {
        Debug.Log(direction);
        currentDirectionInput = direction;
    }

    [Button("Debug Next Encounter", EButtonEnableMode.Playmode)]
    private void SetBattleUIVisibility(bool visibility)
    {
        if (visibility)
        {
            enemy.AddToClassList("Visible");
            enemy.RemoveFromClassList("Hidden");
            player.AddToClassList("Visible");
            player.RemoveFromClassList("Hidden");
        }
        else
        {
            enemy.AddToClassList("Hidden");
            enemy.RemoveFromClassList("Visible");
            player.AddToClassList("Hidden");
            player.RemoveFromClassList("Visible");
            SetDialogueBoxText("");
        }

    }
    private void DeployMonster()
    {
        playerCurrentHP = playerMaxHP;
        if (debugEnemyHealth)
        {
            enemyMaxHP = 1;
        }
        else
        {
            enemyMaxHP = currentMonster.MaxHP;
        }
        enemyCurrentHP = enemyMaxHP;

        enemyName = currentMonster.Name;
        uiMonsterSprite.style.unityBackgroundImageTintColor = currentMonster.Tint;
        enemy.RemoveFromClassList("Hidden");
        enemy.AddToClassList("Visible");
        SetBattleUIVisibility(true);
        SetDialogueBoxText($"A wild {currentMonster.Name} appears!");

        SetMonsterSprite((int)currentMonster.MonsterType);
        animator.SetBool("Death", false);
        animator.SetBool("Spawned", true);
        monsterSprite.enabled = true;

        Invoke("NextQuestionOrQuiz", inputDelay);
    }

    private bool TryGenerateRandomMonster()
    {
        if (campaignOrder.TryPeek(out _))
        {
            currentMonster = monsterLibrary.GetRandomMonsterByDifficulty(campaignOrder.Dequeue());
            return true;
        }
        else
        {
            StartVictorySequence();
            return false;
        }
    }


    private void SetMonsterSprite(int monster)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (monster == i)
            {
                animator.SetLayerWeight(i, 1);
            }
            else
            {
                animator.SetLayerWeight(i, 0);
            }
        }
    }

    private void VictoryCondition()
    {
        restartAction.Invoke();
    }

    private void ResolveBattle(bool answerCorrect, string answer)
    {
        uiMonsterSprite.Focus();
        SetTestVisible(false);
        if (answerCorrect)
        {
            enemyCurrentHP -= 1;
            SetDialogueBoxText($"{answer} is Correct! Dealt damage to {enemyName}!");
            
            if (enemyCurrentHP > 0)
            {
                animator.SetTrigger("Hurt");
            }
        }
        else
        {
            playerCurrentHP -= 1;
            SetDialogueBoxText($"Incorrect! The correct answer was {answer}.");
            animator.SetTrigger("Attack");
        }

        if (enemyCurrentHP <= 0)
        {
            animator.SetBool("Death", true);
            SetDialogueBoxText($"You defeated the {enemyName}!");
        }

        if (playerCurrentHP <= 0)
        {
            SetDialogueBoxText($"Defeat: You have been Conjugated.");
        }

        Invoke("NextQuestionOrQuiz", inputDelay);
    }

    private void OnDestroy()
    {
        leftButton.clicked -= leftButtonPressed;
        rightButton.clicked -= rightButtonPressed;
        centerButton.clicked -= centerButtonPressed;
    }
}
