using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed = 180f;
    public float moveSpeed = 5f;

    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();  
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);

        // Movimiento local → mundo
        Vector3 movement = new Vector3(0f, 0f, vertical * moveSpeed);
        movement = transform.TransformDirection(movement) * Time.deltaTime;

        // Mover con CharacterController
        controller.Move(movement);
    }
}

