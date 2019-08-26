using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public int DelayTime;
    public float Radius;
    public int Angel;
}

public class MonsterMapData:BaseData<MonsterMapData>
{
    public int MWave;
    public int MIndex;
    public MonsterCfg MCfg;
    public Vector3 MBornPos;
    public Vector3 MBornRote;
    public int MLevel;
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string MName;
    public string ResPath;
    public BattleProps MBattleProps;
}


public class MapCfg : BaseData<MapCfg>
{
    public string MapName;
    public string SceneName;
    public int Power;
    public Vector3 MainCamPos;
    public Vector3 MainCamRote;
    public Vector3 PlayerBornPos;
    public Vector3 PlayerBornRote;
    public List<MonsterMapData> Monsters;
    
}

public class GuideCfg : BaseData<GuideCfg>
{
    public int NpcID;
    public string DialoArr;
    public int ActID;
    public int Coin;
    public int Exp;
}

public class StrengthenCfg : BaseData<StrengthenCfg>
{
    public int Pos;
    public int StarLv;
    public int AddHP;
    public int AddHurt;
    public int AddDef;
    public int MinLv;
    public int Coin;
    public int Crystal;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string TaskName;
    public int Count;
    public int Exp;
    public int Coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int Prgs;
    public bool IsTaked;
}

public class SkillCfg : BaseData<SkillCfg>
{
    public string Name;
    public int CDTime;
    public int Duration;
    public int AniAction;
    public string FX;
    public bool IsCombo;
    public DamageType DmgType;
    public List<int> SkillMoveLst;
    public List<int> SkillActionLst;
    public List<int> SkillDamageLst;


}

public class SKillMoveCfg : BaseData<SKillMoveCfg>
{
    public int DelayTime;
    public int MoveTime;
    public float MoveDis;
}

public class BaseData<T>
{
    public int ID;
}

public class BattleProps
{
    public int HP;
    public int PD;
    public int SD;
    public int PA;
    public int SA;
    public int Dodge;
    public int Pierce;
    public int Critical;

}