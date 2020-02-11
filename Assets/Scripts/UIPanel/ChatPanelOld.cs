using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    public Text txtWorld;
    public Text txtGuild;
    public Text txtFriend;

    public Text txtChat;

    public InputField iptChat;
    private int chatType;
    private List<string> chatList = new List<string>();

    private bool canSend = true;

    protected override void OnOpen()
    {
        base.OnOpen();
        chatType = 0;
        FreshPanel();
    }

    private void FreshPanel()
    {
        switch (chatType)
        {
            case 0:
                AssemblyChatMsg(chatType);
                SetTextColor(txtWorld, Constans.ChatPanelTypeSelected);
                SetTextColor(txtGuild, Constans.ChatPanelTypeNonSelected);
                SetTextColor(txtFriend, Constans.ChatPanelTypeNonSelected);
                break;
            case 1:
                SetText(txtChat, "当前未加入公会");
                SetTextColor(txtWorld, Constans.ChatPanelTypeNonSelected);
                SetTextColor(txtGuild, Constans.ChatPanelTypeSelected);
                SetTextColor(txtFriend, Constans.ChatPanelTypeNonSelected);
                break;
            case 2:
                SetText(txtChat, "暂无好友信息");
                SetTextColor(txtWorld, Constans.ChatPanelTypeNonSelected);
                SetTextColor(txtGuild, Constans.ChatPanelTypeNonSelected);
                SetTextColor(txtFriend, Constans.ChatPanelTypeSelected);
                break;
        }
    }

    public void OnCLickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        chatType = 0;
        FreshPanel();
    }

    public void OnCLickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        chatType = 1;
        FreshPanel();
    }

    public void OnCLickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constans.UIClickBtn);
        chatType = 2;
        FreshPanel();
    }


    protected override void RegisterUIEvents()
    {
        iptChat.onEndEdit.AddListener(s =>
            {
                if (!canSend)
                {
                    GameRoot.AddTips("频率过高，请稍后");
                    return;
                }

                {
                    if (iptChat.text != null && !string.IsNullOrEmpty(iptChat.text))
                    {
                        if (iptChat.text.Length > 12)
                        {
                            GameRoot.AddTips("more than 12");
                        }
                        else
                        {
                            netSvc.SendMsg(new GameMsg
                            {
                                cmd = (int) CMD.SndChat,
                                SndChat = new SndChat()
                                {
                                    msg = s
                                }
                            });
                            iptChat.text = "";
                            canSend = false;
                            timerSvc.AddTimeTask(tid => { canSend = true; }, 1.0f, TimeUnit.Second);
                        }
                    }
                }
            }
        );
    }

//    IEnumerator MsgTimer()
//    {
//        yield return new  WaitForSeconds(1.0f);
//        canSend = true;
//    }
    private void AssemblyChatMsg(int type)
    {
        StringBuilder chatMsgBuilder = new StringBuilder();
        for (int i = 0; i < chatList.Count; i++)
        {
            chatMsgBuilder.Append(chatList[i]);
            chatMsgBuilder.Append("\n");
        }

        SetText(txtChat, chatMsgBuilder.ToString());
    }

    public void AddChatMsg(GameMsg msg)
    {
        var data = msg.PshChat;
        chatList.Add(Constans.Color(data.name + ":", TxtColor.Blue) + data.msg);
        if (chatList.Count > 9)
        {
            chatList.RemoveAt(0);
        }

        if (IsOpen)
        {
            FreshPanel();
        }
    }
}