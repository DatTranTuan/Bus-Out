using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Passengers : MonoBehaviour
{
    private ColorType colorType;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<Material> listColor = new List<Material>();

    [SerializeField] private CarControl carDestination;

    [SerializeField] private float speed;

    public ColorType ColorType { get => colorType; set => colorType = value; }
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }
    public List<Material> ListColor { get => listColor; set => listColor = value; }

    private void Awake()
    {
        //RandomColor();
    }

    public void RandomColor()
    {
        int randomIndex = Random.Range(0, 4);

        MeshRenderer.material = ListColor[randomIndex];
        ColorType = (ColorType)randomIndex;
    }

    

    public void MoveToCar()
    {
        carDestination = LevelManager.Instance.CheckParkingCar(colorType);

        //if (destination != null)
        //{
        //    transform.DOMove(new Vector3(LevelManager.Instance.FirstMove.transform.position.x, 0f, LevelManager.Instance.FirstMove.transform.position.z), 2f).OnComplete(() =>
        //    {
        //        transform.DOMove(new Vector3(destination.transform.position.x, 0f, destination.transform.position.z), 2f).OnComplete(() =>
        //        {
        //            Destroy(gameObject);
        //            LevelManager.Instance.ListPassen.RemoveAt(0);
        //            LevelManager.Instance.CheckPassenger();
        //        });
        //    });
        //}

        float firstDistance = Vector3.Distance(transform.position, LevelManager.Instance.FirstMove.position);
        //float firstDuration = firstDistance / speed;

        if(carDestination != null)
        {

            float secondDistance = Vector3.Distance(transform.position, carDestination.transform.position);
            //float secondDuration = secondDistance / speed;

            transform.DOMove(LevelManager.Instance.FirstMove.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOMove(carDestination.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    LevelManager.Instance.ListPassen.RemoveAt(0);
                    LevelManager.Instance.IsMoving = true;
                    Destroy(gameObject);
                    LevelManager.Instance.CheckListPassen();
                    carDestination.CurrentPassen++;
                    carDestination.UpdateText();
                });
            });
        }
    }
}
