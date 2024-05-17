using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameKeyboardManager;

public class Potal : MonoBehaviour
{
    public int potalID;
    bool isPlayerCheck = false;
    SceneLoadManager sceneManager;
    GameKeyboardManager keyboard;
    void Start() {
        sceneManager = SceneLoadManager.instance.gameObject.GetComponent<SceneLoadManager>();
        keyboard = GameKeyboardManager.instance.gameObject.GetComponent<GameKeyboardManager>();
    }

    void Update()
    {
        // 임시로 10스테이지 즉, 1-10 에서 엔딩  

        if ((Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Interaction))) && isPlayerCheck && sceneManager.mapCount == 10) {
            // 엔딩 
            sceneManager.Ending();
        }

        if ((Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Interaction))) && isPlayerCheck && sceneManager.mapCount < 10) {
            Debug.Log("다음 맵으로 이동합니다.");
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items) {
                item.SetActive(false);
            }
            GameManager.instance.potalID = this.potalID;
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
