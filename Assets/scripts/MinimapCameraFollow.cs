using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 50f, 0f); // altura sobre el jugador

    void LateUpdate()
    {
        if (player == null) return;
        Vector3 newPos = player.position + offset;
        transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // apuntando hacia abajo
    }
}
