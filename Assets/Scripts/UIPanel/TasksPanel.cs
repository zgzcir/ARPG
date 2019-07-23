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
        FreshUI();
    }

    private void FreshUI()
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
                Taked = taskInfoArr[2].Equals("1")
            };
            if (trd.Taked)
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
            GameObject go = resSvc.LoadPrefab(PathDefine.TaskRewardItem);
            go.transform.SetParent(TaskRewardItemParent);
            go.transform.localPosition = Vector3.zero;
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
            Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
            btnTake.onClick.AddListener(() => 
            {
              Debug.Log(go.name);
            });
        }
    }

//    public void OnNavButtonClick()
//    {
//        if (curMainLineData != null)
//        {
//            PlayerOprateSys.Instance.NavGuide(curMainLineData);
//        }
//    }

    private void ClickTakeBtn()
    {
        
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