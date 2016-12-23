/**************************************************************************************/
/*! @file   RudderRotation.cs
***************************************************************************************
@brief      舵のコントロールを追加するクラス
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/

using UnityEngine;
using System.Collections;

public class RudderRotation : BaseObject
{

    public float mHandling{ get; set; }

    public override void mOnUpdate()
    {
        //Rote
        float shipDirection = GameInfo.mInstance.mGetHandleRotation() * (mHandling / 100);
        transform.eulerAngles += Vector3.up * shipDirection * Time.deltaTime;
    }

}
