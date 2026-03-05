// https://youtu.be/XMLf24yTV4Y?si=_pMBSPwxC_DZR9uO
// https://youtu.be/1903h0KI7tE?si=oR9oXs1qDtGPacVQ
// https://docs.unity3d.com/Manual/class-VideoPlayer.html
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class SpawnOnClick : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> infos;
    private List<string> spawnedInfos = new List<string>();

    [SerializeField]
    public List<GameObject> videos;
    private List<string> spawnedVideos = new List<string>();

    [SerializeField]
    public bool debugging;

    // Update is called once per frame
    void Update()
    {
        SpawnObjectOnTouch();

        if (debugging)
        {
            SpawnObjectOnTouchDebug();
        }
    }

    // Checks if the user touches the screen
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Spawns the info for an object when the player clicks on that object
    void SpawnObjectOnTouch()
    {
        // Check for user input
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("Player clicked on object: " + hit.transform.name);

            // Check if for the object exists an info
            foreach (GameObject i in infos)
            {
                // Names need to match EXACTLY
                if (hit.transform.name == i.name + "_button" && !spawnedInfos.Contains(i.name))
                {
                    Vector3 pos = hit.point;
                    // Offset
                    pos.z += 0.25f;
                    pos.y += 0.25f;
                    Instantiate(i, pos, transform.rotation);

                    spawnedInfos.Add(i.name);
                    return;
                }
            }

            // Check if for the object exists an video
            foreach (GameObject i in videos)
            {
                // Names need to match EXACTLY
                if (hit.transform.name == i.name + "_button" && !spawnedVideos.Contains(i.name))
                {
                    Vector3 pos = hit.point;
                    // Offset
                    pos.z += 0.25f;
                    pos.y += 0.25f;
                    Instantiate(i, pos, transform.rotation);

                    spawnedInfos.Add(i.name);
                    return;
                }
            }

            Debug.Log("No info could be spawned for this object");

            // If object info is clicked, it disappears
            if (!hit.transform.name.Contains("_button") && hit.transform.name.Contains("info_"))
            {
                spawnedInfos.Remove(hit.transform.gameObject.name);
                Destroy(hit.transform.gameObject);
            }

            if (!hit.transform.name.Contains("_button") && hit.transform.name.Contains("video_"))
            {
                spawnedVideos.Remove(hit.transform.gameObject.name);
                Destroy(hit.transform.gameObject);
            }
        }
    }

    // Only used for debugging
    void SpawnObjectOnTouchDebug()
    {
        // Check for mouse click (left button)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("Player clicked on object: " + hit.transform.name);

                // Check if for the object exists an info
                foreach (GameObject i in infos)
                {
                    // Names need to match EXACTLY
                    if (hit.transform.name == i.name + "_button" && !spawnedInfos.Contains(i.name))
                    {
                        Vector3 pos = hit.point;
                        // Offset
                        pos.z += 0.25f;
                        pos.y += 0.25f;
                        Instantiate(i, pos, transform.rotation);

                        spawnedInfos.Add(i.name);
                        return;
                    }
                }

                // Check if for the object exists an video
                foreach (GameObject i in videos)
                {
                    // Names need to match EXACTLY
                    if (hit.transform.name == i.name + "_button" && !spawnedVideos.Contains(i.name))
                    {
                        Vector3 pos = hit.point;
                        // Offset
                        pos.z += 0.25f;
                        pos.y += 0.25f;
                        Instantiate(i, pos, transform.rotation);

                        spawnedInfos.Add(i.name);
                        return;
                    }
                }

                Debug.Log("No info could be spawned for this object");

                // If object info is clicked, it disappears
                if (!hit.transform.name.Contains("_button") && hit.transform.name.Contains("info_"))
                {
                    spawnedInfos.Remove(hit.transform.gameObject.name);
                    Destroy(hit.transform.gameObject);
                }
                
                if (!hit.transform.name.Contains("_button") && hit.transform.name.Contains("video_"))
                {
                    spawnedVideos.Remove(hit.transform.gameObject.name);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
