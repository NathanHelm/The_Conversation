using Persistence;
using UnityEngine;

namespace Data
{

    public class SpawnData : StaticInstanceData<SpawnData>, ISaveLoad, IExecution
    {
        public Vector2 savedSpawnPosition { get; set; }
        public Vector2 spawnPosition { get; set; }

        public void Load()
        {
            
        }

        public (FileNames, JsonObject[])[] Save()
        {
            /*
              this.questionID = DialogueData.INSTANCE.currentQuestionID;
        this.characterID = DialogueData.INSTANCE.currentCharacterID;
        this.previousScene = SceneData.INSTANCE.currentScene;

        JsonInterviewObject jsonInterviewObject = new JsonInterviewObject(questionID, characterID, previousScene);
        
        return new (FileNames, JsonObject[])[]
        {
            (FileNames.InterviewFile, new JsonInterviewObject[]{ jsonInterviewObject }),     
        };
            */

            JsonSpawnObject jsonSpawnObject = new JsonSpawnObject();

            return new (FileNames, JsonObject[])[]
            {
            (FileNames.SpawnFile, new JsonSpawnObject[]{ jsonSpawnObject }),
            };
        }
    }
}