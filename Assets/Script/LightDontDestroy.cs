using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDontDestroy : MonoBehaviour
{
    void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
