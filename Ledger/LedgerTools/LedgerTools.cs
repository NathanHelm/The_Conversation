using JetBrains.Annotations;

public class LedgerTools
{
    SubjectActionData<ObserverAction.LedgerToolActions, LedgerImage> subject = new();
    public void AddImageToLedgerImage(LedgerImage ledgerImage)
    {
        subject.NotifyObservers(ObserverAction.LedgerToolActions.addImageToLedger, ledgerImage);
    }
    
}