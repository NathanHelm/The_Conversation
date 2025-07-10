using UnityEngine;

namespace Persistence
{
    public class JsonSpawnObject : JsonObject
    {
    public Vector2 savedPlayerPosition; //with this information and the scene that we are currently on, we can calculate the closest spawn position.

    }
}