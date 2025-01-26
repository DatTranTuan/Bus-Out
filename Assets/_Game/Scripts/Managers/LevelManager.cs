using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class LevelManager : Singleton<LevelManager>
{
    private Dictionary<ColorType, Queue<CarControl>> dictCar = new Dictionary<ColorType, Queue<CarControl>>();
    private bool isProgress;

    [SerializeField] private GameObject heliCopTer;
    [SerializeField] private Transform heliStartPos;
    [SerializeField] private Transform heliEndPos;

    [SerializeField] private List<Material> listCarMaterial = new List<Material>();
    [SerializeField] private List<Material> listVanMaterial = new List<Material>();
    [SerializeField] private List<Material> listBusMaterial = new List<Material>();

    [SerializeField] private List<CarControl> listCars = new List<CarControl>();
    [SerializeField] private List<CarControl> listAllCars = new List<CarControl>();

    [SerializeField] private List<ParkPlate> listPlate = new List<ParkPlate>();
    [SerializeField] private List<ParkSlot> listParkSlot = new List<ParkSlot>();

    [SerializeField] private List<Passengers> listPassen = new List<Passengers>();
    [SerializeField] private Vector3[] passenArray;
    private int passenArrayIndex;

    [SerializeField] private Passengers passenPrefabs;

    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform firstMove;

    [SerializeField] private ParkSlot vIPSlot;

    [SerializeField] private bool isVIP = false;

    [SerializeField] private GameObject allCars;

    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;

    private int passenCount;

    //private int carTouchCount = 0;

    public List<Passengers> ListPassen { get => listPassen; set => listPassen = value; }
    public List<ParkPlate> ListPlate { get => listPlate; set => listPlate = value; }
    public Transform FirstMove { get => firstMove; set => firstMove = value; }
    public bool IsVIP { get => isVIP; set => isVIP = value; }
    public ParkSlot VIPSlot { get => vIPSlot; set => vIPSlot = value; }
    public List<ParkSlot> ListParkSlot { get => listParkSlot; set => listParkSlot = value; }
    public GameObject HeliCopTer { get => heliCopTer; set => heliCopTer = value; }
    public int PassenCount { get => passenCount; set => passenCount = value; }
    public Vector3[] PassenArray { get => passenArray; set => passenArray = value; }
    public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
    public List<CarControl> ListCars { get => listCars; set => listCars = value; }

    //public int CarTouchCount { get => carTouchCount; set => carTouchCount = value; }

    private void Start()
    {
        //CheckPassenArrayLength();

        //StartCoroutine(SpawnPassenger());

        //LoadLevel();
    }

    public void LoadLevel()
    {
        if (listCars.Count > 0)
        {
            listCars.Clear();
        }

        if (DataManager.Instance.CurrentLevel == 1)
        {
            GameManager.Instance.Map1.SetActive(true);

            CarControl[] carControls = GameManager.Instance.Map1.GetComponentsInChildren<CarControl>(true);

            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map1.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();
        }
        else if (DataManager.Instance.CurrentLevel == 2)
        {
            GameManager.Instance.Map2.SetActive(true);

            CarControl[] carControls = GameManager.Instance.Map2.GetComponentsInChildren<CarControl>(true);

            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map2.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();
        }
        else if (DataManager.Instance.CurrentLevel == 3)
        {
            GameManager.Instance.Map3.SetActive(true);
            GameManager.Instance.Map2.SetActive(false);


            CarControl[] carControls = GameManager.Instance.Map3.GetComponentsInChildren<CarControl>(true);

            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map3.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();
        }
        else if (DataManager.Instance.CurrentLevel == 4)
        {
            GameManager.Instance.Map4.SetActive(true);
            GameManager.Instance.Map3.SetActive(false);


            CarControl[] carControls = GameManager.Instance.Map4.GetComponentsInChildren<CarControl>(true);

            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map4.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();
        }
        else if (DataManager.Instance.CurrentLevel == 5)
        {
            GameManager.Instance.Map5.SetActive(true);
            GameManager.Instance.Map4.SetActive(false);

            CarControl[] carControls = GameManager.Instance.Map5.GetComponentsInChildren<CarControl>(true);

            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map5.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();
        }
    }

    public void NextLevel()
    {
        DataManager.Instance.CurrentLevel++;

        listAllCars.Clear();

        if (GameManager.Instance.Map1.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map2.GetComponentsInChildren<CarControl>();

            listAllCars.AddRange(carControls);

            GameManager.Instance.Map1.SetActive(false);
            GameManager.Instance.Map2.SetActive(true);
        }
        else if (GameManager.Instance.Map2.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map3.GetComponentsInChildren<CarControl>();

            listAllCars.AddRange(carControls);

            GameManager.Instance.Map2.SetActive(false);
            GameManager.Instance.Map3.SetActive(true);
        }
        else if (GameManager.Instance.Map3.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map4.GetComponentsInChildren<CarControl>();

            listAllCars.AddRange(carControls);

            GameManager.Instance.Map3.SetActive(false);
            GameManager.Instance.Map4.SetActive(true);
        }
        else if (GameManager.Instance.Map4.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map5.GetComponentsInChildren<CarControl>();

            listAllCars.AddRange(carControls);

            GameManager.Instance.Map4.SetActive(false);
            GameManager.Instance.Map5.SetActive(true);
        }
        else if (GameManager.Instance.Map5.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map1.GetComponentsInChildren<CarControl>();

            listAllCars.AddRange(carControls);

            GameManager.Instance.Map5.SetActive(false);
            GameManager.Instance.Map1.SetActive(true);
        }

        passenArray = new Vector3[passenCount];
        dictCar.Clear();
        PassenCount = 0;

        CheckPassenArrayLength();
        StartCoroutine(SpawnPassenger());
    }

    public void ReplayLevel()
    {
        if (listCars.Count > 0)
        {
            listCars.Clear();
        }

        if (GameManager.Instance.Map1.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map1.GetComponentsInChildren<CarControl>(true);

            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map1.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();
        }
        else if (GameManager.Instance.Map2.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map2.GetComponentsInChildren<CarControl>(true);
            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map2.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();

        }
        else if (GameManager.Instance.Map3.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map3.GetComponentsInChildren<CarControl>(true);
            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map3.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();

        }
        else if (GameManager.Instance.Map4.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map4.GetComponentsInChildren<CarControl>(true);
            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map4.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();

        }
        else if (GameManager.Instance.Map5.activeInHierarchy)
        {
            CarControl[] carControls = GameManager.Instance.Map5.GetComponentsInChildren<CarControl>(true);
            ListCars.AddRange(carControls);

            CarControl[] carControl = GameManager.Instance.Map5.GetComponentsInChildren<CarControl>(true);

            listAllCars.AddRange(carControl);

            ReMoveAllCars();

        }

       
    }

    public void ReMoveAllCars()
    {
        for (int i = 0; i < ListCars.Count; i++)
        {
            //listCars[i].gameObject.SetActive(true);

            if (ListCars[i].IsLeaving || ListCars[i].IsTurning || ListCars[i].IsMoving)
            {
                ListCars[i].Respawn();
            }

            //listCars.RemoveAt(i);
        }

        for (int i = 0; i < listParkSlot.Count; i++)
        {
            listParkSlot[i].IsEmpty = true;
            listParkSlot[i].Car = null;
        }

        for (int i = 0; i < listPassen.Count; i++)
        {
            //listPassen.RemoveAt(i);
            Destroy(listPassen[i].gameObject);
        }


        listPassen.Clear();
        passenArray = new Vector3[passenCount];
        dictCar.Clear();
        PassenCount = 0;

        CheckPassenArrayLength();
        StartCoroutine(SpawnPassenger());
    }

    public void MovingPassen()
    {
        Vector3 targetPosition;

        for (int i = 0; i < ListPassen.Count; i++)
        {
            Passengers currentObject;

            currentObject = ListPassen[i];
            targetPosition = PassenArray[i];

            currentObject.ChangeAnim("Run");

            currentObject.transform.DOMove(targetPosition, 0.11f).SetEase(Ease.Linear).OnComplete(() =>
            {
                currentObject.ChangeAnim("Idle");
            });
        }
    }

    public bool IsOneEmpty()
    {
        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (listParkSlot[i].IsEmpty)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsAllEmpty()
    {
        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (!listParkSlot[i].IsEmpty)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsAllNotEmpty()
    {
        int index = 0;

        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (listParkSlot[i].Car != null && !listParkSlot[i].IsEmpty)
            {
                index++;

                if (index == listParkSlot.Count)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void ArrangeSkill()
    {
        if (ListParkSlot != null && !isVIP)
        {
            CheckArrangeSkill();
        }
        else if (isVIP)
        {
            ChangeVIPPassenColorPos(vIPSlot.Car.ColorType);
            CheckPassenger(vIPSlot.Car);
        }
        else
        {
            return;
        }
    }

    public void CheckArrangeSkill()
    {
        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (!listParkSlot[i].IsEmpty)
            {
                ChangeArrangePassenColorPos(ListParkSlot[i].Car);
                CheckPassenger(listParkSlot[i].Car);
                break;
            }
        }
    }

    public void ChangeArrangePassenColorPos(CarControl car)
    {
        int index2 = 0;

        int max = car.MaxPassen - car.CurrentPassen;

        for (int i = 0; i < listPassen.Count; i++)
        {
            if (listPassen[i].ColorType == car.ColorType && !isVIP && index2 < max)
            {
                var temp = listPassen[index2];
                var pos = listPassen[index2].transform.position;

                listPassen[index2] = listPassen[i];
                listPassen[index2].transform.position = listPassen[i].transform.position;

                listPassen[i] = temp;
                listPassen[i].transform.position = pos;

                index2++;
            }
        }

        ChangePassenPosSkill();
    }

    public void ChangeVIPPassenColorPos(ColorType colorType)
    {
        int index = 0;

        for (int i = 0; i < listPassen.Count; i++)
        {
            if (listPassen[i].ColorType == colorType && index < vIPSlot.Car.MaxPassen)
            {
                var temp = listPassen[index];
                var pos = listPassen[index].transform.position;

                listPassen[index] = listPassen[i];
                listPassen[index].transform.position = listPassen[i].transform.position;

                listPassen[i] = temp;
                listPassen[i].transform.position = pos;

                index++;
            }
        }

        ChangePassenPosSkill();
    }

    public void VIPHelicopter(CarControl car)
    {
        HeliCopTer.SetActive(true);

        float speed = 15f;

        Vector3 carPos = new Vector3(car.transform.position.x, HeliCopTer.transform.position.y, car.transform.position.z);
        Vector3 vipPos = new Vector3(vIPSlot.Destination.transform.position.x, HeliCopTer.transform.position.y, vIPSlot.Destination.transform.position.z);
        Vector3 endHeliPos = heliEndPos.transform.position;

        float t1 = Vector3.Distance(carPos, HeliCopTer.transform.position) / speed;
        float t2 = Vector3.Distance(vipPos, HeliCopTer.transform.position) / speed;
        float t3 = Vector3.Distance(endHeliPos, HeliCopTer.transform.position) / speed;

        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -385, transform.rotation.eulerAngles.z);

        HeliCopTer.transform.position = heliStartPos.transform.position;

        Sequence sequence1 = DOTween.Sequence();

        SoundManager.Instance.Play("Heli");
        sequence1.Append(HeliCopTer.transform.DOMove(carPos, t1).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            vipPos = new Vector3(vIPSlot.transform.position.x, HeliCopTer.transform.position.y, vIPSlot.transform.position.z);
            t2 = Vector3.Distance(vipPos, HeliCopTer.transform.position) / speed;
        }));

        sequence1.Append(car.transform.DOMoveY(3f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            car.transform.SetParent(HeliCopTer.transform);
        }));

        sequence1.Append(HeliCopTer.transform.DOMove(vipPos, t2).SetEase(Ease.InOutCubic));
        sequence1.Append(car.transform.DORotateQuaternion(targetRotation, 0.5f));
        sequence1.Append(car.transform.DOMoveY(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            car.transform.SetParent(null);
            t3 = Vector3.Distance(endHeliPos, HeliCopTer.transform.position) / speed;
        }));

        sequence1.Append(HeliCopTer.transform.DOMove(endHeliPos, t3).SetEase(Ease.InOutCubic));
        sequence1.OnComplete(() =>
        {
            HeliCopTer.transform.position = heliStartPos.transform.position;
            sequence1.Kill();
            HeliCopTer.SetActive(false);
        });
    }

    public void ChangePassenPosSkill()
    {
        for (int i = 0; i < listPassen.Count; i++)
        {
            Passengers currentObject = listPassen[i];
            Vector3 targetPosition = PassenArray[i];

            if (Vector3.Distance(currentObject.transform.position, targetPosition) > 0.01f)
            {
                currentObject.transform.position = targetPosition;
            }
        }
    }

    //public void JumbleSkill()
    //{
    //    int randomIndex;

    //    for (int i = 0; i < listAllCars.Count; i++)
    //    {
    //        if (listAllCars[i].CarType == CarType.Car)
    //        {
    //            randomIndex = Random.Range(0, 4);
    //            listAllCars[i].MeshRenderer.material = listCarMaterial[randomIndex];
    //            listAllCars[i].ColorType = (ColorType)randomIndex;
    //        }
    //        else if (listAllCars[i].CarType == CarType.Van)
    //        {
    //            randomIndex = Random.Range(0, 4);
    //            listAllCars[i].MeshRenderer.material = listVanMaterial[randomIndex];
    //            listAllCars[i].ColorType = (ColorType)randomIndex;
    //        }
    //        else
    //        {
    //            randomIndex = Random.Range(0, 4);
    //            listAllCars[i].MeshRenderer.material = listBusMaterial[randomIndex];
    //            listAllCars[i].ColorType = (ColorType)randomIndex;
    //        }
    //    }

    //    AddCarsToList();
    //}

    public void TurboSkill()
    {
        for (int i = 0; i < listPassen.Count; i++)
        {
            listPassen[i].Speed *= 2;
        }

        for (int i = 0; i < ListCars.Count; i++)
        {
            ListCars[i].Speed *= 2;
        }
    }

    public void ResetTurboSkill()
    {
        for (int i = 0; i < listPassen.Count; i++)
        {
            listPassen[i].Speed /= 2;
        }

        for (int i = 0; i < ListCars.Count; i++)
        {
            ListCars[i].Speed /= 2;
        }
    }


    public void CheckPassenArrayLength()
    {
        int index = 0;

        for (int i = 0; i < listAllCars.Count; i++)
        {
            index += (int)listAllCars[i].CarType;
        }

        passenArray = new Vector3[index];
        //passenArrayIndex = passenArray.Length - 1;
        passenArrayIndex = 0;
    }

    public void SpawnPassengers(List<CarControl> carList)
    {
        List<ColorType> listColorType = new List<ColorType>();

        foreach (var item in carList)
        {
            listColorType.AddRange(Enumerable.Repeat(item.ColorType, item.MaxPassen));
        }

        //listColorType.Shuffle();

        for (int i = 0; i < listColorType.Count; i++)
        {
            Vector3 newPos = spawnPos.position + new Vector3(passenArrayIndex, 0, 0);
            Passengers passen = Instantiate(passenPrefabs, newPos, Quaternion.identity);

            passen.SkinnedMeshRenderer.material = passen.ListColor[(int)listColorType[i]];
            passen.ColorType = listColorType[i];

            //ListPassen.Insert(0, passen);
            listPassen.Add(passen);
            PassenArray[passenArrayIndex] = new Vector3(passen.transform.position.x, passen.transform.position.y, passen.transform.position.z);

            passen.transform.rotation = Quaternion.Euler(0, -90, 0);
            passen.transform.SetParent(spawnPos.transform);

            PassenCount++;
            passenArrayIndex++;

            //yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator SpawnPassenger()
    {
        List<CarControl> listCarControl = new List<CarControl>();

        listAllCars.Shuffle();

        while (listAllCars.Count > 0)
        {
            for (int i = listAllCars.Count - 1; i >= 0; i--)
            {
                CarControl item = listAllCars[i];

                //Ray ray = new Ray(item.transform.position + new Vector3(0, 0.5f, 0), item.transform.forward);

                //bool resultCount = Physics.Raycast(ray, 50f, LayerMask, QueryTriggerInteraction.Ignore);

                Ray rightRay = new Ray(item.LeftRaycast.position, item.transform.forward);
                Ray leftRay = new Ray(item.RighttRaycast.position, item.transform.forward);

                Debug.DrawRay(leftRay.origin, leftRay.direction * 10f, Color.black, 100f);
                Debug.DrawRay(rightRay.origin, rightRay.direction * 10f, Color.red, 100f);

                bool leftResult = Physics.Raycast(leftRay, 500f, layerMask, QueryTriggerInteraction.Ignore);
                bool rightResult = Physics.Raycast(rightRay, 500f, layerMask, QueryTriggerInteraction.Ignore);

                //Debug.DrawRay(ray.origin, ray.direction * 10f, item.MeshRenderer.material.color, 100f);


                if (!leftResult && !rightResult)
                {
                    listCarControl.Add(item);

                    //SpawnPassengers(item);
                    listAllCars.RemoveAt(i);

                    item.Active();

                    //item.MeshRenderer.material.color = Color.white;

                    //item.gameObject.SetActive(false);
                    //yield return new WaitForSeconds(1f);  
                }
            }

            yield return null;
        }

        if (GameManager.Instance.Map1 != null && GameManager.Instance.Map1.activeInHierarchy)
        {
            for (int i = 0; i < listCarControl.Count; i++)
            {
                List<CarControl> listCar = new List<CarControl>();
                listCar.Add(listCarControl[i]);
                SpawnPassengers(listCar);

                yield return null;
            }
        }
        else
        {
            for (int i = 0; i < listCarControl.Count - 3; i += 3)
            {
                List<CarControl> listCar = new List<CarControl>();
                listCar.Add(listCarControl[i]);

                if (i + 1 < listCarControl.Count)
                {
                    listCar.Add(listCarControl[i + 1]);
                }

                if (i + 2 < listCarControl.Count)
                {
                    listCar.Add(listCarControl[i + 2]);
                }

                SpawnPassengers(listCar);
                //StartCoroutine(SpawnPassengers(listCar));

                yield return null;
            }
        }


        GameManager.Instance.UpdateCountSignText();

        //listCarDistances = listCarDistances.OrderBy(car => car.Value).ToList();

        //for (int i = 0; i < listCarDistances.Count; i++)
        //{
        //    SpawnPassengers(listCarDistances[i].Key);
        //}
    }

    public void AddUnlockParkSlot(ParkSlot parkSlot)
    {
        ListParkSlot.Add(parkSlot);
        parkSlot.SpriteRenderer.sprite = parkSlot.UnlockSprite;
        parkSlot.IsLocked = false;
        //parkSlot.ParkPlate.BoxCollider.enabled = true;
    }

    public void CheckPassenger(CarControl car)
    {
        var color = car.ColorType;

        if (!dictCar.ContainsKey(color) && car != null)
        {
            dictCar.Add(color, new Queue<CarControl>());
        }

        dictCar[color].Enqueue(car);

        if (!isProgress)
        {
            isProgress = true;
            StartCoroutine(FillPassenger(car));
        }
    }

    public IEnumerator FillPassenger(CarControl car)
    {
        var color = car.ColorType;

        var passenColor = GetFirstPassenColor();

        if (color != passenColor && IsAllNotEmpty())
        {
            GameManager.Instance.Losing();
        }

        while (color == passenColor)
        {
            MovingPassen();

            if (car.IsMax)
            {
                dictCar.TryGetValue(color, out Queue<CarControl> value);

                if (value == null || value.Count == 0)
                {
                    break;
                }
                else
                {
                    car = value.Peek();
                    color = car.ColorType;
                }
            }

            bool isMax = car.FillPassenger(listPassen[0]);

            if (color == passenColor)
            {
                listPassen.RemoveAt(0);
            }

            if (isMax)
            {
                dictCar[color].Dequeue();
                dictCar.Remove(color);
            }

            passenColor = GetFirstPassenColor();

            if (color != passenColor)
            {
                dictCar.TryGetValue(passenColor, out Queue<CarControl> value);


                if (value == null || value.Count == 0)
                {
                    break;
                }
                else
                {
                    car = value.Peek();
                    color = car.ColorType;
                }
            }

            yield return new WaitForSeconds(0.11f);
        }

        isProgress = false;

        yield return new WaitForSeconds(0.36f);

        //if (color != passenColor && IsAllNotEmpty() && !isProgress)
        //{
        //    GameManager.Instance.Losing();
        //}


        if (ListPassen.Count <= 0)
        {
            GameManager.Instance.Winning();
        }

    }

    private ColorType GetFirstPassenColor()
    {
        if (listPassen.Count > 0)
        {
            return listPassen[0].ColorType;
        }

        return ColorType.None;
    }

    public ParkPlate CheckEmptyParkSlot()
    {
        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (listParkSlot[i].IsEmpty)
            {
                //listParkSlot[i].ParkPlate.BoxCollider.enabled = true;
                listParkSlot[i].IsEmpty = false;
                return listParkSlot[i].ParkPlate;
            }
        }

        return null;
    }

    public void SetParkSlot(ParkPlate parkPlate)
    {
        for (int i = 0; i < listParkSlot.Count; i++)
        {
            if (listParkSlot[i].ParkPlate == parkPlate)
            {
                listParkSlot[i].IsEmpty = false;
                //listParkSlot[i].ParkPlate.BoxCollider.enabled = true;
            }
        }
    }
}

public static class ListExtension
{
    /// <summary>
    /// Shuffles the elements of a list in-place using Unity's Random.Range.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // Random index between 0 and i (inclusive)

            // Swap elements at i and j
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
