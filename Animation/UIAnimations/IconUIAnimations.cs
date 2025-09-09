using System.Collections.Generic;
using Data;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class IconUIAnimations : StaticInstance<IconUIAnimations>, IExecution
{
    public static SystemActionCall<IconUIAnimations> onStartImageUIAnimations = new SystemActionCall<IconUIAnimations>();

    [SerializeField]
    private UIScriptableObject imageUIScriptableObject;
    private GameObject iconPrefab;

    public Renderer drawLedgerUIPageIndexRenderer { get; set; } //image at current index the will undergo image "drawing animation"

    private List<GameObject> spawnedIconObject = new();
    private Dictionary<Icon, Material> iconToRendererDictionary = new(); 

    public override void m_Start()
    {
        onStartImageUIAnimations.RunAction(this);
        var iconTextures = imageUIScriptableObject.icons;
        iconPrefab = imageUIScriptableObject.iconPrefab;

        for (int i = 0; i < iconTextures.Length; i++)
        {
            spawnedIconObject.Add(Instantiate(iconPrefab, GameObject.FindGameObjectWithTag("IconParent").transform));
            spawnedIconObject[i].transform.localPosition = new Vector3(-i * imageUIScriptableObject.leftIconShift, 0, 0);
            var renderer = spawnedIconObject[i].GetComponent<RawImage>().material;
            spawnedIconObject[i].GetComponent<RawImage>().texture = iconTextures[i].texture;
           // UIManager.INSTANCE.ChangeTexture(ref renderer, iconTextures[i].texture);
            ImageUIAnimations.INSTANCE.FadeOutIcon(renderer, 0);
            iconToRendererDictionary.Add(iconTextures[i].iconNumber, renderer);

        }

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
    public Material GetIconRenderer(Icon icon)
    {
        return iconToRendererDictionary[icon];
    }
    public void FadeInIconRenderer(Material material)
    {
        ImageUIAnimations.INSTANCE.FadeInIcon(material, 1f);
    }
     public void FadeOutIconRenderer(Material material)
    {
        ImageUIAnimations.INSTANCE.FadeOutIcon(material, 0.2f);
    }

    public void DrawLedgerImageUI()
    {
        ImageUIAnimations.INSTANCE.DrawImage(drawLedgerUIPageIndexRenderer.material);
    }

    public void EraseLedgerImageUI()
    {
        ImageUIAnimations.INSTANCE.EraseImage(drawLedgerUIPageIndexRenderer.material);
    }


}
