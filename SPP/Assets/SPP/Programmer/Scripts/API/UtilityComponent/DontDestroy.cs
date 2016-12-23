/**************************************************************************************/
/*! @file   DontDestroy.cs
***************************************************************************************
*@brief      削除しないオブジェクト用コンポーネント。
***************************************************************************************
*@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections.Generic;
/******************************************************************************* 
@brief   削除しないオブジェクト用コンポーネント。
*/
public class DontDestroy : BaseObject {

    // このコンポーネントをアタッチしているオブジェクトのリスト
    static Dictionary<string, GameObject> m_logList = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> mLogList
    {
        get { return m_logList; }
        private set { m_logList = value; }
    }


    /****************************************************************************** 
    @brief      BaseObjectの実装
    @note       管理リストに登録された時によばれる
    @return     none
    */
    protected override void mOnRegistered()
    {
        base.mOnRegistered();

        if (!mLogList.ContainsKey(this.gameObject.name))
        {
            mUnregisterList(this);
            DontDestroyOnLoad(this.gameObject);
            mLogList.Add(this.gameObject.name, this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
