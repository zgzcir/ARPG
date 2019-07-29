using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChaInfoPanel : HandleablePanel
{
    [FormerlySerializedAs("ImgChar")] public RawImage ImgCha;

    public Text TxtInfo;
    public Text TxtExp;
    public Image ImgExpPrg;


    public Text TxtJob;
    public Text TxtHp;
    public Text TxtPd;
    public Text TxtSd;
    public Text TxtPa;
    public Text TxtSa;
    public Text TxtDs;

    public Button BtnClose;

    private Vector2 startPos;
    private Vector2 endPos;


    public override void Init()
    {
        base.Init();
        RegisterUIEvents();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        MainCitySys.Instance.OnSwitchChaInfoPanel(true);
        FreshPanel();
    }

    protected override void OnClose()
    {
        base.OnClose();
        MainCitySys.Instance.OnSwitchChaInfoPanel(false);
        MainCitySys.Instance.ResetChaCameraTrans();
    }
    public void FreshPanel()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(TxtInfo, pd.Level + " " + pd.Name);
        SetText(TxtExp, pd.Exp);
        SetText(TxtJob, pd.HP);
        SetText(TxtHp, pd.HP);
        SetText(TxtPd, pd.PD);
        SetText(TxtSd, pd.SD);
        SetText(TxtPa, pd.PA);
        SetText(TxtSa, pd.SA);
        SetText(TxtDs, pd.Pierce);
        //dodge
        //critical
    }
    protected override void RegisterUIEvents()
    {
        OnClickDown(ImgCha.gameObject, (eventData) => { startPos = eventData.position; });
        OnClickDrag(ImgCha.gameObject, evtData =>
        {
            float rotate = evtData.position.x - startPos.x;
            MainCitySys.Instance.SetChaCameraRotate(rotate * Time.deltaTime * Constans.ChaInFoPanelRotateSpeed);
        });


    }
}