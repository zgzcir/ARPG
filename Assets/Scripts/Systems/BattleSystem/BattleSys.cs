using UnityEngine;
using UnityEngine.Serialization;

public class BattleSys : BaseSystem
{
    public static BattleSys Instance;

    public BattleManager BattleManager;
    [FormerlySerializedAs("BattlePanel")] public BattleControllPanel battleControllPanel;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("BattleSys Connected");
    }

    public void EntoBattle(int mapID)
    {
        var go = new GameObject
        {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Instance.transform);
        BattleManager = go.AddComponent<BattleManager>();
        BattleManager.InitManager(mapID);
    }
    public void SetBattlePanelState(bool isActive = true)
    {
        battleControllPanel.SetPanelState(isActive);
    }
    public void SetSelfPlayerMoveMobileDir(Vector2 dir)
    {
        BattleManager.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        BattleManager.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return battleControllPanel.currentDir;
    }
    
    
}