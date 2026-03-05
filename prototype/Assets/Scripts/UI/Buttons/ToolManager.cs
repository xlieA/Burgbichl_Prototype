using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolManager : MonoBehaviour
{
    [SerializeField] public GameObject o_compass;
    [SerializeField] public GameObject o_shovel;
    [SerializeField] public GameObject o_pickaxe;
    [SerializeField] private Vector3 startPosition = new Vector3(0, -120, 0);


    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "GPS")
        {
            // Disable all tools
            o_compass.SetActive(false);
            o_shovel.SetActive(false);
            o_pickaxe.SetActive(false);
            o_pickaxe.transform.localRotation = Quaternion.Euler(startPosition);
        }
    }

    // Enables gameobject of given name
    public void EnableTool(string name)
    {
        GameObject tool = getTool(name);

        if (tool != null)
        {
            tool.SetActive(true);
        }
    }

    // Disables gameobject of given name
    public void DisableTool(string name)
    {
        GameObject tool = getTool(name);
        
        if (tool != null)
        {
             tool.SetActive(false);
        }
    }

    // Gets corresponding gameobject for a given name
    private GameObject getTool(string name)
    {
        if (name.Contains("Compass"))
        {
            return this.o_compass;
        }
        else if (name.Contains("Shovel"))
        {
            return this.o_shovel;
        }
        else if (name.Contains("Pickaxe"))
        {
            return this.o_pickaxe;
        }
        else
        {
            return null;
        }
    }
}
