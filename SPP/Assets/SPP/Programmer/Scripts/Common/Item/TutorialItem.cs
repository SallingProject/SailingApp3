using UnityEngine;
using System.Collections;

public class TutorialItem : BaseObject
{

    [SerializeField]
    ItemDefine m_define;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);
    }

    void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
        GameInfo.mInstance.UserAction.SetItem(m_define);
    }
}
