using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// メニューのロジック
/// </summary>
public class MenuController : MonoBehaviour
{
    // メニューのアニメーター
    [SerializeField]
    private Animator animator;
    // メニューが開かれているかどうか
    private bool IsMenu;
    

    // Start is called before the first frame update
    void Start()
    {
        IsMenu = false;
    }

    void OnTriggerEnter(Collider t)
    {
        Debug.Log(t.gameObject.name);
    }

    /// <summary>
    /// Ons the click.
    /// </summary>
    public void OnClick(int id)
    {
        Debug.Log("押された!"+ id);  // ログを出力s

        switch (id)
        {

            case 0:// メニューボタン
                IsMenu = !IsMenu;
                animator.SetBool("IsOpen", IsMenu);
                break;
            case 1:// イベント追加
                Debug.Log("イベント追加！");
                SceneManager.LoadScene("Create_Event");
                break;
            case 2:// カレンダー
                Debug.Log("カレンダー画面遷移！");
                SceneManager.LoadScene("Calendar_Menu");
                break;
            case 3:// グラフ
                Debug.Log("グラフ画面遷移！");
                SceneManager.LoadScene("Graph");
                break;
            case 4:// 持ち物
                Debug.Log("持ち物画面遷移！");
                DialogHandler.ShowDialog("ゴメンネ！", "まだこの機能は実施されてないよ！");
                break;
            case 5:// アプリ
                Debug.Log("アプリモード起動！");
                DialogHandler.ShowDialog("ゴメンネ！", "まだこの機能は実施されてないよ！");
                break;
            case 6:// ショップ
                Debug.Log("ショップ画面遷移!");
                DialogHandler.ShowDialog("ゴメンネ！", "まだこの機能は実施されてないよ！");
                break;
            default:
                Debug.Log("おいｨ！？");
                break;
        }

        

    }

}
