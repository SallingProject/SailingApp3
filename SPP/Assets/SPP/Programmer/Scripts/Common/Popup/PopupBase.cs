/**************************************************************************************/
/*! @file   PopupBase.cs
***************************************************************************************
@brief      PopupWindowの基底クラス
***************************************************************************************
@author     Ko Hashimoto and Kana Yoshidumi
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PopupBase : BaseObject {


    [SerializeField]
    RectTransform m_popupWindowBase;
    RectTransform m_popupWindow;

    [SerializeField]
    RectTransform m_popupRoot;

    PopupButton m_popupButton;
    public PopupButton PopupButton
    {
        get { return m_popupButton; }
    }

    Image m_blackFade;
    public Image BlackFade
    {
        get { return m_blackFade; }
    }

   private  System.Action<EButtonId> m_buttonCallback;
    public System.Action<EButtonId> mButtonCallback
    {
        set
        {
            if (m_popupButton != null)
                m_popupButton.mOnClickCallback = value;

            m_buttonCallback = value;
        }
    }

    class PopupAction
    {
        public System.Action _begin;
        public System.Action _run;
        public System.Action _end;
    }

    PopupAction m_openAction = new PopupAction();
    PopupAction m_closeAction = new PopupAction();

    private EButtonSet m_buttonSet = EButtonSet.Set1;
    public EButtonSet mButtonSet
    {
        private get { return m_buttonSet; }
        set
        {
            if (m_popupButton != null)
                m_popupButton.transform.SetActive(false);

            if (value != EButtonSet.SetNone)
            {
                var root = m_popupWindow.transform.FindInChildren("Popup", false);
                var buttonGroup = root.transform.FindInChildren("Button", false);
                string path = (value == EButtonSet.Set1) ? "ButtonSet1" : "ButtonSet2";
                var button = buttonGroup.transform.FindInChildren(path, false);

                if (m_popupButton == null || m_popupButton.name != button.name)
                {
                    m_popupButton = button.GetComponent<PopupButton>();
                    
                    m_popupButton.mOnClickCallback = m_buttonCallback;
                }
            }
            if (m_popupButton != null)
                m_popupButton.transform.SetActive(true);

            m_buttonSet = value;
        }
    }

    float m_time = float.NaN;
    protected override void Awake()
    {
        base.Awake();
        mUnregisterList(this);

        if (m_popupWindow == null)
        {
            m_popupWindow = mCreate(m_popupWindowBase) as RectTransform;
        }
        m_popupWindow.SetParent(m_popupRoot, false);
        m_popupWindow.transform.localScale = new Vector3(1, 0, 0);

        if(m_blackFade == null)
        {
            m_blackFade = m_popupWindow.transform.FindInChildren("BackFade", false).GetComponent<Image>();
            m_blackFade.transform.SetActive(false);
        }
        m_popupWindow.SetActive(false);

        mButtonSet = EButtonSet.SetNone;
        mUnregister();
    }

    public EPopupState mPopupState
    {
        get;
        private set;
    }

    
    public virtual void Open(System.Action openBeginAction, System.Action openning = null, System.Action openEnd = null, float time = 0.25f)
    {

        m_popupWindow.SetActive(true);
        m_popupWindow.transform.localScale = new Vector3(1, 0);

        m_openAction._begin = openBeginAction;
        m_openAction._run = openning;
        m_openAction._end = openEnd;
        m_time = time;

        OnOpen();
    }

    public virtual void Close(System.Action closeBeginAction, System.Action closening = null, System.Action closeEnd = null, float time = 0.25f)
    {
        m_closeAction._begin = closeBeginAction;
        m_closeAction._run = closening;
        m_closeAction._end = closeEnd;
        m_time = time;

        m_popupWindow.transform.localScale = new Vector3(1, 1);
        OnCloseAnimation();
    }

    void OnOpen()
    {
        var tweener = m_popupWindow.DOScale(new Vector3(1f, 1f), m_time).SetEase(Ease.InOutQuart);
        tweener
        .OnStart(()=>
        {
            m_blackFade.transform.SetActive(true);
            if (m_openAction._begin != null)
            {
                m_openAction._begin.Invoke();
                m_openAction._begin = null;
            }
            mPopupState = EPopupState.OpenBegin;
        })
        .OnUpdate(() =>
        {
            if (m_openAction._run != null)
            {
                m_openAction._run.Invoke();
                m_openAction._run = null;
            }
            mPopupState = EPopupState.Openning;
        })
         .OnComplete(() =>
         {

             if (m_openAction._end != null)
             {
                 m_openAction._end.Invoke();
                 m_openAction._end = null;
             }
             mPopupState = EPopupState.OpenEnd;
             m_popupButton.transform.SetActive(true);
         });
        
    }

    void OnCloseAnimation()
    {
        var tweener = m_popupWindow.DOScale(new Vector3(1f, 0f), m_time).SetEase(Ease.InOutQuart);
        tweener.OnStart(()=>
        {
            if (m_closeAction._begin != null)
            {
                m_closeAction._begin.Invoke();
                m_closeAction._begin = null;
            }
            m_popupButton.transform.SetActive(false);
            mPopupState = EPopupState.CloseBegin;
        })
         .OnUpdate(() =>
         {
             if (m_closeAction._run != null)
             {
                 m_closeAction._run.Invoke();
                 m_closeAction._run = null;
             }
             mPopupState = EPopupState.Closing;
         })
         .OnComplete(() =>
         {

             if (m_closeAction._end != null)
             {
                 m_closeAction._end.Invoke();
                 m_closeAction._end = null;
             }
             m_blackFade.transform.SetActive(false);
             mPopupState = EPopupState.CloseEnd;
         });
    }



    public void SetButtonText(EButtonId id,string text)
    {
        switch (id)
        {
            case EButtonId.Ok:
                if (m_popupButton.OkText != null)
                {
                    m_popupButton.OkText.text = text;
                }
                break;

            case EButtonId.Cancel:
                if (m_popupButton.CancelText != null)
                {
                    m_popupButton.CancelText.text = text;
                }
                break;
        }
    }
}
