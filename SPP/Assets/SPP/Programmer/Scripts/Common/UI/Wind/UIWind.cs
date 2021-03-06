﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIWind : BaseObject
{
    //マテリアル関連
    public Material mat;
    private Color color;

    [SerializeField]
    private WindObject m_wind;
    [SerializeField]
    private ShipMove m_ship;
   

    public override void mOnUpdate()
    {

        base.mOnUpdate();

        //m_WindDirectionの値分回転
        transform.eulerAngles = new Vector3(0, 0, m_ship.transform.eulerAngles.y - m_wind.mWindDirection);

        float diff = m_ship.transform.eulerAngles.y - m_wind.mWindDirection;
        if(diff >= 180)
        {
            diff = diff - 360;
        }
        //風向きが-25～25の値の時UIの色を赤に変える それ以外は緑
        if (diff >= -25.0 &&
           diff <= 25.0)
        {
            //赤
            color = new Color(1.0F, 0.0F, 0.0F, 1.0F);
        }
        else
        {
            //緑
            color = new Color(0.0F, 1.0f, 0.0F, 1.0F);
        }

        //色を変更
        mat.SetColor("_Color", color);
    }
}