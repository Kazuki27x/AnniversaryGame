using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField, Header("視差効果"), Range(0, 1)]
    private float m_parallaxEffect;

    [SerializeField] private GameObject m_camera;
    private float m_bgOneLength;
    private float m_startBgPosX;
    private float m_startCameraPosX;

    // Start is called before the first frame update
    void Awake()
    {
        m_startBgPosX = transform.position.x;
        if ((m_parallaxEffect < 1))
        {
            m_bgOneLength = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        }
        m_startCameraPosX = m_camera.transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Parallax();
    }

    private void Parallax()
    {
        // 
        float distPosX = (m_camera.transform.position.x - m_startCameraPosX) * m_parallaxEffect;
        //
        float currentPosX = m_startBgPosX + distPosX;
        // 
        transform.position = new Vector3(currentPosX, transform.position.y, transform.position.z);

        // 背景とカメラの位置が一定数離れたら、背景を移動（ループ用）
        if (m_parallaxEffect < 1)
        {
            if (m_camera.transform.position.x - currentPosX > m_bgOneLength)
            {
                m_startBgPosX += m_bgOneLength;
            }
            else if (m_camera.transform.position.x - currentPosX < -m_bgOneLength)
            {
                m_startBgPosX -= m_bgOneLength;
            }
        }

    }
}