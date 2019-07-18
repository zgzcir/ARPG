using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class ManulPanel : BasePanel
 {
     public GameObject HandleBar;
     public GameObject MoveablePart;
 
 
 
     private void DefaultUiEventsRegister()
     {
         if (HandleBar != null)
         {
             OnClickDrag(HandleBar.gameObject, evtData =>
             {
                 AdjustToTop();
                 MoveablePart.transform.position = evtData.position;
             });
         }

         if (MoveablePart != null)
         {
             OnClickDown(MoveablePart.gameObject, evtData => { AdjustToTop(); });
         }

     }
     
     protected override void OnOpen()
     {
         base.OnOpen();
         DefaultUiEventsRegister();
         AdjustToTop();
     }
 }