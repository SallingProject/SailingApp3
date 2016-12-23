/**********************************************************************************************/
/*! @file   BaseObject.cs
*********************************************************************************************
@brief      すべてのオブジェクトの基底クラス
*********************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
**********************************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/******************************************************************************* 
@brief   すべてのオブジェクトの基底クラス
*/
public abstract class BaseObject: MonoBehaviour{
    
    #region メンバ変数、メンバ変数のアクセサー
    /******************************************************************************
    @brief      BaseObject型オブジェクト管理配列
    */
    static LinkedList<BaseObject> m_objectList = new LinkedList<BaseObject>();

    static LinkedList<BaseObject> m_managerObjectList = new LinkedList<BaseObject>();


    static List<BaseObject> m_unregisterList = new List<BaseObject>();

    public static LinkedList<BaseObject> mObjectList
    {
        get { return m_objectList; }
        private set { m_objectList = value; }
    }

    public static LinkedList<BaseObject> mManagerObjectList
    {
        get { return m_managerObjectList; }
        private set { m_managerObjectList = value; }
    }

    /******************************************************************************
   @brief      自身の参照オブジェクト
   */
    private BaseObject m_owner;
    public BaseObject mOwner
    {
        get { return m_owner; }
        private set { m_owner = value; }
    }

    /**************************************************************************************
    @brief      削除時の関数が呼ばれたかのフラグ。mOnDeleteが一度だけ呼ぶことを保証するため
    */
    private bool m_isCallDeleted = false;

    /**************************************************************************************
    @brief      BaseObjectを基底クラスとして作成されたコンポーネントの総数
    */
    public static int mObjectCount
    {
        get;
        private set;
    }

    /**************************************************************************************
    @brief      BaseObjectを基底クラスとして作成されたManagerコンポーネントの総数
    */
    public static int mManagerObjectCount
    {
        get;
        private set;
    }
    #endregion

    #region MonoBehaviorによる実装のもの
    /****************************************************************************** 
    @brief      オブジェクト生成時に呼ばれる。MonoBehaviorの実装
    @note       スクリピティングランタイム適用のため、コーティングルールには従いません。
                基本的にオーバーライドせずmOnRegistered関数かStart関数をオーバーライドして使用してください。
                オーバーライドする場合は必ず派生先の先頭でbase.Awake()と記述してください。
    @return     none
    */
    protected virtual void Awake()
    {
        m_isCallDeleted = false;
        mOwner = this;
        mRegisterList(this);
    }
    
    /****************************************************************************** 
    @brief      オブジェクトのアップデート前に呼ばれる。MonoBehaviorの実装
    @note       スクリピティングランタイム適用のため、コーティングルールには従いません。
    @return     none
    */
    protected virtual void Start(){ return; }

    /****************************************************************************** 
    @brief      アクティブオブジェクト削除時に呼ばれる。MonoBehaviorの実装
    @note       スクリピティングランタイム適用のため、コーティングルールには従いません。オーバーライドしないでください。
    @return     none
    */
    protected void OnDestroy()
    {

        Transform transform = this.gameObject.transform;

        foreach(var child in transform.GetChildrenComponentTo<BaseObject>())
        {
            if(child != null)
            {
                child.mDeleteCallback();
                mUnregisterList(child);
            }
        }

        mUnregisterList(this);
        this.mDeleteCallback();
    }
    #endregion

    #region 派生先でオーバーライドが可能なもの
    /******************************************************************************
    @brief      更新処理。継承先で実装する。
    @return     none
    */
    public virtual void mOnUpdate() { return; }

    /******************************************************************************
    @brief      マネージャー系の更新処理。継承先で実装する。
    @return     none
    */
    public virtual void mOnFastUpdate() { return; }

    /******************************************************************************
    @brief      一番最後の更新処理。継承先で実装する。
    @return     none
    */
    public virtual void mOnLateUpdate() { return; }

    /******************************************************************************
    @brief      物理系の更新処理。継承先で実装する。
    @return     none
    */
    public virtual void mOnFixedUpdate() { return; }

    /******************************************************************************
    @brief      予約したオブジェクトを管理対象から外す
    @return     none
    */
    public void mUnregister()
    {
        foreach(var obj in m_unregisterList)
        {
            if (mObjectList.Remove(obj))
            {
                obj.mOnUnregistered();
            }
        }
        m_unregisterList.Clear();
    }
    /****************************************************************************** 
    @brief      管理リストへ登録された時に1度だけ呼ばれる。オーバーライド可能
    @return     none
    */
    protected virtual void mOnRegistered() { return; }

