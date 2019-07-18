using UnityEngine;

public class MapCfg : BaseData<MapCfg>
{
    public string mapname;
    public string scenename;
    public Vector3 maincampos;
    public Vector3 maincamrote;
    public Vector3 playerbornpos;
    public Vector3 playerbornrote;
}

public class GuideCfg : BaseData<GuideCfg>
{
    public int npcid;
    public string dilogarr;
    public int actid;
    public int coin;
    public int exp;
}
public class StrengthenCfg : BaseData<StrengthenCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;

}


public class BaseData<T>
{
    public int id;
}