using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkPlate : Singleton<ParkPlate>
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParkSlot destination;

    private CarControl car;

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Car"))
    //    {
    //        if (car.IsLeaving)
    //        {
    //            boxCollider.enabled = true;
    //        }
    //    }
    //}


    public BoxCollider BoxCollider { get => boxCollider; set => boxCollider = value; }
    public ParkSlot Destination { get => destination; set => destination = value; }
    public CarControl Car { get => car; set => car = value; }
}