    /****************************************************************************** 
    @brief     管理リストから外された時に1度だけ呼ばれる。オーバーライド可能
    @return     none
    */
    protected virtual void mOnUnregistered() { return; }

    /****************************************************************************** 
    @brief      削除実行時に1度だけ呼ばれる。オーバーライド可能
    @return     none
    */
    protected virtual void mOnDelete() { return; }

    #endregion

    #region 静的関数群
    /******************************************************************************
    @brief      削除実行時１度だけ呼ばれる。
    @note       一度実行されている場合は実行しない
    @return     none
    */
    private void mDeleteCallback()
    {
        if (m_isCallDeleted) return;
        m_isCallDeleted = true;
        mOnDelete();
        mObjectCount = mObjectList.Count;
    }

    /****************************************************************************** 
    @brief      指定オブジェクトを管理リストの最後尾に追加する。
    @note       すでにあるオブジェクトは追加できません。
    @return     none
    */
    static public void mRegisterList(BaseObject input)
    {

        if (mSerch(input) != null) return;
        mObjectList.AddLast(input);
        
        input.mOnRegistered();
        mObjectCount = mObjectList.Count;
    }

    /****************************************************************************** 
    @brief      指定オブジェクトを管理リストの最後尾に追加する。
    @note       すでにあるオブジェクトは追加できません。
    @return     none
    */
    static public void mManagerRegisterList(BaseObject input)
    {
        if (mManagerSerch(input) != null) return;
            mManagerObjectList.AddLast(input);
        mManagerObjectCount = mManagerObjectList.Count;
    }

    /****************************************************************************** 
    @brief      指定オブジェクトが管理リストの管理対象なら管理リストから外す
    @return     none
    */
    static public void mUnregisterList(BaseObject input)
    {
        if (mSerch(input) == null && mManagerSerch(input) == null)
            return; 
        m_unregisterList.Add(input);

        return;
    }

    /****************************************************************************** 
    @brief      オブジェクト検索用
    @return     発見時：そのオブジェクトの参照/ないとき:null
    */
    static public BaseObject mSerch(BaseObject input)
    {
        var findObject = mObjectList.Find(input);
        if(findObject != null)
        {
            return findObject.Value;
        }
        return null;
    }

    /****************************************************************************** 
    @brief      Managerオブジェクト検索用
    @return     発見時：そのオブジェクトの参照/ないとき:null
    */
    static public BaseObject mManagerSerch(BaseObject input)
    {
        var findObject = mManagerObjectList.Find(input);
        if (findObject != null)
        {
            return findObject.Value;
        }
        return null;
    }

    /********************************************************************************************
    @brief      オブジェクト削除用。削除したいオブジェクトがBaseObject型なら管理リストからも削除
    @note       MonoBehaviorのDestroy関数は使用せずこの関数を使用してください。
    @return     none
    */
    static public void mDelete<T>(T remove) where T : UnityEngine.Object
    {
        if (remove == null) return;
        
        Destroy(remove);
        return;
    }

    static public void mDeleteImmediate<T>(T remove) where T : UnityEngine.Object
    {
        if (remove == null) return;

        DestroyImmediate(remove);
        return;
    }

    /********************************************************************************************
    @brief      オブジェクト削除用。削除したいオブジェクトがBaseObject型なら管理リストからも削除
    @note       MonoBehaviorのDestroy関数は使用せずこの関数を使用してください。
    @return     none
    */
    static public void mDelete(BaseObject remove)
    {
        if (remove == null) return;

        if (mSerch(remove) != null)
            mUnregisterList(remove);

        Destroy(remove.gameObject);
        return;
    }

    static public void mDeleteImmediate(BaseObject remove)
    {
        if (remove == null) return;

        if (mSerch(remove) != null)
            mUnregisterList(remove);

        DestroyImmediate(remove);
        return;
    }


    /****************************************************************************** 
    @brief      オブジェクト生成用。生成したオブジェクトがBaseObject型なら管理リストに登録
    @note       MonoBehaviorのInstantiate関数は使用しないでください。
    @return     成功時：生成したオブジェクト/失敗時:null
    */
    static public T mCreate<T>(T input) where T : UnityEngine.Object
    {
        if (input == null) return null;
        T output = Instantiate(input) as T;
        return output;
    }

    #endregion
}