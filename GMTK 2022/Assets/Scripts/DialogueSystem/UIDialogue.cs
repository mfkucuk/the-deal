using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "UIDialogueData", menuName = "ScriptableObjects/UIDialogueDataScriptableObject", order = 1)]
public class UIDialogue : Dialogue
{
    
    public Sprite[] sprites;
    public RuntimeAnimatorController[] animators;

    public List<AnimatorStateNamesDic> animatorStateNames;
    public List<DiffColorWordIndexDic> diffColorWordIndex;
    public List<DiffColorDic> diffColor;
}

[System.Serializable]
public class AnimatorStateNamesDic {
    public int id;
    public string animationStateName;
}

[System.Serializable]
public class DiffColorWordIndexDic {
    public int id;
    public int diffColorWordIndex;
}

[System.Serializable]
public class DiffColorDic {
    public int id;
    public Color diffColor;
}


public class RealUIDialogue : RealDialogue
{
    public string[] animatorStateNames;
    public int[] diffColorWordIndex;
    public Color[] diffColor;

    public override void Init(Dialogue dialogue)
    {
        base.Init(dialogue);
        
        int arraysLength = dialogue.sentences.Length;
        
        UIDialogue uiDialogue = dialogue as UIDialogue;
        
        animatorStateNames = new string[arraysLength];
        diffColorWordIndex = new int[arraysLength];
        diffColor = new Color[arraysLength];
    }

    public void SetCustomAnimatorStateName(int index, string customAnimatorStateName)
    {
        animatorStateNames[index] = customAnimatorStateName;
    }
    
    public void SetCustomDiffColorWordIndex(int index, int customDiffColorWordIndex)
    {
        this.diffColorWordIndex[index] = customDiffColorWordIndex;
    }
    
    public void SetCustomDiffColor(int index, Color customDiffColor)
    {
        this.diffColor[index] = customDiffColor;
    }
    

}