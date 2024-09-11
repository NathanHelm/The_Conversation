using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class TriggerManager : StaticInstance<TriggerManager>
{

    public TriggerData triggerData;
    public Trigger[] triggers;

    public void SetUpTrigger() //runs on enter dialog states
    {
       triggerData = GameEventManager.INSTANCE.OnEventFunc<TriggerData>("triggerdata");

       triggers = triggerData.triggers;
      

    }


    public int GetCharacterID(Collider collider2D, Trigger trigger) //return the character id based on what object collided with it 
    {

        if (trigger.triggerCol == null)
        {
            throw new NullReferenceException("trigger doesn't have a trigger collider");
        }

        if (trigger.charactersOnTrigger.Count > 1)
        {
            throw new Exception("two characters are inside the trigger");
        }

       
        return trigger.charactersOnTrigger[0].bodyID; //negative id values symbolize something went wrong
 

        throw new NullReferenceException("characters is not found");
    }


    //events for 
    private void triggerRunDialog()
{


}



    

     

}

