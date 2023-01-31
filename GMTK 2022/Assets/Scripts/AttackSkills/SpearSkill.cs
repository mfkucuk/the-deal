using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSkill : Skill
{
    public override void Start()
    {
        base.Start();

        skillName = "Spear";
        skillIndex = 1;
        skillType = TYPE.ATTACK;
        skillDesc = "Attacks in 3 tile length in a 1 tile radius.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.THRUST_3;
        movePattern = Player.MOVEMENT.NONE;

        rarity = 1;

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
        int newX2 = Player.Instance.PlayerPos.x - x;
        int newY2 = Player.Instance.PlayerPos.y - y;

        for (int i = 0; i < step; i++)
        {
            if (x - i * newX2 >= 0 && x - i * newX2 < 8 && y - i * newY2 >= 0 && y - i * newY2 < 8)
            {
                Grid.Instance.getTileAtPosition(x - i * newX2, y - i * newY2).Hitbox.SetActive(true);
                shaker.StartMouseOnShaking(Grid.Instance.getTileAtPosition(x - i * newX2, y - i * newY2).transform);
            }
        }
    }

    public override void HidePattern(int x, int y)
    {
        int newX2 = Player.Instance.PlayerPos.x - x;
        int newY2 = Player.Instance.PlayerPos.y - y;

        for (int i = 0; i < step; i++)
        {
            if (x - i * newX2 >= 0 && x - i * newX2 < 8 && y - i * newY2 >= 0 && y - i * newY2 < 8)
            {
                Grid.Instance.getTileAtPosition(x - i * newX2, y - i * newY2).Hitbox.SetActive(false);
                Grid.Instance.getTileAtPosition(x - i * newX2, y - i * newY2).transform.localPosition = Grid.Instance.getTileAtPosition(x - i * newX2, y - i * newY2).tilePos;
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
