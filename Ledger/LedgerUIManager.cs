using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Codice.CM.Common;
using Data;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI 
{
    public class LedgerUIManager : StaticInstance<LedgerUIManager>
    {
        public static SystemActionCall<LedgerUIManager> onStartLedgerData = new SystemActionCall<LedgerUIManager>();
        public static SystemActionCall<LedgerUIManager> onFlipPage = new SystemActionCall<LedgerUIManager>();
        public static SystemActionCall<(bool,int,LedgerUIManager)> onAfterFlipPage = new SystemActionCall<(bool,int,LedgerUIManager)>();
        public static SystemActionCall<(bool,int,LedgerUIManager)> onFlipAt90Degrees = new SystemActionCall<(bool,int,LedgerUIManager)>();
       
         public static SystemActionCall<(bool,int,LedgerUIManager)> onBeginFlipPage = new SystemActionCall<(bool,int,LedgerUIManager)>();

       
        public static SystemActionCall<LedgerUIManager> onBorderCheck = new SystemActionCall<LedgerUIManager>();
        
  
        
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

        private List<GameObject> imageObjects = new List<GameObject>();
        public bool isPageMoving = false;

        public bool isLeft {get; set;} = false;
 
        public float flipPageSpeed; //in seconds

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

            base.OnEnable();
        }
        public override void m_Start()
        {
            if (GameObject.FindGameObjectWithTag("Ledger") != null)
            {
                ledgerObject = GameObject.FindGameObjectWithTag("Ledger");
            }
            else
            {
                Debug.LogError("ledger gameobject is null");
            }
            firstPageMat = ledgerScriptableObject.firstPageMat;
            lastPageMat = ledgerScriptableObject.lastPageMat;
            defaultMat = ledgerScriptableObject.defaultMat;

            pagePrefab = ledgerScriptableObject.pagePrefab;
            doubleSidedPagePrefab = ledgerScriptableObject.doubleSidedPagePrefab;

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
            if(nbookObj == null)
            {
                return;
            }
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
               // page.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, .1f);
                return;
            }
             if(index == max)
            {
                page = CreatePage();
                mat = lastPageMat;
                rotatePageObjects.Add(page);
                page.GetComponentInChildren<Renderer>().material = mat; //setting material.
               // page.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, -.1f);
                return;
            }
       
                page = CreateDoubleSidedPage();
                frontPage = page.transform.GetChild(0).gameObject;
                backPage = page.transform.GetChild(1).gameObject;
                


                pageObjects.Add(frontPage);
                pageObjects.Add(backPage);

                var frontPageImage = frontPage.transform.GetChild(frontPage.transform.childCount - 1).gameObject; //get 
                var backPageImage = backPage.transform.GetChild(backPage.transform.childCount - 1).gameObject;

                imageObjects.Add(frontPageImage);
                imageObjects.Add(backPageImage);

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
        public void ChangeLayerUp(int index, int nextImageQueue)
        {
           var image = imageObjects[index].GetComponent<Renderer>();
           ChangeRenderQueue(ref image, 3100, Color.red);
           var page = pageObjects[index].GetComponent<Renderer>();
           ChangeRenderQueue(ref page, 3000, Color.white);


           if(isLeft) 
           {
                if(index - 1 >= 0)
                {
                    var nextImage = imageObjects[index - 1].GetComponent<Renderer>();
                    ChangeRenderQueue(ref nextImage, nextImageQueue + 1, Color.yellow);
                        
                    var nextPage = pageObjects[index - 1].GetComponent<Renderer>();
                    ChangeRenderQueue(ref nextPage, nextImageQueue, Color.white);
                }
           }
           else //is right
           {
              if(index + 1 < pageObjects.Count - 1)
              {
                        var nextImage = imageObjects[index + 1].GetComponent<Renderer>();
                        ChangeRenderQueue(ref nextImage, nextImageQueue + 1, Color.yellow);
                        
                        var nextPage = pageObjects[index + 1].GetComponent<Renderer>();
                        ChangeRenderQueue(ref nextPage, nextImageQueue, Color.white);
              }
           }
        }
        
        public void ChangeLayerDown(int index, int prevIndexOneQueue, int prevIndexTwoQueue)
        {

           Renderer pageR = pageObjects[index].GetComponent<Renderer>();
           Renderer imageR = imageObjects[index].GetComponent<Renderer>();

            int max = pageObjects.Count - 1;

           

           if(isLeft) 
           {
                if(index + 1 <= max)
                {
                Renderer pagePrev = pageObjects[index + 1].GetComponent<Renderer>();
                Renderer imagePrev = imageObjects[index + 1].GetComponent<Renderer>();

                ChangeRenderQueue(ref pagePrev, prevIndexTwoQueue, Color.white);
                ChangeRenderQueue(ref imagePrev, prevIndexTwoQueue + 1, Color.magenta);
                }
              if(index + 2 <= max)
              {
                    Renderer pagePrev = pageObjects[index + 2].GetComponent<Renderer>();
                    Renderer imagePrev = imageObjects[index + 2].GetComponent<Renderer>();

                    ChangeRenderQueue(ref pagePrev, prevIndexOneQueue, Color.white);
                    ChangeRenderQueue(ref imagePrev, prevIndexOneQueue + 1, Color.blue);
              }
              if(index + 3 <= max)
              {
                   Renderer pagePrev = pageObjects[index + 3].GetComponent<Renderer>();
                   Renderer imagePrev = imageObjects[index + 3].GetComponent<Renderer>();

                    ChangeRenderQueue(ref pagePrev, prevIndexTwoQueue, Color.white);
                    ChangeRenderQueue(ref imagePrev, prevIndexTwoQueue + 1, Color.magenta);
              }
           }
           else //is right
           {
            
            if(index - 1 >= 0)
            {
                Renderer pagePrev = pageObjects[index - 1].GetComponent<Renderer>();
                Renderer imagePrev = imageObjects[index - 1].GetComponent<Renderer>();

                ChangeRenderQueue(ref pagePrev, prevIndexTwoQueue, Color.white);
                ChangeRenderQueue(ref imagePrev, prevIndexTwoQueue + 1, Color.magenta);
            }
              if(index - 2 >= 0)
              {
                    Renderer pagePrev = pageObjects[index - 2].GetComponent<Renderer>();
                    Renderer imagePrev = imageObjects[index - 2].GetComponent<Renderer>();

                    ChangeRenderQueue(ref pagePrev, prevIndexOneQueue, Color.white);
                    ChangeRenderQueue(ref imagePrev, prevIndexOneQueue + 1, Color.blue);
              }
              if(index - 3 >= 0)
              {
                   Renderer pagePrev = pageObjects[index - 3].GetComponent<Renderer>();
                   Renderer imagePrev = imageObjects[index - 3].GetComponent<Renderer>();

                    ChangeRenderQueue(ref pagePrev, prevIndexTwoQueue, Color.white);
                    ChangeRenderQueue(ref imagePrev, prevIndexTwoQueue + 1, Color.magenta);
              }
           }

           
        }

        public void LayerDownAtIndex(int index, int indexQueue) //will change layer of page and image at the index, use this function if you are rese
        {
            Renderer pagePrev = pageObjects[index].GetComponent<Renderer>();
            Renderer imagePrev = imageObjects[index].GetComponent<Renderer>();
            ChangeRenderQueue(ref pagePrev, indexQueue, Color.white);
            ChangeRenderQueue(ref imagePrev, indexQueue + 1, Color.magenta);
        }
  
       

        public void FlipPageLeft(int pindex, int index)
        {
            onFlipPage.RunAction(this);
            StartCoroutine(FlipPageAnimation(isLeft,pindex, index, 180, 0));
            LedgerMovement.INSTANCE.MoveHandAwaitPoint();
        }
        public void FlipPageRight(int pindex, int index)
        {
           // ChangeLayerRight(index, pageObjects.Count - 1);
            onFlipPage.RunAction(this);
            StartCoroutine(FlipPageAnimation(isLeft,pindex, index, 0, 180));
            LedgerMovement.INSTANCE.MoveHandAwaitPoint();
        }
        public Renderer GetPageOverlayRenderer(int index)
        {
           var temp =  pageObjects[index].GetComponentsInChildren<Renderer>()[1];
           return temp;
        }

    
        public void ChangeRenderQueue(ref Renderer renderer, int r, Color c)
        {
            renderer.material.renderQueue = r; 
             
            if(renderer.material.HasColor("_Color"))
            {
            renderer.material.SetColor("_Color", c);
            }
            else
            {
                Debug.LogError("no color found");
            }
        }
     
        public void ChangeTexture(ref Renderer renderer, Texture texture)
        {
             if(renderer.material.HasTexture("_MainTex"))
             {
                renderer.material.SetTexture("_MainTex", texture);
             }
             else
             {
                Debug.Log("_MainTex does not exist for this shader");
             }
        }

        public void ChangeBorderLeft()
        {
            Renderer r = rotatePageObjects[0].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r, 3300, Color.white);
        
        }
        public void NoBorder()
        {
            Renderer r = rotatePageObjects[0].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r, 2000, Color.white);

            Renderer r2 = rotatePageObjects[^1].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r2, 2000, Color.white);
        }
        public void ChangeBorderRight()
        {
            Renderer r = rotatePageObjects[^1].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r, 3200, Color.white);

         //   rotatePageObjects[rotatePageObjects.Count - 1].GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 0, 0 , 1));
        }
        public void SetTextureToPage(int index, Texture tex)
        {
            Renderer overlayImage = imageObjects[index].GetComponent<Renderer>();
            ChangeTexture(ref overlayImage, tex);
        }

        private IEnumerator FlipPageAnimation(bool isLeft,int pIndex, int index, float startAngle, float endAngle)
        {
            int nindex = pIndex;
            bool nisLeft = isLeft;
            float time = 0.0f;
            bool runOnce = false;
            Debug.Log("FLIPPING ANIMATION IS GOING");
            onBeginFlipPage.RunAction((nisLeft, nindex, this));
            while(time < 1.0f)
            {
                isPageMoving = true;
                if((Mathf.Floor(time * 10f) / 10f) == 0.5 && runOnce == false)
                {
                    onFlipAt90Degrees.RunAction((nisLeft,nindex,this)); 
                    runOnce = true;
                }
              //  Debug.Log("going! " + flipPageSpeed);
                time += Time.deltaTime / flipPageSpeed;
                float theta = Mathf.Lerp(startAngle, endAngle, time);
                rotatePageObjects[index].transform.localEulerAngles = new Vector3(0 ,theta, 0);
                yield return new WaitForFixedUpdate();
            }
            rotatePageObjects[index].transform.localEulerAngles = new Vector3(0, endAngle, 0);
            onAfterFlipPage.RunAction((nisLeft,nindex,this));
            isPageMoving = false;
            
            yield return null;
            
        }
        
    
    }
}
