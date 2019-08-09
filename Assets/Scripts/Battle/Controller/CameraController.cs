using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float yaw;
    private float pitch;
    public float MouseMoveSpeed;
    public Transform Target;
    public float camLen = 2;
    private Vector3 targetEuler;
    private bool canRotate = true;
    private Vector3 camMovePos;
    private Vector3 startJumpPos;
    private bool isJumpState;

    private void LateUpdate()
    {
        /*
         *相机旋转
        if (canRotate)
        {
            yaw += Input.GetAxis("Mouse X") * Constans.CamRotateSpeed;
            pitch -= Input.GetAxis("Mouse Y") * Constans.CamRotateSpeed;
            pitch = pitch < Constans.CamClampDown ? Constans.CamClampDown : pitch;
            pitch = pitch > Constans.CamClampUp ? Constans.CamClampUp : pitch;
            targetEuler = new Vector3(pitch, yaw, 0);
            transform.eulerAngles = targetEuler;
        }*/
//if(Target!=null)
        camMovePos = Target.position - transform.forward * camLen;
        if (isJumpState)
        {
            var pos = camMovePos;
//            camMovePos = Vector3.Lerp(transform.position, pos, 0.01f);
        }
        transform.position = camMovePos;
    }
    public void SetJumpState(bool can = true)
    {
        isJumpState = can;
        if (can)
        {
            startJumpPos = Target.position;
        }
    }
    public void SetRotateState(bool can = true)
    {
        canRotate = can;
    }

    public void SetCameFollow()
    {
 
    }
}