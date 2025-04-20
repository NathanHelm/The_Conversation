using System.Collections;
using UnityEngine;
public class LedgerAnimationsManager : StaticInstance<LedgerAnimationsManager> {

    public static SystemActionCall<LedgerAnimationsManager> onEnableHand = new SystemActionCall<LedgerAnimationsManager>();
      public static SystemActionCall<LedgerAnimationsManager> onHalwayToNewPosition = new SystemActionCall<LedgerAnimationsManager>();
  
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
       Vector3 currenRightHandPosition = rightHandObj.transform.position;

       MoveToNewPosition(rightHandObj.transform, currenRightHandPosition, farthestLeft);
    }
    public void MoveHandToRightPage() //from left to right
    {
        Vector3 currentRightHandPosition = rightHandObj.transform.position;

        MoveToNewPosition(rightHandObj.transform, currentRightHandPosition, farthestRight);
    }
    public void PointAtPage(int index)
    {
        if(index == 0)
        {

        }
    }
    public void MoveToNewPosition(Transform trans, Vector2 oldPosition,Vector2 newPos)
    {
        if(single != null)
        {
            StopCoroutine(single);
            single = null;
        }
        StartCoroutine(single = MoveToNewPositionAnimation(trans, oldPosition, newPos));
    }
    public IEnumerator MoveToNewPositionAnimation(Transform movingTrans, Vector2 oldPosition, Vector2 newPos)
    {
        float time = flipPageAnimationSpeed;
        bool runOnce = false;
        while(time < 1)
        {
            
            if((Mathf.Floor(time * 10f) / 10f) == 0.5 && runOnce == false)
            {  
                runOnce = true;
            }
            Vector3 position =  Vector3.Lerp(oldPosition,newPos,time);
            movingTrans.position = position;
            yield return new WaitForFixedUpdate();    
        }
        yield return null;
    }

}