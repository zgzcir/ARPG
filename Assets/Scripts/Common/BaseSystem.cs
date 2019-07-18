using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
  protected ResSvc resSvc;
  protected AudioSvc audioSvc;
  protected NetSvc netSvc;

  public virtual void InitSys()
  {
    resSvc=ResSvc.Instance;
    audioSvc=AudioSvc.Instance;
      netSvc=NetSvc.Instance;
  }
}
