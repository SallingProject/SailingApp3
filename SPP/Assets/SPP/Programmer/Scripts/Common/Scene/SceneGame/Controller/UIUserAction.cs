/**************************************************************************************/
/*! @file   UIUserAction.cs
***************************************************************************************
@brief      ユーザーの起こすアクション系（アイテムとスピン）管理用
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;
/**************************************************************************************
@brief  	アイテムとスピン管理用
*/
public class UIUserAction : BaseObject {

    [System.Serializable]
    class Item
    {
        public Image _image;

        [System.NonSerialized]
        public bool _canUse;

        [System.NonSerialized]
        public ItemDefine _define;

        // 初期化
        public void Setup()
        {
            _canUse = false;
            _image.transform.localScale = new Vector3(0, 0, 0);
            _define = null;
        }
    }

    [SerializeField]
    private Item m_item;

    [SerializeField]
    private Button m_spin;

    [SerializeField]
    float m_animationSec = 0.5f;

    private int m_userId;

    /**************************************************************************************
    @brief  	スピン押された時のコールバック設定用
    */
    public UnityAction mSpinCallback
    {
        set { m_spin.onClick.RemoveAllListeners();m_spin.onClick.AddListener(value); }
    }
    Vector2 m_itemInitPosition = Vector2.zero;
    ITouchInfo m_touch;

    public bool mIsDown
    {
        get;
        private set;
    }
    protected override void mOnRegistered()
    {
        base.mOnRegistered();

        mUnregisterList(this);

        EventTrigger trigger = m_item._image.GetComponent<EventTrigger>();

        // PointerDownイベントの追加
        {
            EventTrigger.Entry down = new EventTrigger.Entry();
            down.eventID = EventTriggerType.PointerDown;
            down.callback.RemoveAllListeners();
            down.callback.AddListener(Down);
            trigger.triggers.Add(down);
        }

        // PointerUpイベントの追加
        {
            EventTrigger.Entry up = new EventTrigger.Entry();
            up.eventID = EventTriggerType.PointerUp;
            up.callback.RemoveAllListeners();
            up.callback.AddListener(Up);
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

        // 初期位置を記憶
        m_itemInitPosition = m_item._image.rectTransform.anchoredPosition;
        m_userId = 0; // どこからかユーザーIdを取得
        m_item.Setup();
    }

    void Down(BaseEventData eventData)
    {

        mIsDown = true;
        m_touch = InputManager.mInstance.mGetTouchInfo();

    }

    void Drag(BaseEventData eventData)
    {
        if (!m_item._canUse)
            return;
        var touch = InputManager.mInstance.mGetTouchInfo(m_touch.mFingerId);
        m_item._image.rectTransform.anchoredPosition += touch.mLocalDeltaPosition;
    }
    
    void Up(BaseEventData eventData)
    {
        var touch = InputManager.mInstance.mGetTouchInfo(m_touch.mFingerId);
        if (touch.mTouchType == ETouchType.Flick 
            && touch.mFlickDirection == EFlickDirection.Up
            && m_item._canUse)
        {
            GameInfo.mInstance.SetInvokeItem(m_item._define, m_userId);
            m_item._image.transform.SetActive(false);
            m_item._canUse = false;
        }

        mIsDown = false;
        m_item._image.rectTransform.anchoredPosition = m_itemInitPosition;
    }

    /**************************************************************************************
    @brief  	アイテム設定用
    @note       アイテムがある状態だと上書き？それともしかと？
    */
    public void SetItem(ItemDefine define)
    {
        // TODO : 設定処理
        m_item._define = define;
        m_item._image.transform.localScale = new Vector3(0, 0, 0);
        m_item._image.sprite = define.mSprite;
        
        m_item._image.transform.SetActive(true);

        m_item._image.transform.DOScale(new Vector3(1, 1, 1), m_animationSec)
            .OnComplete(() => m_item._canUse = true);

    }

   
}
