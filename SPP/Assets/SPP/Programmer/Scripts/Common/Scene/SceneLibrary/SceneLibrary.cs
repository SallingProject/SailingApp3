/**************************************************************************************/
/*! @file   SceneLibrary.cs
***************************************************************************************
@brief      ライブラリ画面
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SceneLibrary : SceneBase {

    /******************************************************************************* 
    @brief   定数
    */
    private const int kMaxNear = -2;
    private const int kMaxFar  = -18;
    private static readonly Vector3 m_kInitRotation         = new Vector3(0, -150, 0);
    private static readonly Vector3 m_kInitCameraPosition   = new Vector3(0, 0, -10);
    
    [SerializeField]
    private Image m_touchPanel;

    [SerializeField]
    private GameObject m_shipRoot;

    [SerializeField]
    private Button m_resetButton;

    [SerializeField]
    private Button m_backToHomeButton;

    [SerializeField]
    private Button m_nextButton;

    [SerializeField]
    private Button m_prevButton;


    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private List<GameObject> m_shipList;

    int m_currentShip = 0;

    private float m_prevDistance = 0;

    /******************************************************************************* 
    @brief  　初期化処理
    */
    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        
        EventTrigger trigger = m_touchPanel.GetComponent<EventTrigger>();

        // PointerDragイベントの追加
        {
            EventTrigger.Entry drag = new EventTrigger.Entry();
            drag.eventID = EventTriggerType.Drag;
            drag.callback.RemoveAllListeners();
            drag.callback.AddListener(Drag);

            trigger.triggers.Add(drag);
        }
        
        // リセット処理
        m_resetButton.onClick.AddListener(() =>
        {
            m_camera.transform.position = m_kInitCameraPosition;
            m_shipRoot.transform.localEulerAngles = m_kInitRotation;
        });

        
        m_backToHomeButton.onClick.AddListener(() => GameInstance.mInstance.mSceneLoad(new LoadInfo("Home")));


        m_nextButton.onClick.AddListener(() =>
        {
            if (m_currentShip + 1 < m_shipList.Count)
            {
                m_shipList[m_currentShip].SetActive(false);
                m_currentShip += 1;
                ChangeShip(m_currentShip);
            }
        });

        m_prevButton.onClick.AddListener(() =>
        {
            if (m_currentShip - 1 >= 0)
            {
                m_shipList[m_currentShip].SetActive(false);
                m_currentShip -= 1;
                ChangeShip(m_currentShip);
            }
        });
    }

    void ChangeShip(int id)
    {
        m_shipList[id].SetActive(true);
        m_camera.transform.position = m_kInitCameraPosition;
        m_shipRoot.transform.localEulerAngles = m_kInitRotation;
    }

    /******************************************************************************* 
    @brief  　ドラッグ中の処理
    */
    void Drag(BaseEventData eventData)
    {
        if (InputManager.mInstance.mTouchCount < 2)
        {
            var touch = InputManager.mInstance.mGetTouchInfo();

            switch (touch.mTouchType)
            {
                case ETouchType.Swipe:
                    //移動量に応じて角度計算
                    float xAngle = touch.mLocalDeltaPosition.y;
                    float yAngle = -touch.mLocalDeltaPosition.x;

                    if (Mathf.Abs(xAngle) > Mathf.Abs(yAngle))
                    {
                        m_shipRoot.transform.Rotate(Vector3.right * xAngle / 2, Space.World);
                    }
                    else
                    {
                        m_shipRoot.transform.Rotate(Vector3.up * yAngle / 2, Space.World);
                    }
                    break;
            }

            
            
        }
        else
        {
            // ズーム
            var touch1 = InputManager.mInstance.mGetTouchInfo(0);
            var touch2 = InputManager.mInstance.mGetTouchInfo(1);

            float distance = Vector2.Distance(touch1.mPosition, touch2.mPosition);
            
            if(m_prevDistance > distance)
            {
                // 近づける処理

                if (m_camera.transform.position.z + 1 < kMaxNear)
                    m_camera.transform.position += new Vector3(0, 0, 1);
            }
            else
            {
                //　遠ざける処理
                if (m_camera.transform.position.z - 1 > kMaxFar)
                    m_camera.transform.position -= new Vector3(0, 0, 1);

            }
            m_prevDistance = distance;
        }
    }
}
