using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Codice.CM.Common;
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
        public static SystemActionCall<(int,LedgerUIManager)> onAfterFlipPage = new SystemActionCall<(int,LedgerUIManager)>();
        public SystemActionCall<(int,LedgerUIManager)> onFlipAt90Degrees = new SystemActionCall<(int,LedgerUIManager)>();
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
        public bool isPageMoving = false;

        public bool isLeft {get; set;} = false;

        private int previousNeighbourIndex = -1;
        private int neighbourIndex = -1;

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
        
        public (Renderer, Renderer) GetChildrenOfRotateIndex(int rotateIndex)
        {
            
            Renderer[] pages = rotatePageObjects[rotateIndex].GetComponentsInChildren<Renderer>();
            if(pages.Length < 4)
            {
                return (null, null);
            }
            return (pages[0], pages[2]);
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
        public void ChangeRenderQueueImage(ref Renderer renderer, int r, Color c)
        {
            renderer = renderer.GetComponentsInChildren<Renderer>()[1]; //obtain the image
            if(renderer == null)
            {
                Debug.Log("image was not obtained");
            }
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



        public void ChangeLayerLeft(int rotateIndex, int index)
        {
          Color c = Color.white;
         //  onBorderCheck.RunAction(this); //obtain whether is left or right page movement

           //new idea ==> 
           Renderer renderer =  pageObjects[index].GetComponent<Renderer>();
           ChangeRenderQueue(ref renderer, 3000, Color.blue);
            
           if(GetChildrenOfRotateIndex(rotateIndex).Item1 == null || GetChildrenOfRotateIndex(rotateIndex).Item2 == null)
           {
            return;
           }
           if(!isLeft)
           {
               
             Renderer frontPage = GetChildrenOfRotateIndex(rotateIndex).Item1;
             if(rotateIndex == 1)
             {
                ChangeRenderQueueImage(ref frontPage, 3100, c);
                return;
             }
            int max = rotatePageObjects.Count; 
            if(rotateIndex > max - 2)
            {
                return;
            }
          // Renderer frontPage = GetChildrenOfRotateIndex(rotateIndex).Item1; //frontpage
           Renderer backPage = GetChildrenOfRotateIndex(rotateIndex - 1).Item2;

           previousNeighbourIndex = neighbourIndex;
           neighbourIndex = rotateIndex;
           frontPage = GetChildrenOfRotateIndex(neighbourIndex).Item1;
           ChangeRenderQueueImage(ref frontPage, 3100, c);
           ChangeRenderQueueImage(ref backPage, 3100,  c);
           }
           else //is left
           {
             Renderer frontPage = GetChildrenOfRotateIndex(rotateIndex).Item1;
             if(rotateIndex == 1)
             {
                ChangeRenderQueueImage(ref frontPage, 3100, c);
                return;
             }
             
            previousNeighbourIndex = neighbourIndex;
            neighbourIndex = rotateIndex - 1;
            Renderer backPage = GetChildrenOfRotateIndex(neighbourIndex).Item2;

           ChangeRenderQueueImage(ref frontPage, 3100, c);
           ChangeRenderQueueImage(ref backPage, 3100, c);
           }
          
        

        }
        public void ChangeOverLayDown(int i) //i hate this
        {
            if(previousNeighbourIndex == -1)
            {
                return;
            }
            Renderer neighbour = pageObjects[previousNeighbourIndex].GetComponent<Renderer>();
            Renderer current = pageObjects[i].GetComponent<Renderer>();
            ChangeRenderQueueImage(ref neighbour, 3000, Color.white);
            ChangeRenderQueueImage(ref current, 3000, Color.white);


            
        }
        public void ChangeLayerDown(int i)
        {
           Renderer r = pageObjects[i].GetComponent<Renderer>();
           ChangeRenderQueue(ref r, 2000, Color.grey);
        }
       
        public void ChangeBorderLeft()
        {
            Renderer r = rotatePageObjects[0].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r, 3300, Color.red);
        
        }
        public void NoBorder()
        {
            Renderer r = rotatePageObjects[0].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r, 3000, Color.white);

            Renderer r2 = rotatePageObjects[^1].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r2, 3000, Color.white);
        }
        public void ChangeBorderRight()
        {
            Renderer r = rotatePageObjects[^1].GetComponentInChildren<Renderer>();
            ChangeRenderQueue(ref r, 3200, Color.red);

         //   rotatePageObjects[rotatePageObjects.Count - 1].GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 0, 0 , 1));
        }
        public void SetTextureToPage(int index, Texture tex)
        {
            Renderer pageObjectRenderer = pageObjects[index].GetComponent<Renderer>();
            Renderer overlayImage = pageObjectRenderer.GetComponentsInChildren<Renderer>()[1];
            ChangeTexture(ref overlayImage, tex);
        }

        private IEnumerator FlipPageAnimation(int index, float startAngle, float endAngle)
        {
            int nindex = index;
            onFlipPage.RunAction(this);
            isPageMoving = true;
            float time = 0.0f;
            bool runOnce = false;
            Debug.Log("FLIPPING ANIMATION IS GOING");
            while(time < 1.0f)
            {
                if((Mathf.Floor(time * 10f) / 10f) == 0.5 && runOnce == false)
                {
                    onFlipAt90Degrees.RunAction((nindex,this)); 
                    runOnce = true;
                }
                Debug.Log("going! " + flipPageSpeed);
                time += Time.deltaTime / flipPageSpeed;
                float theta = Mathf.Lerp(startAngle, endAngle, time);
                rotatePageObjects[index].transform.localEulerAngles = new Vector3(0 ,theta, 0);
                yield return new WaitForFixedUpdate();
            }
            rotatePageObjects[index].transform.localEulerAngles = new Vector3(0, endAngle, 0);
            onAfterFlipPage.RunAction((nindex,this));
            isPageMoving = false;
            
            yield return null;
            
        }
        
    
    }
}
