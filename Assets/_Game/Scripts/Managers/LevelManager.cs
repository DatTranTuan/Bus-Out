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

    [SerializeField] private CarControl carCenter;
    [SerializeField] private GameObject allCars;

    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;

    private int passenCount = 0;

    private int carTouchCount = 0;

    public List<Passengers> ListPassen { get => listPassen; set => listPassen = value; }
    public List<ParkPlate> ListPlate { get => listPlate; set => listPlate = value; }
    public Transform FirstMove { get => firstMove; set => firstMove = value; }
    public bool IsVIP { get => isVIP; set => isVIP = value; }
    public ParkSlot VIPSlot { get => vIPSlot; set => vIPSlot = value; }
    public List<ParkSlot> ListParkSlot { get => listParkSlot; set => listParkSlot = value; }
    public GameObject HeliCopTer { get => heliCopTer; set => heliCopTer = value; }
    public int PassenCount { get => passenCount; set => passenCount = value; }
    public Vector3[] PassenArray { get => passenArray; set => passenArray = value; }
    public int CarTouchCount { get => carTouchCount; set => carTouchCount = value; }

    private void Start()
    {
        CheckPassenArrayLength();

        AddCarsToList();

        GameManager.Instance.UpdateCountSignText();
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
            if (!listParkSlot[i].IsEmpty)
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

        for (int i = 0; i < listPassen.Count; i++)
        {
            // && index2 < ListParkSlot[0].Car.MaxPassen
            if (listPassen[i].ColorType == car.ColorType && !isVIP && index2 < car.MaxPassen)
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
                //listPassen[i].ChangeAnim("Run");
                currentObject.transform.position = targetPosition;
            }
            else
            {
                //listPassen[i].ChangeAnim("Idle");
            }

        }
    }

    public void JumbleSkill()
    {
        int randomIndex;

        for (int i = 0; i < listCars.Count; i++)
        {
            if (listCars[i].CarType == CarType.Car)
            {
                randomIndex = Random.Range(0, 4);
                listCars[i].MeshRenderer.material = listCarMaterial[randomIndex];
                listCars[i].ColorType = (ColorType)randomIndex;
            }
            else if (listCars[i].CarType == CarType.Van)
            {
                randomIndex = Random.Range(0, 4);
                listCars[i].MeshRenderer.material = listVanMaterial[randomIndex];
                listCars[i].ColorType = (ColorType)randomIndex;
            }
            else
            {
                randomIndex = Random.Range(0, 4);
                listCars[i].MeshRenderer.material = listBusMaterial[randomIndex];
                listCars[i].ColorType = (ColorType)randomIndex;
            }
        }
    }

    public void TurboSkill()
    {
        for (int i = 0; i < listPassen.Count; i++)
        {
            listPassen[i].Speed *= 2;
        }

        for (int i = 0; i < listCars.Count; i++)
        {
            listCars[i].Speed *= 2;
        }
    }

    public void ResetTurboSkill()
    {
        for (int i = 0; i < listPassen.Count; i++)
        {
            listPassen[i].Speed /= 2;
        }

        for (int i = 0; i < listCars.Count; i++)
        {
            listCars[i].Speed /= 2;
        }
    }

    public void SpawnPassen()
    {
        // Sinh ra hanh khach

        for (int i = 1; i <= 42; i++)
        {
            Vector3 newPos = spawnPos.position + new Vector3(i, 0, 0);
            Passengers passen = Instantiate(passenPrefabs, newPos, Quaternion.identity);

            if (i == 5 || i == 2 || i == 19 || i == 22)
            {
                passen.SkinnedMeshRenderer.material = passen.ListColor[2];
                passen.ColorType = ColorType.Red;
            }
            else if (i == 10 || i == 3 || i == 16 || i == 27 || i == 6 || i == 1 || i == 12 || i == 30)
            {
                passen.SkinnedMeshRenderer.material = passen.ListColor[1];
                passen.ColorType = ColorType.Green;
            }
            else if (i == 18 || i == 9 || i == 25 || i == 8 || i == 14 || i == 21 || i == 7 || i == 11)
            {
                passen.SkinnedMeshRenderer.material = passen.ListColor[3];
                passen.ColorType = ColorType.Purple;
            }
            else
            {
                passen.SkinnedMeshRenderer.material = passen.ListColor[0];
                passen.ColorType = ColorType.Blue;
            }

            ListPassen.Add(passen);
            PassenArray[i - 1] = new Vector3(passen.transform.position.x, passen.transform.position.y, passen.transform.position.z);

            passen.transform.rotation = Quaternion.Euler(0, -90, 0);
            passen.transform.SetParent(spawnPos.transform);

            PassenCount++;
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
        passenArrayIndex = passenArray.Length - 1;
    }

    public void SpawnPassengers(CarControl car)
    {
        for (int i = 1; i <= (int)car.CarType; i++)
        {
            Vector3 newPos = spawnPos.position + new Vector3(passenArrayIndex, 0, 0);
            Passengers passen = Instantiate(passenPrefabs, newPos, Quaternion.identity);

            passen.SkinnedMeshRenderer.material = passen.ListColor[(int)car.ColorType];
            passen.ColorType = car.ColorType;

            ListPassen.Insert(0, passen); 
            PassenArray[passenArrayIndex] = new Vector3(passen.transform.position.x, passen.transform.position.y, passen.transform.position.z);

            passen.transform.rotation = Quaternion.Euler(0, -90, 0);
            passen.transform.SetParent(spawnPos.transform);

            PassenCount++;
            passenArrayIndex--; 
        }
    }

    public void AddCarsToList()
    {
        Vector3 forwardDirection = transform.forward;      
        Vector3 rightDirection = transform.right;          
        Vector3 leftDirection = -transform.right;

        List<KeyValuePair<CarControl, float>> listCarDistances = new List<KeyValuePair<CarControl, float>>();

        foreach (CarControl item in listAllCars)
        {
            float distance = Vector3.Distance(item.transform.position, carCenter.transform.position);

            if (Physics.Raycast(transform.position, forwardDirection, out RaycastHit hitForward, rayDistance, layerMask))
            {
              
            }
            else
            {
                
            }

            if (Physics.Raycast(transform.position, rightDirection, out RaycastHit hitRight, rayDistance, layerMask))
            {
                
            }
            else
            {
                
            }

            if (Physics.Raycast(transform.position, leftDirection, out RaycastHit hitLeft, rayDistance, layerMask))
            {
               
            }
            else
            {
                
            }

            listCarDistances.Add(new KeyValuePair<CarControl, float>(item, distance));
        }

        listCarDistances = listCarDistances.OrderBy(car => car.Value).ToList();

        for (int i = 0; i < listCarDistances.Count; i++)
        {
            SpawnPassengers(listCarDistances[i].Key);
        }
    }
     
    public void AddUnlockParkSlot(ParkSlot parkSlot)
    {
        ListParkSlot.Add(parkSlot);
        parkSlot.SpriteRenderer.sprite = parkSlot.UnlockSprite;
        parkSlot.IsLocked = false;
        parkSlot.ParkPlate.BoxCollider.enabled = true;
    }

    public void CheckPassenger(CarControl car)
    {
        var color = car.ColorType;

        if (!dictCar.ContainsKey(color))
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
            //yield return new WaitForSeconds(0.76f);

            if (isMax)
            {
                //StartCoroutine(car.DelayTurn(0.1f));
                dictCar[color].Dequeue();
            }

            listPassen.RemoveAt(0);

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

        if (color != passenColor && IsAllNotEmpty() && !isProgress)
        {
            GameManager.Instance.Losing();
        }

        isProgress = false;

        yield return new WaitForSeconds(0.36f);

        if (ListPassen.Count <= 0)
        {
            GameManager.Instance.WinningPanel.SetActive(true);
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
                //listParkSlot[i].IsEmpty = false;
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
                listParkSlot[i].ParkPlate.BoxCollider.enabled = true;
            }
        }
    }
}
