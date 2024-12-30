using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkSlot : MonoBehaviour
{
    private ColorType colorType;

    private bool isEmpty = true;

    private CarControl car;

    private void OnTriggerExit(Collider other)
    {
        isEmpty = true;
    }

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public CarControl Car { get => car; set => car = value; }
}
