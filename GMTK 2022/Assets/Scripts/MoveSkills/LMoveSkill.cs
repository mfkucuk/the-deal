using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMoveSkill : Skill
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        skillName = "Jump";
        skillIndex = 4;
        skillType = TYPE.MOVE;
        skillDesc = "Move in a L-shape.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.NONE;
        movePattern = Player.MOVEMENT.LMOVE;

        rarity = 2;
    }
    public override void HidePattern(int x, int y)
    {

    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {

    }
}
