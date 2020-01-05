using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogPanel : HandleablePanel
{
    public Text txtDialog;
    public GameObject partClick;

    private GuideCfg curTaskData;
    private string[] dialogArr;
    private int index;
    private int maxIndex;

    private PlayerData pd;


    protected override void OnOpen()
    {
        base.OnOpen();
        RegisterUIEvents();
        pd = GameRoot.Instance.PlayerData;
        curTaskData = MainSys.Instance.GetCurGuideData();
        dialogArr = curTaskData.DialoArr.Split('#');
        maxIndex = dialogArr.Length - 1;
        index = 1;
        SetDialogContent();
    }


    private void SetDialogContent()
    {
        string[] arr = dialogArr[index].Split('|');
        SetText(txtDialog, arr[1].Replace("$name", pd.Name));
        //设置头像使用pathDefine  
        if (arr[0] == "0")
        {
            txtDialog.color = Color.yellow;
        }
        else
        {
            txtDialog.color = Color.white;
        }
    }

    protected override void RegisterUIEvents()
    {
        OnClickDown(partClick, evtData =>
        {
            if (index < maxIndex)
            {
                index++;
                SetDialogContent();
            }
            else if (index == maxIndex)
            {
                GameMsg msg = new GameMsg()
                {
                    cmd = (int) CMD.ReqGuide,
                    ReqGuide = new ReqGuide()
                    {
                        id = curTaskData.ID
                    }
                };
                netSvc.SendMsg(msg);
                MainSys.Instance.SwitchPanel(this);
            }
        });
    }
}