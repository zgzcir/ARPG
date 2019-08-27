
    using System;
    using UnityEngine;

    public class MonsterController:Controller
    {
        private void Update()
        {
            if (isMove)
            {
                SetDir();
                SetMove();
            }
            
        }
        private void SetDir()
        {
        
            float angle = Vector2.SignedAngle(InputDir, new Vector2(0, 1)) ;
            Vector3 eulerAngles = new Vector3(0, angle, 0);
            transform.localEulerAngles = eulerAngles;
        
//        targetRotation = Mathf.Atan2(InputDir.x, InputDir.y) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
//        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
//                                    ref currentVelocity, Constans.RotateSmooth);
        }
        private void SetMove()
        {
            CharacterController.Move(Time.deltaTime * Constans.MonsterMoveSpeed * transform.forward);
            CharacterController.Move(Time.deltaTime * Constans.MonsterMoveSpeed * Vector3.down);
        }

    }
