using UnityEngine;

public class DoorMover : MonoBehaviour//->Uso de feel
{
    [SerializeField] private Transform openPosition;
    [SerializeField] private Transform closedPosition;
    [SerializeField] private float moveSpeed = 3f;

    private bool isClosed = false;
    private Vector3 targetPosition;

    void Start()
    {
        if (openPosition != null)
        {
            transform.position = openPosition.position;
            targetPosition = openPosition.position;
        }
    }

    void Update(){
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void ToggleDoor()
    {
        isClosed = !isClosed;

        if (isClosed)
        {
            if (closedPosition != null)
                targetPosition = closedPosition.position;
        }
        else
        {
            if (openPosition != null)
                targetPosition = openPosition.position;
        }
    }

    public void SetDoorState(bool closed)
    {
        isClosed = closed;

        if (isClosed)
        {
            if (closedPosition != null)
                targetPosition = closedPosition.position;
        }
        else
        {
            if (openPosition != null)
                targetPosition = openPosition.position;
        }
    }

    public bool IsClosed()
    {
        return isClosed;
    }
}
