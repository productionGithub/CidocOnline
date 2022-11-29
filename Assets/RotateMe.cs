using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    private float rotationInc;

    // Start is called before the first frame update
    void Start()
    {
        rotationInc = 150.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationInc * Time.deltaTime);
    }
}
