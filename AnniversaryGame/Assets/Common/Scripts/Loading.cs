using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using TMPro;

public class Loading : MonoBehaviour
{

    private Animator animator;
    [NonSerialized] public bool m_isLoading = false;
    [NonSerialized] public bool m_isFade = false;

    public TextMeshProUGUI m_loadingText;
    private int m_loadCountNumber = 0;
    private float m_currentMS;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        // Anim動作登録
        ObservableStateMachineTrigger trigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        trigger.OnStateEnterAsObservable().Skip(1)
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo stateInfo = onStateInfo.StateInfo;
                // フェード終了
                if (stateInfo.IsName("Base Layer.Loading1") || stateInfo.IsName("Base Layer.Loading2"))
                {
                    m_isFade = false;
                }
                else if (stateInfo.IsName("Base Layer.LoadingWait"))
                {
                    m_isFade = false;
                    if (this.gameObject.activeSelf)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }).AddTo(this);
    }

    private void FixedUpdate()
    {
        m_currentMS += Time.deltaTime;
        if (m_currentMS > 1)
        {
            m_currentMS = 0;
            m_loadCountNumber++;
            if (m_loadCountNumber > 3){ m_loadCountNumber = 0; }
            switch (m_loadCountNumber)
            {
                case 0: m_loadingText.text = "Loading"; break;
                case 1: m_loadingText.text = "Loading."; break;
                case 2: m_loadingText.text = "Loading.."; break;
                case 3: m_loadingText.text = "Loading..."; break;
            }
        }
    }

    public void StartFadeIn()
    {
        if (this.gameObject.activeSelf)
        {
            return;
        }
        this.gameObject.SetActive(true);
        this.GetComponent<Animator>().SetTrigger("FadeTrigger");
        m_isFade = true;
    }

    public void StartFadeOut()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        this.GetComponent<Animator>().SetTrigger("FadeTrigger");
        m_isFade = true;
    }
}
