public class ObserverActions
{

}
namespace ObserverAction
{
    public enum LedgerToolActions
    {
        addImageToLedger, 
        removeImageToLedger,
    }
    public enum LedgerActions
    {
        activeLedger,
        createLedger,
        movePageLeft,
        movePageRight,
        onSetTextureToPageImage,
        onAfterMovePageFurthestLeft,
        onSelectPage,

        onAddedPrimaryKeyToLedgerImage,
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

    public enum PlayerActions
    {
        onOmitRayClue,
        onOmitRayCharacter
    }
    public enum ClueCameraActions
    {
        onSpawnCamera, //add ledger image because we have our texture (our render texture)..
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
    public enum MemoryTransform
    {
        onAfterEnableTransformation
    }
    public enum StateMachineAction //should done something like this a long time ago... oh well.
    {
        onEnterInterviewScene
    }

}
