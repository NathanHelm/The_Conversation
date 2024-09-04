using UnityEngine;
using System.Collections;
using Data;
public class DimensionDataMono : StateMono<DimensionData>
{
    public void OnEnable()
    {
        Value = DimensionData.INSTANCE;
    }

}

