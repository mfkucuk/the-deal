using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }

    [HideInInspector] public Tile PlayerPos;

    public ArrayList diagonals;
    public ArrayList sides;

    public Skill currentAttackSkill;
    public Skill currentMoveSkill;

    public AttackDice _attackDice;
    public MoveDice _moveDice;
    public LastDice _lastDice;

    private bool extraSkill;
    private List<Skill> _bonusSkills;

    public AudioSource dealDamage;
    [SerializeField] private BoneSpriteController boneSpriteController;

    [HideInInspector]
    public bool IsAttacked = false;

    [SerializeField] private Transform _cameraPos;
    [SerializeField] private Shake shaker;
    
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject centerOfGameScene;
    [SerializeField] private Animator backGroundAnimation;
    [SerializeField] private Vector3 scaleVector;

    [SerializeField] private GameObject weapons;
    
    [SerializeField] private Animator skulLDamageAnimator;
    [SerializeField] private TMP_Text skullDamageText;

    public bool PlayerDead { get; set; } = false;

    [HideInInspector]
     public bool BoardReady = false;
        
     [HideInInspector]
     public Vector3 currentPos;

     public Action OnPlayerDeath;
     public Action OnPlayerTakeDamage;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public int step;
    public bool isAttacking;

    public MODE mode;
    public MOVEMENT move_pat;
    public ATTACK_PATTERNS atk_pat;
    public enum MODE
    {
        MOVEMENT = 0,
        ATTACK = 1
    }

    public enum MOVEMENT
    {
        MOVE1 = 0,
        MOVE2,
        MOVE3,
        TELEPORT,
        LMOVE,
        MOVE4,
        HOOK,
        CURSED_TELEPORT,


        NONE
    };

    public enum ATTACK_PATTERNS
    {
        SINGLE_STRIKE = 0,
        THREE_BY_THREE = 1,
        WHIRLWIND = 2,
        THRUST_2 = 3,
        THRUST_3 = 4,
        SMITE = 5,
        LONG_AOE = 6,
        CHARGE = 7,
        DAGGER,


        NONE
    }

    private void Start()
    {
        StartCoroutine(StartCaracther());

        currentPos = transform.position;
        
        diagonals = new ArrayList();
        sides = new ArrayList();
        mode = MODE.MOVEMENT;
        atk_pat = ATTACK_PATTERNS.SINGLE_STRIKE;
        isAttacking = false;
        step = 2;

        _bonusSkills = new List<Skill>();
        extraSkill = false;
        for (int i = 3; i < InventoryData.Instance.GetAttackSkillCount(); i++)
        {
            if (InventoryData.Instance.GetAttackSkillData(i) > 0 && !(InventoryData.Instance.GetLatestSkill() == i))
            {
                Skill newSkill = Instantiate(_attackDice._attackPrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _bonusSkills.Add(newSkill);
                extraSkill = true;
            }
        }

        for (int i = 3; i < InventoryData.Instance.GetMoveSkillCount(); i++)
        {
            if (InventoryData.Instance.GetMoveSkillData(i) > 0 && !(InventoryData.Instance.GetLatestSkill() == i + 8))
            {
                Skill newSkill = Instantiate(_moveDice._movePrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _bonusSkills.Add(newSkill);
                extraSkill = true;
            }
        }

        _lastDice.dice.OnDiceRolled += OnDiceRolled;
        _lastDice.dice.diceSides = new Sprite[_bonusSkills.Count];
        _lastDice.dice.diceFaceAmount = _bonusSkills.Count;
        for (int i = 0; i < _bonusSkills.Count; i++)
        {
            _lastDice.dice.diceSides[i] = _bonusSkills[i].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    IEnumerator StartCaracther()
    {
        WaitForSeconds wfs = new WaitForSeconds(1f);
        
        while (!BoardReady)
        {
            yield return wfs;
        }
        
        AudioManager.Instance.Play("CharacterSpawn");
        
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        
        if(GameSceneData.Instance.GetGameSceneTutorial() == 2)
        {
            PlayerPos = Grid.Instance.getTileAtPosition(1, 6);
            transform.position = new Vector2((PlayerPos.x + TileHolder.Instance.x) * TileHolder.Instance.s, (PlayerPos.y + TileHolder.Instance.y) * TileHolder.Instance.s);
            
            //GameSceneData.Instance.SetGameSceneTutorial(3);
        }
        else
        {
            PlayerPos = Grid.Instance.getTileAtPosition(Random.Range(0, 8), 0);
            transform.position = new Vector2((PlayerPos.x + TileHolder.Instance.x) * TileHolder.Instance.s, (PlayerPos.y + TileHolder.Instance.y) * TileHolder.Instance.s);
        }

        CalculateAllDiagonalsOfPlayer(PlayerPos.x, PlayerPos.y, 8);
        CalculateAllSidesOfPlayer();
    }

    public void MoveInAllDirections(int posX, int posY, int steps) 
    {
        for (int i = posX - steps; i <= posX + steps; i++)
        {
            for (int j = posY - steps; j <= posY  + steps; j++)
            {
                if ((i >= 0 && j >= 0 && i < 8 && j < 8) && (i == posX - steps || i == posX + steps ||
                    j == posY - steps || j == posY + steps || i == 0 || i == 7 || j == 0 || j == 7)
                    && !(i == posX && j == posY))
                {
                    Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);               
                }
            }
        }
        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(false);
        }
    }

    public void MoveInHorizantalAndVertical(int posX, int posY, int steps)
    {
        for (int i = posX - steps; i <= posX + steps; i++)
        {
            for (int j = posY - steps; j <= posY + steps; j++)
            {
                if ((i >= 0 && j >= 0 && i < 8 && j < 8) && (i == posX || j == posY) && !(i == posX && j == posY))
                {
                    Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);              
                }
            }
        }
        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(false);
        }
    }

    public void MoveInDiagonal(int posX, int posY, int steps)
    {
        int i = posX - steps;
        int j = posY - steps;
        for (; i <= posX + steps && j <= posY + steps; i++, j++)
        {
            if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == posX && j == posY))
            {
                Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);
            }
        }
        i = posX + steps;
        j = posY - steps;
        for (; i >= posX - steps && j <= posY + steps; i--, j++)
        {
            if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == posX && j == posY))
            {
                Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);
            }
        }
        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(false);
        }
    }

    public void TeleportToEdge(int posX, int posY)
    {
        Grid.Instance.getTileAtPosition(0, posY).canTakeAction.SetActive(true);
        Grid.Instance.getTileAtPosition(7, posY).canTakeAction.SetActive(true);
        Grid.Instance.getTileAtPosition(posX, 0).canTakeAction.SetActive(true);
        Grid.Instance.getTileAtPosition(posX, 7).canTakeAction.SetActive(true);

        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(false);
        }

        PlayerPos.canTakeAction.SetActive(false);
    }

    public void MoveInL(int posX, int posY)
    {
        for (int i = posX - 2; i <= posX + 2; i++)
        {
            for (int j = posY - 2; j <= posY + 2; j++)
            {
                if ((i >= 0 && i < 8 && j >= 0 && j < 8) && (i == posX - 2 || i == posX + 2 || j == posY - 2 || j == posY + 2))
                {
                    if ((PlayerPos.x + PlayerPos.y) % 2 == 0)
                    {
                        if ((i + j) % 2 == 1)
                        {
                            Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);
                        }
                    }
                    else
                    {
                        if ((i + j) % 2 == 0)
                        {
                            Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(false);
        }
    }

    public void HookEnemy(int posX, int posY)
    {
        Tile highlightTile = null;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Grid.Instance.getTileAtPosition(i, j).highlight.activeSelf)
                {
                    highlightTile = Grid.Instance.getTileAtPosition(i, j);
                    Grid.Instance.getTileAtPosition(i, j).highlight.SetActive(false);
                }
            }
        }

        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(true);
        }

        if (highlightTile != null)
            PlayerMouseEnter((int)highlightTile.tilePos.x, (int)highlightTile.tilePos.y, highlightTile.Shaker);
    }

    public void AttackInAllDirections(int posX, int posY, int steps)
    {
        isAttacking = true;
        Tile highlightTile = null;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Grid.Instance.getTileAtPosition(i, j).highlight.activeSelf)
                {
                    highlightTile = Grid.Instance.getTileAtPosition(i, j);
                    Grid.Instance.getTileAtPosition(i, j).highlight.SetActive(false);
                }
            }
        }

        for (int i = posX - steps; i <= posX + steps; i++) 
        {
            for (int j = posY - steps; j <= posY + steps; j++)
            {
                if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == posX && j == posY))
                {                   
                    Grid.Instance.getTileAtPosition(i, j).canTakeAction.SetActive(true);
                    
                }
            }
        }
        if (atk_pat == ATTACK_PATTERNS.CHARGE)
        {
            foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
            {
                en.Key.canTakeAction.SetActive(false);
            }
        }

        if (highlightTile != null)
            PlayerMouseEnter((int)highlightTile.tilePos.x, (int)highlightTile.tilePos.y, highlightTile.Shaker);
    }

    public void AttackCertainEnemy(int posX, int posY)
    {
        isAttacking = true;

        Tile highlightTile = null;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Grid.Instance.getTileAtPosition(i, j).highlight.activeSelf)
                {
                    highlightTile = Grid.Instance.getTileAtPosition(i, j);
                    Grid.Instance.getTileAtPosition(i, j).highlight.SetActive(false);
                }
            }
        }

        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            en.Key.canTakeAction.SetActive(true);
        }

        if (highlightTile != null)
            PlayerMouseEnter((int)highlightTile.tilePos.x, (int)highlightTile.tilePos.y, highlightTile.Shaker);
    }

    public void CalculateAllDiagonalsOfPlayer(int posX, int posY, int steps)
    {
        diagonals.Clear();

        int i = posX - steps;
        int j = posY - steps;
        for (; i <= posX + steps && j <= posY + steps; i++, j++)
        {
            if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == posX && j == posY))
            {
                diagonals.Add(Grid.Instance.getTileAtPosition(i, j));
            }
        }
        i = posX + steps;
        j = posY - steps;
        for (; i >= posX - steps && j <= posY + steps; i--, j++)
        {
            if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == posX && j == posY))
            {
                diagonals.Add(Grid.Instance.getTileAtPosition(i, j));
            }
        }
    }

    public void CalculateAllSidesOfPlayer()
    {
        sides.Clear();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == PlayerPos.x && j == PlayerPos.y)
                    && (i == PlayerPos.x || j == PlayerPos.y))
                {
                    sides.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }
    }

    public void TakeDamage(int damageTaken)
    {
        if (!PlayerDead)
        {
            shaker.DoShake(_cameraPos);
            AudioManager.Instance.Play("FeyyazAh");
            
            SkullData.Instance.SetHealthDatas(SkullData.Instance.GetHealtDatas()-damageTaken);
            boneSpriteController.UpdateBones();
            
            skullDamageText.text = SkullData.Instance.GetHealtDatas().ToString();
            skulLDamageAnimator.Play("SkullDamage");
            
            
            OnPlayerTakeDamage?.Invoke();
            
            if (SkullData.Instance.GetHealtDatas() <= 0)
            {
                PlayerDead = true;
                OnPlayerDeath?.Invoke();
                StartCoroutine(PlayerDeath());
            } 
        }

    }

    public IEnumerator PlayerDeath()
    {
        weapons.SetActive(false);

        AudioManager.Instance.Play("PlayerDie");
        
        playerAnimator.SetTrigger("Dead");
        backGroundAnimation.Play("Dead");
        head.GetComponent<SpriteRenderer>().enabled = true;

        StartCoroutine(Grid.Instance.DestroyEnemies(0f));
        StartCoroutine(Grid.Instance.HideAllGrids(0f));

        yield return new WaitForSeconds(0.5f);
        
        head.transform.SetParent(null);
        
        head.transform.DOMove(centerOfGameScene.transform.position, 2f);
        head.transform.DOScale(scaleVector, 2f);

        yield return new WaitForSeconds(3f);

        SettingsData.Instance.ResetData();
        FightSceneData.Instance.ResetData();
        
        DialogueData.Instance.ResetData();
        
        SkullData.Instance.ResetData();
        
        InventoryData.Instance.ResetData();
        InventoryData.Instance.SaveData();
        
        SkillData.Instance.ResetData();
        SkillData.Instance.SaveData();
        
        DialogueData.Instance.ResetData();

        if (extraSkill)
        {       
            _lastDice.InitLastDice();          
        }
        else
        {
            LevelController.Instance.LoadGameOver();
            Destroy(this.gameObject);
        }
    }

    private void OnDiceRolled(int res)
    {
        StartCoroutine(WaitAndGameOver());
        
        _lastDice.diceThrowAnimator.SetBool("ThrowDice", false);
        
        AudioManager.Instance.Play("TakeWeapon");
        
        _lastDice.sprite.GetComponent<SpriteRenderer>().sprite = _bonusSkills[res - 1].gameObject.GetComponent<SpriteRenderer>().sprite;

        if (_bonusSkills[res - 1].skillType == Skill.TYPE.ATTACK)
        {
            InventoryData.Instance.SetAttackSkillData(1, _bonusSkills[res - 1].skillIndex);
            InventoryData.Instance.SetLatestSkill(_bonusSkills[res - 1].skillIndex);
        }
        else
        {
            InventoryData.Instance.SetMoveSkillData(1, _bonusSkills[res - 1].skillIndex);
            InventoryData.Instance.SetLatestSkill(_bonusSkills[res - 1].skillIndex + 8);
        }

        _lastDice.dice.OnDiceRolled -= OnDiceRolled;
    }

    IEnumerator WaitAndGameOver()
    {
        yield return new WaitForSeconds(2f);
        LevelController.Instance.LoadGameOver();
        Destroy(this.gameObject);
    }

    public void LifeSteal(int lifeStealAmount)
    {
        SkullData.Instance.SetHealthDatas(SkullData.Instance.GetHealtDatas()+lifeStealAmount);
        boneSpriteController.UpdateBones();
    }

    public void PlayerMouseEnter(int x, int y, Shake shaker)
    {
        if (Grid.Instance.getTileAtPosition(x, y).canTakeAction.activeSelf == true)
        {
            shaker.mouseOn = false;
            switch (Player.Instance.atk_pat)
            {
                case Player.ATTACK_PATTERNS.SINGLE_STRIKE:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.THREE_BY_THREE:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.WHIRLWIND:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.THRUST_2:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.THRUST_3:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.SMITE:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.LONG_AOE:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.CHARGE:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;

                case Player.ATTACK_PATTERNS.DAGGER:
                    currentAttackSkill.ShowPattern(x, y, shaker);
                    break;
            }
        }
    }

    public void PlayerMouseExit(int x, int y, Shake shaker)
    {
        if (Grid.Instance.getTileAtPosition(x, y).canTakeAction.activeSelf == true)
        {
            shaker.resetShaking();
            switch (Player.Instance.atk_pat)
            {
                case Player.ATTACK_PATTERNS.SINGLE_STRIKE:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.THREE_BY_THREE:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.WHIRLWIND:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.THRUST_2:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.THRUST_3:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.SMITE:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.LONG_AOE:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.CHARGE:
                    currentAttackSkill.HidePattern(x, y);
                    break;

                case Player.ATTACK_PATTERNS.DAGGER:
                    currentAttackSkill.HidePattern(x, y);
                    break;
            }
        }
    }
    
}
