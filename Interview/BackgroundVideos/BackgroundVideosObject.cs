using UnityEngine;
using UnityEngine.Video;
[System.Serializable]
public class BackgroundVideosObject //use for scriptable object
{

    public BackgroundVideoColors[] backgroundVideoColors;
    [SerializeField]
    private VideoClip videoClip;
    public VideoClip VideoClip
    {
        get { return videoClip; }
        set { videoClip = value; }
    }

}