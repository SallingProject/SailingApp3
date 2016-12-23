/**************************************************************************************/
/*! @file   UIHandleController.cs
***************************************************************************************
@brief      セイルを動かすコントローラークラス
***************************************************************************************
@author     Ko Hashimoto and Kana Yoshidumi
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class UISailController : BaseObject {

    static readonly float m_kMax = 0.995f;


    [SerializeField]
    Image m_root;

    [SerializeField]
    Image m_controllObject;

    [SerializeField]
    RectTransform m_right;

    [SerializeField]
    RectTransform m_left;

    public bool mIsDown
    {
        get;
        private set;
    }

    public float mBarProgress
    {
        get;
        private set;
    }
    protected override void mOnRegistered()
    {
        base.mOnRegistered();

        EventTrigger trigger = m_controllObject.GetComponent<EventTrigger>();

        // PointerDownイベントの追加
        {
            EventTrigger.Entry down = new EventTrigger.Entry();
            down.eventID = EventTriggerType.PointerDown;
            down.callback.RemoveAllListeners();
            down.callback.AddListener(data => mIsDown = true);
            trigger.triggers.Add(down);
        }

        // PointerUpイベントの追加
        {
            EventTrigger.Entry up = new EventTrigger.Entry();
            up.eventID = EventTriggerType.PointerUp;
            up.callback.RemoveAllListeners();
            up.callback.AddListener(data => mIsDown = false);
            trigger.triggers.Add(up);
        }

        // Dragイベントの追加
        {
            EventTrigger.Entry drag = new EventTrigger.Entry();
            drag.eventID = EventTriggerType.Drag;
            drag.callback.RemoveAllListeners();
            drag.callback.AddListener(Drag);
            trigger.triggers.Add(drag);
        }

        mBarProgress = 0f;
    }

    void Drag(BaseEventData eventData)
    {

        var touch = InputManager.mInstance.mGetTouchInfo();
        if (touch.mLocalDeltaPosition.x > 0)
        {
            if (m_controllObject.rectTransform.anchoredPosition.x + touch.mLocalDeltaPosition.x < m_right.anchoredPosition.x)
            {
                m_controllObject.rectTransform.anchoredPosition += new Vector2(touch.mLocalDeltaPosition.x, 0);       
            }
        }
        else 
        {

            if (m_controllObject.rectTransform.anchoredPosition.x + touch.mLocalDeltaPosition.x > m_left.anchoredPosition.x)
            {
                m_controllObject.rectTransform.anchoredPosition += new Vector2(touch.mLocalDeltaPosition.x, 0);
            }
        }


        float ObjectX =  m_controllObject.rectTransform.anchoredPosition.x;
        bool isRight = (ObjectX > 0);

        if (isRight)
        {
            
            mBarProgress = ObjectX / m_right.anchoredPosition.x;
            if(mBarProgress > m_kMax)
            {
                mBarProgress = 1f;
            }
  //          Debug.Log(mBarProgress);

        }
        else
        {

            mBarProgress = -(ObjectX / m_left.anchoredPosition.x);
            if (mBarProgress < -m_kMax)
            {
                mBarProgress = -1;
            }
//            Debug.Log(mBarProgress);
        }
    }
}
