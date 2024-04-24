using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public enum PatternName{
        Rayser,
        Heal,
        LongDistanceAttack,
    }
    public PatternName patternName;

    public float rayserDamage;

    void Update() {
        if(patternName == PatternName.Heal) {
            StartCoroutine(HealDestroy());
        }    
    }

    IEnumerator HealDestroy() {
        yield return new WaitForSeconds(4.5f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Floor") && patternName == PatternName.Rayser) {
            gameObject.SetActive(false);
        }

        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player") && !playerLogic.isDashCheck && !playerLogic.isDamaged) {
            playerLogic.health -= rayserDamage;
            Debug.Log(playerLogic.health);
        }
    }
}
