using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : BaseSystem
{

    
    public static BattleSys Instance;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("BattleSys Connected");
    }

    public void EntoBattle(int mapID)
    {
        GameObject go=new GameObject()
        {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Instance.transform);

        BattleManager battleManager = go.AddComponent<BattleManager>();
        battleManager.InitManager(mapID);
    }

}
