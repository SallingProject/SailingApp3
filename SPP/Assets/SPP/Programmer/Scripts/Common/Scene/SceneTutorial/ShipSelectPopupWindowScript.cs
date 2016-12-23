/**************************************************************************************/
/*! @file   ShipSelectPopupWindowScript.cs
***************************************************************************************
@brief      船選択時のポップアップに関する処理
***************************************************************************************
@author     Ryo Sugiyama
***************************************************************************************
* Copyright © 2016 RyoSugiyama All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ShipSelectPopupWindowScript : PopupBase
{

    [SerializeField]
    private GameObject m_contens;
    [SerializeField]
    private EShipType m_type;

    GameObject m_viewContent;
    
    public void CloseEnd()
    {
        m_viewContent.SetActive(false);
        m_contens.SetActive(false);

        m_viewContent = null;
    }    
    void PopupAction(EButtonId id)
    {
        switch (id)
        {
            case EButtonId.Ok:
                PlayerPrefs.SetInt(SaveKey.mShipKey, (int)m_type);
                GameInstance.mInstance.mSceneLoad(new LoadInfo("InTutorial"));
                Close();

                break;
            case EButtonId.Cancel:
                Close();
                break;
        }
    }
    public void GetShipType(EShipType type)
    {
        m_type = type;
    }
    public void Open(GameObject content)
    {
       
        mButtonSet = EButtonSet.Set2;
        PopupButton.mOnClickCallback = PopupAction;
        m_viewContent = content;
        base.Open(null,null , ()=> 
        {
            m_contens.SetActive(true);
            m_viewContent.SetActive(true);
        });
    
    }

    public void Close()
    {
        base.Close(CloseEnd, null, null);
    }

}
