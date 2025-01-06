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

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public CarControl Car { get => car; set => car = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Sprite UnlockSprite { get => unlockSprite; set => unlockSprite = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public ParkPlate ParkPlate { get => parkPlate; set => parkPlate = value; }

    private void OnMouseDown()
    {
        if (IsLocked)
        {
            Debug.Log("On Click Mouse Down");

            BuyingManager.Instance.ParkSlot = this;
            BuyingManager.Instance.ClickUnlockSlot();

            ParkPlate.BoxCollider.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isEmpty = true;
    }
}
