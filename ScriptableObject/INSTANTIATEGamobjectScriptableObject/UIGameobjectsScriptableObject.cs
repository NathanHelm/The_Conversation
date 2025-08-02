using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InstGameObject
{
    [CreateAssetMenu(fileName = "UIGameObjectSO", menuName = "ScriptableObjects/InstGameObject/UIGamObjectSO", order = 0)]
    public class UIGameobjectsScriptableObject:ScriptableObject
    {
        [Header("Objects that will be added to UI object")]
        public GameObject UICanvas;
        public GameObject LedgerUI;
    }
}
