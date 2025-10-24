using UnityEngine;
public class lookobject : MonoBehaviour{

    public Transform object3D;
    void Update()
    {
            transform.LookAt(object3D);
    }
}
