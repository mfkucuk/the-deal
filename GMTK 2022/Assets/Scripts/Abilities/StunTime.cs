using UnityEngine;

public class StunTime : Abilities
{
    private int stunChance = 25;
    private int stunTimeAmount = 1;

    public int ind = 0;

    public override bool DoAbility(Enemy enemy)
    {
        int res = Random.Range(0, 101);

        if (res < stunChance)
        {
            if (enemy.GetComponentInChildren<TextTimer>() != null)
            {
                enemy.GetComponentInChildren<TextTimer>().AddTime(stunTimeAmount);
                return true;
            }
        }

        return false;
    }

    public override void LevelUp(Skill skill)
    {
        SkillData.Instance.SetAbilityData(0, level + 1, skill.skillIndex);
        level = SkillData.Instance.GetAbilityData(0, skill.skillIndex);

        stunChance = 20 + level * 5;
        stunTimeAmount = level;
    }
    public override void ApplyLevel(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(0, skill.skillIndex);

        stunChance = 20 + level * 5;
        stunTimeAmount = level + 4;
    }

    public override int GetInd()
    {
        return ind;
    }

    public override string GetName()
    {
        return "Stun Time";
    }

    public override string GetInfo(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(0, skill.skillIndex) + 1;

        if (level == 1)
        {
            return "Stun Chance: " + (20 + level * 5) + "%\n" + "Stun Time Amount: " + (level + 4);
        }
        else
        {
            return "Stun Chance: " + (20 + (level - 1) * 5) + "% -> " + (20 + level * 5) + "%\n" + "Stun Time Amount: " + (level + 3) + " -> " + (level + 4);
        }
    }
}
