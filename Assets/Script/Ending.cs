using UnityEngine;

public class Ending : MonoBehaviour
{
    void Start() {
        UIManager.Instance.isEnding = true;
    }

    public void ReStart() {
        // Player ���� �ʱ�ȭ 
        GameManager.instance.GameReset();
        
        // ��Ȱ��ȭ�� UI �ٽ� Ȱ��ȭ 
        GameManager.instance.playerPrefab.SetActive(true);
        GameManager.instance.gameObject.SetActive(true);
        UIManager.Instance.gameUI.gameObject.SetActive(true);
        
        // ���ξ����� �ٽ���ȯ
        LodingScene.LoadScene(0);
    }
}
