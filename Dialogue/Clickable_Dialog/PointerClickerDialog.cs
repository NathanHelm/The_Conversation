using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
[RequireComponent(typeof(TMP_Text))]
public class PointerClickerDialog : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private TMP_Text tmptext;
	[SerializeField]
	private Canvas canvasToCheck;
	[SerializeField]
	private Camera cameraToUse;

    private static Action<string> onClickedOnEvent;

    private void OnEnable() //todo proper referencing
    {
        if(canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cameraToUse = null;
        }
        else
        {
            cameraToUse = canvasToCheck.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3();
        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(tmptext, mousePosition, cameraToUse);
        if(linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = tmptext.textInfo.linkInfo[linkTaggedText];

            onClickedOnEvent?.Invoke(linkInfo.GetLinkText());
        }
    }
}

