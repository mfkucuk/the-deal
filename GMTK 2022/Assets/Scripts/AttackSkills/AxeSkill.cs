using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSkill : Skill
{
    public override void Start()
    {
        base.Start();

        skillName = "Axe";
        skillIndex = 4;
        skillType = TYPE.ATTACK;
        skillDesc = "Attacks in 3x3 area in a 3 tile radius.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.LONG_AOE;
        movePattern = Player.MOVEMENT.NONE;

        rarity = 4;

        step = SkillData.Instance.GetAttackStepData(skillIndex);
        damage = SkillData.Instance.GetAttackDamageData(skillIndex);

        if (SkillData.Instance.GetAbilityData(SkillData.Instance.STUN_TIME, skillIndex) != 0) abilities.Add(new StunTime());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.STUN_TURN, skillIndex) != 0) abilities.Add(new Stun());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.RECOIL, skillIndex) != 0) abilities.Add(new Recoil());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.LIFESTEAL, skillIndex) != 0) abilities.Add(new LifeSteal());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.BLEEDING, skillIndex) != 0) abilities.Add(new Bleeding());

        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].ApplyLevel(this);
            ability[abilities[i].GetInd()].SetActive(true);
            ability[abilities[i].GetInd()].transform.position = slots[i].transform.position;
        }
    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {
        for (int i = x - step; i <= x + step; i++)
        {
            for (int j = y - step; j <= y + step; j++)
            {
                if ((i >= 0 && i < 8 && j >= 0 && j < 8) && !(i == Player.Instance.PlayerPos.x && j == Player.Instance.PlayerPos.y))
                {
                    Grid.Instance.getTileAtPosition(i, j).Hitbox.SetActive(true);
                    shaker.StartMouseOnShaking(Grid.Instance.getTileAtPosition(i, j).transform);
                }
            }
        }
    }

    public override void HidePattern(int x, int y)
    {
        for (int i = x - step; i <= x + step; i++)
        {
            for (int j = y - step; j <= y + step; j++)
            {
                if ((i >= 0 && i < 8 && j >= 0 && j < 8) && !(i == Player.Instance.PlayerPos.x && j == Player.Instance.PlayerPos.y))
                {
                    Grid.Instance.getTileAtPosition(i, j).Hitbox.SetActive(false);
                    Grid.Instance.getTileAtPosition(i, j).transform.localPosition = Grid.Instance.getTileAtPosition(i, j).tilePos;
                }
            }
        }
    }

    public override void ImproveStep(int byThisValue)
    {
        base.ImproveStep(byThisValue);
        SkillData.Instance.SetAttackStepData(step, skillIndex);
    }

    public override void ImproveDamage(int byThisValue)
    {
        base.ImproveDamage(byThisValue);
        SkillData.Instance.SetAttackDamageData(damage, skillIndex);
    }
}
