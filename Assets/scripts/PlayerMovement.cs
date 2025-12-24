using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed = 180f;  // Velocidad de giro
    public float moveSpeed = 5f;        // Velocidad de movimiento
    public float gravity = 9.8f;        // Gravedad
    public float jumpForce = 7f;        // Fuerza del salto

    private CharacterController controller;
    private float verticalVelocity = 0f; // Velocidad vertical para gravedad/salto

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- Giro ---
        float horizontal = Input.GetAxis("Horizontal");
        float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);

        // --- Movimiento ---
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(0f, 0f, verticalInput * moveSpeed);

        // Convertir a espacio global
        move = transform.TransformDirection(move);

        // --- Gravedad ---
        if (controller.isGrounded)
        {
            verticalVelocity = 0f; // Mantener pegado al suelo

            if (Input.GetButtonDown("Jump"))
                verticalVelocity = jumpForce; // Salto
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        // Aplicar movimiento
        controller.Move(move * Time.deltaTime);
    }
}

