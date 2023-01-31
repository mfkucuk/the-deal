using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SkullMovement : MonoBehaviour
{
    [SerializeField] private TMP_Text diceText;

    [SerializeField] private BoneUIController boneUIController;

    [SerializeField] private CharacterMovement characterMovement;
    
    [SerializeField] private DiceAmountController diceAmountController;

    [SerializeField] private Transition transition;

    [SerializeField] private TriggerBossTalk triggerBossTalk;

    [SerializeField] private Dialogue bossDialogue;

    public bool TimeToFight { get; set; } = false;

    private bool notMoved = true;

    private Vector3 holdLastPos;
    
    [HideInInspector]
    public bool moveFinished = true;

    private float moveX = 0;
    private float moveY = 0;

    private TweenCallback tweenCallback;

    private bool wallCollide = false;

    private void Start()
    {
        if (SkullData.Instance.GetGameOpened() == 1)
        {
            transform.position = new Vector3(SkullData.Instance.GetSavedPos().x, SkullData.Instance.GetSavedPos().y, transform.position.z);
            SkullData.Instance.SetGameOpened(0);
        }
        else
        {
            transform.position = new Vector3(SkullData.Instance.GetPosDatas().x, SkullData.Instance.GetPosDatas().y, transform.position.z);
        }

        characterMovement.OnJumpFinished += OnJumpFinished;
    }

    private void OnDestroy()
    {
        characterMovement.OnJumpFinished -= OnJumpFinished;
    }

    void Update()
    {

        if (moveFinished && int.Parse(diceText.text) > 0 && !DiceAmountController.Instance.cantMove && !PuaseMenu.GameIsPaused && !DialogueManager.Instance.DialogueStopGame) 
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveX = 0;
                moveY = 1;
                DoMove();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveX = 0;
                moveY = -1;
                DoMove();
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveX = -1;
                moveY = 0;
                DoMove();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveX = 1;
                moveY = 0;
                DoMove();
            }
        }

    }

    private void DoMove()
    {
        moveFinished = false;
        notMoved = false;

        holdLastPos = transform.position;

        diceText.text = (int.Parse(diceText.text) - 1).ToString();

        characterMovement.Jump(transform, new Vector3(transform.position.x + moveX , transform.position.y + moveY, transform.position.z), "JumpVoice", true);

    }

    public IEnumerator WaitAndFight()
    {
        SkullData.Instance.SetSkullCurrDice(0);
        SkullData.Instance.SetSkullTotalDice(3);
        
        SkullData.Instance.SetPosDatas(transform.position.x, transform.position.y);
        
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(transition.EndTransition("FightScene"));

    }

    private void OnJumpFinished()
    {
        if (!wallCollide)
        {
            if (diceText.text == "0")
            {
                diceAmountController.cantMove = true;
   
                if (DiceAmountController.Instance.FightCallDiceAmount == 0)
                {
                    TimeToFight = true;
                    
                    SkullData.Instance.SetCantMove(0);
                    
                    StartCoroutine(WaitAndFight());
                }

            }
            
            moveFinished = true;
        }
   
    }

    IEnumerator LastWait()
    {
        yield return new WaitForSeconds(0.7f);
        
        wallCollide = false;
        moveFinished = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            diceText.text = (int.Parse(diceText.text) + 1).ToString();
            wallCollide = true;
            characterMovement.Jump(transform, holdLastPos, "JumpVoice");
            StartCoroutine(LastWait());
        }
        else if (collision.CompareTag("Bone"))
        {
            AudioManager.Instance.Play("PickUp");
            Destroy(collision.gameObject);

            SkullData.Instance.SetHealthDatas(SkullData.Instance.GetHealtDatas() + 1);
            boneUIController.UpdateBones();
        }
        else if (collision.CompareTag("triggerEnd"))
        {
            DialogueTrigger.Instance.SetDialogue(bossDialogue, 0);
            triggerBossTalk.StartActions();
            DialogueTrigger.Instance.TriggerDialogue();
        }

    }

}
