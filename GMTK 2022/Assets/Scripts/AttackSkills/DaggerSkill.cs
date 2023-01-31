using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerSkill : Skill
{
    public override void Start()
    {
        base.Start();

        skillName = "Dagger";
        skillIndex = 2;
        skillType = TYPE.ATTACK;
        skillDesc = "Attacks in 1 tile in a 1 tile radius.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.DAGGER;
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
        Grid.Instance.getTileAtPosition(x, y).Hitbox.SetActive(true);
        shaker.StartMouseOnShaking(Grid.Instance.getTileAtPosition(x, y).transform);
    }

    public override void HidePattern(int x, int y)
    {
        Grid.Instance.getTileAtPosition(x, y).Hitbox.SetActive(false);
        Grid.Instance.getTileAtPosition(x, y).transform.localPosition = Grid.Instance.getTileAtPosition(x, y).tilePos;
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
