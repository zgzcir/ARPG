using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntoSceneSys : BaseSystem
{
    public static EntoSceneSys Instance;
    public MainPanel MainPanel;
    private PlayerController PlayerController;
    private CameraController CameraController;
    public Transform[] npcPosTrans;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("EntoSceneSys Connected");
    }   

 
}