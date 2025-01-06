using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Passengers : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private String currentAnimName;

    private ColorType colorType;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private List<Material> listColor = new List<Material>();

    [SerializeField] private CarControl carDestination;

    [SerializeField] private float speed;

    private bool isMoving = false;

    public ColorType ColorType { get => colorType; set => colorType = value; }
    public List<Material> ListColor { get => listColor; set => listColor = value; }
    public SkinnedMeshRenderer SkinnedMeshRenderer { get => skinnedMeshRenderer; set => skinnedMeshRenderer = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    private void Awake()
    {
        //RandomColor();
    }

    public void RandomColor()
    {
        int randomIndex = UnityEngine.Random.Range(0, 4);

        SkinnedMeshRenderer.material = ListColor[randomIndex];
        ColorType = (ColorType)randomIndex;
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

   

    public void MoveToCar()
    {
        carDestination = LevelManager.Instance.CheckParkingCar(colorType);

        //ChangeAnim("Run");

        if (carDestination != null)
        {
            float secondDistance = Vector3.Distance(LevelManager.Instance.FirstMove.position, carDestination.transform.position);

            transform.DOMove(LevelManager.Instance.FirstMove.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                IsMoving = true;
                LevelManager.Instance.CheckListCars();

                Quaternion secondTargetRotation = Quaternion.LookRotation(carDestination.transform.position - transform.position);
                transform.DORotateQuaternion(secondTargetRotation, 0.25f).SetEase(Ease.Linear);
                transform.DOMove(carDestination.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    LevelManager.Instance.ListPassen.RemoveAt(0);
                    Destroy(gameObject);
                    LevelManager.Instance.IsMoving = true;
                    IsMoving = false;
                    LevelManager.Instance.CheckListCars();
                    carDestination.CurrentPassen++;
                    carDestination.UpdateText();
                    BuyingManager.Instance.Coin++;
                    BuyingManager.Instance.UpdateCoin();
                });
            });
        }
        else
        {
            GameManager.Instance.CheckLosing();
        }
    }
}
