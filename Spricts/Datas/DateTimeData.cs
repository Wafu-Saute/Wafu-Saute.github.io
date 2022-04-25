using System;
using UnityEngine;

/// <summary>
///  トークデータで使用するデータの一つ
///  日付指定の会話データを作成する場合、
///  こちらを使用します。
///  (例：1月1日に何か発言させたい場合)
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "DateTime", menuName = "CreateData/CreateDateTime")]
public class DateTimeData : ScriptableObject
{
    /// <summary>
    /// 日付指定の名前
    /// </summary>
    [SerializeField]
    private string _name;

    /// <summary>
    /// 日付指定のID
    /// </summary>
    [SerializeField]
    private int _id;

    /// <summary>
    /// 指定日付
    /// </summary>
    [SerializeField]
    private SerializableDate _date = DateTime.Now;

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

    public SerializableDate Date
    {
        get { return _date; }
        set{ _date = value; }
    }

}
