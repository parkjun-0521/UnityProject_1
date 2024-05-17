using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeyboardManager : MonoBehaviour
{
    public static GameKeyboardManager instance;

    public enum KeyCodeTypes {
        LeftMove,
        RightMove,
        Jump,
        Attack,
        Dash,
        Interaction,
        Pickup
    }

    private Dictionary<KeyCodeTypes, KeyCode> keyMappings;

    void Awake() {
        instance = this;

        keyMappings = new Dictionary<KeyCodeTypes, KeyCode>();

        keyMappings[KeyCodeTypes.LeftMove] = KeyCode.LeftArrow;
        keyMappings[KeyCodeTypes.RightMove] = KeyCode.RightArrow;
        keyMappings[KeyCodeTypes.Jump] = KeyCode.X;
        keyMappings[KeyCodeTypes.Attack] = KeyCode.Z;
        keyMappings[KeyCodeTypes.Dash] = KeyCode.C;
        keyMappings[KeyCodeTypes.Interaction] = KeyCode.V;
        keyMappings[KeyCodeTypes.Pickup] = KeyCode.F;
    }

    public KeyCode GetKeyCode( KeyCodeTypes action ) {
        return keyMappings[action];
    }

    public void SetKeyCode( KeyCodeTypes action, KeyCode keyCode ) {
        keyMappings[action] = keyCode;
    }

    void Update() {
        // 설정 창 띄웠을 때 TextInput 창의 값을 반영시키는 로직 추가 SetKeyCode() 사용 
    }
}
