using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MovementSpeed;
    public float PositionMargin;
    public Vector3 PositionSetting;

    [HideInInspector]
    public Vector3 TargetPosition;

    private void Start()
    {
        TargetPosition = Vector3.zero;
    }

    private void Update()
    {
        MoveCamera(TargetPosition);
    }

    private void MoveCamera(Vector3 target)
    {
        Vector3 targetPosition = target + PositionSetting;

        if ((targetPosition - transform.position).magnitude > PositionMargin)
        {
            transform.Translate((targetPosition - transform.position).normalized * MovementSpeed * Time.deltaTime);
        }
    }
}
