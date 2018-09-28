using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float ForceMoveDelay;
    public GameObject RootPrefab;
    public GameObject MainCamera;

    private bool cameraResetting;
    private bool lockInput;
    private float timeUntilMove;
    private int score;
    private Text scoreText;
    private Vector3 currentPosition;
    private Vector3 nextPosition;

    [HideInInspector]
    public enum Directions { Null, Right, Left, Up, Down };
    public Directions MovementDirection;
    public int RootParts;

    private void Start()
    {
        scoreText = GameObject.Find("/Canvas/Score Text").GetComponent<Text>();
        currentPosition = Vector3.zero;
        timeUntilMove = ForceMoveDelay;
    }

    private void Update()
    {
        if (!lockInput)
        {
            PlayerControls();
            ForceMove();
        }
        else if (lockInput && cameraResetting)
        {
            if (((MainCamera.GetComponent<CameraController>().TargetPosition
                + MainCamera.GetComponent<CameraController>().PositionSetting)
                - MainCamera.transform.position).magnitude
                <= MainCamera.GetComponent<CameraController>().PositionMargin)
            {
                lockInput = false;
                cameraResetting = false;
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void Die()
    {
        RootParts = 0;
        MovementDirection = Directions.Null;
        currentPosition = Vector3.zero;
        MainCamera.GetComponent<CameraController>().TargetPosition = Vector3.zero;
        cameraResetting = true;
    }

    public void Ready()
    {
        ++RootParts;
        timeUntilMove = ForceMoveDelay;
        lockInput = false;
        currentPosition = nextPosition;
    }

    private void ForceMove()
    {
        if (!lockInput)
        {
            if (timeUntilMove < 0 && MovementDirection != Directions.Null)
            {
                if (MovementDirection == Directions.Right)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(2, 0, 0);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Right;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, -90)));
                }
                else if (MovementDirection == Directions.Left)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(-2, 0, 0);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Left;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                else if (MovementDirection == Directions.Up)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(0, 0, 2);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Up;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(90, 0, 0)));
                }
                else if (MovementDirection == Directions.Down)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(0, 0, -2);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Down;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
                }
            }
            else if (timeUntilMove > 0)
            {
                timeUntilMove -= Time.deltaTime;
            }
        }
    }

    private void PlayerControls()
    {
        //Horizontal movement
        if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") == 0)
        {
            //Right
            if (Input.GetAxis("Horizontal") > 0 && MovementDirection != Directions.Left)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(2, 0, 0);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Right;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, -90)));
            }
            //Left
            else if (Input.GetAxis("Horizontal") < 0 && MovementDirection != Directions.Right)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(-2, 0, 0);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Left;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
            }
        }
        //Vertical movement
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") != 0)
        {
            //Up
            if (Input.GetAxis("Vertical") > 0 && MovementDirection != Directions.Down)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(0, 0, 2);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Up;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(90, 0, 0)));
            }
            //Down
            else if (Input.GetAxis("Vertical") < 0 && MovementDirection != Directions.Up)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(0, 0, -2);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Down;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
            }
        }
    }
}
