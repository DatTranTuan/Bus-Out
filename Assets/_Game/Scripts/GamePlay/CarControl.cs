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

    private bool isLeftTurnPass = false;
    private bool isRightTurnPass = false;

    private bool isLeaving = false;

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
        firstPos = transform.position;

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
                transform.DORotateQuaternion(targetRotation, 0.10f);
                isTurning = false;
            }
            else if (isRightBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightCorner.position.x, 0f, rightCorner.position.z), Speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.10f);
                isTurning = false;
            }

            if (isLeftTurnPass)
            {
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.10f);
                isLeftTurnPass = false;
            }
            else if (isRightTurnPass)
            {
                //transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.10f);
                isRightTurnPass = false;
            }

        }

    }

    private void OnMouseDown()
    {
        currentCar = this;

        if (LevelManager.Instance.IsVIP)
        {
            if (LevelManager.Instance.VIPSlot.IsEmpty && !LevelManager.Instance.HeliCopTer.activeInHierarchy)
            {
                Debug.Log("VIPPPP");
                //transform.position = LevelManager.Instance.VIPSlot.transform.position;

                //Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -208, transform.rotation.eulerAngles.z);
                //transform.DORotateQuaternion(targetRotation, 0.5f);

                LevelManager.Instance.VIPHelicopter(this);
            }
        }
        else
        {
            MoveStraightOut();
        }
    }

    public void MoveStraightOut()
    {
        // Di thang roi bai do

        IsMoving = true;
    }

    public void MoveStraightToPark()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);

        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        transform.DORotateQuaternion(targetRotation, 0.10f);
    }

    public void UpdateText()
    {
        currenTxtPassen++;

        if (currenTxtPassen < MaxPassen)
        {
            passenText.text = currenTxtPassen + "/" + MaxPassen;

            //if (currentPassen > 0)
            //{
            //    LevelManager.Instance.CarChecking(ColorType, this);
            //}
        }
        else if (currenTxtPassen == MaxPassen)
        {
            LevelManager.Instance.IsVIP = false;

            //Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -208, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, -208, transform.rotation.z);

            IsLeaving = true;
            IsMoving = true;
            StartCoroutine(DelayTurn(0.10f));

            passenText.gameObject.SetActive(false);
            //LevelManager.Instance.SetPlateCollide();
        }

    }

    public bool FillPassenger(Passengers passen)
    {
        if (!IsMax)
        {
            currentPassen++;

            passen.MoveToCar(this);
            //LevelManager.Instance.CarTakePassen(passen, this);
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
        // Neu cham vao xe khac => 

        // Cham vao ria bai do xe =>
        if (other.CompareTag("LeftBottom") && !isTurning && !IsLeaving)
        {
            isLeftBottom = true;
            isTurning = true;
        }
        else if (other.CompareTag("RightBottom") && !isTurning && !IsLeaving)
        {
            isRightBottom = true;
            isTurning = true;
        }
        else if (other.CompareTag("Left") || other.CompareTag("Right") && !isTurning && !IsLeaving)
        {
            MoveStraightToPark();
        }

        // Cham vao goc bai do
        if (other.CompareTag("LeftCorner") || other.CompareTag("RightCorner") && isTurnCorner)
        {
            isTurnCorner = false;
            isLeftBottom = false;
            isRightBottom = false;
            MoveStraightToPark();
        }

        if (other.CompareTag("LeftTurnPass") && !isTurning && !IsLeaving)
        {
            isLeftTurnPass = true;
            isTurning = true;
            gameObject.tag = "Untagged";
        }
        else if (other.CompareTag("RightTurnPass") && !isTurning && !IsLeaving)
        {
            isRightTurnPass = true;
            isTurning = true;
            gameObject.tag = "Untagged";
        }

        if (other.CompareTag("Plate") && !IsLeaving)
        {
            //LevelManager.Instance.SetNextCollider();
            IsMoving = false;

            ParkPlate parkPlate = other.GetComponent<ParkPlate>();

            if (parkPlate.ParkSlot.IsEmpty)
            {
                parkPlate.ParkSlot.Car = this;
            }

            parkPlate.ParkSlot.IsEmpty = false;
            parkPlate.ParkSlot.ColorType = ColorType;


            transform.DOMove(parkPlate.ParkSlot.Destination.position, 0.10f).SetEase(Ease.Linear);
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -385, transform.rotation.eulerAngles.z);
            transform.DORotateQuaternion(targetRotation, 0.10f).OnComplete(() =>
            {
                passenText.gameObject.SetActive(true);
                arrowMark.SetActive(false);

                if (LevelManager.Instance.IsVIP)
                {
                    LevelManager.Instance.ArrangeSkill();
                }
                else
                {
                    //LevelManager.Instance.CarChecking(this);
                    LevelManager.Instance.CheckPassenger(this);
                }
            });

            parkPlate.Car = this;
            parkPlate.BoxCollider.enabled = false;

        }

        if (other.CompareTag("ParkSlot") && !isLeaving)
        {
            ParkSlot parkSlot = other.GetComponent<ParkSlot>();
            Debug.Log(other.gameObject.name);
            parkSlot.ColorType = ColorType;

            //if (parkPlate.IsEmpty)
            //{
            //    parkPlate.Car = this;
            //}

            //parkPlate.IsEmpty = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            CarControl car = collision.gameObject.GetComponent<CarControl>();

            if (car != currentCar)
            {
                //car.transform.DOShakeScale(0.2f, 1f, 10, 90f, true, ShakeRandomnessMode.Harmonic);
                //car.transform.DOShakePosition(0.5f, 1f, 10, 90f, false, true, ShakeRandomnessMode.Harmonic);

                //car.transform.DORotate(new Vector3(0, 0f, 15f), 2f)
                //         .SetEase(Ease.InOutSine)
                //         .SetLoops(-1, LoopType.Yoyo);

                IsMoving = false;
                transform.DOMove(firstPos, 0.5f).SetEase(Ease.Linear);
            }
        }
    }
}
