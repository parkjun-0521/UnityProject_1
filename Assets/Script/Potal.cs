using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    bool playerCheck = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && playerCheck) {
            Debug.Log("���� ������ �̵��մϴ�.");
            SceneManager.LoadScene(1);
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if( collision.CompareTag("Player") ) {
            Debug.Log("�÷��̾�");
            playerCheck = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            playerCheck = false;
        }
    }
}
