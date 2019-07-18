using System.Collections;
 using System.Collections.Generic;
 using Protocol;
 using UnityEngine;
 using UnityEngine.UI;
 
 public class LoginPanel : BasePanel
 {
     public InputField iptAcct;
     public InputField iptPass;
     public Button btnEnter;
 
     protected override void OnOpen()
     {
         base.OnOpen();
         if (PlayerPrefs.HasKey("acct") && PlayerPrefs.HasKey("pass"))
         {
             iptAcct.text = PlayerPrefs.GetString("acct");
             iptPass.text = PlayerPrefs.GetString("pass");
         }
         else
         {
             iptAcct.text = "";
             iptPass.text = "";
         }
     }
 
     public void ClickEnterButton()
     {
         audioSvc.PlayUIAudio(Constans.UILoginBtn);
         string acct = iptAcct.text;
         string pass = iptPass.text;
         if (!string.IsNullOrEmpty(acct) && !string.IsNullOrEmpty(pass))
         {
             PlayerPrefs.SetString("acct", acct);
             PlayerPrefs.SetString("pass", pass);
 
             GameMsg msg = new GameMsg()
             {
                 cmd = (int) CMD.ReqLogin,
                 ReqLogin = new ReqLogin()
                 {
                     acct = acct,
                     pass = pass
                 }
             };
             netSvc.SendMsg(msg);
         }
         else
         {
             GameRoot.AddTips("账号密码不能为空");
         }
     }
 }