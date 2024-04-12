using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    public int stageCount = 0;
    public int mapCount = 0;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void BasicRoom() {
        int random = Random.Range(1, 7);
        SceneManager.LoadScene(random);
        Debug.Log("�Ϲ� ��");
    }
    public void ShopRoom() {
        int random = Random.Range(7, 9);
        SceneManager.LoadScene(random);
        Debug.Log("����");
    }
    public void MiddleBossRoom() {
        SceneManager.LoadScene(9);
        Debug.Log("�߰�����");
    }
    public void BossRoom() {
        int sceneNum = mapCount / stageCount;
        SceneManager.LoadScene(sceneNum);
        Debug.Log("����");
    }
    
}
