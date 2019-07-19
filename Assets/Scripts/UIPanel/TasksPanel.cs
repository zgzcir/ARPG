using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksPanel : HandleablePanel
{
    public Transform TaskItemParent;
    private List<TaskItem> taItems = new List<TaskItem>();

    private GuideCfg curMainLineData;

    protected override void OnOpen()
    {
        base.OnOpen();
        FreshUI();
    }

    private void FreshUI()
    {
        curMainLineData = resSvc.GetGuideCfg(GameRoot.Instance.PlayerData.guideid);
        if (curMainLineData != null)
        {
            //TODOself
            if (taItems.Count >= 1)
            {
                return;
            }
            SetTaskBar(curMainLineData);
        }
    }

    public void OnNavButtonClick()
    {
        if (curMainLineData != null)
        {
            PlayerOprateSys.Instance.NavGuide(curMainLineData);
        }
    }

    private void SetTaskBar(GuideCfg guideCfg)
    {
        TaskItem taskItem = Instantiate(resSvc.LoadItem<TaskItem>(PathDefine.TaskItem), TaskItemParent, true);
//        taskItem.SetItem(guideCfg);
        taskItem.GetComponent<Button>().onClick.AddListener(() =>
        {
//            FreshTargetTask(taskItem.TaskID);
        });
        TaskItemParent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(0, taskItem.GetComponent<RectTransform>().sizeDelta.y + 3);
        taItems.Add(taskItem);
    }
}