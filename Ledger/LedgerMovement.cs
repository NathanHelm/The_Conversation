using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Data;
using Unity.Mathematics;
using UnityEngine;
public class LedgerMovement : StaticInstance<LedgerMovement> {

    public SystemActionCall<LedgerMovement> onEnableHand = new SystemActionCall<LedgerMovement>();
    public SystemActionCall<LedgerMovement> onHalwayToNewPosition = new SystemActionCall<LedgerMovement>();
    
    public SystemActionCall<LedgerMovement> onAfterCreateHands = new SystemActionCall<LedgerMovement>();
    
   // === add animations to these ==========================================================================================================
    public SystemActionCall<LedgerMovement> onMove = new SystemActionCall<LedgerMovement>();
    public SystemActionCall<LedgerMovement> onPointHand = new SystemActionCall<LedgerMovement>();
     
     public SystemActionCall<LedgerMovement> onWritingHand = new SystemActionCall<LedgerMovement>();
     public SystemActionCall<LedgerMovement> onAfterWritingHand = new SystemActionCall<LedgerMovement>();

     public SystemActionCall<LedgerMovement> onAfterFlipAwait = new SystemActionCall<LedgerMovement>();
    // ==================================================================================================================================

    [SerializeField]
    private HandScriptableObject handScriptableObject;

    public bool isLeft {get; set;}

    public int pageObjectIndex {get; set;}
    
    public float flipPageAnimationTime;

    private float zFrequency = 0.5f, zAmplitude = 0.7f; 
    private float drawingFreq = 50f, drawingAmp = 90f;

    private Vector2 pageWriteStartOffset, pageWriteEndOffset;

    private Vector3 leftHandBasePostion;


    public Vector2 farthestRight {get; set;}

    public Vector2 farthestLeft {get; set;}

    public Vector2 leftPageObjectPosition {get; set;}
    public Vector2 rightPageObjectPosition {get; set;}



    private GameObject leftHandPrefab,rightHandPrefab;
    public GameObject leftHandObj {get; set;}
    public GameObject rightHandObj {get; set;}

    private GameObject handsObject;

    public Stack<IEnumerator> moveToPosStack = new(), writeStack = new();
    public IEnumerator recentCoroutine; 

    public override void m_Start()
    {
        farthestRight = handScriptableObject.farthestRight;
        farthestLeft = handScriptableObject.farthestLeft;

        leftHandPrefab = handScriptableObject.leftHandPrefab;
        rightHandPrefab = handScriptableObject.rightHandPrefab;

        leftPageObjectPosition = handScriptableObject.leftPageObjectPosition;
        rightPageObjectPosition = handScriptableObject.rightPageObjectPosition;

        pageWriteStartOffset = handScriptableObject.pageWriteStartOffset;
        pageWriteEndOffset = handScriptableObject.pageWriteEndOffset;

        leftHandBasePostion = handScriptableObject.leftHandBasePosition;
    }
    public void StopMoveRecentState()
    {
        if(recentCoroutine == null)
        {
            return;
        }
        StopCoroutine(recentCoroutine);
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
       
       LedgerData.INSTANCE.leftHandAnim = leftHandObj.GetComponent<Animator>();
       LedgerData.INSTANCE.rightHandAnim = rightHandObj.GetComponent<Animator>();

       LedgerData.INSTANCE.leftHandObj = leftHandObj;
       LedgerData.INSTANCE.rightHandObj = rightHandObj;

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
    //==left hand functions=======================================================================================================================================
    public void MoveHandLeft()
    {
        //new position : ) 
        var currentPos = leftHandObj.transform.position;
        
        MoveToNewPosition(leftHandObj.transform, currentPos, leftHandBasePostion, flipPageAnimationTime * 2,600f);
    }
    //================================================================================================================================================

    public void MoveHandToLeftPage() //from right to left
    {
        //we are using the right hand, moving from the rightpage to the lefpage
       Vector3 currenRightHandPosition = farthestRight;
       MoveToNewPositionFlip(rightHandObj.transform, currenRightHandPosition, farthestLeft, flipPageAnimationTime, 800f);
    }
    public void MoveHandToRightPage() //from left to right
    {
        Vector3 currentRightHandPosition = farthestLeft;
        MoveToNewPositionFlip(rightHandObj.transform, currentRightHandPosition, farthestRight, flipPageAnimationTime, 800f);
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
        onPointHand.RunAction(this);
       
        Vector2 moveToPagePosition = rightPageObjectPosition;

        Vector3 currentRightHandPosition = rightHandObj.transform.localPosition;

        
        if(pageObjectIndex % 2 == 0)
        {
            moveToPagePosition = rightPageObjectPosition;
        }
        else
        {
            moveToPagePosition = leftPageObjectPosition;
        }

        MoveToNewPosition(rightHandObj.transform, currentRightHandPosition, moveToPagePosition, flipPageAnimationTime * .5f, 200f);

    }
    
    public void HandWriting()
    {
        
        onWritingHand.RunAction(this);
       
        Vector2 moveToPagePosition = leftPageObjectPosition + pageWriteEndOffset;

        Vector3 currentRightHandPosition = leftPageObjectPosition + pageWriteStartOffset;

        
        if(pageObjectIndex % 2 == 0 && pageObjectIndex != 0)
        {

            moveToPagePosition = rightPageObjectPosition +  pageWriteEndOffset;
            currentRightHandPosition = rightPageObjectPosition + pageWriteStartOffset;
     
        }

        MoveToNewPositionWriting(rightHandObj.transform, currentRightHandPosition, moveToPagePosition, flipPageAnimationTime * 1.9f);

    }
    public void MoveToNewPosition(Transform trans, Vector3 oldPosition,Vector3 newPos, float speed, float radius)
    {
        recentCoroutine = MoveToNewPositionAnimation(trans, oldPosition, newPos, speed, radius,false);
        StartCoroutine(recentCoroutine);
    }
    public void MoveToNewPositionFlip(Transform trans, Vector2 oldPosition,Vector2 newPos, float speed, float radius)
    {
        recentCoroutine = MoveToNewPositionAnimation(trans, oldPosition, newPos, speed, radius,true);
        moveToPosStack.Push(recentCoroutine);
        StartCoroutine(recentCoroutine);
    }

    public void MoveToNewPositionWriting(Transform trans, Vector2 oldPosition, Vector2 newPos, float speed)
    {
        recentCoroutine = WritingMovementAnimation(trans, oldPosition, newPos, speed);
        writeStack.Push(recentCoroutine);
        StartCoroutine(recentCoroutine);
    }


    private IEnumerator MoveToNewPositionAnimation(Transform movingTrans, Vector3 oldPosition, Vector3 newPos, float speed, float radius, bool isFlip)
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
        if(isFlip)
        {
        moveToPosStack.Pop();
        }
        yield return null;
    }
    private Vector3 RainbowCurve(float t, float radius) //ouh i like this
    {

        float theta = Mathf.Lerp(Mathf.PI * 2, Mathf.PI, t);

        float x = Mathf.Cos(theta);
        float z = Mathf.Sin(theta);
        
        return new Vector3(x, 0, z) * radius;
    }

