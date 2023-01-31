using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightSceneBackground : MonoBehaviour
{
    private static FightSceneBackground _instance;

    public static FightSceneBackground Instance
    {
        get { return _instance; }
    }

    [SerializeField] private Animator backgroundAnimator;
    [SerializeField] private GameObject player;
    [SerializeField] private AttackDice _attackDice;
    [SerializeField] private MoveDice _moveDice;

    [SerializeField] private GameObject[] chooses;
    [SerializeField] private GameObject[] news;
    [SerializeField] private GameObject[] damageText;
    [SerializeField] private GameObject[] abilities1;
    [SerializeField] private GameObject[] abilities2;
    [SerializeField] private GameObject[] abilities3;

    [SerializeField] private Tile[] _skillInfoTiles;
    [SerializeField] private GameObject _skull;
    [SerializeField] private GameObject _dummy1;
    [SerializeField] private GameObject _dummy2;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _rarity;
    [SerializeField] private GameObject[] _rarities;

    private List<Skill> _newSkills;
    private List<Skill> _existingSkills;
    private List<Skill> _chosenSkills;


    private Shake shakeController;
    public GameObject desc;
    private List<Skill> skills;

    [SerializeField] private PuaseMenu puaseMenu;
    
    [SerializeField] private GameObject lastHealth;

    [SerializeField] private Transition transition;

    private GameObject[] _abilities;

    public Action OnShopStarted;

    
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        skills = new List<Skill>();
        _newSkills = new List<Skill>();
        _existingSkills = new List<Skill>();
        _chosenSkills = new List<Skill>();
        for (int i = 0; i < InventoryData.Instance.GetAttackSkillCount(); i++)
        {
            if (InventoryData.Instance.GetAttackSkillData(i) > 0)
            {
                Skill newSkill = Instantiate(_attackDice._attackPrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _existingSkills.Add(newSkill);
            }
        }

        for (int i = 0; i < InventoryData.Instance.GetAttackSkillCount(); i++)
        {
            if (InventoryData.Instance.GetAttackSkillData(i) == 0)
            {
                Skill newSkill = Instantiate(_attackDice._attackPrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _newSkills.Add(newSkill);
            }
        }

        for (int i = 0; i < InventoryData.Instance.GetMoveSkillCount(); i++)
        {
            if (InventoryData.Instance.GetMoveSkillData(i) == 0)
            {
                Skill newSkill = Instantiate(_moveDice._movePrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _newSkills.Add(newSkill);
            }
        }
        shakeController = FindObjectOfType<Shake>();

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AccessTile(i, j).infoTilePos = AccessTile(i, j).transform.position;
                AccessTile(i, j).gameObject.SetActive(false);
            }
        }

        _skull.SetActive(false);
        _dummy1.SetActive(false);
        _dummy2.SetActive(false);

        HideSkillInfo();

        puaseMenu.OnPaused += OnPaused;
        puaseMenu.OnResumed += OnResumed;
        
        Enemy.OnAllEnemyDied += OnAllEnemyDied;

    }

    private void OnDestroy()
    {
        puaseMenu.OnPaused += OnPaused;
        puaseMenu.OnResumed += OnResumed;
        
        Enemy.OnAllEnemyDied -= OnAllEnemyDied;
    }

    private void OnPaused()
    {
        AudioManager.Instance.SetMusicVolume("FightMusic", 0.1f);
        Grid.Instance.CloseColliders();
    }

    private void OnResumed()
    {
        AudioManager.Instance.SetMusicVolume("FightMusic", 0.5f);
        Grid.Instance.OpenColliders();
    }


    private void OnAllEnemyDied()
    {
        StartCoroutine(WaitSec());
    }

    IEnumerator WaitSec()
    {
        lastHealth.SetActive(false);
            
        yield return new WaitForSeconds(0.5f);
    
        player.SetActive(false);
        StartCoroutine(Grid.Instance.DestroyGrids(0.1f));
        backgroundAnimator.Play("EndFight");
    }

    private void EndSceneController() // calling in animation event (endAnimation)
    {
        OnShopStarted?.Invoke();
        
        if (FightSceneData.Instance.GetBossFightIsReady() == 1)
        {
            StartCoroutine(transition.EndTransition("Credits"));
        }
        else if (FightSceneData.Instance.GetTotalFight() % 3 == 1)
        {
            StartCoroutine(EndFightUI());
        }
        else
        {
            StartCoroutine(transition.EndTransition("GameScene"));
        }
        
    }
    
    private IEnumerator EndFightUI() 
    {
        WaitForSeconds wfs = new WaitForSeconds(0.5f);
        
        AudioManager.Instance.Stop("FightMusic");
        AudioManager.Instance.Play("ShopMusic");
        SettingsData.Instance.SetMusicBeforeSettings("ShopMusic");

        for (int i = 0; i < chooses.Length; i++)
        {
            yield return wfs;
            
            AudioManager.Instance.Play("CharacterTakeDamage");
            chooses[i].SetActive(true);
        }

        skills = ChooseSkills();
    }

    private List<Skill> ChooseSkills()
    {
       
        int index = 0;
        while (true)
        {

            int res = Random.Range(0, 101);

            if (index == 0) _abilities = abilities1;
            if (index == 1) _abilities = abilities2;
            if (index == 2) _abilities = abilities3;

            Skill skill = null;
            int rand1 = -1;

            if (_newSkills.Count != 0)
            {
                rand1 = Random.Range(0, _newSkills.Count);
                skill = _newSkills[rand1];
            }

            if (skill != null)
            {
                if (res < (25 - skill.rarity * 5) && _newSkills.Count != 0)
                {

                    _newSkills.RemoveAt(rand1);

                    _chosenSkills.Add(skill);
                    skill.transform.position = chooses[index].transform.position;
                    news[index].SetActive(true);
                }
                else
                {
                    int res2 = Random.Range(0, 101);
                    int rand = Random.Range(0, _existingSkills.Count);
                    Skill s = _existingSkills[rand];
                    _existingSkills.RemoveAt(rand);

                    if (res2 < (80 + s.rarity * 3) || s.damage >= 2)
                    {
                        if (s.abilities.Count == 0 || s.abilities.Count == 1)
                        {
                            int abilitiyRes = Random.Range(0, 5);

                            _abilities[abilitiyRes].SetActive(true);
                            s.transform.position = chooses[index].transform.position;
                        }
                        else if (s.abilities.Count == 2)
                        {
                            _abilities[s.abilities[Random.Range(0, 2)].GetInd()].SetActive(true);
                            s.transform.position = chooses[index].transform.position;
                        }
                    }
                    else
                    {
                        s.transform.position = chooses[index].transform.position;
                        damageText[index].SetActive(true);
                    }

                    _chosenSkills.Add(s);
                }
            }
            else
            {
                int res2 = Random.Range(0, 101);
                int rand = Random.Range(0, _existingSkills.Count);
                Skill s = _existingSkills[rand];
                _existingSkills.RemoveAt(rand);

                if (res2 < (80 + s.rarity * 3) || s.damage >= 2)
                {
                    if (s.abilities.Count == 0 || s.abilities.Count == 1)
                    {
                        int abilitiyRes = Random.Range(0, 5);

                        _abilities[abilitiyRes].SetActive(true);
                        s.transform.position = chooses[index].transform.position;
                    }
                    else if (s.abilities.Count == 2)
                    {
                        _abilities[s.abilities[Random.Range(0, 2)].GetInd()].SetActive(true);
                        s.transform.position = chooses[index].transform.position;
                    }
                }
                else
                {
                    s.transform.position = chooses[index].transform.position;
                    damageText[index].SetActive(true);
                }

                _chosenSkills.Add(s);
            }

            index++;
            if (_chosenSkills.Count == 3) break;
        }

        return _chosenSkills;
    }

    public void SelectSkill()
    {
        var index = -1;

        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].clicked)
            {
                index = i;
                break;
            }
        }


        if (index == 0) _abilities = abilities1;
        if (index == 1) _abilities = abilities2;
        if (index == 2) _abilities = abilities3;

        if (skills[index].skillType == Skill.TYPE.ATTACK)
        {
            if (InventoryData.Instance.GetAttackSkillData(skills[index].skillIndex) == 0)
            {
                InventoryData.Instance.SetAttackSkillData(1, skills[index].skillIndex);
            }
            else
            {
                if (damageText[index].activeSelf)
                {
                    skills[index].ImproveDamage(1);
                }
                else
                {
                    var b = 0;
                    for (int i = 0; i < _abilities.Length; i++)
                    {
                        if (_abilities[i].activeSelf)
                        {
                            b = i;
                            break;
                        }
                    }

                    SkillData.Instance.SetAbilityData(b, SkillData.Instance.GetAbilityData(b, skills[index].skillIndex) + 1, skills[index].skillIndex);
                    for (int i = 0; i < skills[index].abilities.Count; i++)
                    {
                        skills[index].abilities[i].ApplyLevel(skills[index]);
                    }
                }
            }
        }
        else
        {
            if (InventoryData.Instance.GetMoveSkillData(skills[index].skillIndex) == 0)
            {
                InventoryData.Instance.SetMoveSkillData(1, skills[index].skillIndex);
            }
        }

        skills[index].clicked = false;
        
        GameSceneData.Instance.SetGameSceneTutorial(3);
        
        StartCoroutine(transition.EndTransition("GameScene"));
    }

    public void HoverSkill()
    {
        Abilities a = null;
        string text = "";
        desc.SetActive(true);

        var index = -1;

        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].hovered)
            {
                index = i;
                break;
            }
        }

        if (index == 0) _abilities = abilities1;
        if (index == 1) _abilities = abilities2;
        if (index == 2) _abilities = abilities3;

        if (news[index].activeSelf)
        {
            ShowSkillInfo(skills[index], null, "");
        }
        else
        {
            if (damageText[index].activeSelf)
            {
                text += "Damage: " + skills[index].damage + " -> " + (skills[index].damage + 1);
            }
            else
            {
                for (int i = 0; i < _abilities.Length; i++)
                {
                    if (_abilities[i].activeSelf)
                    {
                        if (i == 0) a = new StunTime();
                        if (i == 1) a = new Stun();
                        if (i == 2) a = new Recoil();
                        if (i == 3) a = new LifeSteal();
                        if (i == 4) a = new Bleeding();

                        text += a.GetInfo(skills[index]);
                    }
                }
            }
        }

        desc.GetComponent<TextMeshPro>().SetText(text);
    }

    public void ShowSkillInfo(Skill skill, Sprite skillIcon, string desc)
    {
        // Skill info
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AccessTile(i, j).gameObject.SetActive(true);
            }
        }

        _skull.SetActive(true);
        _name.gameObject.SetActive(true);
        _name.SetText(skill.skillName);
        _rarity.gameObject.SetActive(true);
        _rarities[skill.rarity - 1].SetActive(true);

        // Attack
        if (skill is SwordSkill)
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }

            AccessTile(2, 2).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(2, 2).transform);
            AccessTile(1, 2).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(1, 2).transform);
            AccessTile(2, 1).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(2, 1).transform);
        }

        if (skill is SpearSkill)
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }

            AccessTile(2, 2).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(2, 2).transform);
            AccessTile(3, 3).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(3, 3).transform);
            AccessTile(4, 4).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(4, 4).transform);

        }

        if (skill is HammerSkill)
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).Hitbox.SetActive(true);
                        shakeController.StartMouseOnShaking(AccessTile(i, j).transform);
                    }
                }
            }
        }

        if (skill is WhirlwindSkill)
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                        AccessTile(i, j).Hitbox.SetActive(true);
                        shakeController.StartMouseOnShaking(AccessTile(i, j).transform);
                    }
                }
            }
        }

        if (skill is AxeSkill)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }

            for (int i = 2; i < 5; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    AccessTile(i, j).Hitbox.SetActive(true);
                    shakeController.StartMouseOnShaking(AccessTile(i, j).transform);
                }
            }
        }

        if (skill is ChargeSkill)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }

            AccessTile(2, 3).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(2, 3).transform);
            AccessTile(3, 2).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(3, 2).transform);
            AccessTile(3, 4).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(3, 4).transform);
            AccessTile(4, 3).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(4, 3).transform);
        }

        if (skill is SmiteSkill)
        {
            AccessTile(4, 4).canTakeAction.SetActive(true);
            AccessTile(4, 4).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(4, 4).transform);
            AccessTile(3, 1).canTakeAction.SetActive(true);
            AccessTile(3, 1).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(3, 1).transform);

            _dummy1.SetActive(true);
            _dummy2.SetActive(true);
        }

        if (skill is DaggerSkill)
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }

            AccessTile(2, 2).Hitbox.SetActive(true);
            shakeController.StartMouseOnShaking(AccessTile(2, 2).transform);
        }

        // Move Skill
        if (skill is Move1Skill)
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    AccessTile(i, j).canTakeAction.SetActive(true);
                }
            }
        }

        if (skill is Move2Skill)
        {
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i == 0 || i == 3 || j == 0 || j == 3)
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (skill is Move3Skill)
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (i == 0 || i == 4 || j == 0 || j == 4)
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (skill is Move4Skill)
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (i == 0 || i == 4 || j == 0 || j == 4)
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (skill is HookSkill)
        {
            AccessTile(4, 4).canTakeAction.SetActive(true);
            AccessTile(3, 1).canTakeAction.SetActive(true);

            _dummy1.SetActive(true);
            _dummy2.SetActive(true);
        }

        if (skill is TeleportSkill)
        {
            AccessTile(0, 1).canTakeAction.SetActive(true);
            AccessTile(1, 0).canTakeAction.SetActive(true);
            AccessTile(1, 4).canTakeAction.SetActive(true);
            AccessTile(4, 1).canTakeAction.SetActive(true);
        }

        if (skill is LMoveSkill)
        {
            AccessTile(0, 3).canTakeAction.SetActive(true);
            AccessTile(2, 3).canTakeAction.SetActive(true);
            AccessTile(3, 2).canTakeAction.SetActive(true);
            AccessTile(3, 0).canTakeAction.SetActive(true);
        }

        if (skill is CursedTeleportSkill)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    AccessTile(i, j).canTakeAction.SetActive(true);
                }
            }
        }
    }

    public void HideSkillInfo()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AccessTile(i, j).transform.position = AccessTile(i, j).infoTilePos;
                AccessTile(i, j).canTakeAction.SetActive(false);
                AccessTile(i, j).Hitbox.SetActive(false);
                AccessTile(i, j).gameObject.SetActive(false);
            }
        }

        shakeController.resetShaking();

        _name.gameObject.SetActive(false);
        _rarity.gameObject.SetActive(false);
        foreach (GameObject r in _rarities)
        {
            r.SetActive(false);
        }

        _skull.SetActive(false);
        _dummy1.SetActive(false);
        _dummy2.SetActive(false);
    }

    public void UnhoverSkill()
    {
        desc.SetActive(false);
        HideSkillInfo();
    }

    private Tile AccessTile(int i, int j)
    {
        return _skillInfoTiles[i * 5 + j];
    }
}
