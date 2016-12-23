/**************************************************************************************/
/*! @file   PopupWindow.cs
***************************************************************************************
@brief     チュートリアル時のポップアップに関する処理
***************************************************************************************
@author     Ryo Sugiyama
***************************************************************************************
* Copyright © 2016 RyoSugiyama All Rights Reserved.
***************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialPopup : PopupBase
{
    
    private GameObject m_comtens;

   public void Open(GameObject content, System.Action openedCallback)
    {

        m_comtens = content;

        if (m_comtens != null)
            m_comtens.SetActive(false);
        base.Open(null, null,
            () =>
            {
                if (m_comtens != null)
                    m_comtens.SetActive(true);
                openedCallback.Invoke();
            });
    }

    public void Close(System.Action closeBegin)
    {
        base.Close(closeBegin);
    }
}
