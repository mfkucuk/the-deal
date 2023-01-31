using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    private static InventoryData _instance;
    public static InventoryData Instance { get { return _instance; } }

    private const string INVENTORY_DATA_GET_GAME_OPENED = "INVENTORY_DATA_GET_GAME_OPENED";

    private const string ATTACK_DICE_FACE_COUNT = "ATTACK_DICE_FACE_COUNT";
    private const string MOVE_DICE_FACE_COUNT = "MOVE_DICE_FACE_COUNT";
    private const string IN_INVENTORY = "IN_INVENTORY";
    private const string MOVE_SKILL_COUNT = "MOVE_SKILL_COUNT";
    private const string ATTACK_SKILL_COUNT = "ATTACK_SKILL_COUNT";

    private const string S_ATTACK_DICE_FACE_COUNT = "S_ATTACK_DICE_FACE_COUNT";
    private const string S_MOVE_DICE_FACE_COUNT = "S_MOVE_DICE_FACE_COUNT";
    private const string S_IN_INVENTORY = "S_IN_INVENTORY";
    private const string S_MOVE_SKILL_COUNT = "S_MOVE_SKILL_COUNT";
    private const string S_ATTACK_SKILL_COUNT = "S_ATTACK_SKILL_COUNT";

    private const string LATEST_SKILL = "LATEST_SKILL";

    private string[] _attackSkillList;
    private string[] _moveSkillList;
    private string[] _attackPlaced;
    private string[] _movePlaced;

    private string[] _savedAttackSkillList;
    private string[] _savedMoveSkillList;
    private string[] _savedAttackPlaced;
    private string[] _savedMovePlaced;



    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        PlayerPrefs.SetInt(INVENTORY_DATA_GET_GAME_OPENED, 1);
        SetLatestSkill(-1);

        PlayerPrefs.SetInt(MOVE_SKILL_COUNT, 8);
        PlayerPrefs.SetInt(ATTACK_SKILL_COUNT, 8);

        _attackSkillList = new string[GetAttackSkillCount()];
        _moveSkillList = new string[GetMoveSkillCount()];
        _attackPlaced = new string[GetAttackSkillCount()];
        _movePlaced = new string[GetMoveSkillCount()];

        _savedAttackSkillList = new string[GetAttackSkillCount()];
        _savedMoveSkillList = new string[GetMoveSkillCount()];
        _savedAttackPlaced = new string[GetAttackSkillCount()];
        _savedMovePlaced = new string[GetMoveSkillCount()];

        SetAttackDiceFaceData(3);
        SetMoveDiceFaceData(3);
        SetInventoryData(0);

        // Hardcoded attack skills
        _attackSkillList[0] = "SwordSkill";
        _attackSkillList[1] = "SpearSkill";
        _attackSkillList[2] = "DaggerSkill";
        _attackSkillList[3] = "WhirlwindSkill";
        _attackSkillList[4] = "AxeSkill";
        _attackSkillList[5] = "ChargeSkill";
        _attackSkillList[6] = "SmiteSkill";
        _attackSkillList[7] = "HammerSkill";

        _attackPlaced[0] = "SwordPlaced";
        _attackPlaced[1] = "SpearPlaced";
        _attackPlaced[2] = "DaggerPlaced";
        _attackPlaced[3] = "WhirlwindPlaced";
        _attackPlaced[4] = "AxePlaced";
        _attackPlaced[5] = "ChargePlaced";
        _attackPlaced[6] = "SmitePlaced";
        _attackPlaced[7] = "HammerPlaced";

        // Hardcoded move skills
        _moveSkillList[0] = "Move1Skill";
        _moveSkillList[1] = "Move2Skill";
        _moveSkillList[2] = "Move3Skill";
        _moveSkillList[3] = "TeleportSkill";
        _moveSkillList[4] = "LMoveSkill";
        _moveSkillList[5] = "Move4Skill";
        _moveSkillList[6] = "HookSkill";
        _moveSkillList[7] = "CursedTeleportSkill";

        _movePlaced[0] = "Move1Placed";
        _movePlaced[1] = "Move2Placed";
        _movePlaced[2] = "Move3Placed";
        _movePlaced[3] = "TeleportPlaced";
        _movePlaced[4] = "LMovePlaced";
        _movePlaced[5] = "Move4Placed";
        _movePlaced[6] = "HookPlaced";
        _movePlaced[7] = "CursedTeleportPlaced";

        for (int i = 0; i < _attackSkillList.Length; i++)
        {
            _savedAttackSkillList[i] = _attackSkillList[i] + "Saved";
        }

        for (int i = 0; i < _moveSkillList.Length; i++)
        {
            _savedMoveSkillList[i] = _moveSkillList[i] + "Saved";
        }

        for (int i = 0; i < _attackPlaced.Length; i++)
        {
            _savedAttackPlaced[i] = _attackPlaced[i] + "Saved";
        }

        for (int i = 0; i < _movePlaced.Length; i++)
        {
            _savedMovePlaced[i] = _movePlaced[i] + "Saved";
        }

        SetAttackSkillData(2, 0);
        SetAttackSkillData(2, 1);
        SetAttackSkillData(2, 2);

        SetMoveSkillData(2, 0);
        SetMoveSkillData(2, 1);
        SetMoveSkillData(2, 2);

        SetAttackSkillPlaced(0, 0);
        SetAttackSkillPlaced(1, 1);
        SetAttackSkillPlaced(2, 2);

        SetMoveSkillPlaced(0, 0);
        SetMoveSkillPlaced(1, 1);
        SetMoveSkillPlaced(2, 2);

        for (int i = 3; i < GetAttackSkillCount(); i++)
        {
            SetAttackSkillData(0, i);
        }

        for (int i = 3; i < GetMoveSkillCount(); i++)
        {
            SetMoveSkillData(0, i);
        }

        if (GetGameOpened() == 1)
        {
            GetSaveData();
            PlayerPrefs.SetInt(INVENTORY_DATA_GET_GAME_OPENED, 0);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public int GetGameOpened()
    {
        return PlayerPrefs.GetInt(INVENTORY_DATA_GET_GAME_OPENED);
    }

    public void SetInventoryData(int isInventory)
    {
        PlayerPrefs.SetInt(IN_INVENTORY, isInventory);
    }

    public int GetInventoryData()
    {
        return PlayerPrefs.GetInt(IN_INVENTORY);
    }

    public void SetAttackDiceFaceData(int diceFaceCount)
    {
        PlayerPrefs.SetInt(ATTACK_DICE_FACE_COUNT, diceFaceCount);
    }

    public int GetAttackDiceFaceData()
    {
        return PlayerPrefs.GetInt(ATTACK_DICE_FACE_COUNT);
    }

    public void SetMoveDiceFaceData(int diceFaceCount)
    {
        PlayerPrefs.SetInt(MOVE_DICE_FACE_COUNT, diceFaceCount);
    }

    public int GetMoveDiceFaceData()
    {
        return PlayerPrefs.GetInt(MOVE_DICE_FACE_COUNT);
    }

    public void SetMoveSkillData(int isEquipped, int index)
    {
        PlayerPrefs.SetInt(_moveSkillList[index], isEquipped);
    }

    public int GetMoveSkillData(int index)
    {
        return PlayerPrefs.GetInt(_moveSkillList[index]);
    }

    public void SetAttackSkillData(int isEquipped, int index)
    {
        PlayerPrefs.SetInt(_attackSkillList[index], isEquipped);
    }

    public int GetAttackSkillData(int index)
    {
        return PlayerPrefs.GetInt(_attackSkillList[index]);
    }

    public void SetMoveSkillPlaced(int placed, int index)
    {
        PlayerPrefs.SetInt(_movePlaced[index], placed);
    }

    public int GetMoveSkillPlaced(int index)
    {
        return PlayerPrefs.GetInt(_movePlaced[index]);
    }

    public void SetAttackSkillPlaced(int placed, int index)
    {
        PlayerPrefs.SetInt(_attackPlaced[index], placed);
    }

    public int GetAttackSkillPlaced(int index)
    {
        return PlayerPrefs.GetInt(_attackPlaced[index]);
    }

    public int GetMoveSkillCount()
    {
        return PlayerPrefs.GetInt(MOVE_SKILL_COUNT);
    }

    public int GetAttackSkillCount()
    {
        return PlayerPrefs.GetInt(ATTACK_SKILL_COUNT);
    }

    public void SetLatestSkill(int latestSkill)
    {
        PlayerPrefs.SetInt(LATEST_SKILL, latestSkill);
    }

    public int GetLatestSkill()
    {
        return PlayerPrefs.GetInt(LATEST_SKILL);
    }

    public void ResetData()
    {
        SetAttackDiceFaceData(3);
        SetMoveDiceFaceData(3);
        SetInventoryData(0);

        SetAttackSkillData(2, 0);
        SetAttackSkillData(2, 1);
        SetAttackSkillData(2, 2);

        SetMoveSkillData(2, 0);
        SetMoveSkillData(2, 1);
        SetMoveSkillData(2, 2);

        SetAttackSkillPlaced(0, 0);
        SetAttackSkillPlaced(1, 1);
        SetAttackSkillPlaced(2, 2);

        SetMoveSkillPlaced(0, 0);
        SetMoveSkillPlaced(1, 1);
        SetMoveSkillPlaced(2, 2);

        for (int i = 3; i < GetAttackSkillCount(); i++)
        {
            SetAttackSkillData(0, i);
        }

        for (int i = 3; i < GetMoveSkillCount(); i++)
        {
            SetMoveSkillData(0, i);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(S_ATTACK_DICE_FACE_COUNT, GetAttackDiceFaceData());
        PlayerPrefs.SetInt(S_MOVE_DICE_FACE_COUNT, GetMoveDiceFaceData());
        PlayerPrefs.SetInt(S_IN_INVENTORY, GetInventoryData());
        PlayerPrefs.SetInt(S_ATTACK_SKILL_COUNT, GetAttackSkillCount());
        PlayerPrefs.SetInt(S_MOVE_SKILL_COUNT, GetMoveSkillCount());

        for (int i = 0; i < _attackSkillList.Length; i++)
        {
            PlayerPrefs.SetInt(_savedAttackSkillList[i], GetAttackSkillData(i));
        }

        for (int i = 0; i < _moveSkillList.Length; i++)
        {
            PlayerPrefs.SetInt(_savedMoveSkillList[i], GetMoveSkillData(i));
        }

        for (int i = 0; i < _attackPlaced.Length; i++)
        {
            PlayerPrefs.SetInt(_savedAttackPlaced[i], GetAttackSkillPlaced(i));
        }

        for (int i = 0; i < _movePlaced.Length; i++)
        {
            PlayerPrefs.SetInt(_savedMovePlaced[i], GetMoveSkillPlaced(i));
        }
    }

    public void GetSaveData()
    {
        PlayerPrefs.SetInt(ATTACK_DICE_FACE_COUNT, PlayerPrefs.GetInt(S_ATTACK_DICE_FACE_COUNT, 3));
        PlayerPrefs.SetInt(MOVE_DICE_FACE_COUNT, PlayerPrefs.GetInt(S_MOVE_DICE_FACE_COUNT, 3));
        PlayerPrefs.SetInt(IN_INVENTORY, PlayerPrefs.GetInt(S_IN_INVENTORY, 0));
        PlayerPrefs.SetInt(ATTACK_SKILL_COUNT, PlayerPrefs.GetInt(S_ATTACK_SKILL_COUNT, 8));
        PlayerPrefs.SetInt(MOVE_SKILL_COUNT, PlayerPrefs.GetInt(S_MOVE_SKILL_COUNT, 8));

        PlayerPrefs.SetInt(_attackSkillList[0], PlayerPrefs.GetInt(_savedAttackSkillList[0], 2));
        PlayerPrefs.SetInt(_attackSkillList[1], PlayerPrefs.GetInt(_savedAttackSkillList[1], 2));
        PlayerPrefs.SetInt(_attackSkillList[2], PlayerPrefs.GetInt(_savedAttackSkillList[2], 2));

        PlayerPrefs.SetInt(_moveSkillList[0], PlayerPrefs.GetInt(_savedMoveSkillList[0], 2));
        PlayerPrefs.SetInt(_moveSkillList[1], PlayerPrefs.GetInt(_savedMoveSkillList[1], 2));
        PlayerPrefs.SetInt(_moveSkillList[2], PlayerPrefs.GetInt(_savedMoveSkillList[2], 2));

        PlayerPrefs.SetInt(_attackPlaced[0], PlayerPrefs.GetInt(_savedAttackPlaced[0], 0));
        PlayerPrefs.SetInt(_attackPlaced[1], PlayerPrefs.GetInt(_savedAttackPlaced[1], 1));
        PlayerPrefs.SetInt(_attackPlaced[2], PlayerPrefs.GetInt(_savedAttackPlaced[2], 2));

        PlayerPrefs.SetInt(_movePlaced[0], PlayerPrefs.GetInt(_savedMovePlaced[0], 0));
        PlayerPrefs.SetInt(_movePlaced[1], PlayerPrefs.GetInt(_savedMovePlaced[1], 1));
        PlayerPrefs.SetInt(_movePlaced[2], PlayerPrefs.GetInt(_savedMovePlaced[2], 2));

        for (int i = 3; i < _attackSkillList.Length; i++)
        {
            PlayerPrefs.SetInt(_attackSkillList[i], PlayerPrefs.GetInt(_savedAttackSkillList[i], 0));
        }

        for (int i = 3; i < _moveSkillList.Length; i++)
        {
            PlayerPrefs.SetInt(_moveSkillList[i], PlayerPrefs.GetInt(_savedMoveSkillList[i], 0));
        }

        for (int i = 3; i < _attackPlaced.Length; i++)
        {
            PlayerPrefs.SetInt(_attackPlaced[i], PlayerPrefs.GetInt(_savedAttackPlaced[i], 0));
        }

        for (int i = 3; i < _movePlaced.Length; i++)
        {
            PlayerPrefs.SetInt(_movePlaced[i], PlayerPrefs.GetInt(_savedMovePlaced[i], 0));
        }
    }
}
