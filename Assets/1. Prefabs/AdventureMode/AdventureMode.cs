using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Properties;
using Hieki.Search;
using UnityEngine;
using UnityEngine.UIElements;

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

    private Action restartAction;

    [SerializeField] private float inputDelay = 5;

    private QuizConfiguration config;
    private Monster currentMonster;
    [SerializeField] private GameObject monster;
    private Animator animator;
    private SpriteRenderer monsterSprite;
    private Image uiMonsterSprite;
    private Queue<MonsterDifficulty> campaignOrder;
    [SerializeField] MonsterLibrary monsterLibrary;

    public bool Initialized { get; private set; }

    void Start()
    {
        battleMenuRoot = battleMenu.rootVisualElement;
        testScreenRoot = testScreen.rootVisualElement.MQ<VisualElement>("Panel");
        uiMonsterSprite = battleMenuRoot.MQ<Image>("EnemySprite");
        animator = monster.GetComponent<Animator>();
        monsterSprite = monster.GetComponent<SpriteRenderer>();
        battleMenuRoot.dataSource = this;
        SetBattleScreenVisible(false);
        monsterSprite.enabled = false;
    }

    private void Update()
    {
        uiMonsterSprite.style.backgroundImage = new StyleBackground(monsterSprite.sprite);
    }

    public void InitializeAdventure(float playerMaxHP, int enemies, QuizConfiguration config, Action restartAction = null)
    {
        SetTestVisible(false);
        SetBattleScreenVisible(true);
        config.Strictmode = true;
        this.config = config;
        simpleTest.InitializeQuiz(config);
        battleMenu.enabled = true;
        this.playerMaxHP = playerMaxHP;
        playerCurrentHP = playerMaxHP;

        campaignOrder = GenerateMonsterOrder(enemies);

        GenerateMonster();
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

    private Queue<MonsterDifficulty> GenerateMonsterOrder(int monsterCount)
    {
        Queue<MonsterDifficulty> monsters = new Queue<MonsterDifficulty>();
        if (monsterCount < 3)
        {
            monsters.Enqueue(MonsterDifficulty.Easy);
            monsters.Enqueue(MonsterDifficulty.Easy);
            return monsters;
        }

        for (int i = 0; i < monsterCount / 3; i++)
        {
            monsters.Enqueue(MonsterDifficulty.Easy);
        }

        for (int i = 0; i < monsterCount / 3; i++)
        {
            monsters.Enqueue(MonsterDifficulty.Medium);
        }

        for (int i = 0; i < monsterCount / 3; i++)
        {
            monsters.Enqueue(MonsterDifficulty.Hard);
        }

        monsters.Enqueue(MonsterDifficulty.Boss);

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
        }
        else
        {
            battleMenuRoot.SetEnabled(false);
            battleMenuRoot.visible = false;
        }
    }
    private void SetTestVisible(bool visible)
    {
        if (visible)
        {
            testScreenRoot.style.opacity = 1;
        }
        else
        {
            testScreenRoot.style.opacity = 0;
        }
    }

    private void SetQuestion()
    {
        if (enemyCurrentHP <= 0)
        {
            GenerateMonster();
            return;
        }

        if (playerCurrentHP <= 0)
        {
            restartAction.Invoke();
            return;
        }
        SetTestVisible(true);
        simpleTest.PrepareNextQuestion();
    }
    private void GenerateMonster()
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

        playerCurrentHP = 3;
        enemyMaxHP = currentMonster.MaxHP;
        enemyCurrentHP = currentMonster.MaxHP;
        enemyName = currentMonster.Name;
        uiMonsterSprite.style.unityBackgroundImageTintColor = currentMonster.Tint;
        battleText = $"A wild {currentMonster.Name} appears!";
        simpleTest.SetQuestionTypes(currentMonster.ConjugationTypes);
        simpleTest.PrepareNextQuestion();
        animator.SetBool("Death", false);
        animator.SetBool("Spawned", true);
        Invoke("SetQuestion", inputDelay);
    }

    private void VictoryCondition()
    {

        restartAction.Invoke();
    }

    private void ResolveBattle(bool answerCorrect)
    {
        SetTestVisible(false);
        if (answerCorrect)
        {
            enemyCurrentHP -= 1;
            battleText = $"Correct! Dealt damage to {enemyName}!";
            animator.SetTrigger("Hurt");
        }
        else
        {
            playerCurrentHP -= 1;
            battleText = $"Incorrect! {enemyName} damaged you!";
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
}
