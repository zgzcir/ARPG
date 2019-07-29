using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : BasePanel
{
    private PlayerData pd;
    public Button[] btnArr;
    public Transform pointerTrans;

    protected override void OnOpen()
    {
        base.OnOpen();
        pd = GameRoot.Instance.PlayerData;
        FreshUIPanel();
        AdjustToTop();
    }

    public void FreshUIPanel()
    {
        int mission = pd.Mission;
        for (int i = 0; i < btnArr.Length; i++)
        {
            if (i <= mission % 1000 - 1)
            {
                SetActive(btnArr[i].gameObject);
                if (i == mission % 1000 - 1)
                {
                    pointerTrans.SetParent(btnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(0, -350, 0);
                }
            }
            else
            {
                SetActive(btnArr[i].gameObject, false);
            }
        }
    }

    public void OnMissionBtnClick(int mid)
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        int power = resSvc.GetMapCfgData(mid).Power;
        if (power > pd.Power)
        {
            GameRoot.AddTips("体力值不足");
        }
        else
        {
            netSvc.SendMsg(new GameMsg()
            {
                cmd = (int) CMD.ReqMission,
                ReqMission = new ReqMission()
                {
                    MID = mid
                }
            });
        }
    }
}