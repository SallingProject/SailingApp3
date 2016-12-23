/**************************************************************************************/
/*! @file   SelectShipScene.cs
***************************************************************************************
@brief      船選択時に関する処理
***************************************************************************************
@author     Ryo Sugiyama
***************************************************************************************
* Copyright © 2016 RyoSugiyama All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectShipScene : SceneBase
{

    [SerializeField]
    private EShipType m_id;
    [SerializeField]
    private ShipSelectPopupWindowScript m_popup;

    [SerializeField]
    private GameObject m_viewObject;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
    }
    public void ButtonPush()
    {
        m_popup.GetShipType(m_id);
        m_popup.Open(m_viewObject);
   
    }

}
