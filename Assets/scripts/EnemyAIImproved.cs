using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 1. Importante: Necesario para usar NavMeshAgent

public class EnemyAIImproved : MonoBehaviour
{
    // --- Variables Públicas (Configuración en Inspector) ---
    public EnemyVision vision;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float stoppingDistance = 1f; // Distancia para considerar "llegado" al destino o al jugador

    // --- Variables Privadas (Internas) ---
    private NavMeshAgent agent; // 2. Referencia al nuevo componente NavMeshAgent
    private int currentPoint = 0;

    // 3. Máquina de Estados: Usamos un Enum para definir los comportamientos
    public enum AIState { Patrol, ChasePlayer }
    private AIState currentState = AIState.Patrol; // Empezamos patrullando

    void Start()
    {
        // Obtener el componente NavMeshAgent en lugar del CharacterController
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("EnemyAIImproved requiere un componente NavMeshAgent!");
            enabled = false; // Desactiva el script si falta el componente
            return;
        }

        // Configurar la distancia de frenado del agente automáticamente
        agent.stoppingDistance = stoppingDistance;

        // Iniciar la primera acción de patrulla
        GoToNextPatrolPoint();
    }

    void Update()
    {
        // 4. Lógica principal que cambia de estado y ejecuta el comportamiento actual
        switch (currentState)
        {
            case AIState.Patrol:
                HandlePatrolState();
                break;

            case AIState.ChasePlayer:
                HandleChaseState();
                break;
        }
    }

    // --- Métodos de Gestión de Estados ---

    private void HandlePatrolState()
    {
        // Si el agente ha llegado cerca de su destino actual:
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            GoToNextPatrolPoint();
        }

        // Condición para CAMBIAR de estado: ¿Ve al jugador?
        if (vision.CanSeePlayer())
        {
            SetState(AIState.ChasePlayer);
        }
    }

    private void HandleChaseState()
    {
        // Perseguir al jugador constantemente
        agent.SetDestination(vision.player.position);

        // Condición para CAMBIAR de estado: ¿Perdió de vista al jugador?
        // Añadir lógica para atacar si está muy cerca (por ejemplo, stoppingDistance se encargaría de esto)
        if (!vision.CanSeePlayer())
        {
            SetState(AIState.Patrol);
            // Cuando volvemos a patrullar, necesitamos darle un destino inmediatamente
            GoToNextPatrolPoint();
        }
    }

    // --- Métodos Auxiliares ---

    private void SetState(AIState newState)
    {
        currentState = newState;
        // Ajustar la velocidad al cambiar de estado
        if (newState == AIState.Patrol)
        {
            agent.speed = patrolSpeed;
        }
        else if (newState == AIState.ChasePlayer)
        {
            agent.speed = chaseSpeed;
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        // Asignar el destino al NavMeshAgent (él se encarga del movimiento real)
        agent.SetDestination(patrolPoints[currentPoint].position);

        // Mover al siguiente punto en la lista (circularmente)
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}
