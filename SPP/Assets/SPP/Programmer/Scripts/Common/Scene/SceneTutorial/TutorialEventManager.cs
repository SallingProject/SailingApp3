using UnityEngine;
using System.Collections.Generic;

public class TutorialEventManager : BaseObject {

    [SerializeField]
    TutorialPopup m_popup;
    
    [SerializeField]
    List<TutorialEventArea> m_eventAreaList = new List<TutorialEventArea>();

    [SerializeField]
    RectTransform m_contentRoot;

    GameObject m_visibleContent;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);

        foreach(var index in m_eventAreaList)
        {
            SetEventArea(index);   
        }
    }


    /**************************************************************************************
    @brief  	イベント設定用。ラムダ式での設定のため重複回避のため関数わけ
    */
    void SetEventArea(TutorialEventArea area)
    {
        area.mEventCallback = id =>
        {
            m_popup.mButtonCallback = buttonType => DefaultPageCallback(buttonType, area);
            area.mAnimationId = 0;

            var first = mCreate(area.Animations[area.mAnimationId]);
            first.transform.SetParent(m_contentRoot.transform, false);
            m_visibleContent = first;
            m_popup.mButtonSet = EButtonSet.Set2;
            m_popup.SetButtonText(EButtonId.Ok, "次へ");
            m_popup.SetButtonText(EButtonId.Cancel, "前へ");
            m_popup.Open(m_contentRoot.gameObject, area.BeginEvent);
        };
    }

    /**************************************************************************************
    @brief  	前のページ表示用
    */
    private void EventPrevPage(TutorialEventArea area)
    {
        if (area.mAnimationId < 0)
            area.mAnimationId = 0;


        m_popup.SetButtonText(EButtonId.Ok, "次へ");

        var prev = mCreate(area.Animations[area.mAnimationId]);
        prev.transform.SetParent(m_contentRoot.transform, false);
        m_visibleContent = prev;
    }

    /**************************************************************************************
    @brief  	通常ののページのときのコールバック設定
    */
    private void DefaultPageCallback(EButtonId id, TutorialEventArea area)
    {

        m_contentRoot.SetActive(false);
        mDeleteImmediate(m_visibleContent);

        switch (id)
        {
            case EButtonId.Ok:
                area.mAnimationId += 1;

                if (area.mAnimationId > area.Animations.Count - 1)
                {
                    m_popup.Close(() => { area.ExitEvent(); m_contentRoot.SetActive(false); });
                    return;
                }

                if (area.mAnimationId >= area.Animations.Count - 1)
                {
                    m_popup.SetButtonText(EButtonId.Ok, "OK");
                }

                var next = mCreate(area.Animations[area.mAnimationId]);
                next.transform.SetParent(m_contentRoot.transform, false);

                m_visibleContent = next;

                break;

            case EButtonId.Cancel:

                area.mAnimationId -= 1;
                EventPrevPage(area);

                break;
        }

        m_contentRoot.SetActive(true);
    }
}
