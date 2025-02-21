using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI 
{
    public class LedgerUIManager : StaticInstance<LedgerUIManager>
    {
        public static SystemActionCall<LedgerUIManager> onStartLedgerData = new SystemActionCall<LedgerUIManager>();
        private GameObject ledgerObject;
        [SerializeField]
        private LedgerScriptableObject ledgerScriptableObject;

        private Sprite pageSprite;
        private Material firstPageMat, lastPageMat, defaultMat;
        private GameObject pagePrefab, doubleSidedPagePrefab;

        private List<GameObject> pageObjects = new List<GameObject>();

        public bool isPageMoving = false;

        /*

         ledger based functionality.

         */
        public override void OnEnable()
        {

            MManager.onStartManagersAction.AddAction((MManager m) => { m.ledgerUIManager = this; });


            firstPageMat = ledgerScriptableObject.firstPageMat;
            lastPageMat = ledgerScriptableObject.lastPageMat;
            defaultMat = ledgerScriptableObject.defaultMat;

            pagePrefab = ledgerScriptableObject.pagePrefab;
            doubleSidedPagePrefab = ledgerScriptableObject.doubleSidedPagePrefab;

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
            page = CreatePage();
            Material mat = null;
            if(index == 0)
            {
                Debug.Log("index ==>" + index);
                mat = firstPageMat;
            }
            else if(index == max)
            {
                mat = lastPageMat;
            }
            else{
                mat = defaultMat;
            }
            page.GetComponentInChildren<Renderer>().material = mat; //setting material.
            
            pageObjects.Add(page);
        }

        private GameObject CreatePage()
        {
            if(ledgerObject == null)
            {
                Debug.LogError("ledger not found");
                //throw new NullReferenceException("ledger not found");
                return null;
               
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

        public void ReplaceMatSprite(int index, Material material)
        {
            pageObjects[index].GetComponentInChildren<Renderer>().material = material;
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
           // ChangeLayerLeft(index, pageObjects.Count - 1);
            StartCoroutine(FlipPageAnimation(index, 180, 0));
        }
        public void FlipPageRight(int index)
        {
           // ChangeLayerRight(index, pageObjects.Count - 1);
            StartCoroutine(FlipPageAnimation(index, 0, 180));
        }

        private void ChangeLayerLeft(int index, int max)
        {
            if(index == 0)
            {
                pageObjects[index].GetComponentInChildren<Renderer>().sortingOrder = 1;
               return;
            }
            pageObjects[index].GetComponentInChildren<Renderer>().sortingOrder = 0;
            pageObjects[index - 1].GetComponentInChildren<Renderer>().sortingOrder = 1;
        }
        private void ChangeLayerRight(int index, int max)
        {
            if(index == max)
            {
                Debug.Log("prev layer -->" + pageObjects[index - 1].GetComponentInChildren<Renderer>().sortingOrder+ "this layer -->" +pageObjects[index].GetComponentInChildren<Renderer>().sortingOrder );
                // pageObjects[index - 1].GetComponentInChildren<Renderer>().sortingOrder = 0;
                pageObjects[index].GetComponentInChildren<Renderer>().sortingOrder = 1;
                return;
            }
            pageObjects[index].GetComponentInChildren<Renderer>().sortingOrder = 0;
            pageObjects[index + 1].GetComponentInChildren<Renderer>().sortingOrder = 1;
        }
        private void RenderPage()
        {
            
        }

        private IEnumerator FlipPageAnimation(int index, float startAngle, float endAngle)
        {
            isPageMoving = true;
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
            isPageMoving = false;
            yield return null;
            
        }
        
    
    }
}
