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
    private bool m_isSkip = false;
    private bool m_isFinishTextDisp = false;
    private float textDispTime = 0; // 経過時間に合わせて文字を表示するのに使用

    private InputAction _pushSkip;

    private CancellationToken m_token;

    // Start is called before the first frame update
    void Start()
    {
        m_token = this.GetCancellationTokenOnDestroy();
        // スキップボタンの登録
        SetInputAction();
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

                int len = Mathf.FloorToInt(textDispTime / textSpeed);
                if (len > m_currentDispText.Length)
                {
                    m_mainText.text = m_currentDispText;
                    m_isFinishTextDisp = true;
                }
                else
                {
                    m_mainText.text = m_currentDispText.Substring(0, len);
                }
            }
            // テキスト表示が終了している
            else
            {
                // テキスト表示後の処理は、ボタン押下処理に記載されている}
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
        m_currntIndex = 0;
        await SetTextContent(m_currntIndex, token);
        this.gameObject.SetActive(true);
    }

    private async UniTask SetTextContent(int index, CancellationToken token)
    {
        if (index >= m_textList.Count)
        {
            Debug.Log($"指定したindexはテキストコンテンツのサイズを超えています。index：{index},,size：{m_textList.Count}");
            m_isFinishTextDisp = true;
            return;
        }

        // テキスト変更
        m_currentDispText = m_textList[index].m_mainText;
        // 話し手に合わせて名前やアイコン変更
        switch (m_textList[index].m_speakPlayer)
        {
            case GameUtility.SpeakPlayer.Narattion:
                {
                    m_nameText.text = GameUtility.NARRATION_NAME;
                    Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>("koupenchanGod.PNG", token);
                    m_icon.sprite = ImageAsset;
                }
                break;
            case GameUtility.SpeakPlayer.Player:
                {
                    m_nameText.text = GameUtility.PLAYER_NAME;
                    // 表情切り替え
                    string iconAddress = "koupenchanNormal.PNG";
                    switch (m_textList[index].m_emotion)
                    {
                        case GameUtility.Emotion.Normal:
                            iconAddress = "koupenchanNormal.PNG";
                            break;
                        case GameUtility.Emotion.Smile:
                            iconAddress = "koupenchamSmile.PNG";
                            break;
                        case GameUtility.Emotion.Surprise:
                            iconAddress = "koupenchanSurprise.PNG";
                            break;
                        case GameUtility.Emotion.Angry:
                            iconAddress = "koupenchanAngry.PNG";
                            break;
                        case GameUtility.Emotion.Grid:
                            iconAddress = "koupenchanGrid.PNG";
                            break;
                        case GameUtility.Emotion.Sad:
                            iconAddress = "koupenchanSad.PNG";
                            break;
                        default: break;
                    }
                    Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>(iconAddress, token);
                    m_icon.sprite = ImageAsset;
                }
                break;
        }
        // 写真があれば表示
        if (m_textList[index].m_photoAddress.Equals("="))
        {
            // 同じ画像を表示
            m_photo.enabled = true;
        }
        else if (!m_textList[index].m_photoAddress.Equals(""))
        {
            Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>(m_textList[index].m_photoAddress, token);
            if (ImageAsset != null)
            {
                m_photo.sprite = ImageAsset;
                m_photo.enabled = true;
            }
            else
            {
                Debug.Log($"指定したindexの写真addressはありません。index：{index},,m_photoAddress：{m_textList[index].m_photoAddress}");
                m_photo.enabled = false;
            }
        }
        else
        {
            // 画像非表示
            m_photo.enabled = false;
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

    public async void SkipText(InputAction.CallbackContext context)
    {
        if (m_nowPlay && !m_isSkip)
        {
            m_isSkip = true;

            // テキスト表示が終了していない
            if (!m_isFinishTextDisp)
            {
                // 一気に表示
                m_mainText.text = m_currentDispText;
                m_isFinishTextDisp = true;
                m_isSkip = false;
            }
            else
            {
                // テキスト表示が終了している
                m_currntIndex++;
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
                }
                m_isFinishTextDisp = false;
                textDispTime = 0;
                m_mainText.text = "";
                m_isSkip = false;
            }
        }
    }
}