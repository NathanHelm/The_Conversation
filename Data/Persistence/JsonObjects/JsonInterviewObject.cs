using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence{
[Serializable]
public class JsonInterviewObject : JsonObject{
   public int questionID = 0; //note, we obtain this from 
   public int characterID = 0;
   public SceneNames previousScene;
   public Vector2 levelPosition;

      public JsonInterviewObject(int questionID, int characterID, SceneNames previousScene)
      {
         this.questionID = questionID;
         this.characterID = characterID;
         this.previousScene = previousScene;
      }


}

}