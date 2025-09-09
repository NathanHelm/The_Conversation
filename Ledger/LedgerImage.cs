using UnityEngine;
using System.Collections;
using Codice.ThemeImages;
[System.Serializable]
public class LedgerImage
{
	public string imageDescription  {get;set;} = "";  //TODO maybe change this? 
	public int questionID {get;set;} //certain images unlocks questions that vet can ask to others.
    public Texture ledgerImage { get; set; }


    public int clueID; 
    
    public int clueBodyID = 31; //set value to whatever you desire

    public int clueCameraForiegnKey = 0; //image id (hash) to link clue camera object with camera

    public SceneNames sceneName;


    public LedgerImage(int clueID, string imageDescription, int questionID, Texture ledgerImg, int clueBodyID, SceneNames sceneName, int clueCameraForeignKey)
    {
        this.clueID = clueID;
        this.imageDescription = imageDescription;
        this.questionID = questionID;
        this.clueBodyID = clueBodyID;
        this.ledgerImage = ledgerImg;
        this.clueCameraForiegnKey = clueCameraForeignKey;
        this.sceneName = sceneName;
    }
}

