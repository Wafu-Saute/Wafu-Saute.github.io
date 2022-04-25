using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 会話を開始、管理するスプリクト
/// </summary>
public class TextController : MonoBehaviour
{
    // 会話表示用
    [SerializeField]
    private Text contentText;   
    //キャラのアニメーター
    [SerializeField]
    private Animator anime;
    //セリフの再生用
    [SerializeField]
    private AudioSource audioSource;
    //表情変更
    [SerializeField]
    private ChangeBlendShapes CBS;
    // 1文字の表示にかける時間
    [SerializeField]
    [Range(0.001f, 0.3f)]
    private float intervalForCharDisplay = 0.05f;
    //テキストウィンドウ
    [SerializeField]
    private GameObject panel;

    //現在表示している文章番号
    private int currentSentenceNum = 0;
    // 表示中の文字数
    private int lastUpdateCharCount = -1;
    // 0: 単発式(読み上げると自動的に吹き出しが消えます) 1: 複数式(会話データがある限り喋ります)
    private int talkType;
    // 次に再生する表情のindex
    private int faceIndex;
    // 表示にかかる時間
    private float timeUntilDisplay = 0;
    // 文字列の表示を開始した時間
    private float timeBeganDisplay = 1;
    //セリフの再生時間
    private float time;
    // 会話の開始フラグ
    private bool startFlg;
    // 現在の文字列
    private string currentSentence = string.Empty;
    //シナリオを格納
    private string[] sentences;
    // 現在のステート状態を保存する参照
    private AnimatorStateInfo currentState;
    // ひとつ前のステート状態を保存する参照
    private AnimatorStateInfo previousState;        
    // 現在の会話データ
    private TalkData nowTalkData;
    // ボタンの状態を管理するクラス
    private ButtonController buttonController = new ButtonController();

    /// <summary>
    /// 会話を開始する
    /// </summary>
    /// <param name="talkData"></param>
    /// <param name="Type"></param>
    public void StartText(TalkData talkData, int Type)
    {
        nowTalkData = talkData;
        talkType = Type;
        string[] scenarios = new string[1] { talkData.TextContent };
        sentences = scenarios;
        audioSource.clip = talkData.AudioData;

        contentText.text = "";
        currentSentenceNum = 0;
        timeUntilDisplay = 0;

        previousState = anime.GetCurrentAnimatorStateInfo(0);

        //パネルを表示
        panel.SetActive(true);
        //テキストを表示
        contentText.gameObject.SetActive(true);
        // 文言・アニメーションの取得
        SetNextSentence();
        audioSource.Play();
        startFlg = true;
        faceIndex = 0;
    }

    public void Update()
    {
        // アニメ制御
        if (anime.GetBool("AnimFlg"))
        {
            // 現在のステートをチェックし、ステート名が違っていたらfalseに戻す
            currentState = anime.GetCurrentAnimatorStateInfo(0);
            if (previousState.fullPathHash != currentState.fullPathHash)
            {
                anime.SetBool("AnimFlg", false);
                previousState = currentState;
            }
        }

        string resultText = buttonController.GetControllerButton();
        CheckFace();
        // 文章の表示完了 / 未完了
        if (IsAudioStop() && startFlg && talkType == Const.TalkType.Multiple)
        {
            //最後の文章ではない & ボタンが押された
            if (currentSentenceNum < sentences.Length && resultText == "タップ")
            {
                SetNextSentence();
            }
            else if (currentSentenceNum >= sentences.Length && resultText == "タップ")
            {
                // 文章の最後まで行ったかつスペースを押したら初期化処理を実行
                contentText.gameObject.SetActive(false);
                panel.SetActive(false);
            }
        }
        else if(IsAudioStop() && startFlg && talkType == Const.TalkType.Simple)
        {

            if (IsTimeFull())
            {
                CBS.ResetBlendShape();
                contentText.gameObject.SetActive(false);
                panel.SetActive(false);
            }
        }
        else
        {
            //ボタンが押された
            if (Input.GetKeyUp(KeyCode.Space))
            {
                timeUntilDisplay = 0; //※1
            }
        }

        //表示される文字数を計算
        int displayCharCount = (int)(Mathf.Clamp01((Time.time - timeBeganDisplay) / timeUntilDisplay) * currentSentence.Length);
        //表示される文字数が表示している文字数と違う
        if (displayCharCount != lastUpdateCharCount)
        {
            contentText.text = currentSentence.Substring(0, displayCharCount);
            //表示している文字数の更新
            lastUpdateCharCount = displayCharCount;
        }
    }

    /// <summary>
    /// 複数の会話があればここで次の会話を読みこむ
    /// </summary>
    void SetNextSentence()
    {
        currentSentence = sentences[currentSentenceNum];
        timeUntilDisplay = currentSentence.Length * intervalForCharDisplay;
        timeBeganDisplay = Time.time;
        currentSentenceNum++;
        lastUpdateCharCount = 0;

    }

    /// <summary>
    /// 経過時間による表情の変更をチェック
    /// </summary>
    /// <param name="val"></param>
    void CheckFace()
    {
        time = audioSource.time;
        if (nowTalkData ==null ||faceIndex >= nowTalkData.ChangeTimings.Count)
            return;

        if(time >= nowTalkData.ChangeTimings[faceIndex].Timing)
        {
            string id = nowTalkData.ChangeTimings[faceIndex].FaceId;
            Debug.Log("表情変更：" + id);
            CBS.SetBlendShape(id);
            faceIndex++;
        }
    }

    /// <summary>
    /// セリフの音声データが終わったかどうか
    /// </summary>
    /// <returns></returns>
    bool IsAudioStop()
    {
        Debug.Log("再生状態:"+ audioSource.isPlaying);
        return !audioSource.isPlaying; 
    }

    /// <summary>
    /// テキスト表示後の経過時間を管理し、
    /// 3秒経てば知らせる
    /// </summary>
    /// <returns></returns>
    bool IsTimeFull()
    {
        return Time.time > timeBeganDisplay + timeUntilDisplay + 3; 
    }

    /// <summary>
    /// アニメーションを開始する
    /// </summary>
    /// <param name="num"></param>
    public void AnimationStart(int num)
    {
        //AnimatorのTriggerをtrueにする
        anime.SetInteger("IdolNumber", num);
        anime.SetBool("AnimFlg", true);
    }



}