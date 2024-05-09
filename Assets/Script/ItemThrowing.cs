using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class ItemThrowing : MonoBehaviour
{
    /*
     * ȹ���� �������� ������ �������°� ������� ( GameManager.instance.itemStatus �� �ش� �������� Ű���� �ɷ�ġ�� �����ϵ��� ���� ) 
     * 
     * �ؾ��ϴ� �� 
     * ��Ʈ �޴��� �缳�� �ؾ��ϹǷ� SetItemOption() �Լ��� �����ؾ��� 
        public Dictionary<int, List<float>> setItemInfo;        // ui�� ���� ���� �� 
        �̰͵鿡�� �����ۿ� �ش��ϴ� ���� �����ؾ���  
     */
    public Image itemIconImage;

    public void Throwing() {
        if(itemIconImage.sprite == null) {
            return;
        }
        string name = itemIconImage.sprite.name;
        Debug.Log(int.Parse(name));

        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        GameManager gameManagerLogic = GameManager.instance.GetComponent<GameManager>();
        
        // �ɷ�ġ ���� 
        for (int i = 0; i < gameManagerLogic.itemStatus.Count; i++) {
            if (gameManagerLogic.itemStatus[i][0] == int.Parse(name)) {
                playerLogic.itemSumHealth -= gameManagerLogic.itemStatus[i][2];
                playerLogic.itemSumSpeed -= gameManagerLogic.itemStatus[i][3];
                playerLogic.itemSumPower -= gameManagerLogic.itemStatus[i][4];

                if (playerLogic.health > playerLogic.maxHealth)
                    playerLogic.health = playerLogic.maxHealth;
                else
                    playerLogic.health -= gameManagerLogic.itemStatus[i][2];

                // ����Ʈ�� �ִ� Ư�� ��� ���� ���� 
                gameManagerLogic.setItem.Remove((int)gameManagerLogic.itemStatus[i][1]);
                gameManagerLogic.itemStatus.RemoveAt(i);
                break;
            }
        }
        // ��Ʈ �ɼ� �������� ���� �ʱ�ȭ 
        playerLogic.maxHealth = playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth;
        playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed;
        playerLogic.power = playerLogic.upPower + playerLogic.itemSumPower;
        playerLogic.itemSetHealth = 0;
        playerLogic.itemSetSpeed = 0;
        playerLogic.itemSetPower = 0;

        // ���� ������ ��� 
        GameObject throwingItem = gameManagerLogic.poolManager.GetObject(int.Parse(name) + 11);
        throwingItem.transform.position = playerLogic.transform.position;
        // ������ ��ũ��Ʈ �������� 
        ItemManager itemManagerLogic = throwingItem.GetComponent<ItemManager>();
        // ��Ʈ�ɼ� ���� 
        itemManagerLogic.SetItemOption();
        itemManagerLogic.isThrowing = true;

        // ������ UI�� ����
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

        // �ɷ�ġ ���� 
        for (int i = 0; i < gameManagerLogic.itemStatus.Count; i++) {
            if (gameManagerLogic.itemStatus[i][0] == int.Parse(name)) {
                playerLogic.itemSumHealth -= gameManagerLogic.itemStatus[i][2];
                playerLogic.itemSumSpeed -= gameManagerLogic.itemStatus[i][3];
                playerLogic.itemSumPower -= gameManagerLogic.itemStatus[i][4];

                if (playerLogic.health > playerLogic.maxHealth)
                    playerLogic.health = playerLogic.maxHealth;
                else
                    playerLogic.health -= gameManagerLogic.itemStatus[i][2];

                // ����Ʈ�� �ִ� Ư�� ��� ���� ���� 
                gameManagerLogic.setItem.Remove((int)gameManagerLogic.itemStatus[i][1]);
                gameManagerLogic.itemStatus.RemoveAt(i);
                break;
            }
        }
        // ��Ʈ �ɼ� �������� ���� �ʱ�ȭ 
        playerLogic.maxHealth = playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth;
        playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed;
        playerLogic.power = playerLogic.upPower + playerLogic.itemSumPower;
        playerLogic.itemSetHealth = 0;
        playerLogic.itemSetSpeed = 0;
        playerLogic.itemSetPower = 0;

        // ���� ������ ��� 
        GameObject throwingItem = gameManagerLogic.poolManager.GetObject(int.Parse(name) + 11);
        throwingItem.transform.position = playerLogic.transform.position;
        // ������ ��ũ��Ʈ �������� 
        ItemManager itemManagerLogic = throwingItem.GetComponent<ItemManager>();
        // ��Ʈ�ɼ� ���� 
        itemManagerLogic.SetItemOption();
        itemManagerLogic.isThrowing = true;

        // �Ǹ� ���� Coin ����
        GameManager.instance.coinValue += itemManagerLogic.upCost;

        // �ɷ�ġ ������ ���� 
        throwingItem.SetActive(false);

        // ������ UI�� ����
        gameManagerLogic.itemID.Remove(int.Parse(name));
        UIManager.Instance.PlayerStatusUI();

        itemIconImage.sprite = null;
    }
}
