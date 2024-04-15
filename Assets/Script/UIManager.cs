using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinValue;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        coinValue.text = GameManager.instance.coinValue.ToString();
    }
}
