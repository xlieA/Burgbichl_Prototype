// https://docs.unity3d.com/2018.2/Documentation/ScriptReference/EventSystems.EventSystem.IsPointerOverGameObject.html
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    // Checks if the user touches the screen
    public bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Checks if player touches UI
    public bool TouchOnUI()
    {
        // Check if finger is over a UI element
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Debug.Log("Touched the UI");
            return true;
        }

        return false;
    }

    public bool TouchOnUIDebug()
    {
        // Check if the mouse was clicked over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on the UI");
            return true;
        }

        return false;
    }
}