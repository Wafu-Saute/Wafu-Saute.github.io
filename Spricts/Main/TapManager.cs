using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// キャラクターをタップしたかどうか判断し
/// タップしていれば会話を開始するスプリクト
/// </summary>
public class TapManager : MonoBehaviour
{
    // 会話データを管理しているクラス
    [SerializeField]
    private TalkDataManager talkDataManager;
    //カメラを取得
    [SerializeField]
    private Camera camera_object; 
    // プレイヤーのタグ
    [SerializeField]
    private static string playerTag = "Player";
    //レイキャストが当たったものを取得する入れ物
    private RaycastHit hit; 
    // タップした時の時間を格納
    private float setTime;

    // Update is called once per frame  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            setTime = Time.time;

        if (Input.GetMouseButtonUp(0))
        {
            if(Time.time - setTime < 1.0f)
            {
                Ray ray = camera_object.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入

                if (Physics.Raycast(ray, out hit) && !IsUGUIHit(Input.mousePosition))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
                {
                    string objectName = hit.collider.gameObject.tag; //オブジェクト名を取得して変数に入れる
                    Debug.Log(objectName); //オブジェクト名をコンソールに表示
                    if (objectName.Equals(playerTag))
                        talkDataManager.SetInTextController();
                }
            }

        }
    }

    /// <summary>
    /// ＵＩ越しでプレイヤーをタップしていないか確認
    /// </summary>
    /// <param name="_scrPos"></param>
    /// <returns></returns>
    public static bool IsUGUIHit(Vector3 _scrPos)
    { // Input.mousePosition
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = _scrPos;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        return (result.Count > 0);
    }


}
