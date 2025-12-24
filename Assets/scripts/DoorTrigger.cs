using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorTrigger : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Jugador detectado! Intentando abrir la puerta."); // Añade esto
            animator.SetTrigger("Abrir");
        }
    }
}
