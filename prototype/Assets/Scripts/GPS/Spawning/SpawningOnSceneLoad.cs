using System.Collections.Generic;
using UnityEngine;

public class SpawningOnSceneLoad : MonoBehaviour
{
    [SerializeField] private SpawnableObjectManager spawnableObjectManager;
    private List<SpawnableObject> excavationSites;
    private List<SpawnableArtefact> artefacts;


    void Awake()
    {
        spawnableObjectManager = GameObject.Find("SpawnableObjectManager").GetComponent<SpawnableObjectManager>();
        excavationSites = spawnableObjectManager.excavationSites;
        artefacts = spawnableObjectManager.artefacts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // List of all currently spawned excataion sites
    private List<SpawnableObject> GetAllActiveExcavationSites()
    {
        List<SpawnableObject> results = new List<SpawnableObject>();

        foreach (SpawnableObject site in excavationSites)
        {
            if (site.spawned)
            {
                results.Add(site);
            }
        }

        return results;
    }

    // TODO: respawn or DontDestroyOnLoad?
}
