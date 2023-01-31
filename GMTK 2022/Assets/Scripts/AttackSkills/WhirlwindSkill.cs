using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindSkill : Skill
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        skillName = "Whirlwind";
        skillIndex = 3;
        skillType = TYPE.ATTACK;
        skillDesc = "Attacks all tiles in a 1 tile radius.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.WHIRLWIND;
        movePattern = Player.MOVEMENT.NONE;

        rarity = 2;

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
        for (int i = Player.Instance.PlayerPos.x - step; i <= Player.Instance.PlayerPos.x + step; i++)
        {
            for (int j = Player.Instance.PlayerPos.y - step; j <= Player.Instance.PlayerPos.y + step; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8 && !(i == Player.Instance.PlayerPos.x && j == Player.Instance.PlayerPos.y))
                {
                    Grid.Instance.getTileAtPosition(i, j).Hitbox.SetActive(true);
                    shaker.StartMouseOnShaking(Grid.Instance.getTileAtPosition(i, j).transform);
                }
            }
        }
    }

    public override void HidePattern(int x, int y)
    {
        for (int i = Player.Instance.PlayerPos.x - step; i <= Player.Instance.PlayerPos.x + step; i++)
        {
            for (int j = Player.Instance.PlayerPos.y - step; j <= Player.Instance.PlayerPos.y + step; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8 && !(i == Player.Instance.PlayerPos.x && j == Player.Instance.PlayerPos.y))
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
