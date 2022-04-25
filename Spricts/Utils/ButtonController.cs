using UnityEngine;

/// <summary>
/// タップかロングタップかなどを判別するスプリクト
/// </summary>
public class ButtonController
{
    // 押し始めた時間
    private float startTime;     

    public bool IsTapped{ get; set; }

    /// <summary>
    /// タップやロングタップを検出し
    /// 文字列で返すメソッド
    /// </summary>
    /// <returns></returns>
    public string GetControllerButton()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("タップ");
                return "タップ";
            }
            if (Input.touchCount > 1000)
            {
                Debug.Log("ロングタップ");
                return "ロングタップ";
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (startTime <= 0)
            {
                startTime = Time.time;
                Debug.Log("マウスクリック = タップ");
                return "タップ";
            }
            else if (Time.time - startTime >= 1)
            {
                Debug.Log("マウスクリック"+(Time.time - startTime).ToString()+"秒 = ロングタップ");
                return "ロングタップ";
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            startTime = -1;
        }

        return "";
    }
}
