using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DoorUIButton : MonoBehaviour
{
    public List<DoorMover> controlledDoors = new List<DoorMover>();

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

    }
}
