using Data;
using UnityEngine;
public class ImageUIAnimations : StaticInstance<ImageUIAnimations>, IExecution
{
    public static SystemActionCall<ImageUIAnimations> onStartImageUIAnimations = new SystemActionCall<ImageUIAnimations>();

    [SerializeField]
    private ImageUIScriptableObject imageUIScriptableObject;

    public Renderer LedgerImageUIRenderer { get; set; } = null;

    public Renderer InterviewImageUI { get; set; } = null;
    private Renderer ledgerImageUIRenderer;
    private Renderer interviewImageUI;

    public override void m_Start()
    {
        onStartImageUIAnimations.RunAction(this);

        interviewImageUI = InterviewImageUI;
        ledgerImageUIRenderer = LedgerImageUIRenderer;

        ledgerImageUIRenderer.material = imageUIScriptableObject.ledgerImageUIMaterial;

        interviewImageUI.material = imageUIScriptableObject.interviewUIMaterial;


        UIManager.INSTANCE.ChangeTexture(ref ledgerImageUIRenderer, imageUIScriptableObject.ledgerImageUI);
        UIManager.INSTANCE.ChangeTexture(ref interviewImageUI, imageUIScriptableObject.interviewUIIcon);


        base.m_Start();
    }
    
    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction(mm => { mm.imageUIAnimations = this; });
        base.m_OnEnable();
    }
    public override void OnDisable()
    {
        MManager.INSTANCE.imageUIAnimations = null;
        base.OnDisable();
    }
    
    public void DrawLedgerImageUI()
    {
        PageAnimations.INSTANCE.DrawImage(ledgerImageUIRenderer);
    }
    
    public void EraseLedgerImageUI()
    {
        PageAnimations.INSTANCE.EraseImage(ledgerImageUIRenderer);
    }
    public void DrawInterviewIcon()
    {
       PageAnimations.INSTANCE.DrawImage(interviewImageUI);
    }
    public void EraseInterviewIcon()
    {
       PageAnimations.INSTANCE.EraseImage(interviewImageUI);
    }

    
}