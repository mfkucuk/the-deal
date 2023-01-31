using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : Abilities
{
    public int recoilChance = 25;
    private int recoilSquareAmount = 1;

    public int ind = 2;
    public override bool DoAbility(Enemy enemy)
    {
        int res = Random.Range(0, 101);

        if (res < recoilChance)
        {
            enemy.Recoil(recoilSquareAmount);
            return true;
        }

        return false;
    }

    public override void LevelUp(Skill skill)
    {
        SkillData.Instance.SetAbilityData(2, level + 1, skill.skillIndex);
        level = SkillData.Instance.GetAbilityData(2, skill.skillIndex);

        recoilChance = 20 + level * 5;
        recoilSquareAmount = ((level + 1) / 2);
    }

    public override void ApplyLevel(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(2, skill.skillIndex);

        recoilChance = 20 + level * 5;
        recoilSquareAmount = ((level + 1) / 2);
    }

    public override int GetInd()
    {
        return ind;
    }

    public override string GetName()
    {
        return "Knockback";
    }

    public override string GetInfo(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(2, skill.skillIndex) + 1;

        if (level == 1)
        {
            return "Knockback Chance: " + (20 + level * 5) + "%\n" + "Knockback Amount: " + level;
        }
        else
        {
            return "Knockback Chance: " + (20 + (level - 1) * 5) + "% -> " + (20 + level * 5) + "%\n" + "Knockback Amount: " + (((level) / 2)) + " -> " + ((level + 1) / 2);
        }
    }
}
