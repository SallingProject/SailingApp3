using UnityEngine;
using System.Collections;

public class WindModifyInTutorial : BaseObject{

    [SerializeField]
    int m_modifyWindDirection;
    // Use this for initialization
    protected override void mOnRegistered()
    {
        mUnregisterList(this);
    }


    void OnTriggerEnter(Collider col)
    {
        GameInfo.mInstance.m_wind.mWindDirection = m_modifyWindDirection;
    }

}
