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

    private ConjugationTypes conjugationTypes;

    void Start()
    {
        battleMenuRoot = battleMenu.rootVisualElement;
        testScreenRoot = testScreen.rootVisualElement;
        battleMenuRoot.dataSource = this;
        SetBattleScreenVisible(false);
    }

    public void InitializeAdventure(float playerMaxHP, float enemyMaxHP, QuizConfiguration config, Action restartAction = null)
    {
        SetTestVisible(false);
        SetBattleScreenVisible(true);
        config.Strictmode = true;

        simpleTest.InitializeQuiz(config);

        Invoke("SetQuestion", inputDelay);
        battleMenu.enabled = true;
        this.playerMaxHP = playerMaxHP;
        playerCurrentHP = playerMaxHP;
        this.enemyMaxHP = enemyMaxHP;
        enemyCurrentHP = enemyMaxHP;
        this.restartAction = restartAction;
        battleText = $"A wild {enemyName} appears!";

        simpleTest.AnswerSubmitted += ResolveBattle;
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
            restartAction.Invoke();
            return;
        }

        if (playerCurrentHP <= 0)
        {
            restartAction.Invoke();
            return;
        }

        SetTestVisible(true);
    }

    private void ResolveBattle(bool answerCorrect)
    {
        SetTestVisible(false);
        if (answerCorrect)
        {
            enemyCurrentHP -= 1;
            battleText = $"Correct! Dealt damage to {enemyName}!";
        }
        else
        {
            playerCurrentHP -= 1;
            battleText = $"Incorrect! {enemyName} damaged you!";
        }

        if (enemyCurrentHP <= 0)
        {
            battleText = $"You defeated the {enemyName}!";
        }

        if (playerCurrentHP <= 0)
        {
            battleText = $"Defeat: You have been Conjugated.";
        }

        Invoke("SetQuestion", inputDelay);
    }
}
