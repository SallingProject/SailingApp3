/**************************************************************************************/
/*! @file   SelectCourseScript.cs
***************************************************************************************
@brief      コースセレクト時に関する処理
***************************************************************************************
@author     Ryo Sugiyama
***************************************************************************************
* Copyright © 2016 RyoSugiyama All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectCourseScript : SceneBase {

    [SerializeField]
    private ECourseType m_type;
    [SerializeField]
    private CourseSelectPopupWindowScript m_popup;
     

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
    }
    public void ButtonPush()
    {
        m_popup.GetCourseType(m_type);
        m_popup.Open();
    }

}
