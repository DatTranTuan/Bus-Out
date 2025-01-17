using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkSlot : MonoBehaviour
{
    private ColorType colorType;

    private bool isEmpty = true;

    private CarControl car;

    [SerializeField] private bool isLocked;
    [SerializeField] private ParkPlate parkPlate;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite unlockSprite;

    [SerializeField] private Transform destination;

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public CarControl Car { get => car; set => car = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Sprite UnlockSprite { get => unlockSprite; set => unlockSprite = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public ParkPlate ParkPlate { get => parkPlate; set => parkPlate = value; }
    public Transform Destination { get => destination; set => destination = value; }

    private void OnMouseDown()
    {
        if (IsLocked)
        {
            BuyingManager.Instance.ParkSlot = this;
            BuyingManager.Instance.ClickUnlockSlot();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == car)
        {
            isEmpty = true;
        }
    }
}
