using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float FlyBySpeed;
    public float MovementSpeed;
    public float PositionMargin;
    public Vector3 PositionSetting;

    private bool gameOver;
    private bool flyBying;

    // [HideInInspector]
    public Vector3 TargetPosition;

    private void Start()
    {
        TargetPosition = Vector3.zero;
    }

    private void Update()
    {
        MoveCamera(TargetPosition);
    }

    public void GameOver()
    {
        gameOver = true;
        TargetPosition = new Vector3(0, 5, 5);
    }

    private void MoveCamera(Vector3 target)
    {
        Vector3 targetPosition = target + PositionSetting;

        if ((targetPosition - transform.position).magnitude > PositionMargin)
        {
            if (!gameOver)
            {
                transform.Translate((targetPosition - transform.position).normalized * MovementSpeed * Time.deltaTime);
                transform.localPosition = new Vector3(transform.position.x, PositionSetting.y, transform.position.z);
            }
            else
            {
                if (!flyBying)
                {
                    transform.Translate((targetPosition - transform.position).normalized * MovementSpeed * Time.deltaTime);

                    if ((targetPosition - transform.position).magnitude < PositionMargin)
                    {
                        flyBying = true;
                    }
                }
            }
        }
        if (gameOver && flyBying)
        {
            transform.RotateAround(Vector3.zero, Vector3.up, FlyBySpeed * Time.deltaTime);
        }
    }
}
