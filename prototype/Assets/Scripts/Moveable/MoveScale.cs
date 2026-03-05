using System.Collections;
using UnityEngine;

public class MoveScale : MoveTool<Vector3>
{
    float scaleModifier = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.localScale;
        TargetPosition = new Vector3(offset, offset, offset);
    }

    void Update()
    {
        if (debugging && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Start: " + StartPosition);
            Debug.Log("Target: " + TargetPosition);
            StartCoroutine(Lerp());
        }
        if (debugging && Input.GetMouseButtonDown(1))
        {
            Debug.Log("Set scale back");
            StartCoroutine(LerpBack());
        }
    }

    public override IEnumerator Lerp()
    {
        if (CanMove)
        {
            PlaySound();
            
            StartPosition = transform.localScale;
            yield return StartCoroutine(Lerp(StartPosition, TargetPosition));   // Only forward
        }
    }

    public override IEnumerator Lerp(Vector3 start, Vector3 target)
    {
        Debug.Log("Start: " + StartPosition);
        Debug.Log("Target: " + TargetPosition);

        CanMove = false;
        float t = 0;

        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime * speed;
            yield return null;
        }

        transform.localScale = target;
        CanMove = true;
    }

    // Sets the object back to its original scale
    public IEnumerator LerpBack()
    {
        if (CanMove)
        {
             StartPosition = transform.localScale; // Make sure you're lerping from current scale
            yield return StartCoroutine(Lerp(StartPosition, Vector3.one)); // Assuming 1,1,1 is default
            Debug.Log("Set scale back");
        }
    }

    public override void PlaySound()
    {
        AudioManager.instance.Shine();
    }
}
