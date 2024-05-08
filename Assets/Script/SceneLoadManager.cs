using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    public int stageCount = 0;
    public int mapCount = 0;


    public Camera mainCamera;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void BasicRoom() {
        int random = Random.Range(1, 7);
        mainCamera.GetComponent<Camera>().orthographicSize = 7;

        LodingScene.LoadScene(random);
        Debug.Log("일반 맵");
    }
    public void ShopRoom() {
        int random = Random.Range(7, 9);
        mainCamera.GetComponent<Camera>().orthographicSize = 7;
        mainCamera.GetComponent<CameraManager>().mapSize = new Vector2(27, 15);
        mainCamera.GetComponent<CameraManager>().center = new Vector2(0, 7);

        LodingScene.LoadScene(random);
        Debug.Log("상점");
    }
    public void MiddleBossRoom() {
        mainCamera.GetComponent<Camera>().orthographicSize = 8.5f;
        mainCamera.GetComponent<CameraManager>().mapSize = new Vector2(21.5f, 10);
        mainCamera.GetComponent<CameraManager>().center = new Vector2(0, 3);
        LodingScene.LoadScene(9);
        Debug.Log("중간보스");
    }
    public void BossRoom() {
        mainCamera.GetComponent<Camera>().orthographicSize = 8.5f;
        mainCamera.GetComponent<CameraManager>().mapSize = new Vector2(21.5f, 10);
        mainCamera.GetComponent<CameraManager>().center = new Vector2(0, 3);
        int sceneNum = mapCount / stageCount;

        LodingScene.LoadScene(sceneNum);
        Debug.Log("보스");
    }
    
}
