using System;
 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 
 public class LodingPanel : BasePanel
 {
     public Text txtTips;
     public Image imgFG;
     public Image imgPoint;
     public Text txtPrg;
 
     private float fgWidth;
 
     protected override void OnOpen()
     {
         base.OnOpen();
         //  fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
         float globalRate = 1.0f * Constans.ScreenStandardHeight / Screen.height;
         fgWidth = Screen.width * globalRate;
        // fgWidth = Screen.width;
         SetText(txtPrg, "0%");
         SetText(txtTips, "loading...");
         imgFG.fillAmount = 0;
         imgPoint.transform.localPosition = new Vector3(-fgWidth / 2, 0, 0);
     }
 
     public void SerProgress(float prg)
     {
         SetText(txtPrg, (int) (prg * 100) + "%");
         imgFG.fillAmount = prg;
         float posX = prg * fgWidth - (fgWidth / 2);
         imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
     }
 }
 
