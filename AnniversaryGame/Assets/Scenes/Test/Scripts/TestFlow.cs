using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class TestFlow : BaseScene
{

    [SerializeField] Button m_startButton;

    [Header("CSV")]
    [SerializeField] TextMeshProUGUI m_csvText;
    [Header("Stage")]
    [SerializeField] TextMeshProUGUI m_stageNumberText;
    [SerializeField] TextMeshProUGUI m_storyNumberText;
    [Header("Story")]
    [SerializeField] Slider m_stageSlider;
    [SerializeField] Slider m_storySlider;

    // キーアクション
    private InputAction _pushStart;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // 再生ボタンの登録
        m_startButton.OnClickAsObservable().Subscribe(_ => StartStory()).AddTo(this);

        // スライダーの値変更でテキスト更新
        this.ObserveEveryValueChanged(th => th.m_stageSlider.value).Subscribe(value =>
        {
            m_stageNumberText.text = value.ToString();
            UpdateCSVText();
        });
        this.ObserveEveryValueChanged(th => th.m_storySlider.value).Subscribe(value =>
        {
            m_storyNumberText.text = value.ToString();
            UpdateCSVText();
        });
    }

    private void UpdateCSVText()
    {
        int stageNum = (int)m_stageSlider.value;
        int storyNum = (int)m_storySlider.value;
        m_csvText.text = $"Stage{stageNum.ToString("D2")}Story{storyNum.ToString("D2")}.csv";
    }

    private async UniTask StartStory()
    {
        await StartTextWindow(m_csvText.text, m_token);
    }
}

