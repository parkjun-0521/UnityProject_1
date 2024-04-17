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
        // 씬이 로드될 때마다 호출되는 이벤트 핸들러
        // 이름이 objectName인 게임 오브젝트를 찾아서 Awake 메서드를 호출한다.
        GameObject obj = GameObject.Find(objectName);
        if (obj != null) {
            obj.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
