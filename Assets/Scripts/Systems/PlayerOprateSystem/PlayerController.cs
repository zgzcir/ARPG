using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Transform camTrans;
    private CameraController cameraController;
    private Vector3 camOffSet;
    [FormerlySerializedAs("Animator")] public Animator Ani;
    public CharacterController CharacterController;

    private float targetBlend;
    private float currentBlend;

    private float currentVelocity;

    public Transform RayCastPoint;
    
    private Vector2 inputDir;
    public GameObject CameraPivot;//改成transform
    public Transform ChaCameraRotatePivot;
    public Vector2 InputDir
    {
        get => inputDir;
        set
        {
            isMove = value != Vector2.zero;
            inputDir = value;
        }
    }
    private float targetRotation;
    private bool isMove;


    private bool isJump = false;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private static readonly int IsJump = Animator.StringToHash("IsJump");

    public void Init()
    {
        if (Camera.main != null) camTrans = Camera.main.transform;
        cameraController = camTrans.GetComponent<CameraController>();
    }
    private float margin = 0.1f;    
    private bool IsGrounded()
    {
        return Physics.Raycast(RayCastPoint.position, -Vector3.up,  margin);    
    }

    private bool isGrounded=true;
    private void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
   
    }

    private void Update()
    {
        Debug.DrawRay(RayCastPoint.position,-Vector3.up.normalized*10,Color.blue);
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //******************************************///////////////////////////////////////////
        float u = 0;
      

            if (CharacterController.isGrounded)
            {
                if (Input.GetKeyDown(PlayerCfg.Jump))
                {                    cameraController.SetJumpState();
                    u = Constans.PlayerJumpHeight;
                    Ani.SetBool(IsJump, true);
                }
                else
                {
                    Ani.SetBool(IsJump, false);
                    cameraController.SetJumpState(false);

                }
            }
        
        u -= Constans.Gravity * Time.deltaTime;
        //******************************************///////////////////////////////////////////
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        InputDir  = input.normalized;
        if (isMove)
        {
            if (PlayerOprateSys.Instance.IsNavigate)
            {
             PlayerOprateSys.Instance.CancelNavGuide();
            }
            SetBlend(Constans.BlendRun);
            SetDir();
            SetMove();
        }
        else
        {
            SetBlend(Constans.BlendIdle);
        }
        if (PlayerOprateSys.Instance.IsNavigate)
        {
            SetBlend(Constans.BlendRun);
        }
        else
        {
            CharacterController.Move(new Vector3(0,u*Time.deltaTime,0));
        }
        if (!currentBlend.Equals(targetBlend))
        {
            UpdateMixBlend();
        }
    
    }

    private void SetDir()
    {
        targetRotation = Mathf.Atan2(InputDir.x, InputDir.y) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                    ref currentVelocity, Constans.RotateSmooth);
    }

    private void SetMove()
    {
        CharacterController.Move(Time.deltaTime * Constans.PLyerMoveSpeed * transform.forward);
    }

    private void SetJump()
    {
        CharacterController.Move(Time.deltaTime * Constans.PlayerJumpHeight * transform.up);
    }

    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constans.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constans.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constans.AccelerSpeed * Time.deltaTime;
        }

        Ani.SetFloat(Blend, currentBlend);
    }
}