using UnityEngine;
public class Lookobject : MonoBehaviour{

    public Transform object3D;
    void Update()
    {
            transform.LookAt(object3D);
    }
}
