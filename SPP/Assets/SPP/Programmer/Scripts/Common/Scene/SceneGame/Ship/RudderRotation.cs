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
        float move = 20;
        switch (GameInfo.mInstance.mButtonController.mButtonState)
        {
            case UIButtonController.eAround.LEFT:
                move *= -1;
                break;
            case UIButtonController.eAround.RIGHT:
                break;
            case UIButtonController.eAround.NEUTRAL:
                move = 0;
                break;
        }

        //Rote
        float shipDirection = move * (mHandling / 100);

        transform.eulerAngles += Vector3.up * shipDirection * Time.deltaTime;
    }

}
