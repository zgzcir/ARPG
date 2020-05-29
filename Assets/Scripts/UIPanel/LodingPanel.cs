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
    public Slider sliProgress;
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

    public void SetProgress(float targetPrg)
    {
        SetText(txtPrg, (int) (targetPrg * 100) + "%");
        if (sliProgress.value<targetPrg)
        {
            sliProgress.value += 0.01f;
        }
        //         float posX = prg * fgWidth - (fgWidth / 2);
//         imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }



}