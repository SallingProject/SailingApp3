using UnityEngine;
using System.Collections;

public class ReflectedOnCamera: BaseObject {

    public bool mIsOnView
    {
        get;
        private set;
    }

    public Camera mGameCamera
    {
        get{ return Camera.main; }
    }

    protected override void mOnRegistered()
    {
        mUnregisterList(this);
    }


#if UNITY_EDITOR
    /// <summary>
    /// 更新時に呼ばれる
    /// BaseObjectの実装
    /// </summary>
    public void mUpdate()
    {
        mIsOnView = false;
    }
#endif

    /// <summary>
    /// 不可視状態の時に呼ばれる
    /// MonoBehaviorの実装
    /// </summary>
    /// 
#if !UNITY_EDITOR
    void OnBecameInvisible()
    {
        mIsOnView = false;
    }
#endif

    /// <summary>
    /// 可視状態の時に呼ばれる
    /// MonoBehaviorの実装
    /// </summary>
    void OnWillRenderObject()
    {
#if UNITY_EDITOR
        
        if (Camera.current.name == Camera.main.name)
#endif
        {
            // 処理
            mIsOnView = true;
        }
    }
}
