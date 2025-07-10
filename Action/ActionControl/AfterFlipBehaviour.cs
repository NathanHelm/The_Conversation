using System;

namespace ActionControl
{
    public class AfterFlipBehaviour
    {
        public Action<LedgerMovement> writeActionLedgerMovement = lm =>
        {
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