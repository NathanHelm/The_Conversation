using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LedgerSO", menuName = "ScriptableObjects/LedgerSO", order = 7)]
public class LedgerScriptableObject : ScriptableObject
{
    public Material firstPageMat, lastPageMat, defaultMat;
    public Texture interviewIcon;
    public GameObject pagePrefab, doubleSidedPagePrefab;

     [Header
    (
        "amount of double sided page, for page length would be times 2."
    )]
    public int pagesLength;
    [Header
    (
        "speed at which it takes to make it from first to last page."
    )]
    public float flipPageSpeedToEnd;

}
