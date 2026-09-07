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

    [SerializeField] private SimpleTest simpleTest;

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
    [SerializeField] private bool DebugEnemyHealth;

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
    [SerializeField] MonsterLibrary monsterLibrary;
    [SerializeField] private DungeonGenerator dungeonGenerator;
    private Button leftButton;
    private Button rightButton;
    private Button centerButton;
    private DungeonDirection currentDirectionInput;

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
        simpleTest.InitializeQuiz(config);
        battleMenu.enabled = true;
        this.playerMaxHP = playerMaxHP;
        playerCurrentHP = playerMaxHP;

        leftButton.clicked += leftButtonPressed;
        rightButton.clicked += rightButtonPressed;
        centerButton.clicked += centerButtonPressed;
        leftButton.clicked += () => SetNextDirection(DungeonDirection.Left);
        centerButton.clicked += () => SetNextDirection(DungeonDirection.Forward);
        rightButton.clicked += () => SetNextDirection(DungeonDirection.Right);
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
        simpleTest.AnswerSubmitted += ResolveBattle;
        Initialized = true;
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
            simpleTest.AnswerSubmitted -= ResolveBattle;
            simpleTest.Unsubscribe();
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

    private void SetQuestion()
    {
        if (enemyCurrentHP <= 0)
        {
            StartCoroutine(ChooseDungeonPath(NextEncounter));
            return;
        }

        if (playerCurrentHP <= 0)
        {
            restartAction.Invoke();
            return;
        }
        simpleTest.PrepareNextQuestion();
        SetTestVisible(true);
    }
    IEnumerator ChooseDungeonPath(Action action)
    {
        List<Monster> monsters = GenerateMonsters(dungeonGenerator.CurrentTile.Endpoints);
        InitializeDirectionButtons(dungeonGenerator.CurrentTile.Endpoints, monsters);
        battleText = $"Waiting for Input";

        currentDirectionInput = DungeonDirection.None;
        yield return new WaitUntil(() => currentDirectionInput != DungeonDirection.None);
        SetDirectionButtonVisibility(false);
        int currentMonsterIndex = GetSelectedDirectionIndex(currentDirectionInput, dungeonGenerator.CurrentTile.Endpoints);
        currentMonster = monsters[currentMonsterIndex];
        DungeonTile nextTile = dungeonGenerator.GenerateNextTile(currentDirectionInput);
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

    private void InitializeDirectionButtons(List<DungeonEndPoint> Endpoints, List<Monster> monsters)
    {
        SetDirectionButtonVisibility(false);
        for (int i = 0; i < Endpoints.Count; i++)
        {
            if (Endpoints[i].Direction == DungeonDirection.Left)
            {
                leftButton.AddToClassList("Visible");
                leftButton.RemoveFromClassList("Hidden");
                leftText = monsters[i].Name;
            }
            else if (Endpoints[i].Direction == DungeonDirection.Forward)
            {
                centerButton.AddToClassList("Visible");
                centerButton.RemoveFromClassList("Hidden");
                forwardText = monsters[i].Name;
            }
            else if (Endpoints[i].Direction == DungeonDirection.Right)
            {
                rightButton.AddToClassList("Visible");
                rightButton.RemoveFromClassList("Hidden");
                rightText = monsters[i].Name;
            }
        }
    }

    private void SetNextDirection(DungeonDirection direction)
    {
        Debug.Log(direction);
        currentDirectionInput = direction;
    }

    [Button("Debug Next Encounter", EButtonEnableMode.Playmode)]
    private void NextEncounter()
    {
        enemy.AddToClassList("Hidden");
        enemy.RemoveFromClassList("Visible");
    }
    private void DeployMonster()
    {
        playerCurrentHP = playerMaxHP;
        if (DebugEnemyHealth)
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
        battleText = $"A wild {currentMonster.Name} appears!";
        simpleTest.SetQuestionTypes(currentMonster.ConjugationTypes);

        SetMonsterSprite((int)currentMonster.MonsterType);
        animator.SetBool("Death", false);
        animator.SetBool("Spawned", true);
        monsterSprite.enabled = true;
        Invoke("SetQuestion", inputDelay);
    }

    private void TryGenerateRandomMonster()
    {
        if (campaignOrder.TryPeek(out _))
        {
            currentMonster = monsterLibrary.GetRandomMonsterByDifficulty(campaignOrder.Dequeue());
        }
        else
        {
            battleText = $"You are the Conjugation Master!";
            Invoke("VictoryCondition", inputDelay);
            return;
        }
    }

    private List<Monster> GenerateMonsters(List<DungeonEndPoint> endpoints)
    {
        if (campaignOrder.TryPeek(out _))
        {
            List<Monster> monsterList = monsterLibrary.GetMonsterListByDifficulty(campaignOrder.Dequeue());
            List<Monster> choices = new();
            //monsterList.Remove(currentMonster);
            for (int i = 0; i < endpoints.Count; i++)
            {
                Monster monster = monsterLibrary.GetRandomMonsterFromList(monsterList);
                monsterList.Remove(monster);
                choices.Add(monster);
            }
            return choices;
        }
        else
        {
            battleText = $"You are the Conjugation Master!";
            Invoke("VictoryCondition", inputDelay);
            return null;
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
            battleText = $"{answer} is Correct! Dealt damage to {enemyName}!";
            
            if (enemyCurrentHP > 0)
            {
                animator.SetTrigger("Hurt");
            }
        }
        else
        {
            playerCurrentHP -= 1;
            battleText = $"Incorrect! The correct answer was {answer}.";
            animator.SetTrigger("Attack");
        }

        if (enemyCurrentHP <= 0)
        {
            animator.SetBool("Death", true);
            battleText = $"You defeated the {enemyName}!";
        }

        if (playerCurrentHP <= 0)
        {
            battleText = $"Defeat: You have been Conjugated.";
        }

        Invoke("SetQuestion", inputDelay);
    }

    private void OnDestroy()
    {
        leftButton.clicked -= leftButtonPressed;
        rightButton.clicked -= rightButtonPressed;
        centerButton.clicked -= centerButtonPressed;
    }
}
