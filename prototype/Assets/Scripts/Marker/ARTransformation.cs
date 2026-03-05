// https://youtu.be/TN2xUyMfqRQ?si=hnJrP_hsSE7TrcIV
using UnityEngine;

public class ARTransformation : MonoBehaviour
{
    public Vector3 scale;
    public float startDistance;

    // Object that should be transformed
    public GameObject tObject;


    // Update is called once per frame
    void Update()
    {
        Scale();
    }

    private void Scale()
    {
        // Reset object to scale
        tObject = null;

        if (Input.touchCount > 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Get object that should be scaled
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!hit.transform.name.Contains("button"))
                {
                    // Set tObject to the parent or root of the hit object
                    tObject = hit.transform.parent != null ? hit.transform.parent.gameObject : hit.transform.gameObject;
                }
            }

            if (Input.touchCount >= 2 && tObject != null)
            {
                // Use distance between two inputs as scale
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    startDistance = Vector2.Distance(touch0.position, touch1.position);
                    scale = tObject.transform.localScale;
                }
                else
                {
                    Vector2 v1 = touch0.position;
                    Vector2 v2 = touch1.position;

                    float distance = Vector2.Distance(v1, v2);
                    // Smooth scaling transition
                    float factor = distance / startDistance;

                    tObject.transform.localScale = scale * factor;
                }
            }
        }
    }
}
