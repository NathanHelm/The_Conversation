using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data;
using Persistence;
using UnityEngine;

public class InterviewData : StaticInstanceData<InterviewData>, ISaveLoad, IExecution
{
   //data that is nessecary for the interview scene to function...
   public static int questionID = 0; //note, we obtain this from 
   public static int characterID = 0;

    public static string interviewTexturePath = ""; //
   public static SceneNames PREVIOUSSCENE;

    public Renderer interviewFaceRenderer;

    public Vector2 levelPosition = new Vector2();
    public InterviewFace interviewFace { get; set; }

    public void Load()
    {
        //List<JsonInterviewObject> loadList = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonInterviewObject>(FileNames.InterviewFile);
        //TODO change questin+character to static
        DialogueData.INSTANCE.currentQuestionID = questionID;
        DialogueData.INSTANCE.currentCharacterID = characterID;

        if (TextureHandler.INSTANCE.GetTextureAbsolute("InterviewImages/" + interviewTexturePath) != null)
        {
            Texture texture = TextureHandler.INSTANCE.GetTextureAbsolute("InterviewImages/" + interviewTexturePath);
            interviewFace = GameObject.FindGameObjectWithTag("InterviewParent").GetComponent<InterviewFace>();
            interviewFace.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", texture);
        }


    }

    public (FileNames, JsonObject[])[] Save()
    {
        
        questionID = DialogueData.INSTANCE.currentQuestionID;
        characterID = DialogueData.INSTANCE.currentCharacterID;
        PREVIOUSSCENE = SceneData.CURRENTSCENE;

        JsonInterviewObject jsonInterviewObject = new JsonInterviewObject(questionID, characterID, PREVIOUSSCENE);
        
        return new (FileNames, JsonObject[])[]
        {
            (FileNames.InterviewFile, new JsonInterviewObject[]{ jsonInterviewObject }),     
        };
        

    }
}
