using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscureEffect : MonoBehaviour {
    //the material depth shader
    public Material depthShaderMat;
    //the threshold of atmosphere color change after which this script will update the atmospheric shader
    public float thresholdAtmosphereColorChangeToUpdate;

    Color lastColorSet = new Color(0, 0, 0);
    // Update is called once per frame
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Color currCamBackCol = GetComponent<Camera>().backgroundColor;
        if (Mathf.Abs(thresholdAtmosphereColorChangeToUpdate) > Mathf.Epsilon)
        {            
            Vector3 lastColorVec = new Vector3(lastColorSet.r, lastColorSet.g, lastColorSet.b);
            Vector3 currColorVec = new Vector3(currCamBackCol.r, currCamBackCol.g, currCamBackCol.b);

            if (Vector3.SqrMagnitude(lastColorVec - currColorVec) > thresholdAtmosphereColorChangeToUpdate * thresholdAtmosphereColorChangeToUpdate)
            {
                depthShaderMat.SetColor("_skyColor", currCamBackCol);
                lastColorSet = currCamBackCol;
            }
        }
        else
        {
            depthShaderMat.SetColor("_skyColor", currCamBackCol);
        }       
        RenderTexture dest = new RenderTexture(source.width, source.height, 0);
        Graphics.Blit(source, destination, depthShaderMat);
    }
}
