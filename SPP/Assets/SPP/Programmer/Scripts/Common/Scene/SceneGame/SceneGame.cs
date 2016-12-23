/**************************************************************************************/
/*! @file   SceneGame.cs
***************************************************************************************
@brief      ゲームシーンの管理（Initializer）
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/

using UnityEngine;
using System.Collections;

public class SceneGame : SceneBase{

    /*
    とりあえずGameInstanceを持ってくる
    */
    [SerializeField]
    private PointCreater m_creater;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
    }

    protected override void Start()
    {
        //初期化したい順番ごとにクラスを追加していく
        //m_creater.mInitializer();

    }
}
