using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScript : BaseObject{
    [SerializeField]
    private Vector3 m_baseOffsetTPS;

    [SerializeField]
    private Vector3 m_baseRotationTPS;

    [SerializeField]
    private Vector3 m_baseOffsetFPS;

    private enum eCameraMode
    {
        eTPS,eFPS
    }
    private eCameraMode m_mode;

    protected override void Start()
    {
        mChangeTPS();
    }

    protected override void mOnRegistered()
    {
        mUnregisterList(this);
    }

    public void mChangeMode()
    {
        switch (m_mode)
        {
            case eCameraMode.eTPS:
                mChangeFPS();
                break;
            case eCameraMode.eFPS:
                mChangeTPS();
                break;
            default:
                break;
        }
    }

    private void mChangeTPS()
    {
        transform.localPosition= m_baseOffsetTPS;
        m_mode = eCameraMode.eTPS;
    }

    private void mChangeFPS()
    {
        transform.localPosition = m_baseOffsetFPS;
        m_mode = eCameraMode.eFPS;
    }


}
