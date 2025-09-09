using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using PlasticPipe.Server;

public class SpawnerManager : StaticInstance<SpawnerManager>, IExecution
{
    private static Dictionary<SceneNames, Vector2[]> sceneToSpawnPositions = new();
    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction(mm =>
        {
          // mm.spawnerManager = this;
        });
        base.m_OnEnable();
    }

    public override void m_Start()
    {
        base.m_Start();
    }
    public void PopulateSpawnDictionary()
    {
        if (sceneToSpawnPositions.Count == 0)
        {
            sceneToSpawnPositions.Add(SceneNames.SampleScene, new Vector2[] {
                new Vector2(-.9f, 1.6f) });



        }
    }
    public Vector2[] GetSpawnPositionOnScene(SceneNames sceneNames)
    {
        if (!sceneToSpawnPositions.ContainsKey(sceneNames))
        {
            UnityEngine.Debug.LogError("Could not find spawn position(s) for this scene :( ");
        }
        return sceneToSpawnPositions[sceneNames];
    }

}