//=============================================================================
/**
	@file	OnBillboard.cs
	@brief	ビルボード用
	@author	Ko Hashimoto
*/
//=============================================================================
using UnityEngine;
using System.Collections;

public class OnBillboard : BaseObject {

    public override void mOnUpdate()
    {
        base.mOnUpdate();

        //Transform cameraTrans = Camera.main.transform;
        //Transform thisTrans = this.transform;
        //Vector3 lookAt = cameraTrans.position;
        //lookAt.x = lookAt.z = 0;
        //thisTrans.LookAt(lookAt);
    }
}
