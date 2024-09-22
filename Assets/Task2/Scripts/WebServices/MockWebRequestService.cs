using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockWebRequestService : WebRequestService
{
    private Dictionary<string, ModelData> _modelDataList = new Dictionary<string, ModelData>
    {
        {
            "1",
            new ModelData
            {
                attributes = new string[]
                {
                    "wood", "wine", "container", "vintage",
                    "barrel", "old", "cellar", "storage"
                },
                model_name = "SM_Prop_Barrel_01",
                position = new Vector3(-1.6f, 0.75f, 1.5f)
            }
        },
        {
            "3",
            new ModelData
            {
                attributes = new string[]
                {
                    "music", "sound", "audio", "voice",
                    "singing", "karaoke"
                },
                model_name = "SM_Prop_Microphone_01",
                position = new Vector3(-1.6f, 0.75f, 0)
            }
        },
        {
            "6",
            new ModelData
            {
                attributes = new string[]
                {
                    "virtual reality", "gaming", "technology", "simulation",
                    "wearable"
                },
                model_name = "SM_Prop_VR_Headset_01",
                position = new Vector3(-1.6f, 0.75f, -1.5f)
            }
        }
    };

    public override void GetModelByID(string id, System.Action<ModelData> callback)
    {
        if (_modelDataList.ContainsKey(id))
        {
            callback(_modelDataList[id]);
        }
        else
        {
            callback(null);
        }
    }
}