    private IEnumerator WritingMovementAnimation(Transform movingTrans, Vector2 oldPosition, Vector2 newPos, float speed)
    {
        float time = 0.0f;
        while(time < 1)
        {
            time += Time.deltaTime / speed;
          
            Vector3 position =  Vector3.Lerp(oldPosition,newPos,time);

            
            float t = Mathf.SmoothStep(0, 1, time);
            float freqLerp = Mathf.Lerp(0, drawingFreq, t);

            Debug.Log("freq lerp" + freqLerp + "time" + time);

            float drawingWave = SinWave(drawingAmp,freqLerp);

            Debug.DrawRay(movingTrans.localPosition + position + new Vector3(0, 0, 90), movingTrans.localPosition + position + new Vector3(0, drawingWave, 0) + new Vector3(0, 0, 90), Color.red, 10f);

            movingTrans.localPosition = position + new Vector3(0, drawingWave, 0);

            yield return new WaitForFixedUpdate();    
        }
        movingTrans.localPosition = newPos;
        onAfterWritingHand.RunAction(this);
        writeStack.Pop();
        yield return null;
    }
    private float SinWave(float amp, float freq)
    {
        return Mathf.Sin(freq) * amp;
    }
    private Vector2 RotatePos(Vector2 currentPosition, float theta)
    {
       
       float x = currentPosition.x; 
       float y = currentPosition.y; 

       float2x2 matrixA = new float2x2(Mathf.Cos(theta), -Mathf.Sin(theta),
       Mathf.Sin(theta), Mathf.Cos(theta)
       );

       float x2 = matrixA.c0.x * x +  matrixA.c1.x * y;
       float y2 = matrixA.c0.y * x +  matrixA.c1.y * y;

       return new Vector2(x2, y2);
    }

    private IEnumerator MoveHandAndPointAnimation()
    {
        //TODO change it up and make this a statemachine
        //1) flip page
        MoveHand();
        yield return new WaitUntil(() => moveToPosStack.Count == 0);
        Debug.Log("pointing hand!");

       //TODO 
       onAfterFlipAwait.RunAction(this);

    }
    public bool IsFlipPageCoroutineRunning()
    {
        if(moveToPosStack.Count == 0)
        {
            return false;
        }
        return true;
    }
    

}