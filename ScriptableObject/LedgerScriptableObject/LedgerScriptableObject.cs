using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LedgerSO", menuName = "ScriptableObjects/LedgerSO", order = 7)]
public class LedgerScriptableObject : ScriptableObject
{
    public Sprite pageSprite;
    public Sprite lastFrontPageSprite, lastBackPageSprite;
    public Sprite firstFrontPageSprite, firstBackPageSprite;
    public GameObject pagePrefab, doubleSidedPagePrefab;
}
