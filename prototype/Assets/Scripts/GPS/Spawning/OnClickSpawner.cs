// https://youtu.be/XMLf24yTV4Y?si=_pMBSPwxC_DZR9uO
// https://youtu.be/1903h0KI7tE?si=oR9oXs1qDtGPacVQ
// https://docs.unity3d.com/Manual/class-VideoPlayer.html
// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.SetParent.html
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnClickSpawner : MonoBehaviour
{
    [SerializeField] private Button analysis;
    // List of all buttons in the scene except the analysis button
    private List<Button> otherButtons = new List<Button>();
    [SerializeField] private MenuController menuController;

    [SerializeField] private SpawnableObjectManager spawnableObjectManager;
    private List<SpawnableArtefact> artefacts;
    [SerializeField] private GameObject placeholder;

    private SpawnableArtefact artefactToSpawn;
    private Vector3 spawningPosition;

    [SerializeField] private MoveScale moveScale;
    [SerializeField] private MovePickaxe movePickaxe;
    private bool canMovePickaxe = true;     // Avoids moving pickaxe when sending artifact for analysis
    public TMP_Text canMovePickaxeText;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private bool debugging;


    void Awake()
    {
        movePickaxe = GameObject.Find("Camera Offset/Main Camera/Tools/pickaxe").GetComponent<MovePickaxe>();
        moveScale = GameObject.Find("Canvas/PopUp/Artefacts").GetComponent<MoveScale>();

        menuController = GameObject.Find("MenuController").GetComponent<MenuController>();

        playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();

        spawnableObjectManager = GameObject.Find("SpawnableObjectManager").GetComponent<SpawnableObjectManager>();
        artefacts = spawnableObjectManager.artefacts;
    }

    void Start()
    {
        List<GameObject> tmp = new List<GameObject>
        {
            analysis.gameObject
        };

        otherButtons = menuController.GetAllButtons(menuController.canvas, tmp);
    }

    // Update is called once per frame
    void Update()
    {
        if (debugging)
        {
            UsePickaxe();
        }
    }

    // Makes artefact image disappear and spawns 3d model of artefact at marker position
    public void SendToAnalysis()
    {
        if (artefactToSpawn != null)
        {
            AudioManager.instance.Button();

            StartCoroutine(moveScale.LerpBack());
            placeholder.SetActive(false);

            GameObject spawnedArtefact = Instantiate(artefactToSpawn.prefab, spawningPosition, transform.rotation);
            spawnedArtefact.transform.localScale = new Vector3(.3f, .3f, .3f);
            artefactToSpawn.spawned = true;

            // Find parent
            Debug.Log("Parent: " + artefactToSpawn.parent.prefab.name);
            GameObject parentObject = GameObject.Find(artefactToSpawn.parent.prefab.name);

            if (parentObject != null)
            {
                Transform parentTransform = parentObject.transform;
                // Spawn artefact as child of parent
                spawnedArtefact.transform.SetParent(parentTransform);
            }

            // Reset
            analysis.gameObject.SetActive(false);
            artefactToSpawn = null;
            menuController.SetActive(otherButtons);
            canMovePickaxe = true;
            movePickaxe.CanMove = true;
            canMovePickaxeText.text = movePickaxe.CanMove.ToString();
        }
    }

    // Moves pickaxe when player touches screen
    public void UsePickaxe()
    {
        // Check if the left mouse button was clicked
        if (debugging && Input.GetMouseButtonDown(0) && !playerInput.TouchOnUIDebug() && canMovePickaxe)
        {
            StartCoroutine(movePickaxe.Lerp());
            SpawnObjectOnTouchDebug();
        }

        // Check if there is a touch
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && canMovePickaxe)
        {
            if (!playerInput.TouchOnUI())
            {
                StartCoroutine(movePickaxe.Lerp());
                SpawnObjectOnTouch();
            }
        }
    }

    // Spawns the info for an object when the player clicks on that object
    public void SpawnObjectOnTouch()
    {
        // Check for user input
            if (!playerInput.TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            string hitName = hit.transform.name.ToLower();
            Debug.Log("Player clicked on object: " + hitName);
            
            // Check if for the object exists an artefact
            foreach (SpawnableArtefact a in artefacts)
            {
                string expectedName = a.objectName.ToLower();
                Debug.Log("Next spawnable artefact: " + expectedName);
    
                // Names need to match EXACTLY
                if (hitName == "m_" + expectedName && !a.spawned && a.parent.spawned)
                {
                    //Handheld.Vibrate();   // Doesen't work
                    //Vibration.Vibrate(500); // Doesn't work either?

                    // Make all other buttons unclickable
                    menuController.SetInactive(otherButtons);

                    // Make sure pickaxe can't be used
                    canMovePickaxe = false;
                    canMovePickaxeText.text = canMovePickaxe.ToString();

                    // Destroy marker
                    Destroy(hit.transform.gameObject);

                    placeholder.GetComponent<Image>().sprite = a.img;
                    placeholder.SetActive(true);
                    StartCoroutine(moveScale.Lerp());

                    a.spawned = true;

                    analysis.gameObject.SetActive(true);
                    artefactToSpawn = a;
                    spawningPosition = hit.point;
                    return;
                }
            }

            Debug.Log("Couldn't find object to spawn");
        }
    }

    private void SpawnObjectOnTouchDebug()
    {
        // Check for mouse click (left button)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                string hitName = hit.transform.name.ToLower();
                Debug.Log("Player clicked on object: " + hitName);

                // Check if for the object exists an artefact
                foreach (SpawnableArtefact a in artefacts)
                {
                    string expectedName = a.objectName.ToLower();
                    Debug.Log("Next spawnable artefact: " + expectedName);
        
                    // Names need to match EXACTLY
                    if (hitName == "m_" + expectedName && !a.spawned && a.parent.spawned)
                    {
                        //Handheld.Vibrate();   // Doesen't work
                        //Vibration.Vibrate(500); // Doesn't work either?
                        
                        // Make all other buttons unclickable
                        menuController.SetInactive(otherButtons);

                        // Make sure pickaxe can't be used
                        canMovePickaxe = false;
                        canMovePickaxeText.text = canMovePickaxe.ToString();

                        // Destroy marker
                        Destroy(hit.transform.gameObject);
                        analysis.gameObject.SetActive(true);

                        placeholder.GetComponent<Image>().sprite = a.img;
                        placeholder.SetActive(true);
                        StartCoroutine(moveScale.Lerp());

                        //a.spawned = true;

                        analysis.gameObject.SetActive(true);
                        artefactToSpawn = a;
                        spawningPosition = hit.point;
                        return;
                    }
                }

                Debug.Log("Couldn't find object to spawn");
            }
        }
    }
}
