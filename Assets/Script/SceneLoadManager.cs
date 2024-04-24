using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(random);
        Debug.Log("일반 맵");
    }
    public void ShopRoom() {
        int random = Random.Range(7, 9);
        SceneManager.LoadScene(random);
        Debug.Log("상점");
    }
    public void MiddleBossRoom() {
        mainCamera.GetComponent<Camera>().orthographicSize = 10;
        SceneManager.LoadScene(9);
        Debug.Log("중간보스");
    }
    public void BossRoom() {
        int sceneNum = mapCount / stageCount;
        SceneManager.LoadScene(sceneNum);
        Debug.Log("보스");
    }
    
}
