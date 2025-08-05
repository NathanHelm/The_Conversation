using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MemorySpawn
{
    [Serializable]
    public class SubMemorySpawnObject
    {
        public int id;
        public Vector3 subMemoryPos;
        public float scalar2d;
        public float scalar3d;

        public Mesh mesh;
        public Sprite sprite;
    }
}
