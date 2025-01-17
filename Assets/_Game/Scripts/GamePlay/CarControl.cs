using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    public bool IsMax => currentPassen == maxPassen;

    private static CarControl currentCar;

    [SerializeField] private CarType carType;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private GameObject arrowMark;

    private int maxPassen;
    private int currentPassen;

    private int currenTxtPassen = -1;

    [SerializeField] private ColorType colorType;

    [SerializeField] private Transform leftCorner;
    [SerializeField] private Transform rightCorner;

    [SerializeField] private Vector3 firstPos;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Text passenText;

    private bool isMoving = false;

    private bool isTurning = false;

    private bool isTurnCorner = true;
    private bool isLeftBottom = false;
    private bool isRightBottom = false;

    private bool isTurnPass = false;
    private bool isGoing = false;

    private bool isLeaving = false;

    private Quaternion firstRotate;
    private float carTouchTime;
    private Transform target;

    public int CurrentPassen { get => currentPassen; set => currentPassen = value; }
    public int MaxPassen { get => maxPassen; set => maxPassen = value; }
    public CarType CarType { get => carType; set => carType = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsLeaving { get => isLeaving; set => isLeaving = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    private void Awake()
    {
        currentCar = this;

        firstPos = transform.position;
        firstRotate = currentCar.transform.rotation;

        MaxPassen = (int)CarType;
        CurrentPassen = 0;

        UpdateText();
    }

    private void Update()
    {
        if (IsMoving)
        {
            // di chuyen khi bam vao
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            smoke.gameObject.SetActive(true);
        }
        else
        {
            smoke.gameObject.SetActive(false);
        }

        if (isTurning)
        {
            if (isLeftBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(leftCorner.position.x, 0f, leftCorner.position.z), Speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.11f);
                isTurning = false;
            }
            else if (isRightBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightCorner.position.x, 0f, rightCorner.position.z), Speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.11f);
                isTurning = false;
            }

            if (isTurnPass)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                isTurnPass = false;
            }
        }

    }

    private void OnMouseDown()
    {
        currentCar = this;

        if (!passenText.gameObject.activeInHierarchy)
        {
            if (LevelManager.Instance.IsVIP)
            {
                if (LevelManager.Instance.VIPSlot.IsEmpty && !LevelManager.Instance.HeliCopTer.activeInHierarchy)
                {
                    BuyingManager.Instance.VIPNotifi.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        BuyingManager.Instance.VIPNotifi.gameObject.SetActive(false);
                    });

                    LevelManager.Instance.VIPHelicopter(this);
                }
            }
            else
            {
                if (BuyingManager.Instance.IsBuying || LevelManager.Instance.CarTouchCount >= LevelManager.Instance.ListParkSlot.Count && !GameManager.Instance.LosingPanel.activeInHierarchy)
                {
                    return;
                }
                else if (LevelManager.Instance.CarTouchCount < LevelManager.Instance.ListParkSlot.Count)
                {
                    if (GameManager.Instance.TouchTime - carTouchTime >= 0.5f)
                    {
                        var emptyParkSlot = LevelManager.Instance.CheckEmptyParkSlot();

                        // bi goi 2 lan nen tach ra 2 ham CheckEmptyParkSlot() va SetParkSlot()

                        if (emptyParkSlot == null)
                        {
                            return;
                        }
                        else
                        {
                            if (isGoing)
                            {
                                target = emptyParkSlot.transform;
                                LevelManager.Instance.SetParkSlot(emptyParkSlot); 
                            }
                        }

                        carTouchTime = GameManager.Instance.TouchTime;

                        gameObject.layer = 0;
                        LevelManager.Instance.CarTouchCount++;

                        MoveStraightOut();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        else
        {
            return;
        }
    }

    public void MoveStraightOut()
    {
        // Di thang roi bai do

        IsMoving = true;
    }

    public void MoveStraightToPark()
    {
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z);
        transform.DORotateQuaternion(targetRotation, 0.10f);
    }

    public void UpdateText()
    {
        currenTxtPassen++;

        if (currenTxtPassen < MaxPassen)
        {
            passenText.text = currenTxtPassen + "/" + MaxPassen;
        }
        else if (currenTxtPassen == MaxPassen)
        {
            LevelManager.Instance.CarTouchCount--;
            LevelManager.Instance.IsVIP = false;

            //Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -208, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, -208, transform.rotation.z);

            IsLeaving = true;
            IsMoving = true;
            StartCoroutine(DelayTurn(0.10f));

            passenText.gameObject.SetActive(false);
        }

    }

    public bool FillPassenger(Passengers passen)
    {
        if (!IsMax)
        {
            currentPassen++;

            passen.MoveToCar(this);
        }

        return IsMax;
    }

    public IEnumerator DelayTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
        transform.DORotateQuaternion(targetRotation, 0.25f);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftBottom") && !isTurning && !IsLeaving)
        {
            isLeftBottom = true;
            isTurning = true;
            gameObject.layer = 3;
        }
        else if (other.CompareTag("RightBottom") && !isTurning && !IsLeaving)
        {
            isRightBottom = true;
            isTurning = true;
            gameObject.layer = 3;
        }
        else if (other.CompareTag("Left") || other.CompareTag("Right") && !isTurning && !IsLeaving)
        {
            MoveStraightToPark();
            gameObject.layer = 3;
        }

        // Cham vao goc bai do
        if (other.CompareTag("LeftCorner") || other.CompareTag("RightCorner") && isTurnCorner)
        {
            isTurnCorner = false;
            isLeftBottom = false;
            isRightBottom = false;
            MoveStraightToPark();
            gameObject.layer = 3;
        }

        if (other.CompareTag("LeftTurnPass") && !isTurning && !IsLeaving)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            isTurning = true;
        }

        if (other.CompareTag("Plate") && !IsLeaving && isMoving)
        {
            isTurnPass = false;

            isTurning = true;
            IsMoving = false;

            ParkPlate parkPlate = other.GetComponent<ParkPlate>();

            if (parkPlate.ParkSlot.IsEmpty)
            {
                parkPlate.ParkSlot.Car = this;
            }

            //parkPlate.ParkSlot.IsEmpty = false;
            parkPlate.ParkSlot.ColorType = ColorType;


            transform.DOMove(parkPlate.ParkSlot.Destination.position, 0.11f).SetEase(Ease.Linear);
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -385, transform.rotation.eulerAngles.z);
            transform.DORotateQuaternion(targetRotation, 0.11f).OnComplete(() =>
            {
                passenText.gameObject.SetActive(true);
                arrowMark.SetActive(false);

                if (LevelManager.Instance.IsVIP)
                {
                    LevelManager.Instance.ArrangeSkill();
                }
                else
                {
                    LevelManager.Instance.CheckPassenger(this);
                }
            });

            parkPlate.Car = this;
            parkPlate.BoxCollider.enabled = false;

        }

        if (other.CompareTag("ParkSlot") && !isLeaving)
        {
            isGoing = true;

            ParkSlot parkSlot = other.GetComponent<ParkSlot>();
            parkSlot.ColorType = ColorType;
            //parkSlot.IsEmpty = false;
            parkSlot.Car = this;

            if (LevelManager.Instance.IsVIP)
            {
                passenText.gameObject.SetActive(true);
                arrowMark.SetActive(false);
                LevelManager.Instance.ArrangeSkill();
            }
        }

        if (other.gameObject.layer == 3)
        {
            isGoing = false;

            if (LevelManager.Instance.CarTouchCount > 0)
            {
                LevelManager.Instance.CarTouchCount--;
            }

            CarControl car = other.gameObject.GetComponent<CarControl>();

            if (car != currentCar && !car.IsMoving && isMoving)
            {
                IsMoving = false;
                transform.DOMove(firstPos, 0.5f).SetEase(Ease.Linear);
                gameObject.layer = 3;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ParkSlot") && IsLeaving)
        {
            ParkSlot parkSlot = other.GetComponent<ParkSlot>();
            parkSlot.IsEmpty = true;
            parkSlot.ParkPlate.BoxCollider.enabled = true;
        }
    }

}
