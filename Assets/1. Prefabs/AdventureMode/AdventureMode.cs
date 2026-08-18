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

    private int enemiesRemaining;

    private Action restartAction;

    [SerializeField] private float inputDelay = 5;

    private QuizConfiguration config;
    private Monster currentMonster;
    [SerializeField] private GameObject monster;
    private Animator animator;
    private SpriteRenderer monsterSprite;
    private Image uiMonsterSprite;
    [SerializeField] MonsterLibrary monsterLibrary;

    public bool Initialized { get; private set; }

    void Start()
    {
        battleMenuRoot = battleMenu.rootVisualElement;
        testScreenRoot = testScreen.rootVisualElement;
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
        enemiesRemaining = enemies;
        simpleTest.InitializeQuiz(config);
        battleMenu.enabled = true;
        this.playerMaxHP = playerMaxHP;
        playerCurrentHP = playerMaxHP;

        GenerateMonster(MonsterDifficulty.Easy);
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
            testScreenRoot.visible = true;
        }
        else
        {
            testScreenRoot.visible = false;
        }
    }

    private void SetQuestion()
    {
        if (enemyCurrentHP <= 0)
        {
            GenerateMonster(MonsterDifficulty.Easy);
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
    private void GenerateMonster(MonsterDifficulty difficulty)
    {
        if (enemiesRemaining <= 0)
        {
            battleText = $"You are the Conjugation Master!";
            Invoke("VictoryCondition", inputDelay);
        }
        currentMonster = monsterLibrary.GetRandomMonsterByDifficulty(difficulty);
        enemyMaxHP = currentMonster.MaxHP;
        enemyCurrentHP = currentMonster.MaxHP;
        enemyName = currentMonster.Name;
        monsterSprite.color = currentMonster.Tint;
        battleText = $"A wild {currentMonster.Name} appears!";
        simpleTest.SetQuestionTypes(currentMonster.ConjugationTypes);
        simpleTest.PrepareNextQuestion();
        animator.SetBool("Death", false);
        animator.SetBool("Spawned", true);
        Invoke("SetQuestion", inputDelay);

        enemiesRemaining -= 1;
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
