/*************************************************************
@file       RotaWind.cs
@brief      オブジェクトの軸となる存在 
            gameobjectを中心にその周りを回転させる

@author     yuta takatsu

***************************************************************/
using UnityEngine;
using System.Collections;

public class RotaWind : BaseObject
{

    public float m_gizmoSize = 0.3f;
    public Color m_gizmoColor = Color.yellow;

    void OnDrawGizmos()
    {

        Gizmos.color = m_gizmoColor;
        Gizmos.DrawWireSphere(transform.position, m_gizmoSize);
    }
}
