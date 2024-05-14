using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    void Start() {
        UIManager.Instance.isEnding = true;
    }

    public void ReStart() {
        // 메인씬으로 다시전환
        GameManager.instance.GameReset();
        GameManager.instance.playerPrefab.SetActive(true);
        GameManager.instance.gameObject.SetActive(true);
        UIManager.Instance.gameUI.gameObject.SetActive(true);
        LodingScene.LoadScene(0);
    }
}
