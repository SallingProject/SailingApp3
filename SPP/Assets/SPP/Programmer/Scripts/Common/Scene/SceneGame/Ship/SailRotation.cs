/**************************************************************************************/
/*! @file   SailRotation.cs
***************************************************************************************
@brief      セールのコントロール追加するクラス
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SailRotation : BaseObject{
    protected override void mOnRegistered()
    {
        mUnregisterList(this);
    }

    public void mRotateSail(float rote)
    {
        rote = Mathf.Clamp(rote, -90, 90);
        transform.DOLocalRotate(new Vector3(0, rote, 0), 2.0f);
    }


}
