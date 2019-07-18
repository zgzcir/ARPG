using PENet;
 using Protocol;
 
 public class ClientSession : PESession<GameMsg>
 {
     protected override void OnConnected()
     {
         GameRoot.AddTips("已连接到服务器");
     }
 
     protected override void OnReciveMsg(GameMsg msg)
     {
         CommonTool.Log(((CMD) msg.cmd).ToString());
         NetSvc.Instance.AddNetMsg(msg);
     }
 
     protected override void OnDisConnected()
     {
         GameRoot.AddTips("已断开与服务器的连接");
     }
 }