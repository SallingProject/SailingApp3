/**************************************************************************************/
/*! @file   CourseSelectPopupWindowScript.cs
***************************************************************************************
@brief      コースセレクト時のポップアップに関する処理
***************************************************************************************
@author     Ryo Sugiyama
***************************************************************************************
* Copyright © 2016 RyoSugiyama All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CourseSelectPopupWindowScript : PopupBase
{

    [SerializeField]
    private GameObject m_contens;
    [SerializeField]
    private ECourseType m_type;
    [SerializeField]
    private GameObject[] m_coursePref;

    public void OpenEnd()
    {
        m_contens.SetActive(true);
        switch(m_type)
        {
            case ECourseType.straight:
                m_coursePref[0].SetActive(true);
                break;
            case ECourseType.corner:
                m_coursePref[1].SetActive(true);
                break;
            case ECourseType.usingItem:
                m_coursePref[2].SetActive(true);
                break;
            case ECourseType.accelerate:
                m_coursePref[3].SetActive(true);
                break;
        }
        
        
    }
    public void CloseEnd()
    {
        m_contens.SetActive(false);
        switch (m_type)
        {
            case ECourseType.straight:
                m_coursePref[0].SetActive(false);
                break;
            case ECourseType.corner:
                m_coursePref[1].SetActive(false);
                break;
            case ECourseType.usingItem:
                m_coursePref[2].SetActive(false);
                break;
            case ECourseType.accelerate:
                m_coursePref[3].SetActive(false);
                break;
        }
    }
    void PopupAction(EButtonId id)
    {
        switch (id)
        {
            case EButtonId.Ok:
                PlayerPrefs.SetInt(SaveKey.mTutorialKey, (int)m_type);
                GameInstance.mInstance.mSceneLoad(new LoadInfo("Tutorial"));
                Close();
                break;
            case EButtonId.Cancel:
                Close();
                break;
        }
    }
    public void GetCourseType(ECourseType type)
    {
        m_type = type;
    }
    public void Open()
    {

        mButtonSet = EButtonSet.Set2;
        PopupButton.mOnClickCallback = PopupAction;
        
        base.Open(null, null, OpenEnd);

    }

    public void Close()
    {
        base.Close(CloseEnd, null, null);
    }

}
