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
        // ���� â ����� �� TextInput â�� ���� �ݿ���Ű�� ���� �߰� SetKeyCode() ��� 
    }
}
