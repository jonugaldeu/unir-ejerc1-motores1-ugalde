using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public EnemyPatrol patrolScript;
    public EnemyVision visionScript;

    public Transform player;
    public float chaseSpeed = 4f;
    public float chaseRotationSpeed = 6f;

    private bool chasing = false;

    void Update()
    {
        if (visionScript.CanSeePlayer())
        {
            chasing = true;
            patrolScript.enabled = false;
        }
        else if (chasing)
        {
            // Si deja de verlo, sigue 3 segundos y luego vuelve a patrullar
            Invoke(nameof(ReturnToPatrol), 3f);
        }

        if (chasing)
            ChasePlayer();
    }

    void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            chaseRotationSpeed * Time.deltaTime
        );

        transform.position += transform.forward * chaseSpeed * Time.deltaTime;
    }

    void ReturnToPatrol()
    {
        chasing = false;
        patrolScript.enabled = true;
    }
}
