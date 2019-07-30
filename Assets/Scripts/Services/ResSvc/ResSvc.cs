﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;
using Random = System.Random;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance;

    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrengthenCfg(PathDefine.StrenghtenCfg);
        InitTaskRewardCfgDic(PathDefine.TaskRewardCfg);
        CommonTool.Log("ResSvc Connected");
    }
    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded)
    { 
        GameRoot.Instance.LoadingPanel.SetPanelState();
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.LoadingPanel.SerProgress(val);
            if (val == 1)
            {
                if (loaded != null)
                {
                    loaded();
                }  

                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.LoadingPanel.SetPanelState(false);
            }
        };
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    public GameObject LoadPrefab(string path, bool cache = true)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }

        return go;
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();

    public AudioClip LoadAudio(string name, bool cache = false)
    {
        AudioClip au;
        if (!adDic.TryGetValue(name, out au))
        {
            au = Resources.Load<AudioClip>("ResAudio/" + name);
            if (cache)
            {
                adDic.Add(name, au);
            }
        }

        return au;
    }


    #region InitCfgs

    #region RDName

    private List<string> surNameLst = new List<string>();
    private List<string> menNameLst = new List<string>();
    private List<string> womenNameLst = new List<string>();

    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            CommonTool.Log("xml file:" + path + "not exits", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root")?.ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(ele.GetAttributeNode("ID")?.InnerText);
                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surNameLst.Add(e.InnerText);
                            break;
                        case "man":
                            menNameLst.Add(e.InnerText);
                            break;
                        case "woman":
                            womenNameLst.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRDNameData(bool isMan = true)
    {
        Random rd = new Random();
        string rdName = surNameLst[ZCTools.RDInt(0, surNameLst.Count - 1, rd)];

        if (isMan)
        {
            rdName += menNameLst[ZCTools.RDInt(0, menNameLst.Count - 1, rd)];
        }
        else
        {
            rdName += womenNameLst[ZCTools.RDInt(0, womenNameLst.Count - 1, rd)];
        }

        return rdName;
    }

    #endregion

    #region map

    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();

    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            CommonTool.Log("xml file:" + path + "not exits", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mc = new MapCfg
                {
                    id = id
                };
                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.MapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.SceneName = e.InnerText;
                            break;
                        case "power":
                            mc.Power =int.Parse( e.InnerText);
                            break;
                        case "mainCamPos":
                        {
                            string[] varArr = e.InnerText.Split(',');
                            mc.MainCamPos = new Vector3(float.Parse(varArr[0]), float.Parse(varArr[1]),
                                float.Parse(varArr[2]));
                        }
                            break;
                        case "mainCamRote":
                        {
                            string[] varArr = e.InnerText.Split(',');
                            mc.MainCamRote = new Vector3(float.Parse(varArr[0]), float.Parse(varArr[1]),
                                float.Parse(varArr[2]));
                        }
                            break;
                        case "playerBornPos":
                        {
                            string[] varArr = e.InnerText.Split(',');
                            mc.PlayerBornPos = new Vector3(float.Parse(varArr[0]), float.Parse(varArr[1]),
                                float.Parse(varArr[2]));
                        }
                            break;
                        case "playerBornRote":
                        {
                            string[] varArr = e.InnerText.Split(',');
                            mc.PlayerBornRote = new Vector3(float.Parse(varArr[0]), float.Parse(varArr[1]),
                                float.Parse(varArr[2]));
                        }
                            break;
                    }
                }

                mapCfgDataDic.Add(id, mc);
            }
        }
    }

    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }

        return null;
    }

    #endregion


    #region tasks

    private Dictionary<int, GuideCfg> GuideCfgDic = new Dictionary<int, GuideCfg>();

    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            CommonTool.Log("xml file:" + path + "not exits", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeLst = doc.SelectSingleNode("root")?.ChildNodes;
            for (int i = 0; i < nodeLst?.Count; i++)
            {
                XmlElement ele = nodeLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(ele.GetAttributeNode("ID")?.InnerText);
                GuideCfg guideCfg = new GuideCfg()
                {
                    id = id
                };
                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            guideCfg.NpcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            guideCfg.DialoArr = e.InnerText;
                            break;
                        case "actID":
                            guideCfg.ActID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            guideCfg.Coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            guideCfg.Exp = int.Parse(e.InnerText);
                            break;
                    }
                }

                GuideCfgDic.Add(id, guideCfg);
            }
        }
    }

    public GuideCfg GetGuideCfg(int id)
    {
        GuideCfg cfg;
        if (GuideCfgDic.TryGetValue(id, out cfg))
        {
            return cfg;
        }

        return null;
    }

    #endregion

    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (spriteDic.TryGetValue(path, out sp))
        {
            return sp;
        }
        else
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spriteDic.Add(path, sp);
            }
        }

        return sp;
    }

    private Dictionary<string, GameObject> ItemDic = new Dictionary<string, GameObject>();

    public TItemType LoadItem<TItemType>(string path, bool cache = true) where TItemType : class
    {
        if (ItemDic.TryGetValue(path, out var item))
        {
            return item.GetComponent<TItemType>();
        }

        item = Resources.Load<GameObject>(path);
        if (cache)
        {
            ItemDic.Add(path, item);
        }

        return item.GetComponent<TItemType>();
    }

    #region strengthen

    private Dictionary<int, Dictionary<int, StrengthenCfg>> StrengthenCfgDic =
        new Dictionary<int, Dictionary<int, StrengthenCfg>>();

    private void InitStrengthenCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            CommonTool.Log("xml file:" + path + "not exits", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeLst = doc.SelectSingleNode("root")?.ChildNodes;
            for (int i = 0; i < nodeLst?.Count; i++)
            {
                XmlElement ele = nodeLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(ele.GetAttributeNode("ID")?.InnerText);
                StrengthenCfg sd = new StrengthenCfg()
                {
                    id = id
                };
                foreach (XmlElement e in ele.ChildNodes)
                {
                    int value = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos":
                            sd.Pos = value;
                            break;
                        case "starlv":
                            sd.StarLv = value;
                            break;
                        case "addhp":
                            sd.AddHP = value;
                            break;
                        case "addhurt":
                            sd.AddHurt = value;
                            break;
                        case "adddef":
                            sd.AddDef = value;
                            break;
                        case "minlv":
                            sd.MinLv = value;
                            break;
                        case "coin":
                            sd.Coin = value;
                            break;
                        case "crystal":
                            sd.Crystal = value;
                            break;
                    }
                }

                Dictionary<int, StrengthenCfg> dic = null;
                if (StrengthenCfgDic.TryGetValue(sd.Pos, out dic))
                {
                    dic.Add(sd.StarLv, sd);
                }
                else
                {
                    dic = new Dictionary<int, StrengthenCfg>();
                    dic.Add(sd.StarLv, sd);
                    StrengthenCfgDic.Add(sd.Pos, dic);
                }
            }
        }
    }

    public StrengthenCfg GetStrengthenCfg(int pos, int starlv)
    {
        StrengthenCfg sd = null;
        Dictionary<int, StrengthenCfg> dic = null;
        if (StrengthenCfgDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(starlv))
            {
                sd = dic[starlv];
            }
        }
        return sd;
    }

    public int GetPropAddValPreLv(int pos, int slv, int type)
    {
        int val = 0;
        Dictionary<int, StrengthenCfg> posDic = null;
        if (StrengthenCfgDic.TryGetValue(pos, out posDic))
        {
            for (int i = 0; i < slv; i++)
            {
                StrengthenCfg sc;
                if (posDic.TryGetValue(i, out sc))
                {
                    switch (type)
                    {
                        case 1:
                            val += sc.AddHP;

                            break;
                        case 2:
                            val += sc.AddHurt;
                            break;
                        case 3:
                            val += sc.AddDef;
                            break;
                    }
                }
            }
        }

        return val;
    }

    #endregion

    #region task

    private Dictionary<int, TaskRewardCfg> TaskRewardCfgDic = new Dictionary<int, TaskRewardCfg>();

    private void InitTaskRewardCfgDic(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            CommonTool.Log("xml file:" + path + "not exits", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeLst = doc.SelectSingleNode("root")?.ChildNodes;
            for (int i = 0; i < nodeLst?.Count; i++)
            {
                XmlElement ele = nodeLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(ele.GetAttributeNode("ID")?.InnerText);
                TaskRewardCfg taskRewardCfg = new TaskRewardCfg()
                {
                    id = id
                };
                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "taskName":
                            taskRewardCfg.TaskName = e.InnerText;
                            break;
                        case "count":
                            taskRewardCfg.Count = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            taskRewardCfg.Exp = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            taskRewardCfg.Coin = int.Parse(e.InnerText);
                            break;
                    }
                }

                TaskRewardCfgDic.Add(id, taskRewardCfg);
            }

            CommonTool.Log("TaskRewardCfgDic Done");
        }
    }

    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg data;
        if (TaskRewardCfgDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion
    
    #endregion
    private void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }
}