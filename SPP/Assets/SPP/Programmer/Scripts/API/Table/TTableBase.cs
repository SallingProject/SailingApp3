/**********************************************************************************************/
/*! @file   TTableBase.cs
*********************************************************************************************
@brief      インスペクター上でDictionaryみたいなのを使えるようにするたに集めたクラス
*********************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
**********************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**************************************************************************************
@brief  キーと値を自由に設定できるペア型。C++のstd::pair型みたいなもの
*/
[System.Serializable]
public class TPair<Key, Value>
{
    public Key mKey;
    public Value mValue;

    public TPair(Key key, Value value)
    {
        mKey = key;
        mValue = value;
    }

    public TPair(TPair<Key, Value> pair)
    {
        mKey = pair.mKey;
        mValue = pair.mValue;
    }
}

/**************************************************************************************
@brief  インスペクター上でDictionaryみたいなのを使えるようにするクラス
*/
[System.Serializable]
public class TTableBase<Key,Value,Type> where Type :TPair<Key,Value>{
    
    [SerializeField]
    private List<Type> m_list = new List<Type>();
    public List<Type> mList
    {
        get { return m_list; }
        private set { m_list = value; }
    }
    

    /**************************************************************************************
    @brief  ListをDictionaryに変換して取得
    */
    public Dictionary<Key, Value> GetTable()
    {
        Dictionary<Key, Value> table = new Dictionary<Key, Value>();
        if (mList != null)
        {
            table = ConvertListToTable(mList);
        }

        return table;
    }

    /**************************************************************************************
    @brief  ListをDictionaryに変換
    */
    Dictionary<Key,Value> ConvertListToTable(List<Type> list)
    {
        Dictionary<Key, Value> outTable = new Dictionary<Key, Value>();
        foreach(var pair in mList)
        {
            outTable.Add(pair.mKey, pair.mValue);
        }
        return outTable;
    }
}