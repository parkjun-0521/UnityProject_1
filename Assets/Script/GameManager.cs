using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public GameObject fireBallPrefab;

    public GameObject playerPrefab;

    public CinemachineVirtualCamera cameraPlayer;
    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    void Init() {
        GameObject playerObj = Instantiate(player, transform.position = new Vector2(-5, -5), Quaternion.Euler(0f, 0f, 0f));
        cameraPlayer.Follow = playerObj.transform;
        playerPrefab = playerObj;
    }
}
