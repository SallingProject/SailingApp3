/**************************************************************************************/
/*! @file   CreateShip.cs
***************************************************************************************
@brief      船を生成するクラス
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/

using UnityEngine;
using System.Collections;

public class CreateShip : BaseObject{
    private int m_shipDataSize = 0;

    protected override void Start()
    {
        mUnregisterList(this);


        var player = mCreatePlayer();
        //GameInfoへ登録
        GameInfo.mInstance.mCreateShipData(m_shipDataSize);
        GameInfo.mInstance.mShipStatus[0] = player;
    }


    /****************************************************************************** 
    @brief      スクリプタブルオブジェクトのパスを返す
    @param[in]  船のタイプ
    @return     ScriptableObjectPath
    *******************************************************************************/
    private string mShipPath(EShipType type)
    {
        string shipData = "Data/Ship/";
        switch (type)
        {
            case EShipType.Class470:
                shipData += "Ship0004";
                break;
            case EShipType.ClassLaser:
                shipData += "Ship0001";
                break;
            //case EShipType.Class49er:
            //    shipData += "Ship0002";
            //    break;
            //case EShipType.ClassRS_X:
            //    shipData += "Ship0003";
            //    break;
            default:
                shipData += "ShipTest";
                break;
        }
        return shipData;
    }

    /****************************************************************************** 
    @brief      操作するプレイヤーとなるオブジェクトを作る
    @return     ShipStatus
    *******************************************************************************/
    private ShipStatus mCreatePlayer()
    {
        //GameManagerから受け取る
        var scripPath = mShipPath((EShipType)PlayerPrefs.GetInt(SaveKey.mShipKey));

        var scripObj = Resources.Load(scripPath) as ShipDefine;
        var modelpath = scripObj.mPath;

        var obj = Resources.Load(modelpath);
        var instance = mCreate(obj) as GameObject;
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localEulerAngles = Vector3.zero;
        instance.GetComponentInChildren<SailRotation>().enabled = true;

        var shipMove = GetComponent<ShipMove>();
        shipMove.mSetShipDefine(scripObj);
        shipMove.mInitialize();

        var shipStatus = gameObject.GetComponent<ShipStatus>();
        shipStatus.mId = m_shipDataSize;
        shipStatus.mShip = shipMove;

        m_shipDataSize++;
        instance.name = "Player" + m_shipDataSize.ToString();
        return shipStatus;
    }
}
