using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 3f;
    public float rotationSpeed = 5f;

    private int currentPoint = 0;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPoint];

        Vector3 direction = (target.position - transform.position).normalized;

        // Rotación suave hacia el punto
        Quaternion look = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSpeed * Time.deltaTime);

        // Movimiento con colisiones
        Vector3 movement = transform.forward * speed;

        controller.Move(movement * Time.deltaTime);

        // Cambio de punto
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }
}

