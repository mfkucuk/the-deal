using System;
using DG.Tweening;
using UnityEngine;

public class GameSceneDice : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private DiceAmountController diceAmountController;

    [SerializeField] private Transition transition;

    [SerializeField] private Vector3 scaleVector;
    private Vector3 firstScale;

    [SerializeField] private float duration;

    private bool firstTime = true;

    private void Start()
    {
        if (GameSceneData.Instance.GetGameSceneTutorial() == 1)
        {
            this.gameObject.SetActive(false);
        }

        firstScale = transform.localScale;
    }

    public void OnMouseDown()
    {
        if (!PuaseMenu.GameIsPaused)
        {
            if (dice.CanDice && diceAmountController.cantMove && !diceAmountController.WaitDiceRoll && diceAmountController.FightCallDiceAmount > 0 
                && !transition.TransitionStarted && !DialogueManager.Instance.DialogueStopGame) 
                StartCoroutine(dice.RollTheDice());
        }
    }
    
    private void OnMouseEnter()
    {
        if (!PuaseMenu.GameIsPaused && !DialogueManager.Instance.DialogueStopGame)
        {
            if (firstTime)
            {
                transform.DOKill();
                transform.DOScale(scaleVector, duration);
                firstTime = false;
                AudioManager.Instance.Play("HoverItem");
            }
        }
    }
    
    private void OnMouseExit()
    {
        if (!PuaseMenu.GameIsPaused && !DialogueManager.Instance.DialogueStopGame)
        {
            transform.DOKill();
            transform.DOScale(firstScale, duration/2);
            firstTime = true;
        }
    }
    
}