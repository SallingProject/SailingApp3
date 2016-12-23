using UnityEngine;
using System.Collections;

public class DoHitDeactivate : BaseObject {

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);
    }
    void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }
}
