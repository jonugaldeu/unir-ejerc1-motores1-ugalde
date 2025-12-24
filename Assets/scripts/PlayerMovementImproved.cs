using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementImproved : MonoBehaviour
{
    public float rotationSpeed = 180f;  // Velocidad de giro
    public float moveSpeed = 5f;        // Velocidad de movimiento
    public float gravity = 15f;         // Gravedad ajustada para mejor física (antes 9.8f)
    public float jumpForce = 6f;        // Fuerza del salto ajustada (antes 5f)
    public float groundedGracePeriod = 0.2f; // Margen de tiempo para pulsar salto antes/despues de aterrizar

    private CharacterController controller;
    private Vector3 velocity; // Usaremos un Vector3 para la velocidad completa (incluyendo Y)
    private float lastTimeGrounded = 0f; // Tiempo desde la última vez que tocó el suelo
    private bool jumpInputWasPressed = false; // ¿Pulsó salto recientemente?

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("PlayerMovementImproved requiere un componente CharacterController!");
        }
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleJumpAndGravity();

        // Aplicar movimiento final
        controller.Move(velocity * Time.deltaTime);

        // Limpiar el estado de la entrada de salto para el próximo frame
        jumpInputWasPressed = false;
    }

    private void HandleRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);
    }

    private void HandleMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(0f, 0f, verticalInput * moveSpeed);

        // Convertir a espacio global relativo al jugador
        Vector3 globalMove = transform.TransformDirection(moveDirection);

        // Mantenemos la velocidad vertical actual, solo ajustamos X y Z
        velocity.x = globalMove.x;
        velocity.z = globalMove.z;

        // Capturamos el input de salto tan pronto como ocurre
        if (Input.GetButtonDown("Jump"))
        {
            jumpInputWasPressed = true;
        }
    }

    private void HandleJumpAndGravity()
    {
        if (controller.isGrounded)
        {
            // Resetear la velocidad vertical si estamos en el suelo
            velocity.y = -2f;
            lastTimeGrounded = Time.time; // Registrar el momento en que tocamos el suelo
        }
        else
        {
            // Aplicar gravedad
            velocity.y -= gravity * Time.deltaTime;
        }

        // Lógica de Salto Robusto:
        // Permitimos el salto si (estamos en el suelo AHORA) 
        // O (si estuvimos en el suelo hace menos de 'groundedGracePeriod' segundos)
        bool canJump = controller.isGrounded || (Time.time - lastTimeGrounded <= groundedGracePeriod);

        if (jumpInputWasPressed && canJump)
        {
            velocity.y = jumpForce;
            // Opcional: Reiniciar el tiempo de gracia inmediatamente después del salto
            lastTimeGrounded = 0f;
        }
    }
}