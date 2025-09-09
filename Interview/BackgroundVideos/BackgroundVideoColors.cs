using UnityEngine;
using UnityEngine.Video;
[System.Serializable]
public class BackgroundVideoColors //use for scriptable object
{
    [SerializeField]
    private float hue;
    [SerializeField]
    private float saturation;
    [SerializeField]
    private float brightness;
    [SerializeField]
    private Vector4 hsbSteps;

    //yes, im finally doing this shit 
    public Vector4 HsbSteps
    {
        get { return hsbSteps; }
        set { hsbSteps = value; }
    }

    public float Hue
    {
        get { return hue; }
        set { hue = value; }
    }

    public float Saturation
    {
        get { return saturation; }
        set { saturation = value; }
    }

    public float Brightness
    {
        get { return brightness; }
        set { brightness = value; }
    }


}