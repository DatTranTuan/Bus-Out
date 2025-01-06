using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    private static CarControl currentMovingCar;

    [SerializeField] private CarType carType;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem smoke;

    private int maxPassen;
    private int currentPassen;

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

    private void Awake()
    {
        firstPos = transform.position;

        MaxPassen = (int)CarType;
        CurrentPassen = 0;

        UpdateText();
    }

    private void Update()
    {
        if (isMoving)
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
                transform.DORotateQuaternion(targetRotation, 0.25f);
            }
            else if (isRightBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightCorner.position.x, 0f, rightCorner.position.z), Speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.25f);
            }

            if (isLeftTurnPass)
            {
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.10f);
            }
            else if (isRightTurnPass)
            {
                //transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.10f);
            }

            isTurning = false;
        }

    }

    private void OnMouseDown()
    {
        currentMovingCar = this;

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
            Debug.Log("Click Carrr");
            MoveStraightOut();
        }
    }

    public void MoveStraightOut()
    {
        // Di thang roi bai do

        isMoving = true;
    }

    public void MoveStraightToPark()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);

        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        transform.DORotateQuaternion(targetRotation, 0.25f);
    }

    public void UpdateText()
    {
        if (currentPassen <= MaxPassen)
        {
            passenText.text = currentPassen + "/" + MaxPassen;

            if (currentPassen == MaxPassen)
            {
                LevelManager.Instance.IsVIP = false;
                IsLeaving = true;
                isMoving = true;
                StartCoroutine(DelayTurn(0.20f));
                LevelManager.Instance.SetPlateCollide();
            }
        }
    }

    public IEnumerator DelayTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
        transform.DORotateQuaternion(targetRotation, 0.25f);
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
        }
        else if (other.CompareTag("RightTurnPass") && !isTurning && !IsLeaving)
        {
            isRightTurnPass = true;
            isTurning = true;
        }

        if (other.CompareTag("Plate") && !IsLeaving)
        {
            Debug.Log("Plate Triggerrrrrr");

            //LevelManager.Instance.SetNextCollider();
            isMoving = false;

            ParkPlate parkPlate = other.GetComponent<ParkPlate>();

            transform.DOMove(parkPlate.Destination.transform.position, 0.25f).SetEase(Ease.Linear);

            parkPlate.Car = this;
            parkPlate.BoxCollider.enabled = false;
        }

        if (other.CompareTag("ParkSlot") && !isTurning)
        {
            isTurning = true;
            Debug.Log("PlateSlotttttttttt Triggerrrrrr");

            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -208, transform.rotation.eulerAngles.z);
            transform.DORotateQuaternion(targetRotation, 0.5f);

            ParkSlot parkPlate = other.GetComponent<ParkSlot>();
            parkPlate.ColorType = ColorType;
            parkPlate.IsEmpty = false;
            parkPlate.Car = this;


            if (LevelManager.Instance.IsVIP)
            {
                LevelManager.Instance.ArrangeSkill();
            }
            else
            {
                LevelManager.Instance.CheckListCars();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            CarControl car = collision.gameObject.GetComponent<CarControl>();

            if (car != currentMovingCar)
            {
                car.transform.DOShakeScale(0.2f, 1f, 10, 90f, true, ShakeRandomnessMode.Harmonic);
            }

            isMoving = false;
            transform.DOMove(firstPos, 0.5f).SetEase(Ease.Linear);
        }
    }
}
