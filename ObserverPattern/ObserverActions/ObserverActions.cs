public class ObserverActions
{

}
namespace ObserverAction
{

    public enum LedgerActions
    {
        activeLedger,
        createLedger,
        movePageLeft,
        movePageRight,
        onAddImagesToLedger,
        onAfterMovePageFurthestLeft,
        onSelectPage
        /*
        public SystemActionCall<LedgerManager> onActiveLedger = new SystemActionCall<LedgerManager>();
        public SystemActionCall<LedgerManager> onAfterCreateLedger = new SystemActionCall<LedgerManager>();
        public SystemActionCall<LedgerManager> onMovePageLeft = new SystemActionCall<LedgerManager>();
        public SystemActionCall<LedgerManager> onMovePageRight = new SystemActionCall<LedgerManager>();
        public SystemActionCall<LedgerManager> onAddImagesToLedger = new SystemActionCall<LedgerManager>();
        public SystemActionCall<LedgerManager> onAfterMovePageFurthestLeft = new SystemActionCall<LedgerManager>();
        public SystemActionCall<LedgerManager> onSelectPage = new SystemActionCall<LedgerManager>();
        */
    }
    public enum LedgerMovementActions
    {
        onEnableHand,
        onCreatedHands,

        onMoveHand,
        onPointHand,
        onWriteHand,
        afterWriteHand,
        afterFlip

    }

    public enum MemorySpawnerAction
    {
        onAfterStageObjectsSpawn,
        onTransformObjectUpdate, 

    }
    public enum MemoryTransformEnableAction
    {
        circleEnable,
        waveEnable,

    }
    public enum MemoryTransformUpdateAction
    {
        waveUpdate,
        circleUpdate,
    }

}
