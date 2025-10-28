using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DoorUIButton : MonoBehaviour
{
    public List<DoorMover> controlledDoors = new List<DoorMover>();

    [SerializeField] private Image buttonImage;
    [SerializeField] private Color openColor = Color.green;
    [SerializeField] private Color closedColor = Color.red;

    void Start()
    {

        buttonImage = GetComponent<Image>();
        UpdateButtonColor();
    }

    public void OnPressButton()
    {
        int count = controlledDoors.Count;
        int i = 0;

        while (i < count)
        {
            DoorMover door = controlledDoors[i];
            if (door != null)
            {
                door.ToggleDoor();
            }
            i++;
        }

        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        if (buttonImage == null)
        {
            return;
        }

        bool anyClosed = false;
        int count = controlledDoors.Count;
        int i = 0;

        while (i < count)
        {
            DoorMover door = controlledDoors[i];
            if (door != null)
            {
                if (door.IsClosed())
                {
                    anyClosed = true;
                }
            }
            i++;
        }

        if (anyClosed)
        {
            buttonImage.color = closedColor;
        }
        else
        {
            buttonImage.color = openColor;
        }
    }
}
