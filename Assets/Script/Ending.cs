using UnityEngine;

public class Ending : MonoBehaviour
{
    void Start() {
        UIManager.Instance.isEnding = true;
    }

    public void ReStart() {
        // Player 변수 초기화 
        GameManager.instance.GameReset();
        
        // 비활성화된 UI 다시 활성화 
        GameManager.instance.playerPrefab.SetActive(true);
        GameManager.instance.gameObject.SetActive(true);
        UIManager.Instance.gameUI.gameObject.SetActive(true);
        
        // 메인씬으로 다시전환
        LodingScene.LoadScene(0);
    }
}
