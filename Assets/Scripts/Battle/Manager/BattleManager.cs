using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEditor;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private MapCfg mapCfg;

    private StateManager stateManager;
    private SkillManager skillManager;
    private MapManager mapManager;


    private PlayerController playerController;
    private CameraController cameraController;
    private EntityPlayer entitySelfplayer;


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
                entitySelfplayer.Idle();
                PlayerData pd = GameRoot.Instance.PlayerData;
                entitySelfplayer.SetBattleProps(new BattleProps()
                {
                    HP = pd.HP,
                    pa = pd.PA,
                    pd = pd.PD,
                    sa = pd.SA,
                    sd = pd.SD,
                    pierce = pd.Pierce,
                    dodge = pd.Dodge,
                    critical = pd.Critical
                });
                GameRoot.AddTips("四风试炼场");
                BattleSys.Instance.SetBattlePanelState();


                //       audioSvc.PlayBgAudio(Constans.BGCityHappy,true);bgm
            }
        );

        Debug.Log("Init BattleManager Done");
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

        entitySelfplayer = new EntityPlayer()
        {
            StateManager = stateManager,
            Controller = playerController,
            SkillManager = skillManager,
            BattleManager = this
        };

        //ttt   
        cameraController.Target = playerController.CameraPivot.transform;
        cameraController.enabled = true;
        // 
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (!entitySelfplayer.canControl) return;
        if (dir == Vector2.zero)
        {
            entitySelfplayer.Idle();
        }
        else
        {
            entitySelfplayer.Move();
            //
            entitySelfplayer.SetDir(dir);
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

    private void ReleaseNormalAtk()
    {
        Debug.Log("Normal Atk");
    }

    private void ReleaseSkill1()
    {
        entitySelfplayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        Debug.Log("2 Atk");
    }

    private void ReleaseSkill3()
    {
        Debug.Log("3 Atk");
    }


    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }

    public void LoadMonsterByWave(int wave)
    {
        for (int i = 0; i < mapCfg.Monsters.Count; i++)
        {
            MonsterData md = mapCfg.Monsters[i];
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
                    Controller = monsterController,
                    SkillManager = skillManager,
                    BattleManager = this
                };


                go.SetActive(false);
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
}