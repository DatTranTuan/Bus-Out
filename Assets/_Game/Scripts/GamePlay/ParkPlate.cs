using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkPlate : Singleton<ParkPlate>
{
    [SerializeField] private BoxCollider boxCollider;

    public BoxCollider BoxCollider { get => boxCollider; set => boxCollider = value; }
}
