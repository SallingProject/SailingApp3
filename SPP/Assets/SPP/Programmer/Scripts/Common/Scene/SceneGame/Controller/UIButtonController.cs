using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonController : BaseObject
{

    //方向を判断する
    public enum eAround
    {
        LEFT,     //左が押されている状態で使用
        RIGHT,    //右が押されている状態で使用
        NEUTRAL,  //何も押されていない状態で使用
    }

    public eAround mButtonState
    {
        get;
        private set;
    }

    //初期化
    protected override void mOnRegistered()
    {
        base.mOnRegistered();

        mButtonState = eAround.NEUTRAL;
    }

    //押しているButtonに応じてFlagの状態を更新
    public void mOnClick(int mNumber)
    {
        switch ((eAround)mNumber)
        {

            //LEFT
            case eAround.LEFT:
                mButtonState = eAround.LEFT;
                break;

            //RIGHT
            case eAround.RIGHT:
                mButtonState = eAround.RIGHT;
                break;

            //NEUTRAL
            default:
                mButtonState = eAround.NEUTRAL;
                break;
        }
//        Debug.Log(mButtonState);
    }
}