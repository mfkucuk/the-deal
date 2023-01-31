using UnityEngine;
using DG.Tweening;

public class InventoryText : MonoBehaviour
{
    [SerializeField] private Transition transition;
    
    [SerializeField] private DiceAmountController diceAmountController;
    
    [SerializeField] private SkullMovement skullMovement;

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

    private void OnMouseEnter()
    {
        if (!PuaseMenu.GameIsPaused && skullMovement.moveFinished && !diceAmountController.WaitDiceRoll && !skullMovement.TimeToFight && !transition.TransitionStarted 
            && !DialogueManager.Instance.DialogueStopGame)
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
        if (!PuaseMenu.GameIsPaused && skullMovement.moveFinished && !diceAmountController.WaitDiceRoll && !skullMovement.TimeToFight && !transition.TransitionStarted 
            && !DialogueManager.Instance.DialogueStopGame)
        {
            transform.DOKill();
            transform.DOScale(firstScale, duration/2);
            firstTime = true;
        }
    }

    private void OnMouseDown()
    {
        if (!PuaseMenu.GameIsPaused && skullMovement.moveFinished && !diceAmountController.WaitDiceRoll && !skullMovement.TimeToFight && !transition.TransitionStarted 
            && !DialogueManager.Instance.DialogueStopGame)
        {
            if(int.Parse(diceAmountController.text.text) > 0)
                SkullData.Instance.SetCantMove(1);
            else
                SkullData.Instance.SetCantMove(0);
            
            SkullData.Instance.SetPosDatas(skullMovement.transform.position.x, skullMovement.transform.position.y);
            
            SkullData.Instance.SetSkullCurrDice(int.Parse(diceAmountController.text.text));
            SkullData.Instance.SetSkullTotalDice(int.Parse(diceAmountController.fightCallAmountText.text));
            
            StartCoroutine(transition.EndTransition("Inventory"));
        }
    }
}
