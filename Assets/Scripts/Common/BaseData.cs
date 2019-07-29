using UnityEngine;

public class MapCfg : BaseData<MapCfg>
{
    public string MapName;
    public string SceneName;
    public int Power;
    public Vector3 MainCamPos;
    public Vector3 MainCamRote;
    public Vector3 PlayerBornPos;
    public Vector3 PlayerBornRote;
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

public class BaseData<T>
{
    public int id;
}