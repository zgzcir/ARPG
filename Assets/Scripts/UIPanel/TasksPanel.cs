using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TasksPanel : HandleablePanel
{
    [FormerlySerializedAs("TaskItemParent")]
    public Transform TaskRewardItemParent;

    private List<TaskItem> taItems = new List<TaskItem>();

    private List<TaskRewardData> trdList = new List<TaskRewardData>();

    private PlayerData pd;


    protected override void OnOpen()
    {
        base.OnOpen();
 
           pd = pd ?? GameRoot.Instance.PlayerData;
        FreshPanel();
    }

    public void FreshPanel()
    {
     
        trdList.Clear();
        List<TaskRewardData> todoLst = new List<TaskRewardData>();
        List<TaskRewardData> doneLst = new List<TaskRewardData>();
        for (int i = 0; i < pd.TaskRewardArr.Length; i++)
        {
            string[] taskInfoArr = pd.TaskRewardArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData()
            {
                id = int.Parse(taskInfoArr[0]),
                Prgs = int.Parse(taskInfoArr[1]),
                IsTaked = taskInfoArr[2].Equals("1")
            };
            if (trd.IsTaked)
            {
                doneLst.Add(trd);
            }
            else
            {
                todoLst.Add(trd);
            }
        }

        trdList.AddRange(todoLst);
        trdList.AddRange(doneLst);

        for (int i = 0; i < trdList.Count; i++)
        {
            GameObject go;

            if (TaskRewardItemParent.childCount > i + 1)
            {
                go = TaskRewardItemParent.GetChild(i + 1).gameObject;
            }
            else
            {
                go = resSvc.LoadPrefab(PathDefine.TaskRewardItem);
            }

            go.transform.SetParent(TaskRewardItemParent);
//            go.transform.localPosition = Vector3.zero;

               go.transform.localScale = Vector3.one;
            go.name = "taskItem_" + i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trf = resSvc.GetTaskRewardCfg(trd.id);


            SetText(GetTrans(go.transform, "txtName"), trf.TaskName);
            SetText(GetTrans(go.transform, "txtPrg"), trd.Prgs + "/" + trf.Count);
            SetText(GetTrans(go.transform, "txtExp"), "奖励经验：" + trf.Exp);
            SetText(GetTrans(go.transform, "txtCoin"), "奖励金币：" + trf.Coin);
            var prgVal = 1.0f * trd.Prgs / trf.Count;
            Image imgPrgVal = GetTrans(go.transform, "imgPrgBar/imgPrgVal").GetComponent<Image>();
            imgPrgVal.fillAmount = prgVal;
            GetTrans(go.transform, "txtTaken").gameObject.SetActive(trd.IsTaked);
            Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
            btnTake.interactable = trd.Prgs == trf.Count;
          btnTake.gameObject.SetActive(!trd.IsTaked);
            var i1 = i;
              btnTake.onClick.RemoveAllListeners(); 
            btnTake.onClick.AddListener(() =>
            {
                for (int j = 0; j < btnTake.onClick.GetPersistentEventCount(); j++)
                {
                    Debug.Log(btnTake.onClick.GetPersistentMethodName(j));
                }
     
                btnTake.interactable = false;
                netSvc.SendMsg(new GameMsg()
                {    
                    cmd = (int) CMD.ReqTakeTaskReward,
                    ReqTakeTaskReward = new ReqTakeTaskReward()
                    {
                        ID = trdList[i1].id
                    }
                });
//                TaskRewardCfg trc = resSvc.GetTaskRewardCfg(trdList[index].id);
//                int coin = trc.Coin;
//                int exp = trc.Exp;
//                GameRoot.AddTips(coin + "  " + exp);
            });
        }
    }

    private GuideCfg curGuideCfg;

    public void OnNavButtonClick()
    {
        curGuideCfg =  resSvc.GetGuideCfg(pd.GuideID);
        if (curGuideCfg != null)
        {
            PlayerOprateSys.Instance.NavGuide(curGuideCfg);
        }
    }



    private void SetTaskBar(GuideCfg guideCfg)
    {
        TaskItem taskItem =
            Instantiate(resSvc.LoadItem<TaskItem>(PathDefine.TaskRewardItem), TaskRewardItemParent, true);
//        taskItem.SetItem(guideCfg);
        taskItem.GetComponent<Button>().onClick.AddListener(() =>
        {
//            FreshTargetTask(taskItem.TaskID);
        });
        TaskRewardItemParent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(0, taskItem.GetComponent<RectTransform>().sizeDelta.y + 3);
        taItems.Add(taskItem);
    }
}