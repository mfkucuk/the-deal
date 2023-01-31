using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISimpDialogueData", menuName = "ScriptableObjects/UISimpDialogueDataScriptableObject", order = 1)]
public class UISimpDialogue : Dialogue
{
    
    public RuntimeAnimatorController[] animators;
    
    public List<AnimatorStateNamesDic> animatorStateNames;
    public List<DiffColorWordIndexDic> diffColorWordIndex;
    public List<DiffColorDic> diffColor;

}

public class RealUISimpDialogue : RealDialogue
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
