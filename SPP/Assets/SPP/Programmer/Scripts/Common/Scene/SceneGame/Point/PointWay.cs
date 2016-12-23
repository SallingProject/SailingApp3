/**************************************************************************************/
/*! @file   PointWay.cs
***************************************************************************************
@brief      ポイントの判定
***************************************************************************************
@author     Tomoki Numakura
***************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointWay : BaseObject
{

    [SerializeField]
    private GameObject m_wayPrefab;

    private Vector3 m_point;
    private float angle;

    float m_now;                //現在の位置
    float m_prevpos;            //過去の位置
    float m_distance;           //今と過去の位置の距離の差

    [SerializeField]
    private GameObject m_ship;  //船

    [SerializeField]
    private BaseObject m_firstPoint;
    [SerializeField]
    private BaseObject m_secondPoint;

    private PointArrayObject m_pointArray;

    public override void mOnUpdate()
    {
        m_secondPoint = m_pointArray.mGetPoint();
        m_firstPoint = m_pointArray.mGetPrevPoint();
        if (!m_firstPoint.IsValid()) return;
        if (!m_secondPoint.IsValid()) return;


        m_point = m_secondPoint.transform.position + m_firstPoint.transform.position;   //ポイントとポイントの二点間の距離

        m_wayPrefab.transform.position = m_point / 2;                                      //ポイントとポイントの真ん中の場所

        m_point = m_secondPoint.transform.position - m_wayPrefab.transform.position;
        m_wayPrefab.transform.localScale = new Vector3(m_point.magnitude*2,0.01f,1.0f);

        angle = Mathf.Atan2(m_point.z, m_point.x);

        angle *= Mathf.Rad2Deg;
        m_wayPrefab.transform.rotation = Quaternion.AngleAxis(-angle, new Vector3(0, 1, 0));

        mOnPointWay();

    }

    // Use this for initialization
    override protected void Start()
    {
        m_wayPrefab = mCreate(m_wayPrefab);
        m_pointArray = GameInfo.mInstance.m_pointArray;
    }

    void mOnPointWay()
    {
        m_now = Vector3.Distance(m_secondPoint.transform.position, m_ship.transform.position); //船と次のポイントの距離

        if (m_prevpos == 0)
        {
            m_prevpos = m_now;
        }

        m_distance += -(m_now - m_prevpos);     //今の場所と過去の場所の比較をして距離の差を調べる

        m_prevpos = m_now;

    }
}
