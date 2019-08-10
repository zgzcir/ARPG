using UnityEngine;

public class Entityplayer : EntityBase
 {
     public override Vector2 GetDirInput()
     {
         return BattleManager.GetDirInput();
     }
 }