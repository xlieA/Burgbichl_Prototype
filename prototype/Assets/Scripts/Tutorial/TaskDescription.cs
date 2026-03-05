using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MA/TaskDescription")]
public class TaskDescription : ScriptableObject
{
    [TextArea] public List<string> descriptions = new List<string>();
    public bool completed = false;
}
