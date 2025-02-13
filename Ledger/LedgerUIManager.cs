using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace UI 
{
    public class LedgerUIManager : StaticInstance<LedgerUIManager>
    {
        public static SystemActionCall<LedgerUIManager> onStartLedgerData = new SystemActionCall<LedgerUIManager>();
        private GameObject ledgerObject;
        [SerializeField]
        private LedgerScriptableObject ledgerScriptableObject;

        private Sprite pageSprite;
        private Sprite lastFrontPageSprite, lastBackPageSprite;
        private Sprite firstFrontPageSprite, firstBackPageSprite;
        private GameObject pagePrefab, doubleSidedPagePrefab;

        private List<GameObject> pageObjects = new List<GameObject>();

        /*

         ledger based functionality.

         */
        public override void OnEnable()
        {

            MManager.onStartManagersAction.AddAction((MManager m) => { m.ledgerUIManager = this; });


            pageSprite = ledgerScriptableObject.pageSprite;
            lastFrontPageSprite = ledgerScriptableObject.lastFrontPageSprite;
            firstFrontPageSprite = ledgerScriptableObject.firstFrontPageSprite;
            firstBackPageSprite = ledgerScriptableObject.firstBackPageSprite;
            lastBackPageSprite = ledgerScriptableObject.lastBackPageSprite;
            pagePrefab = ledgerScriptableObject.pagePrefab;


            base.OnEnable();
        }
        public override void m_Start()
        {
            if (GameObject.FindGameObjectWithTag("Ledger") != null)
            {
                ledgerObject = GameObject.FindGameObjectWithTag("Ledger");
            }
            base.m_Start();
            onStartLedgerData.RunAction(this);
        }


        public void OpenBook() //enables book gameobject 
        {
            GameObject nbookObj = GetLedger();
            UIManager.INSTANCE.EnableUIObject(ref nbookObj);
        }
        public void CloseBook()
        {
            GameObject nbookObj = GetLedger();
            UIManager.INSTANCE.DisableUIObject(ref nbookObj);
        }
        public void AddPage(int index, int max)
        {
            Debug.Log("adding page");
            GameObject page = null;
            if (index == 0)
            {
                page = CreateDoubleSidedPage();
                page.GetComponentInChildren<Image>().sprite = firstFrontPageSprite;
               page.GetComponentsInChildren<Image>()[1].sprite = firstBackPageSprite;
            }
            else if(index == max)
            {
                page = CreateDoubleSidedPage();
                page.GetComponentInChildren<Image>().sprite = lastFrontPageSprite;
                page.GetComponentsInChildren<Image>()[1].sprite = lastBackPageSprite;
            }
            else
            {
                page = CreatePage();
                page.GetComponentInChildren<Image>().sprite = pageSprite;
            }
            pageObjects.Add(page);
        }

        private GameObject CreatePage()
        {
            if(ledgerObject == null)
            {
                throw new NullReferenceException("ledger not found");
            }
            GameObject g = Instantiate(pagePrefab, GetLedger().transform);
            return g;
        }
        private GameObject CreateDoubleSidedPage()
        {
            if (ledgerObject == null)
            {
                throw new NullReferenceException("ledger not found");
            }
            GameObject g = Instantiate(doubleSidedPagePrefab, GetLedger().transform);
            return g;
        }

        public void ReplacePageSprite(int index, Sprite newSprite)
        {
            pageObjects[index].GetComponentInChildren<Image>().sprite = newSprite;
        }

        public void RemovePage(int index)
        {
            throw new Exception("not implemented yet.");
        }

        public GameObject GetLedger()
        {
            return ledgerObject;
        }

        public void FlipPageLeft(int index)
        {
            StartCoroutine(FlipPageAnimation(index, 180, 0));
        }
        public void FlipPageRight(int index)
        {
            StartCoroutine(FlipPageAnimation(index, 0, 180));
        }

        private IEnumerator FlipPageAnimation(int index, float startAngle, float endAngle)
        {
            float seconds = 1.0f;
            float time = 0.0f;
            Debug.Log("FLIPPING ANIMATION IS GOING");
            while(time < 1.0f)
            {
                Debug.Log("going!");
                time += Time.deltaTime / seconds;
                float theta = Mathf.Lerp(startAngle, endAngle, time);
                pageObjects[index].transform.localEulerAngles = new Vector3(0 ,theta, 0);
                yield return new WaitForFixedUpdate();
            }
            pageObjects[index].transform.localEulerAngles = new Vector3(0, endAngle, 0);
            yield return null;
        }
        
    
    }
}
