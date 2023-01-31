using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3Skill : Skill
{
    public override void Start()
    {
        base.Start();

        skillName = "Move 3";
        skillIndex = 2;
        skillType = TYPE.MOVE;
        skillDesc = "Move 3 tiles.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.NONE;
        movePattern = Player.MOVEMENT.MOVE3;

        rarity = 1;
    }
    public override void ShowPattern(int x, int y, Shake shaker)
    {

    }

    public override void HidePattern(int x, int y)
    {

    }
}
