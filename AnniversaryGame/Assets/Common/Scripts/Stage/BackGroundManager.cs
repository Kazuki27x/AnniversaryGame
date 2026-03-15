using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Actor;
    [Header("ãGêﬂÇÃì]ä∑ì_Ç»ÇÈGameObject")]
    [SerializeField] private GameObject m_ToAutumn;
    [SerializeField] private GameObject m_ToWinter;
    [SerializeField] private GameObject m_ToSpring;
    [SerializeField] private GameObject m_ToSummer;

    [Header("ãGêﬂÇ≤Ç∆Ç…ïœâªÇ∑ÇÈÉIÉuÉWÉFÉNÉg")]
    [SerializeField] private GameObject m_Sun;
    [SerializeField] private GameObject m_GrassList;
    [SerializeField] private GameObject m_Snow;
    [SerializeField] private GameObject m_Sakura;
    bool thiss;

    private Color m_AutumnGrassColor = new Color(255/255f, 200/255f, 90/255f, 255/255f);
    // Start is called before the first frame update
    void Start()
    {
        // ToèH
        this.ObserveEveryValueChanged(t => t.m_Actor.transform.position.x > m_ToAutumn.transform.position.x)
            .Skip(1)
            .Subscribe(value =>
            {
                if (value){ ToAutumn(true); ToSummer(false); }
                else      { ToAutumn(false); ToSummer(true); }
            }).AddTo(this);
        // Toì~
        this.ObserveEveryValueChanged(t => t.m_Actor.transform.position.x > m_ToWinter.transform.position.x)
            .Skip(1)
            .Subscribe(value =>
            {
                if (value) { ToWinter(true); ToAutumn(false); }
                else       { ToWinter(false); ToAutumn(true); }
            }).AddTo(this);
        // Toèt
        this.ObserveEveryValueChanged(t => t.m_Actor.transform.position.x > m_ToSpring.transform.position.x)
            .Skip(1)
            .Subscribe(value =>
            {
                if (value) { ToSpring(true); ToWinter(false); }
                else       { ToSpring(false); ToWinter(true); }
            }).AddTo(this);
        // Toâƒ
        this.ObserveEveryValueChanged(t => t.m_Actor.transform.position.x > m_ToSummer.transform.position.x)
            .Skip(1)
            .Subscribe(value =>
            {
                if (value) { ToSummer(true); ToSpring(false); }
                else       { ToSummer(false); ToSpring(true); }
            }).AddTo(this);

        ResetSeasonObj();
        // ç≈èâÇÕâƒÇ©ÇÁ
        foreach (SpriteRenderer sprite in m_Sun.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        }
    }

    private void ToAutumn(bool isIn)
    {
        Debug.Log("ToAutumn");

        if (isIn)
        {
            DG.Tweening.DOVirtual.Color(Color.white, m_AutumnGrassColor, 1, value => {
                foreach (SpriteRenderer sprite in m_GrassList.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = value;
                }
            });
        }
        else
        {
            DG.Tweening.DOVirtual.Color(m_AutumnGrassColor, Color.white, 1, value => {
                foreach (SpriteRenderer sprite in m_GrassList.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = value;
                }
            });
        }
    }

    private async UniTask ToWinter(bool isIn)
    {
        Debug.Log("ToWinter");
        if (isIn)
        {
            m_Snow.GetComponent<ParticlUpdate>().StartEffect();
        }
        else
        {
            m_Snow.GetComponent<ParticlUpdate>().StopEffect();
        }
    }

    private async UniTask ToSpring(bool isIn)
    {
        Debug.Log("ToSpring");
        if (isIn)
        {
            m_Sakura.GetComponent<ParticlUpdate>().StartEffect();
        }
        else
        {
            m_Sakura.GetComponent<ParticlUpdate>().StopEffect();
        }
    }

    private void ToSummer(bool isIn)
    {
        Debug.Log("ToSummer");
        if (isIn)
        {
            DG.Tweening.DOVirtual.Float(0, 1, 1, value => {
                foreach (SpriteRenderer sprite in m_Sun.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
                }
            });
        }
        else
        {
            DG.Tweening.DOVirtual.Float(1, 0, 1, value => {
                foreach (SpriteRenderer sprite in m_Sun.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
                }
            });
        }
    }

    private void ResetSeasonObj()
    {
        foreach (SpriteRenderer sprite in m_GrassList.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = Color.white;
        }
        m_Snow.GetComponent<ParticlUpdate>().StopEffect();
        m_Snow.GetComponent<ParticlUpdate>().ClearEffect();
        m_Sakura.GetComponent<ParticlUpdate>().StopEffect();
        m_Sakura.GetComponent<ParticlUpdate>().ClearEffect();
        foreach (SpriteRenderer sprite in m_Sun.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        }
    }
}
