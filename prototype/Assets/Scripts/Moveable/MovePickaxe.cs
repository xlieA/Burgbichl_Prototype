using System.Collections;
using UnityEngine;

public class MovePickaxe : MoveTool<Vector3>
{
    // Start is called before the first frame update
    void Start()
    {
        //StartPosition = transform.localRotation.eulerAngles;
        StartPosition = new Vector3(0, -120, 0);
        transform.localRotation = Quaternion.Euler(StartPosition);
        TargetPosition = new Vector3(StartPosition.x, StartPosition.y, StartPosition.z + offset);
    }

    public override IEnumerator Lerp(Vector3 start, Vector3 target)
    {
        CanMove = false;
        float t = 0;

        Quaternion startRotation = Quaternion.Euler(start);
        Quaternion targetRotation = Quaternion.Euler(target);

        while (t < duration)
        {
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t / duration);
            t += Time.deltaTime * speed;

            yield return null;
        }

        transform.localRotation = targetRotation;
        CanMove = true;
    }

    public override void PlaySound()
    {
        AudioManager.instance.Pickaxe();
    }
}
