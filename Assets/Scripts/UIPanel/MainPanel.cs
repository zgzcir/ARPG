using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel

{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirHandle;
    public Text txtLv;
    public Image imgHp;
    public Text txtHp;
    public Text txtName;
    public Transform expPrgsTrans;

    private Vector2 startPos;
    private Vector2 defaultPos;
    private float pointDis;

    protected override void OnOpen()
    {
        base.OnOpen();
        pointDis = Screen.height * 1.0f / Constans.ScreenStandardHeight * Constans.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirHandle, false);
        RegisterUIEvents();
        FreshPanel();
    }


    public void FreshPanel()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        float totalHp = playerData.hp;
        float nowHp = playerData.hp - 50.36f;
        SetText(txtHp, (nowHp / totalHp).ToString("P"));
        imgHp.fillAmount = nowHp / totalHp;
        float posX = -(imgHp.fillAmount * imgHp.GetComponent<RectTransform>().sizeDelta.x);
        txtHp.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        SetText(txtLv, "Lv:" + playerData.level);
        SetText(txtName, playerData.name);
        //exp
        float expPrgVal = playerData.exp * 1.0f / CommonTool.GetExpUpValue(playerData.level) * 100;
        int index = (int) expPrgVal / 10;
        for (int i = 0; i < expPrgsTrans.childCount; i++)
        {
            Image img = expPrgsTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0.01f;
            }
        }

        float globalRate = 1.0f * Constans.ScreenStandardHeight / Screen.height;
        float width = Screen.width * globalRate;
        GridLayoutGroup gridLayoutGroup = expPrgsTrans.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2((width - 10 * 9) / 10f, 18);
    }
    protected override void RegisterUIEvents()
    {
        OnClickDown(imgTouch.gameObject, eventDate =>
        {
            startPos = eventDate.position;
            SetActive(imgDirHandle);
            imgDirBg.transform.position = eventDate.position;
        });
        OnCLickUp(imgTouch.gameObject, eventData =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirHandle, false);
            imgDirHandle.transform.localPosition = Vector2.zero;
            PlayerOprateSys.Instance.SetPlayerMoveMobile(Vector2.zero);
        });
        OnClickDrag(imgTouch.gameObject, eventData =>
        {
            Vector2 dir = eventData.position - startPos;
            float len = dir.magnitude;
            Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
            imgDirHandle.transform.position = startPos + clampDir;
            PlayerOprateSys.Instance.SetPlayerMoveMobile(dir);
        });
    }
}