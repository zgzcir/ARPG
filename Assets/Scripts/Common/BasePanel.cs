using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;
    protected TimerSvc timerSvc;
   public Canvas Canvas;
   public bool IsOpen;

    public virtual void SetPanelState(bool isOpen = true)
    {
        this.IsOpen = isOpen;
        if (gameObject.activeSelf != isOpen)
        {
            SetActive(gameObject, isOpen);
        }

        if (isOpen)
        {
            OnOpen();
        }
        else
        {
            OnClose();
        }
    }

    public virtual void Init()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;    
        timerSvc=TimerSvc.Instance;
    }
 

    protected virtual void OnOpen()
    {
        RegisterUIEvents();
    }

    protected virtual void OnClose()
    {
        ClearPanel();
    }

    protected void ClearPanel()
    {
//        resSvc = null;
//        audioSvc = null;
//        netSvc = null;
    }

    
    protected void AdjustToTop()
    {
        if (Canvas == null)
        {
            Canvas = GameObject.Find("GameRoot/Canvas").GetComponent<Canvas>();
        }
        transform.SetSiblingIndex(Canvas.transform.childCount - 1);
    }

    #region clickevents

    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        UIListener listener = GetOrAddComponent<UIListener>(go);
        listener.OnClickDown = cb;
    }

    protected void OnCLickUp(GameObject go, Action<PointerEventData> cb)
    {
        UIListener listener = GetOrAddComponent<UIListener>(go);
        listener.OnCLickUp = cb;
    }

    protected void OnClickDrag(GameObject go, Action<PointerEventData> cb)
    {
        UIListener listener = GetOrAddComponent<UIListener>(go);
        listener.OnClickDrag = cb;
    }

    protected void OnClick(GameObject go, Action<object> cb, object args)
    {
        UIListener listener = GetOrAddComponent<UIListener>(go);
        listener.OnClick = cb;
        listener.args = args;
    }

    protected virtual void RegisterUIEvents()
    {
    }

    #endregion

    #region ToolFuncs

    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }

        return t;
    }

    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }

    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }

    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }

    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }

    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    #region SetText

    protected void SetText(Text txt, string content = "")
    {
        txt.text = content;
    }

    protected void SetText(InputField ip, string content = "")
    {
        ip.text = content;
    }

    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }

    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }

    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }

    protected void SetText(Text txt, float num = 0)
    {
        SetText(txt, num.ToString());
    }

    #endregion

    protected void SetTextColor(Text txt, string color)
    {
            if (ColorUtility.TryParseHtmlString(color, out Color c))
            {
                txt.color = c;
            }
            else
            {
                Debug.LogError("TryParseHtmlString Wrong");
            }
    }
    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path);
        img.sprite = sp;
    }

    protected void SetImageColor(Image img, string color)
    {
        if (ColorUtility.TryParseHtmlString(color, out Color _color))
        {
            img.color = _color;
        }
        else
        {
            Debug.LogError("TryParseHtmlString Wrong");
        }
    }

    
    #endregion

    protected Transform GetTrans(Transform parentTrans, string name)
    {
        if (parentTrans != null)
        {
            return parentTrans.Find(name);
        }
        else
        {
         return   transform.Find(name);
        }
    }
}