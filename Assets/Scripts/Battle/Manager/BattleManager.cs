using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;
    private int loadCount = 0;
    
    private MapCfg mapCfg;

    private StateManager stateManager;
    private SkillManager skillManager;
    private MapManager mapManager;


    private PlayerController playerController;
    private CameraController cameraController;
    [FormerlySerializedAs("entitySelfplayer")] public EntityPlayer EntitySelfplayer;


    private Dictionary<string, EntityMonster> monstersDic = new Dictionary<string, EntityMonster>();

    public void InitManager(int mid)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        stateManager = gameObject.AddComponent<StateManager>();
        stateManager.InitManager();
        skillManager = gameObject.AddComponent<SkillManager>();
        skillManager.InitManager();


        mapCfg = resSvc.GetMapCfgData(mid);
        resSvc.AsyncLoadScene(mapCfg.SceneName, () =>
            {
                GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
                mapManager = map.GetComponent<MapManager>();
                mapManager.InitManager(this);
                map.transform.localPosition = Vector3.zero;
                map.transform.localScale = Vector3.one;

                if (Camera.main != null) cameraController = Camera.main.GetComponent<CameraController>();
                cameraController.transform.position
                    = mapCfg.MainCamPos;
                cameraController.transform.localEulerAngles = mapCfg.MainCamRote;

                LoadPlayer(mapCfg);
                EntitySelfplayer.Idle();
                PlayerData pd = GameRoot.Instance.PlayerData;
                EntitySelfplayer.SetBattleProps(new BattleProps()
                {
                    HP = pd.HP,
                    PA = pd.PA,
                    PD = pd.PD,
                    SA = pd.SA,
                    SD = pd.SD,
                    Pierce = pd.Pierce,
                    Dodge = pd.Dodge,
                    Critical = pd.Critical
                });
                GameRoot.AddTips("四风试炼场");
                BattleSys.Instance.SetBattlePanelState();
                ActiveCurrentMonsters(); 
                //       audioSvc.PlayBgAudio(Constans.BGCityHappy,true);bgm
            }
        );

        Debug.Log("Init BattleManager Done");
    }
    private void Update()
    {

        foreach (var item in monstersDic)
        {
            EntityMonster em = item.Value;
            em.TickAILogic();
        }
    }
    private void LoadPlayer(MapCfg mapCfg)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.PlayerBattle);
        player.transform.localPosition = mapCfg.PlayerBornPos;
        player.transform.localEulerAngles = mapCfg.PlayerBornRote;
        player.transform.localScale = Vector3.one;

        //todo 2019.7.30 18:28
        playerController = player.GetComponent<PlayerController>();
        playerController.Init();

        EntitySelfplayer = new EntityPlayer()
        {
            StateManager = stateManager,
            SkillManager = skillManager,
            BattleManager = this
        };
EntitySelfplayer.SetCtrl(playerController);
        //ttt   
        cameraController.Target = playerController.CameraPivot.transform;
        cameraController.enabled = true;
        // 
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (!EntitySelfplayer.canControl) return;
        if (dir == Vector2.zero)
        {
            EntitySelfplayer.Idle();
        }
        else
        {
            EntitySelfplayer.Move();
            //
            EntitySelfplayer.SetDir(dir);
        }
    }

    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    private double lastAtkTime = 0;
    private int[] comboArr = {111, 112, 113, 114, 115};
    public int comboIndex = 0;
    private void ReleaseNormalAtk()
    {//Combo
        if (EntitySelfplayer.currentAniState == AniState.Attack)
        {
            double nowAtkTime = TimerSvc.Instance.GetNowTime();
            if (nowAtkTime - lastAtkTime < Constans.ComboSpace&&lastAtkTime!=0)
            {
                if (comboIndex!=comboArr.Length-1)
                {
                    comboIndex++;
                    EntitySelfplayer.ComboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAtkTime;
                }
                else
                {
                    lastAtkTime = 0;
                    comboIndex = 0;
                }
            }
        }
        else if(EntitySelfplayer.currentAniState == AniState.Idle||EntitySelfplayer.currentAniState == AniState.Move)
        {
            comboIndex = 0;
            lastAtkTime=TimerSvc.Instance.GetNowTime();
            EntitySelfplayer.Attack(comboArr[comboIndex]);
        }
    }

    private void ReleaseSkill1()
    {
        EntitySelfplayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        EntitySelfplayer.Attack(102);

    }

    private void ReleaseSkill3()
    {
        EntitySelfplayer.Attack(103);

    }


    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }

    public void LoadMonsterByWave(int wave)
    {
        
        for (int i = 0; i < mapCfg.Monsters.Count; i++)
        {
            MonsterMapData md = mapCfg.Monsters[i];
            if (md.MWave == wave)
            {
                GameObject go = resSvc.LoadPrefab(md.MCfg.ResPath);
                go.transform.localPosition = md.MBornPos;
                go.transform.localEulerAngles = md.MBornRote;
                go.transform.localScale = Vector3.one;

                go.name = "Wave" + wave + "_monster" + md.MIndex;    

                MonsterController monsterController = go.GetComponent<MonsterController>();
                monsterController.Init();

                EntityMonster em = new EntityMonster()
                {
                    StateManager = stateManager,
                    SkillManager = skillManager,
                    BattleManager = this,
                    Name = go.name
                };
                em.SetCtrl(monsterController);

                em.MonsterMapData = md;
                em.SetBattleProps(md.MCfg.MBattleProps);
                GameRoot.Instance.DynamicPanel.AddHpItemInfo(go.name,monsterController.HpRoot,em.HP);
                go.SetActive(false);
                monstersDic.Add(go.name, em);
            }
        }
    }

    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsters = new List<EntityMonster>();
        foreach (var item in monstersDic)
        {
            monsters.Add(item.Value);
        }

        return monsters;
    }

    private void ActiveCurrentMonsters()
    {
      
            GetEntityMonsters().ForEach(m =>
            {
                m.SetActive();
                m.Born();
                TimerSvc.Instance.AddTimeTask(id => { m.Idle(); }, 1500);
            });
    }

    public void RemoveMonster(string key)
    {
        EntityMonster entityMonster;
        if (monstersDic.TryGetValue(key, out entityMonster))
        {
            monstersDic.Remove(key);
        }
        GameRoot.Instance.DynamicPanel.RemoveHpItem(key);
    }

}