using System.Collections.Generic;
using UnityEngine;

public abstract class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] sentences;
    
    public int[] characterCounts;

    public List<TextWriteSpeedDic> textWriteSpeeds;
    public List<AudioClipDic> textAudios;
    public List<ETextEffectsDic> textEffects;
    public List<OverWriteDic> overWrites;

}

[System.Serializable]
public class TextWriteSpeedDic {
    public int id;
    public float textWriteSpeed;
}

[System.Serializable]
public class AudioClipDic {
    public int id;
    public AudioClip textAudio;
}

[System.Serializable]
public class ETextEffectsDic {
    public int id;
    public ETextEffects textEffect;
}

[System.Serializable]
public class OverWriteDic {
    public int id;
    public bool overWrite;
}



public abstract class RealDialogue
{

    public string[] sentences;
    public float[] textWriteSpeeds;
    public AudioClip[] textAudios;
    public ETextEffects[] textEffects;
    public bool[] overWrite;

    private int arraysLength;
    public virtual void Init(Dialogue dialogue)
    { 
        arraysLength = dialogue.sentences.Length;
        
        sentences = new string[arraysLength];
        textWriteSpeeds = new float[arraysLength];
        textAudios = new AudioClip[arraysLength];
        textEffects = new ETextEffects[arraysLength];
        overWrite = new bool[arraysLength];
    }

    public void SetText(int index, string sentence)
    {
        sentences[index] = sentence;
    }

    public void SetCustomTextWriteSpeed(int index, float customTextWriteSpeed)
    {
        textWriteSpeeds[index] = customTextWriteSpeed;
    }
    
    public void SetCustomTextAudio(int index, AudioClip customTextAudio)
    {
        textAudios[index] = customTextAudio;
    }
    
    public void SetCustomTextEffect(int index, ETextEffects customTextEffect)
    {
        textEffects[index] = customTextEffect;
    }
    
    public void SetCustomOverWrite(int index, bool customOverWrite = false)
    {
        overWrite[index] = customOverWrite;
    }

}