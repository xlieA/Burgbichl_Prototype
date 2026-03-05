using UnityEngine;
using UnityEngine.UI;

public class ButtonElement
{
    public Button button;
    public bool isActive;
    public Sprite img;
    public SpawnableObject obj;

    public ButtonElement(string name, Sprite img, SpawnableObject obj) : this(name)
    {
        this.img = img;
        this.obj = obj;

        if (this.button != null)
        {
            this.button.name = obj.objectName;
        }
    }

    public ButtonElement(string name)
    {
        Button temp = GameObject.Find(name).GetComponent<Button>();
        if (temp != null)
        {
            this.button = temp;
        }

        this.isActive = false;
    }
}
