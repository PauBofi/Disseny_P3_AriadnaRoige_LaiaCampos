using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    // Límites del mapa
    public float minX, maxX, minY, maxY;

    // Tamaño ortográfico de la cámara
    private float halfHeight;
    private float halfWidth;
    public Collider2D mapBounds;

    void Start()
    {
        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = cam.aspect * halfHeight;

        Bounds bounds = mapBounds.bounds;
        minX = bounds.min.x;
        maxX = bounds.max.x;
        minY = bounds.min.y;
        maxY = bounds.max.y;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            float clampedX = Mathf.Clamp(player.position.x, minX + halfWidth, maxX - halfWidth);
            float clampedY = Mathf.Clamp(player.position.y, minY + halfHeight, maxY - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
