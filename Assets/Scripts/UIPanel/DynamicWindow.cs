using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DynamicPanel : BasePanel
{
    public Animation tipsAni;
 public Animation playerMissAni;
    public Text txtTips;
    public Transform hpItemRoot;

    private Queue<string> tipsQue = new Queue<string>();
    private bool isTipsShow = false;
    private Dictionary<string, EntityHPItem> entityHpItemDic=new Dictionary<string, EntityHPItem>();
    
    protected override void OnOpen()
    {
        base.OnOpen();
        SetActive(txtTips, false);
    }

    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if (tipsQue.Count > 0 && !isTipsShow)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    private void SetTips(string tips)
    {        AdjustToTop();
        SetActive(txtTips);
        SetText(txtTips, tips);
        AnimationClip clip = tipsAni.GetClip("tipsMove");
        tipsAni.Play();
        StartCoroutine(AniPlayDone(clip.length, () =>
        {
            SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb != null)
        {
            cb();
        }
    }

    public void AddHpItemInfo(string mName,Transform trans,int hp)
    {
        EntityHPItem item = null;
        if (entityHpItemDic.TryGetValue(mName, out item))
        {
            return;
        }
            GameObject go = resSvc.LoadPrefab(PathDefine.EntityHpItem);
            go.transform.localPosition=new Vector3(-1000,0,0);
            go.transform.SetParent(hpItemRoot);
            EntityHPItem ehi = go.GetComponent<EntityHPItem>();
            ehi.InitItemInfo(trans,hp);
            entityHpItemDic.Add(mName,ehi);
    }

    public void  RemoveHpItem(string mName)
    {
        
        EntityHPItem item = null;
        if (entityHpItemDic.TryGetValue(mName, out item))
        {
            entityHpItemDic.Remove(mName);
            Destroy(item.gameObject);
        }
     
    }

    public void SetDodge(string mName)
    {
        EntityHPItem item = null;
        if (entityHpItemDic.TryGetValue(mName, out item))
        {
            item.SetDodge();
        }
    }   public void SetCritical(string mName,int critical)
    {
        EntityHPItem item = null;
        if (entityHpItemDic.TryGetValue(mName, out item))
        {
            item.SetCritical(critical);
        }
    }   public void SetHurt(string mName,int hurt)
    {
        EntityHPItem item = null;
        if (entityHpItemDic.TryGetValue(mName, out item))
        {
            item.SetHurt(hurt);
        }
    }    public void SetHpVal(string mName,int oldVal,int newVal)
    {
        EntityHPItem item = null;
        if (entityHpItemDic.TryGetValue(mName, out item))
        {
            item.SetHpVal(oldVal,newVal );
        }
    }

    public void SetSelfDodge()
    {
        playerMissAni.Stop();
        playerMissAni.Play();
    }


}