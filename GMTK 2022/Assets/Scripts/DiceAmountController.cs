using System.Collections;
using UnityEngine;
using TMPro;

public class DiceAmountController : MonoBehaviour
{
    private static DiceAmountController _instance;
    public static DiceAmountController Instance { get { return _instance; } }

    [SerializeField] private Dice _dice;

    [SerializeField] private GameObject wasd;

    public TMP_Text fightCallAmountText;
    public GameObject swordImage;
    
    public TMP_Text text;
    public bool cantMove = true;
    
    public bool WaitDiceRoll { get; set; } = false;

    public int FightCallDiceAmount { get; set; }
    
    [SerializeField] private bool infinityWalk = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        _dice.OnDiceRolled += OnDiceRolled;
        _dice.OnDiceStartingRolled += OnDiceStartingRolled;

        if (GameSceneData.Instance.GetGameSceneTutorial() == 1)
        {
            cantMove = false;
            
            text.text = "99";
            
            fightCallAmountText.text = SkullData.Instance.GetSkullTotalDice().ToString();
            FightCallDiceAmount = SkullData.Instance.GetSkullTotalDice();
            
            text.enabled = false;
            
            fightCallAmountText.enabled = false;
            swordImage.SetActive(false);
            //fightCallAmountText.text = SkullData.Instance.GetSkullTotalDice().ToString();
            //FightCallDiceAmount = SkullData.Instance.GetSkullTotalDice();
        }
        else
        {
            wasd.SetActive(false);
            
            if (SkullData.Instance.GetCantMove() == 0)
                cantMove = true;
            else if(SkullData.Instance.GetCantMove() == 1)
                cantMove = false;

            if (!infinityWalk)
            {
                FightCallDiceAmount = SkullData.Instance.GetSkullTotalDice();
        
                text.text = SkullData.Instance.GetSkullCurrDice().ToString();

                fightCallAmountText.text = SkullData.Instance.GetSkullTotalDice().ToString();
            }
            else
            {
                cantMove = false;
                text.text = "999";
                fightCallAmountText.text = SkullData.Instance.GetSkullTotalDice().ToString();
                FightCallDiceAmount = SkullData.Instance.GetSkullTotalDice();
            }
        }

    }

    private void OnDestroy()
    {
        _dice.OnDiceRolled -= OnDiceRolled;
        _dice.OnDiceStartingRolled -= OnDiceStartingRolled;
    }

    private void OnDiceStartingRolled()
    {
        AudioManager.Instance.Play("ThrowDice");

        WaitDiceRoll = true;
        
        fightCallAmountText.text = (--FightCallDiceAmount).ToString();
    }
    
    private void OnDiceRolled(int result)
    {
        IncreaseDiceAmount(result);
    }

    public void IncreaseDiceAmount(int result)
    {
        StartCoroutine(In(result));
    }

    IEnumerator In(int amount)
    {
        cantMove = true;

        WaitForSeconds wfsrt = new WaitForSeconds(0.2f);

        for (int i = 0; i <= amount; i++)
        {
            yield return wfsrt;
            text.text = i.ToString();

        }

        cantMove = false;
        _dice.CanDice = true;
        WaitDiceRoll = false;

    }


}
