using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float strength;
    public float duration = 1f;
    public bool mouseOn = false;

    public float cameraStrength;
    public float cameraDuration;

    public void DoShake(Transform transform)
    {
        StartCoroutine(Shaking(transform));
    }

    public void StartMouseOnShaking(Transform trans)
    {
        StartCoroutine(MouseOverShaking(trans));
    }

    IEnumerator Shaking(Transform transform)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < cameraDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere * cameraStrength;
            yield return null;
        }

        transform.position = startPosition;
    }

    IEnumerator MouseOverShaking(Transform trans)
    {
        mouseOn = true;
        Vector3 startPosition = trans.position;

        while (mouseOn)
        {
            trans.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        trans.position = startPosition;
    }

    public void resetShaking()
    {
        mouseOn = false;
        StopAllCoroutines();
    }
}
