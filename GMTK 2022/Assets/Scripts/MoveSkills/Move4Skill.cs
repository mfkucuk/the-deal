using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move4Skill : Skill
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        skillName = "Move 4";
        skillIndex = 5;
        skillType = TYPE.MOVE;
        skillDesc = "Teleport to one edge of the board.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.NONE;
        movePattern = Player.MOVEMENT.MOVE4;

        rarity = 4;
    }
    public override void HidePattern(int x, int y)
    {

    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {

    }
}
