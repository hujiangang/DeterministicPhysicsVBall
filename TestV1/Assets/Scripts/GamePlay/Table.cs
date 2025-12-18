using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table
{
    public static Table Instance;

    private Cueball cueball;

    public List<(Ball obj, Vector3 pos)> balls = new();
    public Table(){
        Instance = this;
    }
   
    public Cueball GetCueball(){
        return cueball;
    }


    public void RegisterBall(Ball obj, Vector3 pos){
        if (obj is Cueball cueball1)
        {
            cueball = cueball1;
        }
        balls.Add((obj, pos));
    }

   public void Init(){
       GameEvents.RegisterBasicEvent(GameBasicEvent.ReRack, ReRack);
   }

    public void ReRack(){
       foreach (var (obj, pos) in balls)
       {
            Debug.Log($"还原球{obj.name}的位置");
            obj.ResetPos(pos);
       }
    }


   public void Reset(){
       
   }


   public void Destroy(){
       GameEvents.UnregisterBasicEvent(GameBasicEvent.ReRack, ReRack);
   }
}
