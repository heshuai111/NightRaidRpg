using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class fpCamera : MonoBehaviour
{
    public Vector2 mouselooKSensitivity = new Vector2(5, 5);
    public Vector2 rotationXLimit = new Vector2(-87, 87);
    public Vector2 rotationYLimit = new Vector2(-360, 360);
    public Vector3 positionOffset = new Vector3(0,2, -0.2f);
    private Vector2 currentMouseLook = Vector2.zero;
    private float x_Angle = 0;
    private float y_Angle = 0;
    private fps_playerParameter1 parmeter;
    private Transform m_Transform;
     void Start()
    {
        parmeter = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<fps_playerParameter1>();
        m_Transform = transform;
      //  m_Transform.localPosition = positionOffset;
    }
     void Update()
    {
        UpdateInput();

    }

     void LateUpdate()
    {
        Quaternion XQuaternion = Quaternion.AngleAxis(y_Angle, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(0, Vector3.left);
        m_Transform.parent.rotation = XQuaternion * yQuaternion;

        yQuaternion = Quaternion.AngleAxis(-x_Angle, Vector3.left);
        m_Transform.rotation = XQuaternion * yQuaternion;
    }  
    private void UpdateInput()
    {if (parmeter.inputSmoothlook == Vector2.zero)
            return;
        if (parmeter.inputView)
        {
            GetMouseLook();
            y_Angle += currentMouseLook.x;
            x_Angle += currentMouseLook.y;
            y_Angle = y_Angle < -360 ? y_Angle += 360 : y_Angle;
            y_Angle = y_Angle > 360 ? y_Angle -= 360 : y_Angle;
            y_Angle = Mathf.Clamp(y_Angle, rotationYLimit.x, rotationYLimit.y);
            x_Angle = x_Angle < -360 ? x_Angle += 360 : x_Angle;
            x_Angle = x_Angle > 360 ? x_Angle -= 360 : x_Angle;
            x_Angle = Mathf.Clamp(x_Angle, rotationXLimit.x, rotationXLimit.y);
        }


    }
    private void GetMouseLook()
    {
        currentMouseLook.x = parmeter.inputSmoothlook.x;
        currentMouseLook.y = parmeter.inputSmoothlook.y;
        currentMouseLook.x *= mouselooKSensitivity.x;
        currentMouseLook.y *= mouselooKSensitivity.y;
        currentMouseLook.y *= -1;


    }
}
