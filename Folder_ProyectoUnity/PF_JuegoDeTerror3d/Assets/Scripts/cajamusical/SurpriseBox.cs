using UnityEngine;
using System.Collections;

public class SurpriseBox : MonoBehaviour
{
    public Transform tableCenter;
    public float rotationSpeed = 50f;
    public bool isSpinning = false;
    public bool isOpen = false;

    public int handleValue = 0;
    public int handleLimit = 20;

    public Transform[] playerPositions; // Jugador y NPCs
    public Transform target; // A quién apunta
    private float stopTimer = 0f;

    public void StartSpin()
    {
        if (isOpen) return;
        isSpinning = true;
        stopTimer = Random.Range(2f, 4f); // tiempo aleatorio de giro
    }

    void Update()
    {
        if (isSpinning && !isOpen)
        {
            transform.RotateAround(tableCenter.position, Vector3.up, rotationSpeed * Time.deltaTime);

            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f)
            {
                StopSpin();
            }
        }
    }

    void StopSpin()
    {
        isSpinning = false;
        int index = Random.Range(0, playerPositions.Length);
        target = playerPositions[index];
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        Debug.Log("La caja apunta a: " + target.name);
    }

    public void ModifyHandle(int amount)
    {
        handleValue += amount;
        handleValue = Mathf.Clamp(handleValue, 0, handleLimit);
        if (handleValue >= handleLimit && !isOpen)
        {
            OpenBox();
        }
    }

    void OpenBox()
    {
        isOpen = true;
        Debug.Log("¡La caja asesina se ha abierto!");
    }

    public void ResetBox()
    {
        isOpen = false;
        handleValue = 0;
    }
}
