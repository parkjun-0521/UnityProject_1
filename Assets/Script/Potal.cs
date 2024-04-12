using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    bool isPlayerCheck = false;
    SceneLoadManager sceneManager;
    void Start() {
        sceneManager = SceneLoadManager.instance.gameObject.GetComponent<SceneLoadManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && isPlayerCheck) {
            Debug.Log("다음 맵으로 이동합니다.");
            int sceneCount = ++sceneManager.mapCount;
            switch (sceneCount % 10) {
                case 1:
                    sceneManager.stageCount++;
                    sceneManager.BasicRoom();
                    break;
                case 2:
                case 3:
                    sceneManager.BasicRoom();
                    break;
                case 4:
                    sceneManager.ShopRoom();
                    break;
                case 5:
                    sceneManager.MiddleBossRoom();
                    break;
                case 6:
                case 7:
                case 8:
                    sceneManager.BasicRoom();
                    break;
                case 9:
                    sceneManager.ShopRoom();
                    break;
                case 0:
                    sceneManager.BossRoom();
                break;
            }
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if( collision.CompareTag("Player") ) {
            Debug.Log("플레이어");
            isPlayerCheck = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = false;
        }
    }
}
