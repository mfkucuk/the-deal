using UnityEngine;
using TMPro;

public class AttackDice : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private Shake shakeController;

    [SerializeField] private Animator diceThrowAnimator;
    
    [SerializeField] private GameObject[] attackWeapons;
    
    [SerializeField] private TMP_Text attackText;
    public Skill[] _attackPrefabs;

    private Skill[] _attackSkills;
    
    [SerializeField] private MoveDice moveDice;

    private int lastResult;
    
    private bool cantDice = true; 
    
    public bool anyDiceRolling { get; set; } = false;
    
    public bool stopAttackDice { get; set; } = false;

    private void Start()
    {
        Enemy.OnAllEnemyDied += OnCantDice;
        Player.Instance.OnPlayerDeath += OnCantDice;

        dice.diceFaceAmount = InventoryData.Instance.GetAttackDiceFaceData();
        _attackSkills = new Skill[InventoryData.Instance.GetAttackDiceFaceData()];
        dice.diceSides = new Sprite[InventoryData.Instance.GetAttackDiceFaceData()];
        var index = 0;
        for (int i = 0; i < InventoryData.Instance.GetAttackSkillCount(); i++)
        {
            if (InventoryData.Instance.GetAttackSkillData(i) == 2)
            {
                Skill newSkill = Instantiate(_attackPrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _attackSkills[index++] = newSkill;
            }
        }

        for (int i = 0; i < _attackSkills.Length; i++)
        {
            dice.diceSides[i] = _attackSkills[i].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }
    
    private void OnDestroy()
    {
        Enemy.OnAllEnemyDied -= OnCantDice;
        Player.Instance.OnPlayerDeath -= OnCantDice;
    }

    private void OnCantDice()
    {
        cantDice = false;
    }
    
    private void Update()
    {
        if (Grid.Instance.GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                shakeController.resetShaking();

                if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.NONE && dice.CanDice && cantDice && !stopAttackDice && !moveDice.anyDiceRolling && !PuaseMenu.GameIsPaused)
                {
                    dice.OnDiceRolled += OnDiceRolled;

                    anyDiceRolling = true;
                    
                    attackText.enabled = false;
                    diceThrowAnimator.SetBool("ThrowDice", true);
                    AudioManager.Instance.Play("ThrowDice");
                    StartCoroutine(dice.RollTheDice());
                }
            } 
        }
        
    }

    private void OnMouseEnter()
    {
        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.NONE && dice.CanDice && Grid.Instance.GameStarted && cantDice && !stopAttackDice)
            shakeController.StartMouseOnShaking(transform);
    }

    private void OnMouseExit()
    {
        shakeController.resetShaking();
    }
    public void OnMouseDown()
    {
        if (Grid.Instance.GameStarted)
        {
            shakeController.resetShaking();
            dice.OnDiceRolled += OnDiceRolled;

            if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.NONE && cantDice && dice.CanDice && !stopAttackDice && !moveDice.anyDiceRolling && !PuaseMenu.GameIsPaused)
            {
                attackText.enabled = false;
            
                anyDiceRolling = true;
                    
                diceThrowAnimator.SetBool("ThrowDice", true);
            
                AudioManager.Instance.Play("ThrowDice");
            
                StartCoroutine(dice.RollTheDice());
            }
        }    
    }

    private void OnDiceRolled(int result)
    {
        lastResult = result;
        Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.PLAYER_TURN;
        Player.Instance.currentAttackSkill = _attackSkills[result - 1];

        anyDiceRolling = false;
        
        diceThrowAnimator.SetBool("ThrowDice", false);
        
        Player.Instance.mode = Player.MODE.ATTACK;
        
        for(int i = 0; i < attackWeapons.Length; i++)
            attackWeapons[i].SetActive(false);
        
        AudioManager.Instance.Play("TakeWeapon");

        Player.Instance.atk_pat = _attackSkills[result - 1].attackPattern;
        dice.rend.sprite = _attackSkills[result - 1].skillImage;
        
        attackWeapons[_attackSkills[result - 1].skillIndex].SetActive(true);
        Grid.Instance.highlightTiles(Player.Instance.PlayerPos.x, Player.Instance.PlayerPos.y);

        dice.OnDiceRolled -= OnDiceRolled;
    }
    
    public void PlayWeaponAnimation()
    {
        attackWeapons[lastResult-1].GetComponentInChildren<Animator>().SetTrigger("Attack");
    }
    
}
