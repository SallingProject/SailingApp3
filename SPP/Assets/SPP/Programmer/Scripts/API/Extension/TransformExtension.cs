/**********************************************************************************************/
/*! @file   TransformExtension.cs
*********************************************************************************************
@brief      Transform型に対しての拡張メソッドをまとめたファイル
*********************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
**********************************************************************************************/
using UnityEngine;
using System.Collections;

/**************************************************************************************
@brief  Transform型に対しての拡張メソッドをまとめた静的クラス
*/
public static class TransformExtension{

    /**************************************************************************************
    @brief        対象のTransformの子供からT型のコンポーネントを取得し、リストとして返す
    @return       子から取得したT型コンポーネントをすべて入れた配列
    */
    public static T[] GetChildrenComponentTo<T>(this Transform target)
    {
        int count = target.childCount;
        Transform transform = target;
        T[] children = new T[count];
        for (int i = 0; i < count; ++i)
        {
            children[i] = transform.GetChild(i).GetComponent<T>();
        }
        return children;
    }

    /**************************************************************************************
    @brief        アクティブ、非アクティブ関係なく子オブジェクトを取得する
    @return       すべての子オブジェクト
    */
    public static GameObject[] GetChildren(this Transform target)
    {
        int count = target.childCount;
        Transform transform = target;
        GameObject[] children = new GameObject[count];
        for (int i = 0; i < count; ++i)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
        return children;
    }

    /*******************************************************************************
    @brief        指定の名前の子オブジェクトを取得する。
                  第三引数をfalseにするとアクティブ、非アクティブ関係なく取得可能
    @return       成功：子オブジェクト/失敗:null
    */
    public static GameObject FindInChildren(this Transform target,string serchName,bool isActiveOnly)
    {
        if (isActiveOnly)
        {
            GameObject child = target.FindChild(serchName).gameObject;
            return child;
        }
        else
        {
            GameObject[] list = target.GetChildren();
            foreach (var child in list)
            {
                if(child.name == serchName)
                {
                    return child;
                }
            }
        }
        return null;
    }

    /**************************************************************************************
    @brief        アクティブ状態を切り替える
    @return       none
    */
    public static void SetActive(this Transform target, bool isActive)
    {
        target.gameObject.SetActive(isActive);
    }

    /**************************************************************************************
    @brief        オブジェクトが存在するかの判定
    @return       存在する：true/存在しない：false
    */
    public static bool IsValid(this Transform target)
    {
        return (target != null) ? true : false;
         
    }
}
