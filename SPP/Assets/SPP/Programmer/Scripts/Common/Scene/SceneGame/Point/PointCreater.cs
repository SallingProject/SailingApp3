using UnityEngine;
using System.Collections;

public class PointCreater : BaseObject{

    [SerializeField]
    private Point[] m_point;

    protected override void mOnRegistered()
    {
        mUnregisterList(this);
        mUnregister();
    }


    // Use this for initialization
    protected override void Start()
    {
        foreach (var obj in m_point)
        {
            obj.mInitializer();
        }
        GameInfo.mInstance.m_pointArray.mGetPoint().GetComponent<Point>().enabled = true;
        var instance = GameInfo.mInstance.m_pointArray.mGetLastPoint().transform.FindInChildren("In", false);
        instance.AddComponent<ClearChecker>();
    }

}
