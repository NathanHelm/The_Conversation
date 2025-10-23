using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Persistence;
using UnityEngine;

public class LedgerNarrativeManager : StaticInstance<LedgerNarrativeManager>,IExecution, ISaveLoad
{
    [SerializeField]
    private LedgerScriptableObject ledgerScriptableObject;
    private static bool isManagerRunningOnce = true;

  

    private Dictionary<int, LedgerNarrativeObject> questionIdToledgerToolObjectDict = new();

    public SubjectActionData<ObserverAction.LedgerNarrativeActions, LedgerImage> subject{ get; set; } = new();
    public Subject<ObserverAction.LedgerNarrativeActions> subject2 { get; set; } = new();
    public void AddImageToLedgerImage(int ledgerNarrativeQuestionId)
    {
        var ledgerNarrativeObject = questionIdToledgerToolObjectDict[ledgerNarrativeQuestionId];

        questionIdToledgerToolObjectDict[ledgerNarrativeQuestionId].persistent = false;
        string path = "LedgerNarrativeImages/" + ledgerNarrativeObject.ledgerImageStreamAssetFileName;
        var texture = TextureHandler.INSTANCE.GetTextureAbsolute(path);

        LedgerImage ledgerImage = new LedgerImage(
        0,
        "",
        ledgerNarrativeObject.questionID,
        texture,
        ClueMono.clueBodyID,
        SceneNames.None,
        -1,
        path
        );

        subject.NotifyObservers(ObserverAction.LedgerNarrativeActions.addImageToLedger, ledgerImage);
    }

    public bool IsLedgerNarrativeInDict(int qid)
    {
        if (questionIdToledgerToolObjectDict.ContainsKey(qid))
        {
            return true;
        }
        return false;
    }
    public void LoadTest()
    {

        List<JsonLedgerNarrativeObject> jsonLedgerNarrativesObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonLedgerNarrativeObject>(FileNames.LedgerToolsFile);
        if (jsonLedgerNarrativesObjects != null)
        {
            if (jsonLedgerNarrativesObjects.Count > 0)
            {
                for (int i = 0; i < jsonLedgerNarrativesObjects.Count; i++)
                {
                    if (jsonLedgerNarrativesObjects[i].persistent == false)
                    {
                        continue;
                    }

                    questionIdToledgerToolObjectDict.Add(jsonLedgerNarrativesObjects[i].questionID, new LedgerNarrativeObject(
                          jsonLedgerNarrativesObjects[i].questionID,
                            jsonLedgerNarrativesObjects[i].ledgerImageStreamAssetFileName,
                              jsonLedgerNarrativesObjects[i].persistent
                        ));
                }
            }
        }
        //BROOOO ADD SOMETHING OTHER THAN COUNT CHECK SO WE'RE NO FUCKED
        if (questionIdToledgerToolObjectDict.Count == 0 && isManagerRunningOnce) //reset and add all ledger textures back. essentially restarting the narrative ascpect of the game.
        {

            foreach (LedgerNarrativeObject ledgerTool in ledgerScriptableObject.ledgerToolObjects)
            {
                if (!questionIdToledgerToolObjectDict.ContainsKey(ledgerTool.questionID))
                {
                    questionIdToledgerToolObjectDict.Add(ledgerTool.questionID, ledgerTool);
                }
                else
                {
                    Debug.LogError("ledger tool question id " + ledgerTool.questionID + "is already saved.");
                }
            }
            isManagerRunningOnce = false;
        }
        subject2.NotifyObservers(ObserverAction.LedgerNarrativeActions.loadLedgerNarrative);
    }

    public (FileNames, JsonObject[])[] Save()
    {
        int[] keys = questionIdToledgerToolObjectDict.Keys.ToArray();

        List<JsonLedgerNarrativeObject> jsonLedgerNarrativesObjects = new();

        for (int i = 0; i < keys.Length; i++)
        {
            jsonLedgerNarrativesObjects.Add(new JsonLedgerNarrativeObject(ClueMono.clueBodyID, questionIdToledgerToolObjectDict[keys[i]].questionID, questionIdToledgerToolObjectDict[keys[i]].ledgerImageStreamAssetFileName, questionIdToledgerToolObjectDict[keys[i]].persistent));
        }
        JsonLedgerNarrativeObject[] temp = jsonLedgerNarrativesObjects.ToArray();
        return new (FileNames, JsonObject[])[]{
            new(FileNames.LedgerToolsFile, temp),
        };
    }

    public void Load()
    {
        LoadTest();
    }
}
