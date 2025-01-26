using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkPlate : Singleton<ParkPlate>
{
    //[SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParkSlot parkSlot;
    [SerializeField] private Transform parkSlotPos;
    [SerializeField] private Transform parkPlatePos;

    private CarControl car;

    public ParkSlot ParkSlot { get => parkSlot; set => parkSlot = value; }
    public CarControl Car { get => car; set => car = value; }
    public Transform ParkSlotPos { get => parkSlotPos; set => parkSlotPos = value; }
    public Transform ParkPlatePos { get => parkPlatePos; set => parkPlatePos = value; }
}
