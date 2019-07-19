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



    protected override void OnOpen()
    {
        base.OnOpen();
        RegisterUIEvents();
        PlayerOprateSys.Instance.OnSwitchChaInfoPanel(true);
        FreshPanel();
    }

    protected override void OnClose()
    {
        base.OnClose();
        PlayerOprateSys.Instance.OnSwitchChaInfoPanel(false);
        PlayerOprateSys.Instance.ResetChaCameraTrans();
    }
    private void FreshPanel()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(TxtInfo, pd.level + " " + pd.name);
        SetText(TxtExp, pd.exp);
        SetText(TxtJob, pd.hp);
        SetText(TxtHp, pd.hp);
        SetText(TxtPd, pd.pd);
        SetText(TxtSd, pd.sd);
        SetText(TxtPa, pd.pa);
        SetText(TxtSa, pd.sa);
        SetText(TxtDs, pd.pierce);
        //dodge
        //critical
    }
    protected override void RegisterUIEvents()
    {
        BtnClose.onClick.AddListener(() => { PlayerOprateSys.Instance.SwitchPanel(this); });
        OnClickDown(ImgCha.gameObject, (eventData) => { startPos = eventData.position; });
        OnClickDrag(ImgCha.gameObject, evtData =>
        {
            float rotate = evtData.position.x - startPos.x;
            PlayerOprateSys.Instance.SetChaCameraRotate(rotate * Time.deltaTime * Constans.ChaInFoPanelRotateSpeed);
        });


    }
}