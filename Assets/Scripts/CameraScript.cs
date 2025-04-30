using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        Vector3 position = transform.position;
        position.x = Player.transform.position.x + 5;
        position.y = Player.transform.position.y + 4;
        transform.position = position;
    }
}
