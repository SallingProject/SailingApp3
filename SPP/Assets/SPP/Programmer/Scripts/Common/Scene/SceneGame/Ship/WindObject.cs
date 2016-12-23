/**************************************************************************************/
/*! @file   WindObject.cs
***************************************************************************************
@brief      風とするオブジェクトに設定するクラス
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/

using UnityEngine;
using System.Collections;

public class WindObject : BaseObject{

    [SerializeField]
    private float m_windForce = 0;

    [SerializeField]
    [Range(0, 360)]
    private float m_windDirection = 0;

    public float mWindForce
    {
        get { return m_windForce; }
        set { m_windForce = value; }
    }
    public float mWindDirection
    {
        get { return m_windDirection; }
        set {
            //360°以上,-にはならないようにする
            if (value >= 360)
            {
                m_windDirection = value - 360;
            }
            else if (value < 0)
            {
                m_windDirection = 360 - value;
            }
            else
                m_windDirection = value;
        }
    }

    protected override void mOnRegistered()
    {
        mUnregisterList(this);
        mUnregister();
    }

}
