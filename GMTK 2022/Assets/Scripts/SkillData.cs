using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    private static SkillData _instance;
    public static SkillData Instance { get { return _instance; } }

    private const string SKILL_DATA_GET_GAME_OPENED = "SKILL_DATA_GET_GAME_OPENED";

    private string[] _attackStep;
    private string[] _attackDamage;
    private string[,] _abilities;

    private string[] _savedAttackStep;
    private string[] _savedAttackDamage;
    private string[,] _savedAbilities;

    public int STUN_TIME = 0;
    public int STUN_TURN = 1;
    public int RECOIL = 2;
    public int LIFESTEAL = 3;
    public int BLEEDING = 4;


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayerPrefs.SetInt(SKILL_DATA_GET_GAME_OPENED, 1);

        _attackStep = new string[InventoryData.Instance.GetAttackSkillCount()];
        _attackDamage = new string[InventoryData.Instance.GetAttackSkillCount()];
        _abilities = new string[5, InventoryData.Instance.GetAttackSkillCount()];

        _savedAttackStep = new string[InventoryData.Instance.GetAttackSkillCount()];
        _savedAttackDamage = new string[InventoryData.Instance.GetAttackSkillCount()];
        _savedAbilities = new string[5, InventoryData.Instance.GetAttackSkillCount()];
        
        _attackStep[0] = "SwordStep";
        _attackStep[1] = "SpearStep";
        _attackStep[2] = "DaggerStep";
        _attackStep[3] = "WhirlwindStep";
        _attackStep[4] = "AxeStep";
        _attackStep[5] = "ChargeStep";
        _attackStep[6] = "SmiteStep";
        _attackStep[7] = "HammerStep";
        
        _attackDamage[0] = "SwordDamage";
        _attackDamage[1] = "SpearDamage";
        _attackDamage[2] = "DaggerDamage";
        _attackDamage[3] = "WhirlwindDamage";
        _attackDamage[4] = "AxeDamage";
        _attackDamage[5] = "ChargeDamage";
        _attackDamage[6] = "SmiteDamage";
        _attackDamage[7] = "HammerDamage";
        
        SetAttackStepData(0, 0);
        SetAttackStepData(3, 1);
        SetAttackStepData(1, 2);
        SetAttackStepData(1, 3);
        SetAttackStepData(1, 4);
        SetAttackStepData(1, 5);
        SetAttackStepData(0, 6);
        SetAttackStepData(1, 7);
        
        SetAttackDamageData(1, 0);
        SetAttackDamageData(1, 1);
        SetAttackDamageData(1, 2);
        SetAttackDamageData(1, 3);
        SetAttackDamageData(1, 4);
        SetAttackDamageData(1, 5);
        SetAttackDamageData(1, 6);
        SetAttackDamageData(1, 7);

        // Stun Time
        _abilities[0, 0] = "SwordStunTime";
        _abilities[0, 1] = "SpearStunTime";
        _abilities[0, 2] = "DaggerStunTime";
        _abilities[0, 3] = "WhirlwindStunTime";
        _abilities[0, 4] = "AxeStunTime";
        _abilities[0, 5] = "ChargeStunTime";
        _abilities[0, 6] = "SmiteStunTime";
        _abilities[0, 7] = "HammerStunTime";

        // Stun 
        _abilities[1, 0] = "SwordStunTurn";
        _abilities[1, 1] = "SpearStunTurn";
        _abilities[1, 2] = "DaggerStunTurn";
        _abilities[1, 3] = "WhirlwindStunTurn";
        _abilities[1, 4] = "AxeStunTurn";
        _abilities[1, 5] = "ChargeStunTurn";
        _abilities[1, 6] = "SmiteStunTurn";
        _abilities[1, 7] = "HammerStunTurn";

        // Recoil
        _abilities[2, 0] = "SwordRecoil";
        _abilities[2, 1] = "SpearRecoil";
        _abilities[2, 2] = "DaggerRecoil";
        _abilities[2, 3] = "WhirlwindRecoil";
        _abilities[2, 4] = "AxeRecoil";
        _abilities[2, 5] = "ChargeRecoil";
        _abilities[2, 6] = "SmiteRecoil";
        _abilities[2, 7] = "HammerRecoil";

        // Life Steal
        _abilities[3, 0] = "SwordLifeSteal";
        _abilities[3, 1] = "SpearLifeSteal";
        _abilities[3, 2] = "DaggerLifeSteal";
        _abilities[3, 3] = "WhirlwindLifeSteal";
        _abilities[3, 4] = "AxeLifeSteal";
        _abilities[3, 5] = "ChargeLifeSteal";
        _abilities[3, 6] = "SmiteLifeSteal";
        _abilities[3, 7] = "HammerLifeSteal";

        // Bleeding
        _abilities[4, 0] = "SwordBleeding";
        _abilities[4, 1] = "SpearBleeding";
        _abilities[4, 2] = "DaggerBleeding";
        _abilities[4, 3] = "WhirlwindBleeding";
        _abilities[4, 4] = "AxeBleeding";
        _abilities[4, 5] = "ChargeBleeding";
        _abilities[4, 6] = "SmiteBleeding";
        _abilities[4, 7] = "HammerBleeding";

        // Save Data
        for (int i = 0; i < _attackDamage.Length; i++)
        {
            _savedAttackDamage[i] = _attackDamage[i] + "Saved";
        }

        for (int i = 0; i < _attackStep.Length; i++)
        {
            _savedAttackStep[i] = _attackStep[i] + "Saved";
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < _attackDamage.Length; j++)
            {
                _savedAbilities[i, j] = _abilities[i, j] + "Saved";
            }
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < InventoryData.Instance.GetAttackSkillCount(); j++)
            {
                SetAbilityData(i, 0, j);
            }
        }

        if (GetGameOpened() == 1)
        {
            GetSaveData();
            PlayerPrefs.SetInt(SKILL_DATA_GET_GAME_OPENED, 0);
        }
    }

    public int GetGameOpened()
    {
        return PlayerPrefs.GetInt(SKILL_DATA_GET_GAME_OPENED);
    }

    public void SetAttackStepData(int step, int index)
    {
        PlayerPrefs.SetInt(_attackStep[index], step);
    }

    public int GetAttackStepData(int index)
    {
        return PlayerPrefs.GetInt(_attackStep[index]);
    }
    public void SetAttackDamageData(int damage, int index)
    {
        PlayerPrefs.SetInt(_attackDamage[index], damage);
    }

    public int GetAttackDamageData(int index)
    {
        return PlayerPrefs.GetInt(_attackDamage[index], 1);
    }

    public void SetAbilityData(int ability, int level, int index)
    {
        PlayerPrefs.SetInt(_abilities[ability, index], level);
    }

    public int GetAbilityData(int ability, int index)
    {
        return PlayerPrefs.GetInt(_abilities[ability, index], 0);
    }

    public void ResetData()
    {
        SetAttackStepData(0, 0);
        SetAttackStepData(3, 1);
        SetAttackStepData(1, 2);
        SetAttackStepData(1, 3);
        SetAttackStepData(1, 4);
        SetAttackStepData(1, 5);
        SetAttackStepData(0, 6);
        SetAttackStepData(1, 7);

        SetAttackDamageData(1, 0);
        SetAttackDamageData(1, 1);
        SetAttackDamageData(1, 2);
        SetAttackDamageData(1, 3);
        SetAttackDamageData(1, 4);
        SetAttackDamageData(1, 5);
        SetAttackDamageData(1, 6);
        SetAttackDamageData(1, 7);

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < InventoryData.Instance.GetAttackSkillCount(); j++)
            {
                SetAbilityData(i, 0, j);
            }
        }
    }

    public void SaveData()
    {
        for (int i = 0; i < _attackDamage.Length; i++)
        {
            PlayerPrefs.SetInt(_savedAttackDamage[i], GetAttackDamageData(i));
        }

        for (int i = 0; i < _attackStep.Length; i++)
        {
            PlayerPrefs.SetInt(_savedAttackStep[i], GetAttackStepData(i));
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < _attackDamage.Length; j++)
            {
                PlayerPrefs.SetInt(_savedAbilities[i, j], GetAbilityData(i, j));
            }
        }
    }

    public void GetSaveData()
    {
        for (int i = 0; i < _attackDamage.Length; i++)
        {
            PlayerPrefs.SetInt(_attackDamage[i], PlayerPrefs.GetInt(_savedAttackDamage[i], 1));
        }

        PlayerPrefs.SetInt(_attackStep[0], PlayerPrefs.GetInt(_savedAttackStep[0], 0));
        PlayerPrefs.SetInt(_attackStep[1], PlayerPrefs.GetInt(_savedAttackStep[1], 3));
        PlayerPrefs.SetInt(_attackStep[6], PlayerPrefs.GetInt(_savedAttackStep[6], 0));

        for (int i = 0; i < _attackStep.Length; i++)
        {
            if (i == 0 || i == 1 || i == 6) continue;
            PlayerPrefs.SetInt(_attackStep[i], PlayerPrefs.GetInt(_savedAttackStep[i], 1));
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < _attackDamage.Length; j++)
            {
                PlayerPrefs.SetInt(_abilities[i, j], PlayerPrefs.GetInt(_savedAbilities[i, j], 0));
            }
        }
    }
}
