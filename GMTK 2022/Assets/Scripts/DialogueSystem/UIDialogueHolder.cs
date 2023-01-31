using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIDialogueDefVal))]
public class UIDialogueHolder : DialogueHolder
{
    [SerializeField] private UIDialogueDefVal uiDialogueDefVal;
    
    [SerializeField] private Image dialogueHolderImage;
    [SerializeField] private Animator dialogueAnimator;

    //EKSTRA
    [SerializeField] private TrailRenderer trailRenderer;
    //-----
    
    private RealUIDialogue realUIDialogue;
    private UIDialogue uiDialogue;

    private Color color = Color.white;
    private int diffColorWordIndex = -1;

    public void Update()
    {
        if (textEffect != ETextEffects.None)
        {
            if (DialogueStopGame)
                UnScaledTextEffectController.Instance.DoTextEffect(dialogueHolderText, textEffect);
            else
                TextEffectsController.Instance.DoTextEffect(dialogueHolderText, textEffect);
        }

        if (diffColorWordIndex != -1)
        {
            if (DialogueStopGame)
                UnScaledTextEffectController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);
            else
                TextEffectsController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);
        }


    }

    public override void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            trailRenderer.enabled = false;
            Time.timeScale = 0f;
        }

        this.gameObject.SetActive(true);
        
        HolderOnStartDialogueActions?.Invoke();
        
        realUIDialogue = new RealUIDialogue();
        
        realUIDialogue.Init(dialogue);
        
        uiDialogue = dialogue as UIDialogue;

        for (int index = 0; index < dialogue.sentences.Length; index++)
            SetDefaultValues(index);
        
    }

    private void SetDefaultValues(int index)
    { 
        realUIDialogue.SetText(index, uiDialogue.sentences[index]); //Set Texts

        realUIDialogue.SetCustomTextWriteSpeed(index, uiDialogueDefVal.textWriteSpeeds[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomTextAudio(index, uiDialogueDefVal.textAudios[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomTextEffect(index, uiDialogueDefVal.textEffects[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomOverWrite(index);
        realUIDialogue.SetCustomAnimatorStateName(index, uiDialogueDefVal.animatorStateNames[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomDiffColorWordIndex(index, uiDialogueDefVal.diffColorWordIndex[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomDiffColor(index, uiDialogueDefVal.diffColor[uiDialogue.characterCounts[index]]);
    }

    private void ControlCustomValues(int index)
    {
        if (index < uiDialogue.textWriteSpeeds.Count)  
            realUIDialogue.SetCustomTextWriteSpeed(uiDialogue.textWriteSpeeds[index].id, uiDialogue.textWriteSpeeds[index].textWriteSpeed);
        if (index < uiDialogue.textAudios.Count)  
            realUIDialogue.SetCustomTextAudio(uiDialogue.textAudios[index].id, uiDialogue.textAudios[index].textAudio);
        if (index < uiDialogue.textEffects.Count)  
            realUIDialogue.SetCustomTextEffect(uiDialogue.textEffects[index].id, uiDialogue.textEffects[index].textEffect);
        if (index < uiDialogue.overWrites.Count)  
            realUIDialogue.SetCustomOverWrite(uiDialogue.overWrites[index].id, uiDialogue.overWrites[index].overWrite);
        if (index < uiDialogue.animatorStateNames.Count)  
            realUIDialogue.SetCustomAnimatorStateName(uiDialogue.animatorStateNames[index].id, uiDialogue.animatorStateNames[index].animationStateName);
        if (index < uiDialogue.diffColorWordIndex.Count)  
            realUIDialogue.SetCustomDiffColorWordIndex(uiDialogue.diffColorWordIndex[index].id, uiDialogue.diffColorWordIndex[index].diffColorWordIndex);
        if (index < uiDialogue.diffColor.Count)  
            realUIDialogue.SetCustomDiffColor(uiDialogue.diffColor[index].id, uiDialogue.diffColor[index].diffColor);
  
    }

    public override RealDialogue OnCustomDialogueActions(int index)
    {
        ControlCustomValues(index);
        
        HolderOnCustomDialogueActions?.Invoke(realUIDialogue, index);
        
        dialogueHolderImage.sprite = uiDialogue.sprites[uiDialogue.characterCounts[index]];
        dialogueAnimator.runtimeAnimatorController = uiDialogue.animators[uiDialogue.characterCounts[index]];

        dialogueAnimator.Play(realUIDialogue.animatorStateNames[index]);

        diffColorWordIndex = realUIDialogue.diffColorWordIndex[index];
        color = realUIDialogue.diffColor[index];

        SetEtextEffects(realUIDialogue.textEffects[index]);
        
        return realUIDialogue;
    }

    public override void OnOneDialogueEndActions()
    {
        dialogueAnimator.Play("NotTalking");
        HolderOnOneDialogueEndActions?.Invoke();
    }

    public override void OnEndDialogueActions()
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = false;
            trailRenderer.enabled = true;
            Time.timeScale = 1f;
        }
        
        if (dialogueEnd == EDialogueEnd.None)
        {
            this.gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.NextDialogue)
        {
            DialogueTrigger.Instance.TriggerDialogue();
            this.gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.LoadScene)
        {
            LevelController.Instance.LoadNextScene();
        }
        
        HolderOnEndDialogueActions?.Invoke();
    }

}
