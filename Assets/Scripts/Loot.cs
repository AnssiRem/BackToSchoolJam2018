using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public float RotateSpeed;
    public float FloatAmplitude;
    public float FloatHeight;
    public float FloatSpeed;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        Vector3 prevRotation = transform.rotation.eulerAngles;
        Vector3 nextRotation = prevRotation + new Vector3(0, RotateSpeed * Time.deltaTime, 0);
        Quaternion nextQuaternion = Quaternion.Euler(nextRotation);

        transform.rotation = nextQuaternion;

        transform.position = new Vector3(initialPosition.x, FloatHeight + FloatAmplitude * Mathf.Sin(FloatSpeed * Time.time), initialPosition.z);
    }
}
