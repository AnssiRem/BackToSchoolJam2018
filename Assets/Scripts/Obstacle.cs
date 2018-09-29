using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Start()
    {
        float size=Random.Range(0.75f,1f);

        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), Random.Range(0, 45));
        transform.localScale = new Vector3(size, Random.Range(0.4f,1f), size);
    }
}
