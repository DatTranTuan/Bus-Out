using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<ParkPlate> listPlate = new List<ParkPlate>();
    [SerializeField] private List<ParkSlot> listParkSlot = new List<ParkSlot>();

    [SerializeField] private List<Passengers> listPassen = new List<Passengers>();
    [SerializeField] private Vector3[] passenArray;

    [SerializeField] private Passengers passenPrefabs;

    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform firstMove;

    private bool isMoving = false;

    private int index = 0;

    public int Index { get => index; set => index = value; }
    public List<Passengers> ListPassen { get => listPassen; set => listPassen = value; }
    public List<ParkPlate> ListPlate { get => listPlate; set => listPlate = value; }
    public Transform FirstMove { get => firstMove; set => firstMove = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    private void Start()
    {
        passenArray = new Vector3[30];

        SpawnPassen();

        SetPlateCollide();
    }

    private void Update()
    {
        if (IsMoving)
        {
            MovingPassen();
        }   
    }

    private void MovingPassen()
    {
        for (int i = 0; i < listPassen.Count; i++)
        {
            GameObject currentObject = listPassen[i].gameObject;
            Vector3 targetPosition = passenArray[i];

            if (Vector3.Distance(currentObject.transform.position, targetPosition) > 0.01f)
            {
                currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, targetPosition, 1f * Time.deltaTime);
            }

            //currentObject.transform.position = targetPosition;
        }
    }


    public void SetPlateCollide()
    {
        index = 0;

        ListPlate[0].BoxCollider.enabled = true;

        for (int i = 1; i < ListPlate.Count; i++)
        {
            ListPlate[i].BoxCollider.enabled = false;
        }
    }

    public void SpawnPassen()
    {
        // Sinh ra hanh khach

        for (int i = 1; i <= 30; i++)
        {
            Vector3 newPos = spawnPos.position + new Vector3(i, 0, 0);
            Passengers passen = Instantiate(passenPrefabs, newPos, Quaternion.identity);

            if (i == 5 || i == 2 || i == 19 || i == 22)
            {
                passen.MeshRenderer.material = passen.ListColor[2];
                passen.ColorType = ColorType.Red;
            }
            else if (i == 10 || i == 3 || i == 16 || i == 27 || i == 6 || i == 1 || i == 12 || i == 30)
            {
                passen.MeshRenderer.material = passen.ListColor[1];
                passen.ColorType = ColorType.Green;
            }
            else if (i == 18 || i == 9 || i == 25 || i == 8 || i == 14 || i == 21 || i == 7 || i == 11)
            {
                passen.MeshRenderer.material = passen.ListColor[3];
                passen.ColorType = ColorType.Purple;
            }
            else
            {
                passen.MeshRenderer.material = passen.ListColor[0];
                passen.ColorType = ColorType.Blue;
            }

            ListPassen.Add(passen);
            passenArray[i - 1] = new Vector3(passen.transform.position.x, passen.transform.position.y, passen.transform.position.z);

            passen.transform.rotation = Quaternion.Euler(0, -90, 0);
            passen.transform.SetParent(spawnPos.transform);
        }
    }

    public void SetNextCollider()
    {
        if (index < listPlate.Count)
        {
            ListPlate[index].BoxCollider.enabled = false;

            index++;

            if (index < listPlate.Count)
            {
                ListPlate[index].BoxCollider.enabled = true;
            }
        }
    }

    public CarControl CheckParkingCar(ColorType colorType)
    {
        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (!listParkSlot[i].IsEmpty && listParkSlot[i].ColorType == colorType)
            {
                return listParkSlot[i].Car;
            }
        }

        return null;
    }

    public void CheckListPassen()
    {
        if (ListPassen.Count <= 0)
        {
            return;
        }
        else
        {
            ListPassen[0].MoveToCar();
        }
    }

    public void CheckPassenger(ColorType colorType, int index)
    {
        if (listPassen[0].ColorType == colorType)
        {
            index++;
        }
    }
}
