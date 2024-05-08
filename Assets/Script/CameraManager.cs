using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    public Transform playerTransform;
    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    public Vector2 center;
    [SerializeField]
    public Vector2 mapSize;

    [SerializeField]
    float cameraMoveSpeed;
    float height;
    float width;

    public int cameraID;

    void Start() {
        DontDestroyOnLoad(gameObject);

        height = Camera.main.orthographicSize;              // 카메라의 세로 크기를 가져옴 
        width = height * Screen.width / Screen.height;      // 카메라를 같은 비율로 지정 ( 가로 세로의 픽셀수를 나누어 비율을 계산하고 높이를 곱해서 높이와 같은 비율로 맞춘다.)
    }

    void Update() {
        if (playerTransform != null && cameraID == 1)
            LimitCameraArea();
    }

    void FixedUpdate() {

        if( playerTransform != null && cameraID == 0)
            LimitCameraArea();
    }

    void LimitCameraArea() {
        // 카메라 이동 
        // Lerp(a,b,t) : a+(a−b) ∗ t : a 위치에서 b 위치로 t 비율만큼의 선형 보간
        if (cameraID == 0) {
            transform.position = Vector3.Lerp(transform.position,
                                              playerTransform.position + cameraPosition,
                                              Time.deltaTime * cameraMoveSpeed);

            // 변수가 일정한 값을 벗어나지 못하도록 범위를 제한하는 Mathf.Clamp() 함수
            // float Mathf.Lerp(float value, float min, float max);
            float lx = mapSize.x - width;
            float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

            float ly = mapSize.y - height;
            float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
        else if(cameraID == 1) {
            transform.position = new Vector3( playerTransform.position.x , playerTransform.position.y, -10);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
