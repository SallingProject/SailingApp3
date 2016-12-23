/**************************************************************************************/
/*! @file   ShipDefine.cs
***************************************************************************************
@brief      船の定義
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/

using UnityEngine;
/************************************************************************************** 
@brief  船の定義クラス
*/
public class ShipDefine : ScriptableObject {

    /************************************************************************************** 
    @brief  船の種類
    */
    [SerializeField]
    EShipType m_type;
    public EShipType mType 
    {
        get { return m_type; }
    }

    /************************************************************************************** 
    @brief  船のパス
    */
    [SerializeField]
    string m_path;
    public string mPath
    {
        get { return m_path; }
    }

    /************************************************************************************** 
    @brief  船の重さ
    */
    [SerializeField]
    float m_weight;
    public float mWeight
    {
        get { return m_weight; }
    }

    /************************************************************************************** 
    @brief  船のハンドリング
    */
    [SerializeField]
    float m_handling;
    public float mHandling
    {
        get { return m_handling; }
    }

    /************************************************************************************** 
    @brief  船の加速度
    */
    [SerializeField]
    float m_acceleration;
    public float mAcceleration
    {
        get { return m_acceleration; }
    }

    /************************************************************************************** 
    @brief  船の最高速度
    */
    [SerializeField]
    float m_maxSpeed;
    public float mMaxSpeed
    {
        get { return m_maxSpeed; }
    }

    
}
