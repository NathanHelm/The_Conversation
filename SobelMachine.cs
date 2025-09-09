using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SobelMachine : MonoBehaviour
{
    [SerializeField]
    public Material sobelMat;
    [SerializeField]
    SobelSetting sobelSetting;

    public void Update()
    {
        /*
        sobelMat.SetFloat("_OutlineThickness", sobelSetting.thickness);
        sobelMat.SetFloat("_OutlineDepthMultiplier", sobelSetting.depthMultiplier);
        sobelMat.SetFloat("_OutlineDepthBias", sobelSetting.depthBias);
        sobelMat.SetFloat("_OutlineNormalMultiplier", sobelSetting.normalMultiplier);
        sobelMat.SetFloat("_OutlineNormalBias", sobelSetting.normalBias);
        sobelMat.SetFloat("_NoiseStrength", sobelSetting.noiseStrength);
        sobelMat.SetColor("_OutlineColor", sobelSetting.color);
        sobelMat.SetFloat("_WiggleFrequency", sobelSetting.freq);
        sobelMat.SetFloat("_WiggleAmplitude", sobelSetting.amp);
        if (sobelMat.HasTexture("_BackgroundTex"))
        {
            sobelMat.SetTexture("_BackgroundTex", sobelSetting.tex);
        }
        */

    }
    public void Start()
    {
       // Camera.main.depthTextureMode |= DepthTextureMode.DepthNormals;
    }
    public void SetSobelFilter(ref Material mat)
    {
        mat.SetFloat("_OutlineThickness", sobelSetting.thickness);
        mat.SetFloat("_OutlineDepthMultiplier", sobelSetting.depthMultiplier);
        mat.SetFloat("_OutlineDepthBias", sobelSetting.depthBias);
        mat.SetFloat("_OutlineNormalMultiplier", sobelSetting.normalMultiplier);
        mat.SetFloat("_OutlineNormalBias", sobelSetting.normalBias);
        mat.SetFloat("_NoiseStrength", sobelSetting.noiseStrength);
        mat.SetColor("_OutlineColor", sobelSetting.color);
        mat.SetFloat("_WiggleFrequency", sobelSetting.freq);
        mat.SetFloat("_WiggleAmplitude", sobelSetting.amp);
        if (mat.HasTexture("_BackgroundTex"))
        {
            mat.SetTexture("_BackgroundTex", sobelSetting.tex);
        }
    }

}
[Serializable]
public class SobelSetting
{
    public float thickness;
    public float depthMultiplier;
    public float depthBias;
    public float normalMultiplier;
    public float normalBias;
    public Color color;

    public float noiseStrength;

    public float freq;
    public float amp;
    public Texture2D tex;

} 
