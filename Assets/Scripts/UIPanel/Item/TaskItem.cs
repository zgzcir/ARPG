using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaskItem : BaseItem
{
    public int TaskID;
 public Text txtTaskName;
//    public void SetItem(GuideCfg guideCfg)
//    {
//        TaskID = guideCfg.ID;
//
//        Color color;
////        switch (TaskType)
////        {
////            case TaskType.MainLine:
////                txtTaskName.color = ColorUtility.TryParseHtmlString(Constans.MainlineTaskColor, out color)
////                    ? color
////                    : Color.yellow;
////                break;
////            case TaskType.SideLine:
////                txtTaskName.color = ColorUtility.TryParseHtmlString(Constans.SidelineTaskColor, out color)
////                    ? color
////                    : Color.yellow;
////                break;
////        }
//
//    }
}