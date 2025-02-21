using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LedgerSO", menuName = "ScriptableObjects/LedgerSO", order = 7)]
public class LedgerScriptableObject : ScriptableObject
{
    public Material firstPageMat, lastPageMat, defaultMat;
    public GameObject pagePrefab, doubleSidedPagePrefab;
}
