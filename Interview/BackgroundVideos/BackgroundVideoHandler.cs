using System.Collections;
using System.Collections.Generic;
using Data;
using ObserverAction;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundVideoHandler : MonoBehaviour, IExecution, IObserver<ObserverAction.StateMachineAction>
{
    private BackgroundVideosObject[] backgroundVideoObjects;
    private VideoPlayer[] videoPlayers;
    [SerializeField]
    private BackgroundVideoScriptableObject backgroundVideoScriptableObject;
    private Material[] backgroundFacesMaterial;
    private Material posterizationMaterial;
    private GameObject videoPlayerPrefab;
     
    //get video players 
    //based on character id, get an index j 
    //set i's video clip to video clip j
    //set video clip's rt to 

    public void m_Awake()
    {

    }

    public void m_OnEnable()
    {
        StateManager.INSTANCE?.subject.AddObserver(this);
    }

    public void m_GameExecute()
    {

        backgroundFacesMaterial = backgroundVideoScriptableObject.backgroundFacesMaterial;
        posterizationMaterial = backgroundVideoScriptableObject.posterizationMaterial;
        videoPlayerPrefab = backgroundVideoScriptableObject.videoPlayerPrefab;
        backgroundVideoObjects = backgroundVideoScriptableObject.backgroundVideosObjects;
        //SpawnVideos();
        videoPlayers = FindObjectsOfType<VideoPlayer>();
    }
    private void SpawnVideos() //TODO apply this function
    {
        int l = backgroundFacesMaterial[0].GetInt("_ImageAmount");
        videoPlayers = new VideoPlayer[l + 1];

        for (int i = 0; i <= l; i++)
        {
            videoPlayers[i] = Instantiate(videoPlayerPrefab, GameObject.FindGameObjectWithTag("VideoPlayerParent").transform).GetComponent<VideoPlayer>();
            videoPlayers[i].targetTexture = new RenderTexture(500, 500, 24);
        }
    }

    private void SetVideoClip(ref VideoPlayer videoPlayer, VideoClip videoClip)
    {
        videoPlayer.clip = videoClip;
    }
    private void SetMaterialToRT(ref VideoPlayer videoPlayer, BackgroundVideosObject backgroundVideoObject, int faceIndex)
    {
        var videoRt = videoPlayer.targetTexture;
        //set posterization values
       
        if (faceIndex == 0)
        {
            foreach (var single in backgroundFacesMaterial)
            {
                int randCol = Random.Range(0, backgroundVideoObject.backgroundVideoColors.Length - 1);
                var color = backgroundVideoObject.backgroundVideoColors[randCol];

                single.SetTexture("_Face1", videoRt);
                single.SetFloat("_Hue1", color.Hue);
                single.SetFloat("_Sat1", color.Saturation);
                single.SetFloat("_Bright1", color.Brightness);
                single.SetVector("_HSBStep", color.HsbSteps);
            }
        }
        else if (faceIndex == 1)
        {
            foreach (var single in backgroundFacesMaterial)
            {
                int randCol = Random.Range(0, backgroundVideoObject.backgroundVideoColors.Length - 1);
                var color = backgroundVideoObject.backgroundVideoColors[randCol];

                single.SetTexture("_Face2", videoRt);
                single.SetFloat("_Hue2", color.Hue);
                single.SetFloat("_Sat2", color.Saturation);
                single.SetFloat("_Bright2", color.Brightness);
                single.SetVector("_HSBStep", color.HsbSteps);
            }
        }
        else
        {
            foreach (var single in backgroundFacesMaterial)
            {
                int randCol = Random.Range(0, backgroundVideoObject.backgroundVideoColors.Length - 1);
                var color = backgroundVideoObject.backgroundVideoColors[randCol];

                single.SetTexture("_Face3", videoRt);
                single.SetFloat("_Hue3", color.Hue);
                single.SetFloat("_Sat3", color.Saturation);
                single.SetFloat("_Bright3", color.Brightness);
                single.SetVector("_HSBStep", color.HsbSteps);
            }
    
        }
    }
 
    public void OnNotify(StateMachineAction data)
    {

        Debug.Log("Hello!");
        if (data == ObserverAction.StateMachineAction.onEnterInterviewScene)
        {

            int i = videoPlayers.Length - 1;
            int max = backgroundVideoObjects.Length;
            // int j = InterviewData.INSTANCE.characterID.GetHashCode() * 1000 % max;
            while (i >= 0)
            {

                int j = (InterviewData.characterID.GetHashCode() * 1000 + i) % max;

                int randCol = Random.Range(0, backgroundVideoObjects[j].backgroundVideoColors.Length - 1);
                var videoPlayer = videoPlayers[i];
                var video = backgroundVideoObjects[j].VideoClip;

                SetVideoClip(ref videoPlayer, video);

                SetMaterialToRT(ref videoPlayer, backgroundVideoObjects[j], i);

                i--;


            }

          //  SetRTtoBgMaterial();

          


        }
    }

}