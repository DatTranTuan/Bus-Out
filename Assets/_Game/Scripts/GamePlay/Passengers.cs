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
    public string CurrentAnimName { get => currentAnimName; set => currentAnimName = value; }
    public CarControl CarDestination { get => carDestination; set => carDestination = value; }

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

        /*anim.ResetTrigger(animName);
        anim.SetTrigger(animName);*/

        anim.Play(animName);

    }

    public void MoveToCar(CarControl carControl)
    {
        Sequence sequence = DOTween.Sequence();

        var carDestination = carControl.transform.position;

        if (carDestination != null)
        {
            Quaternion secondTargetRotation = Quaternion.LookRotation(carControl.transform.position - transform.position);
            sequence.Append(transform.DORotateQuaternion(secondTargetRotation, 0.05f).SetEase(Ease.Linear));

            sequence.Append(transform.DOMove(LevelManager.Instance.FirstMove.position, 0.05f).SetEase(Ease.Linear));
            IsMoving = true;

            sequence.Append(transform.DOMove(carDestination, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                carControl.UpdateText();

                LevelManager.Instance.PassenCount--;

                GameManager.Instance.UpdateCountSignText();
                BuyingManager.Instance.Coin++;
                BuyingManager.Instance.UpdateCoin();

                //passen.IsMoving = false;

                Destroy(gameObject);
            }));


        }
        else
        {
            GameManager.Instance.CheckLosing();
        }
    }

    private void OnDestroy()
    {
        //LevelManager.Instance.MovingPassen();
        //LevelManager.Instance.KillTween();
    }
}
