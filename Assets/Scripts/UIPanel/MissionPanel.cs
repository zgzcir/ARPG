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
            if (i <= mission % 100 - 1)
            {
                SetActive(btnArr[i].gameObject);
                if (i == mission % 100 - 1)
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
        
        
    }
}