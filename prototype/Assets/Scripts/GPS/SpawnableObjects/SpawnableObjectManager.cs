using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectManager : MonoBehaviour
{
    // List of all excavation sites that can be digged up
    [SerializeField] public List<SpawnableObject> excavationSites;
    [SerializeField] public List<SpawnableArtefact> artefacts;
}
