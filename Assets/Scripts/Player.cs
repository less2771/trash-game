using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float rotateSpeed = 1f;

    public float sensitivity = 10000.0f; // Mouse rotation sensitivity.
    public float rotationY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;


        rotationY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }
}
