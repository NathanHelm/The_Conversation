using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Persistence;
public class TesterSaveLoadInterfaceObject : MonoBehaviour, ISaveLoad
{
    DialogueConversation dialogueConversation; //mocking dialogueConversation

    private void Start()
    {
        dialogueConversation = new DialogueConversation();
        DialogueObject dialogueLine = new DialogueObject();
        DialogueObject dialogueLine1 = new DialogueObject();
        DialogueObject dialogueLine2 = new DialogueObject();
        dialogueLine.line = "dialogue line 0";
        dialogueLine1.line = "dialogue line 1";
        dialogueLine2.line = "dialogue line 2";
        dialogueConversation.dialogueObjects = new DialogueObject[] { dialogueLine , dialogueLine1 , dialogueLine2};
        dialogueConversation.ID = 1;
        
        
        
    }
    public void Load()
    {
        
    }

    public (FileNames, JsonObject[])[] Save() 
    {
        List<JsonObject> jsonObjects = new List<JsonObject>();
        List<Persistence.JsonQuestionObject> jsonQObjects = new List<JsonQuestionObject>();

        for(int i = 1; i <= 15; i++)
        {
            JsonQuestionObject jsonObject = new JsonQuestionObject();
            jsonObject.Id = i; 
            jsonObject.name = "this is question: " + jsonObject.Id.ToString();
            jsonObject.question = "question... " + i;
            jsonQObjects.Add(jsonObject); 
        }

        for(int i = 1; i <= 15; i++)
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.Id = i; 
            jsonObject.name = "i am message waltuh." + jsonObject.Id.ToString();
            jsonObjects.Add(jsonObject);
        }

       return new (FileNames, JsonObject[])[] { 

        new (FileNames.myfile, jsonObjects.ToArray()),
        new (FileNames.mydialogfile, jsonQObjects.ToArray())
        };
    }
}