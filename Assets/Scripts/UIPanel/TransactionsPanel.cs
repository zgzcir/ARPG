using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class TransactionsPanel : BasePanel
{
    public Text txtInfo;
    private int type;
    private bool isSameType;
    public Button btnSure;
    public void SetType(int type)
    {
        if (this.type == type)
        {
            isSameType = true;
        }

        this.type = type;
    }



    protected override void OnOpen()
    {
        base.OnOpen();
        btnSure.interactable = true;
        AdjustToTop();
        FreshPanel();
    }

    private void FreshPanel()
    {
        switch (type)
        {
            case 0:
                SetText(txtInfo,
                    "是否花费" + Constans.Color("10钻石", TxtColor.Blue) + "购买" + Constans.Color("100体力", TxtColor.Green));
                break;
            case 1:
                SetText(txtInfo,
                    "是否花费" + Constans.Color("10钻石", TxtColor.Blue) + "购买" + Constans.Color("1000金币", TxtColor.Green));
                break;
        }
    }
    public void OnClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        btnSure.interactable = false;
        netSvc.SendMsg(new GameMsg()
        {
            cmd = (int) CMD.ReqTranscation,
            ReqTranscation = new ReqTranscation()
            {
                type = type,
                cost = 10
            }
        });
    }
}