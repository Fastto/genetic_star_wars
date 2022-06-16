using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
   protected ShipController _ship;
   public bool IsFinished { get; protected set; }
   
   public virtual void Init(ShipController ship)
   {
      _ship = ship;
      Init();
   }

   protected virtual void Init()
   {
      
   }

   protected virtual bool ConditionToFinish()
   {
      return false;
   }
   
   public virtual void Update()
   {
      if(IsFinished)
         return;
      
      if(!ConditionToFinish())
         Run();
      else
      {
         Finish();
      }
   }

   protected virtual void Run()
   {
      
   }
   
   protected virtual void Finish()
   {
      IsFinished = true;
   }
}
