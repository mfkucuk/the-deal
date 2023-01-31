using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : Abilities
{
    private int bleedingChance = 25;
    private int bleedingCount = 1;

    public int ind = 4;

    public override bool DoAbility(Enemy enemy)
    {
        int res = Random.Range(0, 101);

        if (res < bleedingChance)
        {
            enemy.bleedingCount = bleedingCount;
            return true;
        }

        return false;
    }

    public override void LevelUp(Skill skill)
    {
        SkillData.Instance.SetAbilityData(4, level + 1, skill.skillIndex);
        level = SkillData.Instance.GetAbilityData(4, skill.skillIndex);

        bleedingChance = 20 + level * 5;
        bleedingCount = ((level + 1) / 2);
    }

    public override void ApplyLevel(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(4, skill.skillIndex);

        bleedingChance = 20 + level * 5;
        bleedingCount = ((level + 1) / 2);
    }
    public override int GetInd()
    {
        return ind;
    }
    public override string GetName()
    {
        return "Bleeding";
    }

    public override string GetInfo(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(4, skill.skillIndex) + 1;

        if (level == 1)
        {
            return "Bleeding Chance: " + (20 + level * 5) + "%\n" + "Bleeding Amount: " + ((level + 1) / 2);
        }
        else
        {
            return "Bleeding Chance: " + (20 + (level - 1) * 5) + "% -> " + (20 + level * 5) + "%\n" + "Bleeding Amount: " + (((level) / 2)) + " -> " + ((level + 1) / 2);
        }
    }
}
