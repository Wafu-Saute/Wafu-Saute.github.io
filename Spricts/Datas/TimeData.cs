using System;
using UnityEngine;

/// <summary>
///  トークデータで使用するデータの一つ
///  時間指定で会話を作成する場合、
///  こちらを使用します。
///  (例：17:00に何か発言させたい場合)
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Time", menuName = "CreateData/CreateTime")]
public class TimeData : ScriptableObject
{
    /// <summary>
    /// 時間指定の名前
    /// </summary>
    [SerializeField]
    private string _name;

    /// <summary>
    /// 時間指定のID
    /// </summary>
    [SerializeField]
    private int _id;

    /// <summary>
    /// 指定時間
    /// </summary>
    [SerializeField]
    private SerializableTime _time = DateTime.Now;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public SerializableTime Time
    {
        get { return _time; }
        set { _time = value; }
    }
}
