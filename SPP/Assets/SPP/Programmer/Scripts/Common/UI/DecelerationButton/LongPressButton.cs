/*************************************************************
@file       LongPressButton.cs
@brief      押している間にTRUE，その他はFALSEを返す
@author     Hiroto Morikawa

@Update
    2017/1/13   LongPressButton.csを作成
                機能の完成

Copyright © 2017 Hiroto Morikawa All Rights Reserved.
***************************************************************/
using UnityEngine;
using System.Collections;

public class LongPressButton : BaseObject {

    [SerializeField]
    private bool m_isPress;

    /*********************
    @brief      プロパティ
    */
    public bool mIsPress {

        get;
        private set;
    }

    /*****************************************
    @brief      ボタンを押している間TRUEとなる
    */
    public void mPushButton()
    {
        mIsPress = m_isPress = true;
    }

    /********************************************
    @brief       ボタンを押していない間FALSEとなる
    */
    public void mPullButton()
    {
        mIsPress = m_isPress = false;
    }
}
