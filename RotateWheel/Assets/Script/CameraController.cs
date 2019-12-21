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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
