using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 会話中に表情を変更する際に使用するデータ
/// </summary>
[Serializable]
public class StatChangeTiming
{
    // 変更する時間
    [SerializeField]
    float timing;

    // 変更する表情
    [SerializeField]
    string faceId;

    public float Timing
    {
        get { return timing; }
        set { timing = value; }
    }

    public string FaceId
    {
        get { return faceId; }
        set { faceId = value; }
    }

}
