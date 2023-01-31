using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [SerializeField] private ETextEffects customTextEffect;
    [SerializeField] private int diffColorWordIndex = -1;
    [SerializeField] private Color textColor;
    [SerializeField] private TMP_Text tmpText;

    TextEffectsController textEffectsController;

    private void Start()
    {
        textEffectsController = TextEffectsController.Instance;
    }
    void Update()
    {
        if (customTextEffect != ETextEffects.None)
            textEffectsController.DoTextEffect(tmpText, customTextEffect);

        if (diffColorWordIndex != -1)
            textEffectsController.ChangeWordColor(tmpText, diffColorWordIndex, textColor);
    }
}
