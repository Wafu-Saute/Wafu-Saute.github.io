using System;
using UnityEngine;

/// <summary>
/// 会話のジャンルをここで作成します。
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Genre", menuName = "CreateData/CreateGenre")]
public class GenreData : ScriptableObject
{
    /// <summary>
    /// ジャンルの名前
    /// </summary>
    [SerializeField]
    private string _name;

    /// <summary>
    /// ジャンルのID
    /// </summary>
    [SerializeField]
    private int _id;

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

}
