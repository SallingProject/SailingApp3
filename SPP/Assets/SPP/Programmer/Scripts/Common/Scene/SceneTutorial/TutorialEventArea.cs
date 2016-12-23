/**************************************************************************************/
/*! @file   TutorialEventArea.cs
***************************************************************************************
@brief      チュートリアルのイベント発生時におこるやつ
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialEventArea : BaseObject
{
    [SerializeField]
    int m_eventId = 0;

    [SerializeField]
    bool m_isOneTimeOnly = false;

    [SerializeField]
    List<GameObject> m_animations;

    public int mAnimationId
    {
        get;
        set;
    }

    public List<GameObject> Animations
    {
        get { return m_animations; }
    }

    bool m_used = false;

    public System.Action<int> mEventCallback
    {
        private get;
        set;
    }

    

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);
    }
    void OnTriggerEnter(Collider other)
    {
        if ((!m_used && m_isOneTimeOnly))
        {
            mEventCallback.Invoke(m_eventId);

            m_used = m_isOneTimeOnly ? true : false;
        }
    }

    /**************************************************************************************
    @brief  	イベント開始時に呼ぶ
    */
    public void BeginEvent()
    {
        GameInfo.mInstance.mShipStatus[0].mShip.mItemActivate(ItemEffect.Invalid, ShipMove.EEffectTimeType.Infnit);
        GameInfo.mInstance.mShipStatus[0].mShip.mIsInfnit = true;
    }

    /**************************************************************************************
    @brief  	イベント終了時に呼ぶ
    */
    public void ExitEvent()
    {
        GameInfo.mInstance.mShipStatus[0].mShip.mIsInfnit = false;
    }
}
