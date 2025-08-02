using System;
using UnityEngine;

namespace ActionControl
{
    public class AfterFlipBehaviour
    {
        public Action<LedgerMovement> writeActionLedgerMovement = lm =>
        {
            Debug.Log("AFTERPAGEFLIP: playing the writing action!");
            PageAnimations.INSTANCE.DrawImageOnCurrentPage();
            GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
        };
        public Action<LedgerMovement> pointActionLedgerMovement = lm => {
            if (!LedgerMovement.INSTANCE.IsFlipPageCoroutineRunning())
            {
                GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
            }
        };
        
    }
}