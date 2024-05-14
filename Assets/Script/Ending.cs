using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    void Start() {
        UIManager.Instance.isEnding = true;
    }

    public void ReStart() {
        // ���ξ����� �ٽ���ȯ
        GameManager.instance.GameReset();
        GameManager.instance.playerPrefab.SetActive(true);
        GameManager.instance.gameObject.SetActive(true);
        UIManager.Instance.gameUI.gameObject.SetActive(true);
        LodingScene.LoadScene(0);
    }
}
