using UnityEngine;
using System.Collections.Generic;

public class InductionRing : BaseObject {

    static readonly int     m_kCreateCount = 7;
    static readonly float   m_kRadius = 15f;

    [SerializeField]
    GameObject m_ringPref;

    [SerializeField]
    float m_zOffset = 4;

    List<GameObject> m_ringCashList = new List<GameObject>();

    Point m_prevPoint;
    Point m_nextpoint;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);

        m_ringCashList.Clear();
        for(int i=0;i< m_kCreateCount; ++i)
        {
            var add = mCreate(m_ringPref) as GameObject;
            add.transform.SetParent(this.transform, false);
            m_ringCashList.Add(add);
        }
    }


    public void mSetNextPoint(Point nextPoint)
    {

        foreach(var index in m_ringCashList)
        {
            index.SetActive(false);
        }
        m_prevPoint = m_nextpoint;
        m_nextpoint = nextPoint;

        //Debug.Log(m_nextpoint.name);
        if (nextPoint.m_buoyType == Point.eBuoyType.Curve)
        {
            //オブジェクト間の角度差
            float angleDiff = 200f / (float)m_ringCashList.Count;
            //各オブジェクトを円状に配置
            for (int i = 0; i < m_ringCashList.Count; i++)
            {
                Vector3 childPostion = transform.position;

                float angle = (((angleDiff * i)) + m_nextpoint.transform.eulerAngles.y) * Mathf.Deg2Rad;

                childPostion.z += m_kRadius * Mathf.Cos(angle) + m_nextpoint.transform.position.z;
                childPostion.x += m_kRadius * Mathf.Sin(angle) + m_nextpoint.transform.position.x;
                float ringDirection = Mathf.Atan2(Mathf.Sin(angle), Mathf.Cos(angle));

                m_ringCashList[i].SetActive(true);
                m_ringCashList[i].transform.eulerAngles = new Vector3(0, ringDirection*Mathf.Rad2Deg, 0);
                m_ringCashList[i].transform.position = childPostion;
            }
        }
        else
        {
            Vector3 pointPosition = (m_prevPoint == null) ?
                m_nextpoint.transform.position : m_nextpoint.transform.position + m_prevPoint.transform.position;
            
            for (int i = 0; i < m_ringCashList.Count/2; i++)
            {
                m_ringCashList[i].SetActive(true);

                m_ringCashList[i].transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                m_ringCashList[i].transform.position = new Vector3
                    (m_nextpoint.transform.position.x + ((nextPoint.transform.rotation.y / 2) * i),
                    m_nextpoint.transform.position.y,
                     m_nextpoint.transform.position.z - ((m_zOffset * i) + (nextPoint.transform.rotation.y * i)));

            }
        }
    }

    #region テスト用
#if false
    void Update()
    {
        mSetNextPoint(m_nextpoint);
    }
#endif
    #endregion
}
