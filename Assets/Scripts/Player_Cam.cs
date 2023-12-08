using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam : MonoBehaviour
{
    public float XCam;
    public float YCam;

    public Transform orientation;

    public float RotaX;
    public float RotaY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XCam;
        float mY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YCam;

        RotaY += mX;
        RotaX -= mY;
        RotaX = Mathf.Clamp(RotaX, -90f, 90f);

        transform.rotation = Quaternion.Euler(RotaX, RotaY, 0);
        orientation.rotation = Quaternion.Euler(0,RotaY, 0);
    }
}
