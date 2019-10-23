using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public Image impPower;
    public Text txtPower;
    public Text txtConversation;

    private Vector2 startPos;
    private Vector2 defaultPos;
    private float pointDis;


    private int conversationIndex = 0;
    private int conversationIndexMax = 0;
    private List<string> conversations;


    public override void Init()
    {
        base.Init();
        RegisterUIEvents();
        transform.Find("lpart/btnConver").GetComponent<Button>().onClick.AddListener(SetConversation);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        pointDis = Screen.height * 1.0f / Constans.ScreenStandardHeight * Constans.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirHandle, false);
        //todo
        conversations = resSvc.GetConversationContens("chen", 1);
        conversationIndexMax = conversations.Count;
        SetConversation();
        FreshPanel();
    }


    public void FreshPanel()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        float totalHp = 800;
        int nowHp = playerData.HP;
//        SetText(txtHp, (nowHp / totalHp));
        SetText(txtHp, nowHp + "/" + totalHp);
        imgHp.fillAmount = nowHp / totalHp;
        var maxPower = CommonTool.GetPowerLimit(playerData.Level);
        impPower.fillAmount = 1.0f * playerData.Power / maxPower;
        SetText(txtPower, playerData.Power + "/" + maxPower);
        float posX = imgHp.fillAmount * imgHp.GetComponent<RectTransform>().sizeDelta.x;

        txtHp.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        SetText(txtLv, "Lv:" + playerData.Level);
        SetText(txtName, playerData.Name);
        //exp
        float expPrgVal = playerData.Exp * 1.0f / CommonTool.GetExpUpValue(playerData.Level) * 100;
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

//        float globalRate = 1.0f * Constans.ScreenStandardHeight / Screen.height;
        float width = Screen.width * Constans.GloableScreenRate;
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
            MainCitySys.Instance.SetPlayerMoveMobile(Vector2.zero);
        });
        OnClickDrag(imgTouch.gameObject, eventData =>
        {
            Vector2 dir = eventData.position - startPos;
            float len = dir.magnitude;
            Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
            imgDirHandle.transform.position = startPos + clampDir;
            MainCitySys.Instance.SetPlayerMoveMobile(dir);
        });
    }

    public void SetConversation()
    {

        int curIndex = conversationIndex;
        do
        {
            conversationIndex = Random.Range(0, conversationIndexMax);
        } 
        while (curIndex==conversationIndex);
        
        SetText(txtConversation, conversations[conversationIndex]);
    }
}
