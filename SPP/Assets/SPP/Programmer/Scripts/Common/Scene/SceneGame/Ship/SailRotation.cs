/**************************************************************************************/
/*! @file   SailRotation.cs
***************************************************************************************
@brief      セールのコントロール追加するクラス
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/
using UnityEngine;
using System.Collections;

public class SailRotation : BaseObject{
    [SerializeField]
    private bool debugMode = false;
    public override void mOnUpdate()
    {

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.J)){
            transform.localEulerAngles += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.K)){
            transform.localEulerAngles -= Vector3.up;
        }
        if (!debugMode)
#endif
        {
            transform.localEulerAngles = new Vector3(0, GameInfo.mInstance.mGetSailRotation(), 0);
        }


    }

}
