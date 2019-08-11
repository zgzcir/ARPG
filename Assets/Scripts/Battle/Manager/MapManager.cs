using UnityEngine;
 
public class MapManager : MonoBehaviour
{
    private BattleManager battleManager;
    private int waveIndex = 1;
    public void InitManager(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        battleManager.LoadMonsterByWave(waveIndex);
        CommonTool.Log("Init MapManager Done");

    }
}