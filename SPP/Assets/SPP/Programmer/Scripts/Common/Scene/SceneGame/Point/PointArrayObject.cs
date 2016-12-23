/**************************************************************************************/
/*! @file   PointArrayManager.cs
***************************************************************************************
@brief      ポイントを管理するArrayを管理する
@note       とりあえず
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/


using UnityEngine;
using System.Collections.Generic;

public class PointArrayObject : BaseObject {


    private Dictionary<int,BaseObject>m_pointArray = new Dictionary<int, BaseObject>();
    private int m_currentId = 1;


    //ポイント配列に登録
    protected override void mOnRegistered()
    {
        mUnregisterList(this);
    }


    public void mRegisterArray(int index, BaseObject obj)
    {
        m_pointArray.Add(index,obj);
    }
    /****************************************************************************** 
    @brief      ポイントを次へ進める
    @note       次がないときは最後/継承先でしか利用できない
    @return     のね
    *******************************************************************************/
    public void mNext()
    {
        //Debug.Log("ClearPoint:"+m_currentId);
        if (m_currentId > m_pointArray.Count) return;

        //Baseクラスに書き換える
        //mUnregisterList(m_pointArray[m_currentId]);
        m_pointArray[m_currentId].GetComponent<Point>().enabled = false;
        m_pointArray[m_currentId].GetComponent<SphereCollider>().enabled = false;
        m_currentId++;
        if (m_currentId > m_pointArray.Count) return;
        m_pointArray[m_currentId].GetComponent<Point>().enabled = true;
        GameInfo.mInstance.m_targetMarker.mSetTarget(m_pointArray[m_currentId].GetComponent<ReflectedOnCamera>());
        GameInfo.mInstance.InductionRing.mSetNextPoint(m_pointArray[m_currentId].GetComponent<Point>());
    }


    /****************************************************************************** 
    @brief      現在の（次の）ポイントを取得する
    @note       
    @return     ポイント(BaseObject)
    *******************************************************************************/
    public BaseObject mGetPoint() {
        if (m_currentId > m_pointArray.Count) return null;
        return m_pointArray[m_currentId];
    }

    /****************************************************************************** 
    @brief      前のポイントを取得する
    @note       最初は前が無いのでNullを返します。
    @return     ポイント(BaseObject)
    *******************************************************************************/
    public BaseObject mGetPrevPoint()
    {
        if (m_currentId <= 1 || m_currentId > m_pointArray.Count) return null;
        return m_pointArray[m_currentId-1];
    }

    /****************************************************************************** 
    @brief      最後のポイントを取得する
    @return     ポイント(BaseObject)
    *******************************************************************************/
    public BaseObject mGetLastPoint()
    {
        return m_pointArray[m_pointArray.Count];
    }

}
    

