using UnityEngine;

public class RotateMe : MonoBehaviour
{
    private float rotationInc;

    void Start()
    {
        rotationInc = 150.0f;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationInc * Time.deltaTime);
    }
}
