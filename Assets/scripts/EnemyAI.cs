using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyVision vision;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float rotationSpeed = 5f;

    private CharacterController controller;
    private int currentPoint = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (vision.CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPoint];
        MoveTowards(target.position, patrolSpeed);

        // Cambiar de punto al llegar
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        MoveTowards(vision.player.position, chaseSpeed);
    }

    void MoveTowards(Vector3 targetPos, float speed)
    {
        // dirección
        Vector3 direction = (targetPos - transform.position).normalized;

        // rotación suave
        Quaternion lookRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

        // movimiento con CharacterController
        Vector3 move = transform.forward * speed;
        move.y -= 2f; // gravedad ligera

        controller.Move(move * Time.deltaTime);
    }
}
