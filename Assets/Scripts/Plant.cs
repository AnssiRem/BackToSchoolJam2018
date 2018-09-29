using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float GrowthSpeed;

    private float randomSize;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), Random.Range(0, 15)));

        randomSize = Random.Range(0.20f, 0.30f);
    }

    private void Update()
    {
        if (transform.localScale.x < randomSize)
        {
            transform.localScale += new Vector3(GrowthSpeed * Time.deltaTime, GrowthSpeed * Time.deltaTime * 8, GrowthSpeed * Time.deltaTime);
        }
    }
}
