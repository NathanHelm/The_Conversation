using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Persistence;
using UnityEngine;

public class InterviewData : StaticInstanceData<InterviewData>, ISaveLoad, IExecution
{
   //data that is nessecary for the interview scene to function...
   public int questionID = 0; //note, we obtain this from 
   public int characterID = 0;
   public SceneNames previousScene;

    public Renderer interviewFaceRenderer;

    public Vector2 levelPosition = new Vector2();

    public void Load()
    {
        List<JsonInterviewObject> loadList = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonInterviewObject>(FileNames.InterviewFile);

        DialogueData.INSTANCE.currentQuestionID = questionID = loadList[0].questionID;
        DialogueData.INSTANCE.currentCharacterID = characterID = loadList[0].characterID;
    }

    public (FileNames, JsonObject[])[] Save()
    {
        this.questionID = DialogueData.INSTANCE.currentQuestionID;
        this.characterID = DialogueData.INSTANCE.currentCharacterID;
        this.previousScene = SceneData.INSTANCE.currentScene;

        JsonInterviewObject jsonInterviewObject = new JsonInterviewObject(questionID, characterID, previousScene);
        
        return new (FileNames, JsonObject[])[]
        {
            (FileNames.InterviewFile, new JsonInterviewObject[]{ jsonInterviewObject }),     
        };

    }
}
