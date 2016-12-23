/**************************************************************************************/
/*! @file   ResultWindow.cs
***************************************************************************************
@brief      リザルト背景の表示
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/
using UnityEngine;
using System.Collections;

public class ResultWindow : PopupBase {

    [SerializeField]
    private GameObject m_contents;

    public void Open()
    {
        base.mButtonSet = EButtonSet.SetNone;
        base.Open(null, null, PopupOpenEnd);
    }

    private void PopupOpenEnd()
    {
        m_contents.SetActive(true);
        Time.timeScale = 0;
    }


    public void mChangeScene(string str)
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo(str));
        Time.timeScale = 1;
    }

}
