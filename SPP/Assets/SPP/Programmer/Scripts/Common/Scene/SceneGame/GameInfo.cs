using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameInfo : BaseObjectSingleton<GameInfo>{

    [SerializeField]
    public GuideMarker m_targetMarker;
    [SerializeField]
    private UIButtonController m_buttonController;
    public UIButtonController mButtonController
    {
        get { return m_buttonController; }
    }

    [SerializeField]
    private UIUserAction m_userAction;
    public UIUserAction UserAction
    {
        get{return m_userAction;}
    }

    [SerializeField]
    public WindObject m_wind;       //風オブジェクト
    [SerializeField]
    private LongPressButton m_deceleration;     //減速ボタン
    public LongPressButton mDeceleration
    {
        get { return m_deceleration; }
    }

    [SerializeField]
    private InductionRing m_inductionRing;
    public InductionRing InductionRing
    {
        get { return m_inductionRing; }
    }

    [SerializeField]
    public PointArrayObject m_pointArray;       //ポイント配列管理クラス
    private ShipStatus[] m_shipStatus;          //指定関数によって初期化する
    public ShipStatus[] mShipStatus
    {
        get { return m_shipStatus; } set { m_shipStatus = value; }
    }

    private List<ItemDefine> m_itemList = new List<ItemDefine>();

    public bool mControllerTrigger { get; private set; }
    
    private float m_prevControllerRotation = 0;

    public bool mIsEnd{     //ゴールラインを切ったか
        get; set;
    }

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mIsEnd = false;
    }

    /****************************************************************************** 
    @brief      発動するアイテムを保存する。
    @param[in]  発動するアイテムの定義情報
    @param[in]  使用者ID
    @return     none
    *******************************************************************************/
    public void SetInvokeItem(ItemDefine define,int userId)
    {

        if (define == null) return;

        switch (define.mType)
        {
            case ItemType.Invalid:
                break;
            case ItemType.Own:
                m_shipStatus[userId].mShip.mItemActivate(define.mEffect);
                break;
            case ItemType.SomeOne:
                int rand = (int)Random.Range(0, m_shipStatus.Length);
                m_shipStatus[userId].mShip.mItemActivate(define.mEffect);
                break;
            case ItemType.IgnoreOwn:
                foreach (var ship in m_shipStatus){
                    if (userId != ship.mId){
                        ship.mShip.mItemActivate(define.mEffect);
                    }
                }
                break;
            case ItemType.All:
                foreach(var ship in m_shipStatus){
                    ship.mShip.mItemActivate(define.mEffect);
                }
                break;
            default:
                break;
        }
        //m_itemList.Add(define);
    }

    /****************************************************************************** 
    @brief      ShipStatus初期化用関数
    @param[in]  プレイヤー及び敵などの総数
    @return     none
    *******************************************************************************/
    public void mCreateShipData(int size)
    {
        m_shipStatus = new ShipStatus[size];
    }
}
