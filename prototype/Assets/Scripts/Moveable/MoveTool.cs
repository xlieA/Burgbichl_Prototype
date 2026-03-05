// https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
using System.Collections;
using UnityEngine;

public abstract class MoveTool<T> : MonoBehaviour where T : struct
{
    // New position
    public float offset = 1f;
    // Speed of lerp motion
    public float speed = 1f;
    // Time for the lerp motion to finish
    public float duration = 1f;

    private bool canMove = true;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private T startPosition;
    public T StartPosition
    {
        get { return startPosition; }
        set { startPosition = value; }
    }
    private T targetPosition;
    public T TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    [SerializeField] public bool debugging = false;


    void Update()
    {
        if (debugging && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Start: " + startPosition);
            Debug.Log("Target: " + targetPosition);
            StartCoroutine(Lerp());
        }
    }

    // Performs forward and backward motion
    public virtual IEnumerator Lerp()
    {
        if (canMove)
        {
            PlaySound();
            
            yield return StartCoroutine(Lerp(startPosition, targetPosition));   // Forward
            yield return StartCoroutine(Lerp(targetPosition, startPosition));   // Backward

            // Wait for 1 second before allowing another update
            yield return new WaitForSeconds(1f);
        }
    }

    public abstract IEnumerator Lerp(T start, T target);
    public abstract void PlaySound();
}
