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
        dungeonGenerator.TileSequenceComplete += GenerateMonster;
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
        for (int i = 1; i <= monsterCount; i++)
        {
            if (i <= monsterCount * 0.33f)
            {
                monsters.Enqueue(MonsterDifficulty.Easy);
                Debug.Log("Easy");
            }
            else if (i <= monsterCount * 0.66f)
            {
                monsters.Enqueue(MonsterDifficulty.Medium);
                Debug.Log("Medium");
            }
            else if (i < monsterCount)
            {
                monsters.Enqueue(MonsterDifficulty.Hard);
                Debug.Log("Hard");
            }
            else if (i == monsterCount)
            {
                monsters.Enqueue(MonsterDifficulty.Boss);
                Debug.Log("Boss");
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
            NextEncounter();
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
    [Button("Debug Next Encounter", EButtonEnableMode.Playmode)]
    private void NextEncounter()
    {
        dungeonGenerator.GenerateNextTile();
        enemy.AddToClassList("Hidden");
        enemy.RemoveFromClassList("Visible");
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
        Invoke("SetQuestion", inputDelay);
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
}
