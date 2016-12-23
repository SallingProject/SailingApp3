/**************************************************************************************/
/*! @file   BaseObjectSingleton.cs
***************************************************************************************
@brief      シングルトン作成用基底クラス
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/

/******************************************************************************* 
@brief   シングルトン作成用基底クラス
*/
public class BaseObjectSingleton<T> : BaseObject where T : BaseObjectSingleton<T>
{
    private static T m_instance;
    public static T mInstance
    {
        get{ return m_instance; }
    }


    /****************************************************************************** 
    @brief      インスタンスがあるかの確認用    
    */
    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mCheckInstance();
		mUnregisterList (this);

        mManagerRegisterList(this);
    }

    /****************************************************************************** 
    @brief      インスタンスがあるかの確認用    
    @return     インスタンスがある：true / ない：false
    */
    protected bool mCheckInstance()
    {
        if (m_instance == null)
        {
            m_instance = (T)this;
            return true;
        }
        else if (m_instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }


}