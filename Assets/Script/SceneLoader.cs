using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    string objectName = "GameManager";

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded( Scene scene, LoadSceneMode mode ) {
        // ���� �ε�� ������ ȣ��Ǵ� �̺�Ʈ �ڵ鷯
        // �̸��� objectName�� ���� ������Ʈ�� ã�Ƽ� Awake �޼��带 ȣ���Ѵ�.
        GameObject obj = GameObject.Find(objectName);
        if (obj != null) {
            obj.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
