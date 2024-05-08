using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LodingScene : MonoBehaviour
{
    public static int nextScene;
    float timer = 0.0f;
    [SerializeField] Slider progressBar;

    private void Start() {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene( int sceneIndex ) {
        nextScene = sceneIndex;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene() {
        // ºñµ¿±â ½ÄÀ¸·Î ¾ÀÀ» ºÒ·¯¿Í¼­ ¶ç¿öÁÜ 
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        
        while (!op.isDone) {
            timer += Time.time;
            progressBar.value = timer/ 10f;

            if(timer > 10) {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
