using UnityChan;
using UnityEngine;

/// <summary>
/// キャラの表情やアニメーションの変更スプリクト
/// </summary>
public class ChangeBlendShapes : MonoBehaviour
{
    // キャラのアニメーター
    [SerializeField]
    Animator animator;
    // 自動瞬き用のクラス
    [SerializeField]
    AutoBlink autoBlink;
    // 表情(名前、id)
    [SerializeField]
    StringIntDictionary face;
    // アニメーション(名前、id)
    [SerializeField]
    StringIntDictionary anim;

    /// <summary>
    /// キー情報をもとに表情を変更するメソッド
    /// </summary>
    /// <param name="val">キー情報</param>
    public void SetBlendShape(string val)
    {
        if(face.ContainsKey(val))
        {
            autoBlink.enabled = false;
            animator.SetInteger("FaceId", face[val]);
        }
        else if(anim.ContainsKey(val))
        {
            animator.SetInteger("IdolNumber", anim[val]);
            animator.SetTrigger("AnimStart");
        }
        else
        {
            Debug.LogError("指定されたキーがありません。");
        }
    }

    /// <summary>
    /// 表情のリセット
    /// </summary>
    public void ResetBlendShape()
    {
        autoBlink.enabled = true;
        animator.SetInteger("FaceId", face["[face:normal]"]);
        animator.SetInteger("IdolNumber", 0);
    }

}
