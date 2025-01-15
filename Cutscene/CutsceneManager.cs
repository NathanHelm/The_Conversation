using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CutsceneManager : StaticInstance<CutsceneManager>
{
   public (Action, int)[] actionAndTime { get; set; }

   private IEnumerator cutsceneEnumerator;
    

   private IEnumerator ActionAndTime((Action, int)[] actionAndTime)
   {
        foreach((Action, int) a in actionAndTime)
        {
            a.Item1();
            yield return new WaitForSeconds(a.Item2);
        }
   }

    public void PlayAction((Action, int)[] actionAndTime)
    {
        StartCoroutine(cutsceneEnumerator = ActionAndTime(actionAndTime));
    }

   public void StopAction()
   {
        StopCoroutine(cutsceneEnumerator);
   }
}
