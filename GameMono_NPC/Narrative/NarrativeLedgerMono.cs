using ObserverAction;
using UnityEngine;

public class NarrativeLedgerMono : BodyMono, IExecution, IObserver<ObserverAction.LedgerNarrativeActions>
{
    public void m_Awake()
    {
       // throw new System.NotImplementedException();
    }

    public void m_GameExecute()
    {
       
    }

    public void m_OnEnable()
    {
        LedgerNarrativeManager.INSTANCE.subject2.AddObserver(this);
    }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void OnNotify(LedgerNarrativeActions data)
    {
        if (data == LedgerNarrativeActions.loadLedgerNarrative)
        {
            if (!LedgerNarrativeManager.INSTANCE.IsLedgerNarrativeInDict(bodyID))
            {
                Destroy(gameObject); //This narrative object has already been retrieved. Json says so.
            }
        }
    }
}