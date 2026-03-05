// https://medium.com/@derekanderson-dev/using-touch-to-manipulate-the-object-ar-unity-developer-25a19d8a310b
using UnityEngine;

public class Transformation : MonoBehaviour
{
    // Scale
    private Vector3 initialScale;
    private float startDistance;

    // Rotation
    [SerializeField] private float rotationSpeed = 0.1f;
    private Transform target = null;


    void Start()
    {
        // Assign the target to the child object
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        Scale();
        Rotate();
    }

    private void Scale()
    {
        if (Input.touchCount >= 2)
        {
            // Use distance between two inputs as scale
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                // Calculate initial distance between two touch position
                startDistance = Vector2.Distance(touch0.position, touch1.position);
                initialScale = transform.localScale;
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                // Calculate current distance between two touch points
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                // Smooth scaling transition
                float factor = currentDistance / startDistance;

                transform.localScale = initialScale * factor;
            }
        }
    }

    private void Rotate()
    {
        // Assign the target to the child object -> assign child in start will assign placeholder
        if (target == null)
        {
            target = transform.GetChild(0); // Assuming the child is the first child of the parent
            Debug.Log("Child: " + target.name);
        }

        if (Input.touchCount >= 1)
        {
            Touch touch0 = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch0.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit by the raycast is this object
                if (hit.transform == target)
                {
                    if (touch0.phase == TouchPhase.Moved)
                    {
                        // Calculate movement delta
                        Vector2 touch0Delta = touch0.deltaPosition;

                        // Apply rotation based on delta
                        transform.Rotate(
                            touch0Delta.y * rotationSpeed,     // Rotate around the X-axis -> up/down movement
                            -touch0Delta.x * rotationSpeed,    // Rotate around the Y-axis -> left/right movement
                            0                                  // No Z-axis rotation
                        );
                    }
                }
            }
        }
    }
}
