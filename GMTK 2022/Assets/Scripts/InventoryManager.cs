using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager Instance { get { return _instance; } }

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

    // Skill description related
    [SerializeField] private TMP_Text skillDescriptionText;
    [SerializeField] private SpriteRenderer skillIconRenderer;
    [SerializeField] private TMP_Text skillDescription;

    [SerializeField] private Skill[] _attackSkillPrefabs;
    [SerializeField] private Skill[] _moveSkillPrefabs;
    [SerializeField] private Tile[] _skillInfoTiles;
    [SerializeField] private GameObject _skull;
    [SerializeField] private GameObject _dummy1;
    [SerializeField] private GameObject _dummy2;
    [SerializeField] private GameObject[] rarity;
    [SerializeField] private TMP_Text rarityText;

    private float moveDuration = 0.2f;

    public int attackDiceFaceCount;
    public int moveDiceFaceCount;

    public DiceFace[] attackDiceFaces;
    public DiceFace[] moveDiceFaces;
    public ArrayList attackSkills = new ArrayList();
    public ArrayList moveSkills = new ArrayList();

    // Don't touch this
    public GameObject[] attackSlots;
    public GameObject[] moveSlots;

    [SerializeField] private Shake shakeController;

    private void Start()
    {

        InventoryData.Instance.SetInventoryData(1);
        attackDiceFaceCount = InventoryData.Instance.GetAttackDiceFaceData();
        moveDiceFaceCount = InventoryData.Instance.GetMoveDiceFaceData();

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

        for (int i = 0; i < 6; i++)
        {
            attackDiceFaces[i].gameObject.SetActive(false);
            moveDiceFaces[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < attackDiceFaceCount; i++)
        {
            attackDiceFaces[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < moveDiceFaceCount; i++)
        {
            moveDiceFaces[i].gameObject.SetActive(true);
        }


        HideSkillInfo();

        // 1 is for skill exists, but not equipped.
        for (int i = 0; i < InventoryData.Instance.GetAttackSkillCount(); i++)
        {
            if (InventoryData.Instance.GetAttackSkillData(i) == 1)
            {
                Skill newSkill = Instantiate(_attackSkillPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                attackSkills.Add(newSkill);
            }
        }

        for (int i = 0; i < InventoryData.Instance.GetMoveSkillCount(); i++)
        {
            if (InventoryData.Instance.GetMoveSkillData(i) == 1)
            {
                
                Skill newSkill = Instantiate(_moveSkillPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                moveSkills.Add(newSkill);
            }
        }

        // 2 is for skill exists and equipped

        var index = -1;
        for (int i = 0; i < InventoryData.Instance.GetAttackSkillCount(); i++)
        {
            if (InventoryData.Instance.GetAttackSkillData(i) == 2)
            {
                index = InventoryData.Instance.GetAttackSkillPlaced(i);
                Skill newSkill = Instantiate(_attackSkillPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                if (!attackDiceFaces[index].full)
                {
                    newSkill.transform.position = attackDiceFaces[index].transform.position;
                    newSkill.originalPos = attackDiceFaces[index].transform.position;
                    attackDiceFaces[index].full = true;
                    attackDiceFaces[index].activeSkill = newSkill;
                }
            }
        }

        for (int i = 0; i < InventoryData.Instance.GetMoveSkillCount(); i++)
        {
            if (InventoryData.Instance.GetMoveSkillData(i) == 2)
            {
                index = InventoryData.Instance.GetMoveSkillPlaced(i);
                Skill newSkill = Instantiate(_moveSkillPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                if (!moveDiceFaces[index].full)
                {
                    newSkill.transform.position = moveDiceFaces[index].transform.position;
                    newSkill.originalPos = moveDiceFaces[index].transform.position;
                    moveDiceFaces[index].full = true;
                    moveDiceFaces[index].activeSkill = newSkill;
                }
            }
        }

        OnSceneLoad();
    }

    public void OnSceneLoad()
    {
        for (int i = 0; i < moveSkills.Count; i++)
        {
            ((Skill)moveSkills[i]).originalPos = moveSlots[i].transform.position;
            ((Skill)moveSkills[i]).transform.position = moveSlots[i].transform.position;
        }

        for (int i = 0; i < attackSkills.Count; i++)
        {
            ((Skill)attackSkills[i]).originalPos = attackSlots[i].transform.position;
            ((Skill)attackSkills[i]).transform.position = attackSlots[i].transform.position;
        }
    }

    private void OnDestroy()
    {
        InventoryData.Instance.SetInventoryData(0);
    }

    public void OnSkillEquiped(Skill eSkill)
    {
        AudioManager.Instance.Play("InventoryDiceFace");
        
        if (eSkill.skillType == Skill.TYPE.MOVE)
        {
            InventoryData.Instance.SetMoveSkillData(2, eSkill.skillIndex);
            moveSkills.Remove(eSkill);
            for (int i = 0; i < moveSkills.Count; i++)
            {
                ((Skill)moveSkills[i]).originalPos = moveSlots[i].transform.position;
                ((Skill)moveSkills[i]).transform.DOMove(moveSlots[i].transform.position, moveDuration);
            }
        }
        else
        {
            InventoryData.Instance.SetAttackSkillData(2, eSkill.skillIndex);
            attackSkills.Remove(eSkill);
            for (int i = 0; i < attackSkills.Count; i++)
            {
                
                ((Skill)attackSkills[i]).originalPos = attackSlots[i].transform.position;
                ((Skill)attackSkills[i]).transform.DOMove(attackSlots[i].transform.position, moveDuration);
            }
        }
        
    }

    public void OnSkillRemoved(Skill rSkill)
    {
        
        AudioManager.Instance.Play("InventoryDiceFaceOut");
        
        if (rSkill.skillType == Skill.TYPE.MOVE)
        {
            InventoryData.Instance.SetMoveSkillData(1, rSkill.skillIndex);
            moveSkills.Add(rSkill);
            for (int i = 0; i < moveSkills.Count; i++)
            {
                
                ((Skill)moveSkills[i]).originalPos = moveSlots[i].transform.position;
                ((Skill)moveSkills[i]).transform.DOMove(moveSlots[i].transform.position, moveDuration);
            }
        }
        else
        {
            InventoryData.Instance.SetAttackSkillData(1, rSkill.skillIndex);
            attackSkills.Add(rSkill);
            for (int i = 0; i < attackSkills.Count; i++)
            {
                ((Skill)attackSkills[i]).originalPos = attackSlots[i].transform.position;
                ((Skill)attackSkills[i]).transform.DOMove(attackSlots[i].transform.position, moveDuration);
            }
        }
    }

    public bool CheckFilled()
    {
        bool filled = true;

        for (int i = 0; i < moveDiceFaceCount; i++)
        {
            if (moveDiceFaces[i].activeSkill == null)
            {
                filled = false;
            }
        }

        for (int i = 0; i < attackDiceFaceCount; i++)
        {
            if (attackDiceFaces[i].activeSkill == null)
            {
                filled = false;
            }
        }

        return filled;
    }

    public void ShakeEmptyFaces()
    {
        for (int i = 0; i < moveDiceFaceCount; i++)
        {
            if (moveDiceFaces[i].activeSkill == null)
            {
                shakeController.DoShake(moveDiceFaces[i].gameObject.transform);
            }
        }

        for (int i = 0; i < attackDiceFaceCount; i++)
        {
            if (attackDiceFaces[i].activeSkill == null)
            {
                shakeController.DoShake(attackDiceFaces[i].gameObject.transform);
            }
        }
    }

    public void ShowSkillInfo(Skill skill, Sprite skillIcon, string desc)
    {
        skillDescriptionText.gameObject.SetActive(true);
        skillDescription.gameObject.SetActive(true);

        rarityText.gameObject.SetActive(true);
        rarityText.SetText("Rarity: ");

        skillDescriptionText.SetText(skill.skillName);
        skillDescription.SetText(desc);

        rarity[skill.rarity - 1].SetActive(true);

        // Skill info
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                AccessTile(i, j).gameObject.SetActive(true);
            }
        }

        _skull.SetActive(true);

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
                    if (!(i == 1 && j == 1))
                    {
                        AccessTile(i, j).canTakeAction.SetActive(true);
                    }
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
        skillDescriptionText.gameObject.SetActive(false);
        skillIconRenderer.gameObject.SetActive(false);
        skillDescription.gameObject.SetActive(false);
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

        rarityText.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            rarity[i].SetActive(false);
        }

        shakeController.resetShaking();

        _skull.SetActive(false);
        _dummy1.SetActive(false);
        _dummy2.SetActive(false);
    }

    private Tile AccessTile(int i, int j)
    {
        return _skillInfoTiles[i * 5 + j];
    }
}
