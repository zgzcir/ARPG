using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ViewSvc : MonoBehaviour
{
    public static ViewSvc Instance;
    public void InitSvc()
    {
        Instance = this;

        CommonTool.Log("ViewSvc Connected");
    }

    public PostProcessVolume ProcessVolume;

    public void AdjustDepthFieldFL(float value)
    {
        ProcessVolume.profile.GetSetting<DepthOfField>().focalLength.value = new FloatParameter
        {
            value = value
            
        };
    }

    public void SetCursorState(bool visible=true)
    {
        Cursor.visible = visible;
    }
    
}
