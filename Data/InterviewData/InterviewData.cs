using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Persistence;
using UnityEngine;

public class InterviewData : StaticInstance<InterviewData>, ISaveLoad
{
   //data that is nessecary for the interview scene to function...
   public int questionID = 0; //note, we obtain this from 
   public int characterID = 0;
   public SceneNames previousScene;

    public void Load()
    {
       
    }

    public (FileNames, JsonObject[])[] Save()
    {
        this.questionID = DialogueData.INSTANCE.currentQuestionID;
        this.characterID = DialogueData.INSTANCE.currentCharacterID;
        this.previousScene = SceneData.INSTANCE.currentScene;

        JsonInterviewObject jsonInterviewObject = new JsonInterviewObject(questionID, characterID, previousScene);
        
        return new (FileNames, JsonObject[])[]
        {
            (FileNames.InterviewFile, new JsonObject[]{ jsonInterviewObject }),     
        };

    }
}
