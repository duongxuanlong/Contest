using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = Constant.CAMERA_HALF_HEIGHT;
        Constant.CAMERA_HALF_WIDTH = (Constant.CAMERA_HALF_HEIGHT * Screen.width) / Screen.height;

        // Debug.Log("Camera width: " + Constant.CAMERA_HALF_WIDTH + " and height: " + Constant.CAMERA_HALF_HEIGHT);

        float delta = 2f;
        Constant.CAMERA_UP_BOUND = Constant.CAMERA_HALF_HEIGHT + delta;
        Constant.CAMERA_DOWN_BOUND = -Constant.CAMERA_HALF_HEIGHT - delta;
        Constant.CAMERA_LEFT_BOUND = -Constant.CAMERA_HALF_WIDTH - delta;
        Constant.CAMERA_RIGHT_BOUND = Constant.CAMERA_HALF_WIDTH + delta;

        // Debug.Log("Camera Left: " + Constant.CAMERA_LEFT_BOUND + " , right: " + Constant.CAMERA_RIGHT_BOUND
        //         + " up: " + Constant.CAMERA_UP_BOUND + " down: " + Constant.CAMERA_DOWN_BOUND);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
