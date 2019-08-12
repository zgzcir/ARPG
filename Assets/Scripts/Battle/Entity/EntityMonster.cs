public class EntityMonster : EntityBase
 {
     public MonsterMapData MonsterMapData;
 
     public override void SetBattleProps(BattleProps battleProps)
     {
         var level = MonsterMapData.MLevel;
 
         this.BattleProps = new BattleProps()
         {
             HP = battleProps.HP * level,
             SA = battleProps.SA * level,
             PA = battleProps.PA * level,
             SD = battleProps.SD * level,
             PD = battleProps.PD * level,
             Dodge = battleProps.Dodge * level,
             Pierce = battleProps.Pierce * level,
             Critical = battleProps.Critical * level,
         };
         HP = BattleProps.HP;
     }
 }