using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;


    private StateManager stateManager;
    private SkillManager skillManager;
    private MapManager mapManager;

    public void InitManager(int mid)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        stateManager = gameObject.AddComponent<StateManager>();
        stateManager.InitManager();
        skillManager = gameObject.AddComponent<SkillManager>();
        skillManager.InitManager();
        MapCfg mapCfg = resSvc.GetMapCfgData(mid);
        resSvc.AsyncLoadScene(mapCfg.SceneName, () =>
            {
                GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
                mapManager = map.GetComponent<MapManager>();
                
                map.transform.localPosition = Vector3.zero;
                map.transform.localScale = Vector3.one;


                Camera.main.transform.position = mapCfg.MainCamPos;
                Camera.main.transform.localEulerAngles = mapCfg.MainCamRote;

                LoadPlayer(mapCfg);


                //       audioSvc.PlayBgAudio(Constans.BGCityHappy,true); 换个音乐
            }
        );


        Debug.Log("Init BattleManager Done");
    }

    private void LoadPlayer(MapCfg mapCfg)
    {
        
    }
}