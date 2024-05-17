using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemThrowing : MonoBehaviour
{
    public Image itemIconImage;

    public void Throwing() {
        if(itemIconImage.sprite == null) {
            return;
        }
        string name = itemIconImage.sprite.name;
        Debug.Log(int.Parse(name));

        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        GameManager gameManagerLogic = GameManager.instance.GetComponent<GameManager>();
        
        // 능력치 감소 
        for (int i = 0; i < gameManagerLogic.itemStatus.Count; i++) {
            if (gameManagerLogic.itemStatus[i][0] == int.Parse(name)) {
                playerLogic.itemSumHealth -= gameManagerLogic.itemStatus[i][2];
                playerLogic.itemSumSpeed -= gameManagerLogic.itemStatus[i][3];
                playerLogic.itemSumPower -= gameManagerLogic.itemStatus[i][4];

                if (playerLogic.health > playerLogic.maxHealth)
                    playerLogic.health = playerLogic.maxHealth;
                else
                    playerLogic.health -= gameManagerLogic.itemStatus[i][2];

                // 리스트에 있는 특정 모든 원소 삭제 
                gameManagerLogic.setItem.Remove((int)gameManagerLogic.itemStatus[i][1]);
                gameManagerLogic.itemStatus.RemoveAt(i);
                break;
            }
        }
        // 셋트 옵션 재적용을 위한 초기화 
        playerLogic.maxHealth = playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth;
        playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed;
        playerLogic.power = playerLogic.upPower + playerLogic.itemSumPower;
        playerLogic.itemSetHealth = 0;
        playerLogic.itemSetSpeed = 0;
        playerLogic.itemSetPower = 0;

        // 버린 아이템 드랍 
        GameObject throwingItem = gameManagerLogic.poolManager.GetObject(int.Parse(name) + 11);
        throwingItem.transform.position = playerLogic.transform.position;
        // 아이템 스크립트 가져오기 
        ItemManager itemManagerLogic = throwingItem.GetComponent<ItemManager>();
        // 셋트옵션 지정 
        itemManagerLogic.SetItemOption();
        itemManagerLogic.isThrowing = true;

        // 아이템 UI이 갱신
        gameManagerLogic.itemID.Remove(int.Parse(name));
        UIManager.Instance.PlayerStatusUI();
             
        itemIconImage.sprite = null;
    }

    public void ItemSell() {
        if (itemIconImage.sprite == null) {
            return;
        }
        string name = itemIconImage.sprite.name;
        Debug.Log(int.Parse(name));

        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        GameManager gameManagerLogic = GameManager.instance.GetComponent<GameManager>();

        // 능력치 감소 
        for (int i = 0; i < gameManagerLogic.itemStatus.Count; i++) {
            if (gameManagerLogic.itemStatus[i][0] == int.Parse(name)) {
                playerLogic.itemSumHealth -= gameManagerLogic.itemStatus[i][2];
                playerLogic.itemSumSpeed -= gameManagerLogic.itemStatus[i][3];
                playerLogic.itemSumPower -= gameManagerLogic.itemStatus[i][4];

                if (playerLogic.health > playerLogic.maxHealth)
                    playerLogic.health = playerLogic.maxHealth;
                else
                    playerLogic.health -= gameManagerLogic.itemStatus[i][2];

                // 리스트에 있는 특정 모든 원소 삭제 
                gameManagerLogic.setItem.Remove((int)gameManagerLogic.itemStatus[i][1]);
                gameManagerLogic.itemStatus.RemoveAt(i);
                break;
            }
        }
        // 셋트 옵션 재적용을 위한 초기화 
        playerLogic.maxHealth = playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth;
        playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed;
        playerLogic.power = playerLogic.upPower + playerLogic.itemSumPower;
        playerLogic.itemSetHealth = 0;
        playerLogic.itemSetSpeed = 0;
        playerLogic.itemSetPower = 0;

        // 버린 아이템 드랍 
        GameObject throwingItem = gameManagerLogic.poolManager.GetObject(int.Parse(name) + 11);
        throwingItem.transform.position = playerLogic.transform.position;

        // 아이템 스크립트 가져오기 
        ItemManager itemManagerLogic = throwingItem.GetComponent<ItemManager>();

        // 셋트옵션 지정 
        itemManagerLogic.SetItemOption();
        itemManagerLogic.isThrowing = true;

        // 판매 이후 Coin 증가
        GameManager.instance.coinValue += itemManagerLogic.upCost;

        // 능력치 지정후 삭제 
        throwingItem.SetActive(false);

        // 아이템 UI이 갱신
        gameManagerLogic.itemID.Remove(int.Parse(name));
        UIManager.Instance.PlayerStatusUI();

        itemIconImage.sprite = null;
    }
}
