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
        public static SystemActionCall<LedgerUIManager> onFlipPage = new SystemActionCall<LedgerUIManager>();
     
        [SerializeField]
        [Header("the object that will contain the ledger.")]
        private GameObject ledgerObject;
        [SerializeField]
        private LedgerScriptableObject ledgerScriptableObject;

        private Sprite pageSprite;
        private Material firstPageMat, lastPageMat, defaultMat;
        private GameObject pagePrefab, doubleSidedPagePrefab;

        private List<GameObject> pageObjects = new List<GameObject>(); //besides front and back page, page are double the size
        private List<GameObject> rotatePageObjects = new List<GameObject>();
        public bool isPageMoving = false;

        public float flipPageSpeed = 1f; //in seconds

        /*

         ledger based functionality.

         */
        public int GetPageLength()
        {
            return pageObjects.Count;
        }
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

            GameObject frontPage = null;
            GameObject backPage = null;
            
            Material mat = null;
            if(index == 0)
            {
                Debug.Log("index ==>" + index);
                page = CreatePage();
                mat = firstPageMat;
                rotatePageObjects.Add(page);
                page.GetComponentInChildren<Renderer>().material = mat; //setting material.
                return;
            }
             if(index == max)
            {
                page = CreatePage();
                mat = lastPageMat;
                rotatePageObjects.Add(page);
                page.GetComponentInChildren<Renderer>().material = mat; //setting material.
                return;
            }
       
                page = CreateDoubleSidedPage();
                frontPage = page.transform.GetChild(0).gameObject;
                backPage = page.transform.GetChild(1).gameObject;

                pageObjects.Add(frontPage);
                pageObjects.Add(backPage);
                rotatePageObjects.Add(page);
        }
        public void MakePageColor(int index, Color c){
            pageObjects[index].GetComponentInChildren<Renderer>().material.SetColor("_Color", c);
        }

        public void DestroyLedger()
        {
            GameObject pages = GetLedger().transform.GetComponentInChildren<Transform>().gameObject;
            
            
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

        public void ChangeLayerLeft(int index)
        {
           pageObjects[index].GetComponent<Renderer>().sortingOrder = 1;

        }
        public void ChangeLayerDown(int i)
        {
            pageObjects[i].GetComponent<Renderer>().sortingOrder = 0;
        }
       
        public void ChangeBorderLeft()
        {
            rotatePageObjects[0].GetComponentInChildren<Renderer>().sortingOrder = 1;
            rotatePageObjects[0].GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 0, 0 , 1));
        
        }
        public void NoBorder()
        {
            rotatePageObjects[0].GetComponentInChildren<Renderer>().sortingOrder = -1;
            rotatePageObjects[0].GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 1, 1 , 1));
            rotatePageObjects[rotatePageObjects.Count - 1].GetComponentInChildren<Renderer>().sortingOrder = -1;
            rotatePageObjects[rotatePageObjects.Count - 1].GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 1, 1 , 1));
      
        }
        public void ChangeBorderRight()
        {
            rotatePageObjects[rotatePageObjects.Count - 1].GetComponentInChildren<Renderer>().sortingOrder = 1;
            rotatePageObjects[rotatePageObjects.Count - 1].GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 0, 0 , 1));
        }

        private IEnumerator FlipPageAnimation(int index, float startAngle, float endAngle)
        {
            onFlipPage.RunAction(this);
            isPageMoving = true;
            float time = 0.0f;
            Debug.Log("FLIPPING ANIMATION IS GOING");
            while(time < 1.0f)
            {
                Debug.Log("going!");
                time += Time.deltaTime / flipPageSpeed;
                float theta = Mathf.Lerp(startAngle, endAngle, time);
                rotatePageObjects[index].transform.localEulerAngles = new Vector3(0 ,theta, 0);
                yield return new WaitForFixedUpdate();
            }
            rotatePageObjects[index].transform.localEulerAngles = new Vector3(0, endAngle, 0);
            isPageMoving = false;
            yield return null;
            
        }
        
    
    }
}
