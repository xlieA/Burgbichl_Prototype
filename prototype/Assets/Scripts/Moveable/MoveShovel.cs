using System.Collections;
using UnityEngine;

public class MoveShovel : MoveTool<Vector3>
{
    void Start()
    {
        StartPosition = transform.localPosition;
        TargetPosition = new Vector3(StartPosition.x, StartPosition.y + offset, StartPosition.z);
    }

    public override IEnumerator Lerp(Vector3 start, Vector3 target)
    {
        CanMove = false;
        float t = 0;

        while (t < duration)
        {
            transform.localPosition = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime * speed;

            yield return null;
        }

        transform.localPosition = target;
        CanMove = true;
    }

    public override void PlaySound()
    {
        AudioManager.instance.Shovel();
    }
}
