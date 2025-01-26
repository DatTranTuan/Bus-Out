using DG.Tweening;
using System.Collections;
using UnityEditor;
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
    [SerializeField] private Collider colider;
    [SerializeField] private Rigidbody rb;

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

    [SerializeField] private Transform leftRaycast;
    [SerializeField] private Transform righttRaycast;
    [SerializeField] private LayerMask carLayer;

    private bool isMoving = false;

    private bool isTurning = false;

    private bool isTurnCorner = true;
    private bool isLeftBottom = false;
    private bool isRightBottom = false;

    private bool isLeaving = false;

    private float carTouchTime;
    private Transform parkPlateTarget;
    private Transform parkSlotTarget;
    private ParkPlate emptyParkSlot;

    private Tweener tweener;
    private Quaternion firstRotate;

    private Coroutine delayCoroutine;
    private Coroutine delayTurnOffCoroutine;

    public int CurrentPassen { get => currentPassen; set => currentPassen = value; }
    public int MaxPassen { get => maxPassen; set => maxPassen = value; }
    public CarType CarType { get => carType; set => carType = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsLeaving { get => isLeaving; set => isLeaving = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public Quaternion FirstRotate { get => firstRotate; set => firstRotate = value; }
    public Vector3 FirstPos { get => firstPos; set => firstPos = value; }
    public bool IsTurning { get => isTurning; set => isTurning = value; }
    public Transform LeftRaycast { get => leftRaycast; set => leftRaycast = value; }
    public Transform RighttRaycast { get => righttRaycast; set => righttRaycast = value; }

    private void Awake()
    {
        currentCar = this;

        FirstPos = transform.position;
        FirstRotate = currentCar.transform.rotation;

        MaxPassen = (int)CarType;


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

        if (IsTurning)
        {
            if (isLeftBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(leftCorner.position.x, 0f, leftCorner.position.z), Speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.16f);
                IsTurning = false;
            }
            else if (isRightBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightCorner.position.x, 0f, rightCorner.position.z), Speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
                transform.DORotateQuaternion(targetRotation, 0.16f);
                IsTurning = false;
            }

            //if (isTurnPass)
            //{
            //    transform.position = Vector3.MoveTowards(transform.position, parkPlateTarget.position, speed * Time.deltaTime);

            //    if (Vector3.Distance(transform.position, parkPlateTarget.position) < 0.1f)
            //    {
            //        transform.DOMove(parkSlotTarget.position, 0.11f).SetEase(Ease.Linear).OnComplete(() =>
            //        {
            //            isTurnPass = false;
            //        });
            //    }

            //    //isTurnPass = false;
            //}
        }

    }

    public void Respawn()
    {
        if (delayCoroutine != null && delayTurnOffCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            StopCoroutine(delayTurnOffCoroutine);
        }

        //StopCoroutine(DelayTurnOff());
        //StopAllCoroutines();
        //StopCoroutine(DelayTurnOff());
        gameObject.SetActive(true);

        isMoving = false;
        gameObject.layer = 3;

        IsTurning = false;

        isTurnCorner = true;
        isLeftBottom = false;
        isRightBottom = false;

        isLeaving = false;

        currentPassen = 0;
        currenTxtPassen = 0;
        transform.position = firstPos;
        transform.rotation = firstRotate;

        passenText.gameObject.SetActive(false);
        arrowMark.SetActive(true);

        colider.isTrigger = false;

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
                if (BuyingManager.Instance.IsBuying || LevelManager.Instance.IsAllNotEmpty() || GameManager.Instance.LosingPanel.activeInHierarchy
                    || GameManager.Instance.TouchTime - GameManager.Instance.allCarTouchTime < 0.5f)
                {
                    GameManager.Instance.TouchTime = GameManager.Instance.allCarTouchTime;
                    return;
                }
                else if (LevelManager.Instance.IsOneEmpty())
                {
                    GameManager.Instance.allCarTouchTime = GameManager.Instance.TouchTime;

                    if (GameManager.Instance.TouchTime - carTouchTime >= 0.5f)
                    {
                        gameObject.layer = 0;

                        if (CheckCanMove())
                        {
                            CheckDestination();
                        }

                        carTouchTime = GameManager.Instance.TouchTime;

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

    public bool CheckCanMove()
    {
        Ray rightRay = new Ray(LeftRaycast.position, transform.forward);
        Ray leftRay = new Ray(RighttRaycast.position, transform.forward);

        Debug.DrawRay(leftRay.origin, leftRay.direction * 10f, Color.black, 100f);
        Debug.DrawRay(rightRay.origin, rightRay.direction * 10f, Color.red, 100f);

        bool leftResult = Physics.Raycast(leftRay, 500f, carLayer, QueryTriggerInteraction.Collide);
        bool rightResult = Physics.Raycast(rightRay, 500f, carLayer, QueryTriggerInteraction.Collide);

        if (leftResult || rightResult)
        {
            return false;
        }
        else if (!leftResult && !rightResult)
        {
            return true;
        }

        return false;
    }

    public void CheckDestination()
    {
        emptyParkSlot = LevelManager.Instance.CheckEmptyParkSlot();

        if (emptyParkSlot == null)
        {
            return;
        }
        else
        {
            parkPlateTarget = emptyParkSlot.ParkPlatePos.transform;
            parkSlotTarget = emptyParkSlot.ParkSlotPos.transform;
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
        tweener = transform.DORotateQuaternion(targetRotation, 0.11f);


        //transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
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
            SoundManager.Instance.Play("CarLeave");

            LevelManager.Instance.IsVIP = false;

            //Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -208, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, -208, transform.rotation.z);

            IsLeaving = true;
            IsMoving = true;

            BuyingManager.Instance.CoinPlus();
            BuyingManager.Instance.UpdateCoin();

            //StartCoroutine(DelayTurn());
            delayCoroutine = StartCoroutine(DelayTurn());

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

    public IEnumerator DelayTurn()
    {
        yield return new WaitForSeconds(0.11f);
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
        transform.DORotateQuaternion(targetRotation, 0.25f);

        //LevelManager.Instance.CarTouchCount--;
        //parkSlot.IsEmpty = true;

        //Destroy(gameObject, 3f);

        delayTurnOffCoroutine = StartCoroutine(DelayTurnOff());
    }

    public IEnumerator DelayTurnOff()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public void Active()
    {
        colider.isTrigger = true;
        //meshCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftBottom") && !IsTurning && !IsLeaving)
        {
            isLeftBottom = true;
            IsTurning = true;
            gameObject.layer = 3;
            SoundManager.Instance.Play("CarMove");

            //CheckDestination();
        }
        else if (other.CompareTag("RightBottom") && !IsTurning && !IsLeaving)
        {
            isRightBottom = true;
            IsTurning = true;
            gameObject.layer = 3;
            SoundManager.Instance.Play("CarMove");

            //CheckDestination();
        }
        else if (other.CompareTag("Left") || other.CompareTag("Right") && !IsTurning && !IsLeaving)
        {
            IsTurning = true;
            MoveStraightToPark();
            gameObject.layer = 3;
            SoundManager.Instance.Play("CarMove");

            //CheckDestination();
        }

        // Cham vao goc bai do
        if (other.CompareTag("LeftCorner") || other.CompareTag("RightCorner") && isTurnCorner)
        {
            isTurnCorner = false;
            isLeftBottom = false;
            isRightBottom = false;
            MoveStraightToPark();
            gameObject.layer = 3;
            SoundManager.Instance.Play("CarMove");

            //CheckDestination();
        }

        if (other.CompareTag("LeftTurnPass") && !IsLeaving)
        {
            tweener.Kill();

            //CheckDestination();

            if (parkPlateTarget == null)
            {
                Debug.LogError("??????????????????????");
            }
            else
            {
                Vector3 direction = parkPlateTarget.position - transform.position;
                direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;

                transform.DOMove(parkPlateTarget.position, 0.36f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOMove(parkSlotTarget.position, 0.11f).SetEase(Ease.Linear);
                    Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -385, transform.rotation.eulerAngles.z);
                    transform.DORotateQuaternion(targetRotation, 0.11f);
                });

                gameObject.layer = 0;
            }
        }

        if (other.CompareTag("ParkSlot") && !isLeaving)
        {
            IsTurning = true;
            IsMoving = false;

            ParkSlot parkSlot = other.GetComponent<ParkSlot>();
           
            parkSlot.Car = this;

            parkSlot.ColorType = ColorType;


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

            //if (LevelManager.Instance.IsVIP)
            //{
            //    passenText.gameObject.SetActive(true);
            //    arrowMark.SetActive(false);
            //    LevelManager.Instance.ArrangeSkill();
            //}
        }

        if (other.gameObject.layer == 3)
        {
            //if (LevelManager.Instance.CarTouchCount > 0)
            //{
            //    LevelManager.Instance.CarTouchCount--;
            //}

            CarControl car = other.gameObject.GetComponent<CarControl>();

            if (car != currentCar && !car.IsMoving && isMoving)
            {
                SoundManager.Instance.Play("Hit");
                IsMoving = false;
                transform.DOMove(FirstPos, 0.5f).SetEase(Ease.Linear);
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
            //parkSlot.ParkPlate.BoxCollider.enabled = true;
        }
    }

}
