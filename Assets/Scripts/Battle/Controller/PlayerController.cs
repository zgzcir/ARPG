using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Controller
{
    private Vector3 camOffSet;

    private float targetBlend;
    private float currentBlend;

    private float currentVelocity;

    public Transform RayCastPoint;

    public GameObject CameraPivot; //改成transform
    public Transform ChaCameraRotatePivot;
    [FormerlySerializedAs("FX1")] public GameObject Sk1FX;
    public GameObject Sk2FX;
    public GameObject Sk3FX;

    public GameObject NormalAtkFx1;
    public GameObject NormalAtkFx2;
    public GameObject NormalAtkFx3;
    public GameObject NormalAtkFx4;
    public GameObject NormalAtkFx5;

//    private Vector2 inputDir;
//    public Vector2 InputDir
//    {
//        private get => inputDir;
//        set
//        {
//            isMove = value != Vector2.zero;
//            inputDir = value;
//        }
//        
//    }

    private float targetRotation;


    private bool isJump = false;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private static readonly int IsJump = Animator.StringToHash("IsJump");


    private float margin = 0.1f;

    private bool IsGrounded()
    {
        return Physics.Raycast(RayCastPoint.position, -Vector3.up, margin);
    }

    private bool isGrounded = true;

    private void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
    }

    public override void Init()
    {
        base.Init();
        timerSvc = TimerSvc.Instance;
        if (Camera.main != null) camTrans = Camera.main.transform;
        cameraController = camTrans.GetComponent<CameraController>();
        if (Sk1FX != null)
        {
            fxDic.Add(Sk1FX.name, Sk1FX);
        }

        if (Sk2FX != null)
        {
            fxDic.Add(Sk2FX.name, Sk2FX);
        }


        if (Sk3FX != null)
        {
            fxDic.Add(Sk3FX.name, Sk3FX);
        }

        if (NormalAtkFx1 != null)
        {
            fxDic.Add(NormalAtkFx1.name, NormalAtkFx1);
        }

        if (NormalAtkFx2 != null)
        {
            fxDic.Add(NormalAtkFx2.name, NormalAtkFx2);
        }

        if (NormalAtkFx3 != null)
        {
            fxDic.Add(NormalAtkFx3.name, NormalAtkFx3);
        }

        if (NormalAtkFx4 != null)
        {
            fxDic.Add(NormalAtkFx4.name, NormalAtkFx4);
        }

        if (NormalAtkFx5 != null)
        {
            fxDic.Add(NormalAtkFx5.name, NormalAtkFx5);
        }
    }

    private void Update()
    {
        #region kb input

        /*
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //******************************************/ //////////////////////////////////////////
////        float u = 0;
////      
////
////            if (CharacterController.isGrounded)
////            {
////                if (Input.GetKeyDown(PlayerCfg.Jump))
////                {                    cameraController.SetJumpState();
////                    u = Constans.PlayerJumpHeight;
////                    Ani.SetBool(IsJump, true);
////                }
////                else
////                {
////                    Ani.SetBool(IsJump, false);
////                    cameraController.SetJumpState(false);
////
////                }
////            }
////        
////        u -= Constans.Gravity * Time.deltaTime;
        //******************************************///////////////////////////////////////////

        //todo
        //   Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //   InputDir = input.normalized;


        if (isMove)
        {
            if (MainCitySys.Instance.IsNavigate)
            {
                MainCitySys.Instance.CancelNavGuide();
            }

            SetBlend(Constans.BlendMove);
            SetDir();
            SetMove();
        }
        else
        {
            SetBlend(Constans.BlendIdle);
        }

        #endregion

        if (MainCitySys.Instance != null && MainCitySys.Instance.IsNavigate)
        {
            SetBlend(Constans.BlendMove);
        }


        if (!currentBlend.Equals(targetBlend))
        {
            UpdateMixBlend();
        }

        if (SkillMove)
        {
            SetSkillMove();
        }
    }

    private void SetSkillMove()
    {
        CharacterController.Move(Time.deltaTime * SkillMoveSpeed * transform.forward);
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

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    public override void SetFX(string name, float duration)
    {
        GameObject go;
        if (fxDic.TryGetValue(name, out go))
        {
            go.SetActive(true);
            timerSvc.AddTimeTask(tid => { go.SetActive(false); }, duration);
        }
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