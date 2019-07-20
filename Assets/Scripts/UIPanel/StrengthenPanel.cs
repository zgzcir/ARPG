using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StrengthenPanel : HandleablePanel
{
    public Image ImgCurPos;
    public Text TxtStarLv;
    public Transform StarTransGrp;
    public Text TxtHp1;
    public Text TxtHurt1;
    public Text TxtDef1;
    public Text TxtHp2;
    public Text TxtHurt2;
    public Text TxtDef2;
    public Text TxtNeedLv;
    public Text TxtCostCoin;

    public Text TxtCostCrystal;

    public Text TxtCoin;

    public Transform TransCost;
    [FormerlySerializedAs("ButtonStren")] public Button BtnStren;

    public Transform PosBtnTrans;
    public Image[] ImgPoss;
    private int currentIndex;
    private PlayerData playerData;
    private StrengthenCfg nextSc;

    public override void Init()
    {
        base.Init();
        playerData = GameRoot.Instance.PlayerData;
        RegisterUIEvents();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
 
        ClickPosItem(0);
    }

    public void FreshPanel()
    {
        playerData = GameRoot.Instance.PlayerData;

        SetText(TxtCoin, playerData.coin);
        switch (currentIndex)
        {
            case 0:
                SetSprite(ImgCurPos, PathDefine.Head);
                break;
            case 1:
                SetSprite(ImgCurPos, PathDefine.Body);
                break;
            case 2:
                SetSprite(ImgCurPos, PathDefine.Belt);
                break;
            case 3:
                SetSprite(ImgCurPos, PathDefine.Hand);
                break;
            case 4:
                SetSprite(ImgCurPos, PathDefine.Leg);
                break;
            case 5:
                SetSprite(ImgCurPos, PathDefine.Foot);
                break;
        }

        SetText(TxtStarLv, playerData.strenarr[currentIndex] + "星");
        int curStarlv = playerData.strenarr[currentIndex];
        for (int i = 0; i < StarTransGrp.childCount; i++)
        {
            Image image = StarTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curStarlv)
            {
                SetImageColor(image, Constans.StrenStarLightColor);
            }
            else
            {
                SetImageColor(image, Constans.StrenStarDarkColor);
            }
        }

        int nextStarLv = curStarlv + 1;
        int sumAddHP = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 2);
        int sumAddDefen = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 3);

        SetText(TxtHp1, "生命+" + sumAddHP);
        SetText(TxtHurt1, "伤害+" + sumAddHurt);
        SetText(TxtDef1, "防御+" + sumAddDefen);

        int nextStarlv = curStarlv + 1;
        nextSc = resSvc.GetStrengthenCfg(currentIndex, nextStarlv);
        if (nextSc != null)
        {
            SetActive(TxtHp2);
            SetActive(TxtHurt2);
            SetActive(TxtDef2);
            SetActive(TransCost);
            SetActive(BtnStren.gameObject);

            SetText(TxtHp2, "强化后+" + nextSc.addhp);
            SetText(TxtHurt2, "          +" + nextSc.addhurt);
            SetText(TxtDef2, "          +" + nextSc.adddef);
            SetText(TxtNeedLv, nextSc.minlv);
            SetText(TxtCostCoin, nextSc.coin);
            SetText(TxtCostCrystal, nextSc.crystal + "/" + playerData.crystal);
        }
        else
        {
            SetActive(TxtHp2, false);
            SetActive(TxtHurt2, false);
            SetActive(TxtDef2, false);
            SetActive(TransCost, false);
            SetActive(BtnStren.gameObject, false);
        }
    }


    protected override void RegisterUIEvents()
    {
        ImgPoss = new Image[PosBtnTrans.childCount];
        for (int i = 0; i < PosBtnTrans.childCount; i++)
        {
            Image img = PosBtnTrans.GetChild(i).GetComponent<Image>();

            OnClick(img.gameObject, args =>
            {
                audioSvc.PlayUIAudio(Constans.UIClickBtn);
                ClickPosItem((int) args);
            }, i);
            ImgPoss[i] = img;
        }
    }

    private void ClickPosItem(int index)
    {
        currentIndex = index;
        for (int i = 0; i < ImgPoss.Length; i++)
        {
            Transform trans = ImgPoss[i].transform;
            if (i == currentIndex)
            {
                SetSprite(ImgPoss[i], PathDefine.StrItemSelected);
                trans.localPosition = new Vector3(20, trans.localPosition.y, 0);
//                trans.GetComponent<RectTransform>().sizeDelta=new  Vector2(100,100);
            }
            else
            {
                SetSprite(ImgPoss[i], PathDefine.StrItemNonSelected);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
            }
        }

        FreshPanel();
    }

    public void ClickStrenButton()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        if (playerData.strenarr[currentIndex] < 10)
        {
            if (playerData.level < nextSc.minlv)
            {
                GameRoot.AddTips("角色等级不足");
                return;
            }

            if (playerData.crystal < nextSc.crystal)
            {
                GameRoot.AddTips("水晶不足");
                return;
            }

            if (playerData.coin < nextSc.coin)
            {
                GameRoot.AddTips("金币不足");
                return;
            }

            netSvc.SendMsg(new GameMsg()
            {
                cmd = (int) CMD.ReqStrengthen,
                ReqStrengthen = new ReqStrengthen()
                {
                    pos = currentIndex
                }
            });
        }
        else
        {
            GameRoot.AddTips("满级啦");
        }
    }
}