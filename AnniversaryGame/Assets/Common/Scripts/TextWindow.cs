using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;
using TMPro;

public class TextWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_mainText;
    [SerializeField]
    private TextMeshProUGUI m_nameText;
    [SerializeField]
    private Image m_icon;
    [SerializeField]
    private Image m_photo;
    [SerializeField]
    private float textSpeed = 0.1f;

    public bool m_nowPlay = false;

    private List<TextContentData> m_textList = new List<TextContentData>();
    private string m_currentDispText = "";
    private int m_currntIndex = 0;
    private bool m_isFinishTextDisp = false;
    private bool m_isSkip = false;
    private float textDispTime = 0; // 経過時間に合わせて文字を表示するのに使用

    private InputAction _pushSkip;

    private CancellationToken m_token;

    // Start is called before the first frame update
    async void Start()
    {
        m_token = this.GetCancellationTokenOnDestroy();
        // スキップボタンの登録
        SetInputAction();
        // テスト
        List<TextContentData> testTextList = new List<TextContentData>()
        {
            new TextContentData(TextContentData.SpeakPlayer.Player, TextContentData.Emotion.Normal, "おお～！おおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおお", "shibaFront.png"),
            new TextContentData(TextContentData.SpeakPlayer.Narattion, TextContentData.Emotion.None, "ああ～！あああああああああああああああああああああああああああああああああああああああ", "shibaFront.png"),
            new TextContentData(TextContentData.SpeakPlayer.Player, TextContentData.Emotion.Smile, "ほほ～！ほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほほ", "shibaFront.png"),
        };
        SetTextContents(testTextList);
        await PlayTextWindow(m_token);
    }

    // Update is called once per frame
    async void FixedUpdate()
    {
        if (m_nowPlay)
        {
            // テキスト表示が終了していない
            if (!m_isFinishTextDisp)
            {
                textDispTime += Time.deltaTime;

                // クリックされると一気に表示
                if (m_isSkip) { 
                    m_isFinishTextDisp = true;
                    m_isSkip = false;
                }

                int len = Mathf.FloorToInt(textDispTime / textSpeed);
                if (len > m_currentDispText.Length) {
                    m_isFinishTextDisp = true;
                    m_isSkip = false;
                }
                else
                {
                    m_mainText.text = m_currentDispText.Substring(0, len);
                }
            }
            // テキスト表示が終了している
            else
            {
                if (!m_mainText.text.Equals(m_currentDispText))
                {
                    m_mainText.text = m_currentDispText;
                }

                // ボタン押下を待つ
                if (m_isSkip)
                {
                    m_currntIndex++;
                    m_isSkip = false;
                    if (m_currntIndex >= m_textList.Count)
                    {
                        // 終了
                        m_nowPlay = false;
                        m_currntIndex = 0;
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        // 次のテキストに進む
                        await SetTextContent(m_currntIndex, m_token);
                        textDispTime = 0;
                    }
                }
            }
        }
    }

    public void SetTextContents(List<TextContentData> list)
    {
        m_textList = list;
    }

    public async UniTask PlayTextWindow(CancellationToken token)
    {
        m_nowPlay = true;
        this.gameObject.SetActive(true);
        m_currntIndex = 0;
        await SetTextContent(m_currntIndex, token);
    }

    private async UniTask SetTextContent(int index, CancellationToken token)
    {
        if (index >= m_textList.Count)
        {
            Debug.Log($"指定したindexはテキストコンテンツのサイズを超えています。index：{index},,size：{m_textList.Count}");
            m_isFinishTextDisp = true;
            return;
        }

        m_isFinishTextDisp = false;
        // テキスト変更
        m_currentDispText = m_textList[index].m_mainText;
        // 話し手に合わせて名前やアイコン変更
        switch (m_textList[index].m_speakPlayer)
        {
            case TextContentData.SpeakPlayer.Player:
                {
                    m_nameText.text = GameUtility.PLAYER_NAME;
                    // 表情切り替え
                    string iconAddress = "shibaFront.png";
                    switch (m_textList[index].m_emotion)
                    {
                        case TextContentData.Emotion.Normal:
                            iconAddress = "";
                            break;
                        case TextContentData.Emotion.Smile:
                            iconAddress = "";
                            break;
                        case TextContentData.Emotion.Surprise:
                            iconAddress = "";
                            break;
                        default: break;
                    }
                    iconAddress = "shibaFront.png";
                    Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>(iconAddress, token);
                    m_icon.sprite = ImageAsset;
                }
                break;
            case TextContentData.SpeakPlayer.Narattion:
                {
                    m_nameText.text = GameUtility.NARRATION_NAME;
                    Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>("shibaFront.png", token);
                    m_icon.sprite = ImageAsset;
                }
                break;
        }
        // 写真があれば表示
        if (!m_textList[index].m_photoAddress.Equals(""))
        {
            Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>(m_textList[index].m_photoAddress, token);
            if (ImageAsset != null)
            {
                m_photo.sprite = ImageAsset;
            }
            else
            {
                Debug.Log($"指定したindexの写真addressはありません。index：{index},,m_photoAddress：{m_textList[index].m_photoAddress}");
            }
        }
    }

    // キー操作
    private System.IDisposable SetInputAction()
    {
        // 表示中のテキストをスキップ処理
        {
            _pushSkip = GameManager.Instance._InputControls.TextWindow.Skip;
            _pushSkip.started += SkipText;
        }

        return Disposable.Create(() =>
        {
            _pushSkip.started -= SkipText;
        });
    }

    public void SkipText(InputAction.CallbackContext context)
    {
        m_isSkip = true;
    }
}

public struct TextContentData
{
    public enum SpeakPlayer
    {
        Player = 0,
        Narattion = 1,
    };
    public enum Emotion
    {
        None = -1,
        Normal = 0,
        Smile = 1,
        Surprise = 2,
    };
    public SpeakPlayer m_speakPlayer{ get; private set; }
    public Emotion m_emotion { get; private set; }
    public string m_mainText{ get; private set; }
    public string m_photoAddress{ get; private set; }

    public TextContentData(SpeakPlayer speakPlayer, Emotion emotion,string mainText, string photoAddress)
    {
        m_speakPlayer = speakPlayer;
        m_emotion = emotion;
        m_mainText = mainText;
        m_photoAddress = photoAddress;
    }
}