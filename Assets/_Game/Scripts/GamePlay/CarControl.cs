using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CarControl : MonoBehaviour
{
    [SerializeField] private CarType carType;

    private int maxPassen;
    private int currentPassen;

    [SerializeField] private ColorType colorType;

    [SerializeField] private Transform leftCorner;
    [SerializeField] private Transform rightCorner;

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

    private bool isTouchPlate = false;
    private bool isLeaving = false;

    public int CurrentPassen { get => currentPassen; set => currentPassen = value; }

    private void Awake()
    {
        maxPassen = (int)carType;
        CurrentPassen = 0;

        UpdateText();
    }

    private void Update()
    {
        if (isMoving)
        {
            // di chuyen khi bam vao
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (isTurning)
        {
            if (isLeftBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(leftCorner.position.x, 0f, leftCorner.position.z), speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

                Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                //Vector3 targetTransform = new Vector3(0, -90, 0);
                //transform.rotation = Vector3.Lerp(transform.position,targetTransform, rotationSpeed * Time.deltaTime);
            }
            else if (isRightBottom)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightCorner.position.x, 0f, rightCorner.position.z), speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }

            if (isLeftTurnPass)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }
            else if (isRightTurnPass)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
            }

            isTurning = false;
        }

        if (isTouchPlate)
        {
            // do vao o mau Xam

            var parent = LevelManager.Instance.ListPlate[LevelManager.Instance.Index].transform.GetChild(0);
            var target = parent.transform.position;

            float stepX = Mathf.MoveTowards(transform.position.x, target.x, 30 * Time.deltaTime);
            float stepZ = Mathf.MoveTowards(transform.position.z, target.z, 30 * Time.deltaTime);

            transform.position = new Vector3(stepX, transform.position.y, stepZ);
            transform.rotation = Quaternion.Euler(transform.rotation.x, -208, transform.rotation.z);
        }


    }

    private void OnMouseDown()
    {
        Debug.Log("Click Carrr");
        MoveStraightOut();
    }

    public void BackMove()
    {
        // Quay lai cho cu
    }

    public void MoveStraightOut()
    {
        // Di thang roi bai do

        isMoving = true;
    }

    public void MoveStraightToPark()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }

    //public void CheckPassenger()
    //{
    //    if (LevelManager.Instance.ListPassen.Count <= 0)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        if (LevelManager.Instance.ListPassen[0].ColorType == colorType)
    //        {
    //            LevelManager.Instance.ListPassen[0].MoveToCar();
    //        }
    //    }
    //}

    public void UpdateText()
    {
        if (currentPassen <= maxPassen)
        {
            passenText.text = currentPassen + "/" + maxPassen;

            if (currentPassen == maxPassen)
            {
                isLeaving = true;
                isMoving = true;
                StartCoroutine(DelayTurn(0.30f));
                LevelManager.Instance.SetPlateCollide();
            }
        }
    }

    public IEnumerator DelayTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Neu cham vao xe khac => 

        // Cham vao ria bai do xe =>
        if (other.CompareTag("LeftBottom") && !isTurning && !isLeaving)
        {
            isLeftBottom = true;
            isTurning = true;
        }
        else if (other.CompareTag("RightBottom") && !isTurning && !isLeaving)
        {
            isRightBottom = true;
            isTurning = true;
        }
        else if (other.CompareTag("Left") || other.CompareTag("Right") && !isTurning && !isLeaving)
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

        if (other.CompareTag("LeftTurnPass") && !isTurning && !isLeaving)
        {
            isLeftTurnPass = true;
            isTurning = true;
        }
        else if (other.CompareTag("RightTurnPass") && !isTurning && !isLeaving)
        {
            isRightTurnPass = true;
            isTurning = true;
        }

        if (other.CompareTag("Plate") && !isLeaving)
        {
            isTouchPlate = true;
            isMoving = false;
        }

        if (other.CompareTag("ParkSlot"))
        {
            isTouchPlate = false;
            LevelManager.Instance.SetNextCollider();

            ParkSlot parkPlate = other.GetComponent<ParkSlot>();
            parkPlate.ColorType = colorType;
            parkPlate.IsEmpty = false;
            parkPlate.Car = this;

            LevelManager.Instance.CheckListPassen();
        }
    }
}
