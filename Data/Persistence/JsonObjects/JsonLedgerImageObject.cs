using System;
using UnityEngine;
namespace Persistence
{
    [System.Serializable]
    public class JsonLedgerImageObject : JsonObject
    {
        public int questionID; //certain images unlocks questions that vet can ask to others.
        public Texture ledgerImage;
        public Texture[] ledgerOverlays;
        public int clueQuestionID;
        public int clueBodyID = 31;

        public string imageDescription;

        public JsonLedgerImageObject(string imageDescription ,int questionID, Texture ledgerImage, int clueQuestionID, Texture[] ledgerOverlays)
        {
            this.imageDescription = imageDescription;
            this.questionID = questionID;
            this.ledgerImage = ledgerImage;
            this.ledgerOverlays = ledgerOverlays;
            this.clueQuestionID = clueQuestionID;
        }


    }
}