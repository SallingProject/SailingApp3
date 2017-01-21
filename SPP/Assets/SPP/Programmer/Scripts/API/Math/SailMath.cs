﻿/**************************************************************************************/
/*! @file   SailMath.cs
***************************************************************************************
@brief      よく使いそうな計算関数をまとめたクラス
***************************************************************************************
@note       追記はご自由に
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/

using UnityEngine;
using System.Collections;

public class SailMath{

    /************************************************
    @brief  角度からVector2に変換する
    */
    public static Vector2 mDegToVector2(float value)
    {
        return new Vector2(Mathf.Sin(value * Mathf.Deg2Rad), Mathf.Cos(value * Mathf.Deg2Rad));
    }

    /************************************************
    @brief  ラジアンからVector2に変換する
    */
    public static Vector2 mRadToVector2(float value)
    {
        return new Vector2(Mathf.Sin(value), Mathf.Cos(value));
    }
    
    /****************************************************************************** 
    @brief      迎え角を計算する
    @note       fluid   流体,0~360°    target  対象
    @return     迎え角
    *******************************************************************************/
    public static float mAngleAttack(float fluidDirec, float targetDirec)
    {
        Vector2 fluidVec, targetVec;
        fluidVec = mDegToVector2(fluidDirec);
        targetVec = mDegToVector2(targetDirec);
        return Mathf.Acos(Vector2.Dot(fluidVec, targetVec)) * Mathf.Rad2Deg;
    }

}
