using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    // Asegúrate de arrastrar el objeto del jugador aquí en el Inspector de Unity
    public Transform player;
    public float viewDistance = 10f;
    [Tooltip("Ángulo total del cono de visión (60f significa 120 grados totales de visión)")]
    public float viewAngle = 60f;
    [Tooltip("La capa del jugador para hacer el Raycast más eficiente")]
    public LayerMask playerLayer;
    [Tooltip("Capas que deberían bloquear la visión (Muros, Obstáculos)")]
    public LayerMask obstructionLayers;

    // Opcional: Referencia a la posición de los "ojos" del enemigo
    // Si no se asigna, usará transform.position
    public Transform eyePosition;

    private void Start()
    {
        // Si no se asigna eyePosition, usa la posición del objeto
        if (eyePosition == null)
        {
            eyePosition = transform;
        }

        // Es vital que el jugador tenga el Tag "Player" y esté en la Layer "Player"
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned in EnemyVision!");
            this.enabled = false;
        }
    }

    public bool CanSeePlayer()
    {
        // 1. Comprobación de Distancia Básica
        float distanceToPlayer = Vector3.Distance(eyePosition.position, player.position);
        if (distanceToPlayer > viewDistance)
        {
            return false;
        }

        // 2. Comprobación de Ángulo de Visión (Cono de Visión)
        Vector3 directionToPlayer = (player.position - eyePosition.position).normalized;
        float angleToPlayer = Vector3.Angle(eyePosition.forward, directionToPlayer);

        if (angleToPlayer > viewAngle / 2f) // Dividimos el ángulo por 2 para que sea simétrico (60f significa 30º a izq y 30º a der)
        {
            return false;
        }

        // 3. Comprobación de Línea de Visión (Raycast)
        RaycastHit hit;
        // Lanzamos un rayo desde los ojos hacia el jugador
        if (Physics.Raycast(eyePosition.position, directionToPlayer, out hit, viewDistance))
        {
            // Verificamos si lo primero que golpea el rayo es el jugador
            // Usamos CompareTag que es más eficiente que comparar strings directamente
            if (hit.transform.CompareTag("Player"))
            {
                // ¡Vemos al jugador!
                return true;
            }
            else
            {
                // Golpeamos otra cosa (un muro, una caja), así que devolvemos false.
                return false;
            }
        }

        // Si el raycast no golpea nada dentro de la distancia de visión, 
        // algo va mal (quizás el jugador no tiene collider o está muy lejos),
        // pero la lógica anterior ya debería cubrir la mayoría de los casos.
        return false;
    }
}