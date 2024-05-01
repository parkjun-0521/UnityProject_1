using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UpFloor : MonoBehaviour
{
    PlatformEffector2D platformEffector;

    void Start() {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    void Update() {
        if (GameManager.instance.playerPrefab.GetComponent<Player>().downJump) {
            platformEffector.rotationalOffset = 180f;
        }
        else {
            platformEffector.rotationalOffset = 0f;
        }
            
    }
}
