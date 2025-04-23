using System.Collections;
using Data;
using UnityEngine;
public class LedgerMovement : StaticInstance<LedgerMovement> {

    public SystemActionCall<LedgerMovement> onEnableHand = new SystemActionCall<LedgerMovement>();
    public SystemActionCall<LedgerMovement> onHalwayToNewPosition = new SystemActionCall<LedgerMovement>();
    
    public SystemActionCall<LedgerMovement> onAfterCreateHands = new SystemActionCall<LedgerMovement>();
   // === add animations to these ==========================================================================================================
    public SystemActionCall<LedgerMovement> onMove = new SystemActionCall<LedgerMovement>();
    public SystemActionCall<LedgerMovement> onPointHand = new SystemActionCall<LedgerMovement>();
    // ==================================================================================================================================

    [SerializeField]
    private HandScriptableObject handScriptableObject;

    public bool isLeft {get; set;}

    public int pageObjectIndex {get; set;}
    
    public float flipPageAnimationSpeed;

    private float zFrequency = 0.5f, zAmplitude = 0.7f; 



    public Vector2 farthestRight {get; set;}

    public Vector2 farthestLeft {get; set;}

    public Vector2 leftPageObjectPosition {get; set;}
    public Vector2 rightPageObjectPosition {get; set;}



    private GameObject leftHandPrefab,rightHandPrefab;
    public GameObject leftHandObj {get; set;}
    public GameObject rightHandObj {get; set;}

    private GameObject handsObject;

    public IEnumerator single;

    public override void m_Start()
    {
        farthestRight = handScriptableObject.farthestRight;
        farthestLeft = handScriptableObject.farthestLeft;

        leftHandPrefab = handScriptableObject.leftHandPrefab;
        rightHandPrefab = handScriptableObject.rightHandPrefab;

        leftPageObjectPosition = handScriptableObject.leftPageObjectPosition;
        rightPageObjectPosition = handScriptableObject.rightPageObjectPosition;
    }
    public void CreateHands()
    {
        if (GameObject.FindGameObjectWithTag("Hands") != null)
        {
            handsObject = GameObject.FindGameObjectWithTag("Hands");
        }
        else
        {
            Debug.LogError("hands gameobject is null");
        }
            
       leftHandObj = Instantiate(leftHandPrefab, handsObject.transform);
       rightHandObj = Instantiate(rightHandPrefab,handsObject.transform);
       LedgerData.INSTANCE.leftHand = leftHandObj.GetComponent<Animator>();
       LedgerData.INSTANCE.rightHand = rightHandObj.GetComponent<Animator>();
       onAfterCreateHands.RunAction(this);
    }

 
    public void EnableHand()
    {
        onEnableHand.RunAction(this);
        if(leftHandObj == null || rightHandObj == null)
        {
           CreateHands();
        }
        leftHandObj.SetActive(true);
        rightHandObj.SetActive(true);
    }

    public void DisableHand()
    {
        if(leftHandObj == null || rightHandObj == null)
        {
           CreateHands();
        }
        leftHandObj.SetActive(false);
        rightHandObj.SetActive(false);
    }
    public void MoveHandToLeftPage() //from right to left
    {
        //we are using the right hand, moving from the rightpage to the lefpage
       Vector3 currenRightHandPosition = farthestRight;
       MoveToNewPosition(rightHandObj.transform, currenRightHandPosition, farthestLeft, flipPageAnimationSpeed, 800f);
    }
    public void MoveHandToRightPage() //from left to right
    {
        Vector3 currentRightHandPosition = farthestLeft;
        MoveToNewPosition(rightHandObj.transform, currentRightHandPosition, farthestRight, flipPageAnimationSpeed, 800f);
    }

    public void MoveHandAwaitPoint()
    {
        StartCoroutine(MoveHandAndPointAnimation());
    }
   
    public void MoveHand()
    {
        onMove.RunAction(this);
        if(!isLeft)
        {
            MoveHandToLeftPage();
        }
        else
        {
            MoveHandToRightPage();
        }
    }
    
    public void HandPointAtPage()
    {
        if(single != null)
        {
            return;
        }
        onPointHand.RunAction(this);
       
        Vector2 moveToPagePosition = rightPageObjectPosition;

        Vector3 currentRightHandPosition = rightHandObj.transform.localPosition;

        
        if(pageObjectIndex % 2 == 0)
        {
            moveToPagePosition = leftPageObjectPosition;
        }
        else
        {
            moveToPagePosition = rightPageObjectPosition;
        }

        MoveToNewPosition(rightHandObj.transform, currentRightHandPosition, moveToPagePosition, flipPageAnimationSpeed * 0.5f, 200f);

    }
    public void MoveToNewPosition(Transform trans, Vector2 oldPosition,Vector2 newPos, float speed, float radius)
    {
        if(single != null) //allow for an older coroutine to stop running if a new one is running. 
        {
            StopCoroutine(single);
        }
        StartCoroutine(single = MoveToNewPositionAnimation(trans, oldPosition, newPos, speed, radius));
    }
    
    public IEnumerator MoveToNewPositionAnimation(Transform movingTrans, Vector2 oldPosition, Vector2 newPos, float speed, float radius)
    {
        float time = 0.0f;
        bool runOnce = false;
        while(time < 1)
        {
            time += Time.deltaTime / speed;
            if((Mathf.Floor(time * 10f) / 10f) == 0.5 && runOnce == false)
            { 
                onHalwayToNewPosition.RunAction(this); 
                runOnce = true;
            }
            Vector3 position =  Vector3.Lerp(oldPosition,newPos,time);

            Vector3 rainbowCurve = RainbowCurve(time, radius);

            float rCZ = rainbowCurve.z;
            movingTrans.localPosition = position + new Vector3(0, 0, rCZ);

            yield return new WaitForFixedUpdate();    
        }
        movingTrans.localPosition = newPos;
        single = null;
        yield return null;
    }
    public Vector3 RainbowCurve(float t, float radius) //ouh i like this
    {

        float theta = Mathf.Lerp(Mathf.PI * 2, Mathf.PI, t);

        float x = Mathf.Cos(theta);
        float z = Mathf.Sin(theta);
        
        return new Vector3(x, 0, z) * radius;
    }

    public IEnumerator MoveHandAndPointAnimation()
    {
        //TODO change it up and make this a statemachine
        //1) flip page
        MoveHand();

        yield return new WaitUntil(() => single == null);
        Debug.Log("pointing hand!");

       //TODO 
       GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
    }

}