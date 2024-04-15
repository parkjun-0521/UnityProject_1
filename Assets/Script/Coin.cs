using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue;

    Rigidbody2D rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable() 
    {
        // 위로 랜덤한 힘을 줘서 튀어오르는 것 같이 보이게 ( 위 + 좌우 ) 
        int rendom = Random.Range(0, 2);
        float renPower = Random.Range(0f, 3f);
        Vector2 power = (rendom == 0) ? Vector2.left * renPower : Vector2.right * renPower;
        rigid.AddForce(Vector2.up * 5 + power, ForceMode2D.Impulse);
    }


    void Update()
    {
        CoinDrop();
        StartCoroutine(CoinFollow());
    }

    void CoinDrop() {
        if (rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 0.1f, LayerMask.GetMask("Floor"));
            if (hit.collider != null) {
                if (hit.collider.tag == "Floor") {
                    Debug.Log("떨어짐");
                    rigid.velocity = Vector3.zero;
                    rigid.gravityScale = 0;
                }
            }
        }
    }


    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            GameManager.instance.coinValue += this.coinValue;
            gameObject.SetActive(false);
        }
    }

    IEnumerator CoinFollow() {
        yield return new WaitForSeconds(1f);
        RaycastHit2D objectHitTag = Physics2D.CircleCast(transform.position, 20, Vector2.zero, 0f, LayerMask.GetMask("Player"));
        if (objectHitTag.collider != null) {
            Vector2 direction = (GameManager.instance.playerPrefab.transform.position + Vector3.up * 1.25f - transform.position).normalized;
            Debug.Log(direction);
            // 이동 방향으로 이동합니다.
            transform.Translate(direction * 10f * Time.deltaTime);
        }
    }
}
