using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float rotate;
    float votate;
    float left;
    float forward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotate = Input.GetAxis("Mouse X");
        votate = Input.GetAxis("Mouse Y");
        left = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");
        transform.RotateAround(transform.position, Vector3.up, rotate * 720 * Time.deltaTime);
        transform.Rotate(Vector3.right, votate * -720 * Time.deltaTime);
        transform.Translate(Vector3.forward * 15f * forward * Time.deltaTime);
        transform.Translate(Vector3.right * 15f * left * Time.deltaTime);
    }
}
