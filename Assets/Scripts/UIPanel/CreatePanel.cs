using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : BasePanel
{
    public InputField iptName;
    public Button btnRd;
    public Button btnIn;
    public Dropdown dpdSex;

    protected override void OnOpen()
    {
        base.OnOpen();
    }


    public void ClickBtnRd()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        bool sex = dpdSex.value == 0 ? true : false;
        SetText(iptName, ResSvc.Instance.GetRDNameData(sex));
    }
    public void ClickInBtn()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);

        if (string.IsNullOrEmpty(iptName.text))
        {
            GameRoot.AddTips("名字不能为空");
        }
        else
        {
            GameMsg msg = new GameMsg
            {
                cmd = (int) CMD.ReqReName,
                ReqReName = new ReqReName
                {
                    Name = iptName.text
                } 
            };
            netSvc.SendMsg(msg);
        }
    }

    public void ValChanSexDpd()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
    }
}