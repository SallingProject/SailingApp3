/**************************************************************************************/
/*! @file   ResourceManager.cs
***************************************************************************************
@brief      ゲーム全体のマネージャークラス
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;

public class ResourceManager : BaseObjectSingleton<ResourceManager> {
    
    /****************************************************************************** 
    @brief      船のオブジェクトのパスを取得取得
    @return     none
    */
    public GameObject GetShipObject(EShipType type)
    {

        return Resources.Load(GetShipPath(type)) as GameObject;
    }


    public T mLoad<T>(string path) where T : class
    {
        var h = Resources.Load(path);
        return Resources.Load(path)as T;
    }

    public Sprite mSpriteLoad(string path)
    {
        Texture2D tex = Resources.Load(path) as Texture2D;

        Sprite output = Sprite.Create(tex, new Rect(0, 0, 256, 256), Vector2.zero);
        return output;
    }

    /****************************************************************************** 
    @brief      船のオブジェクトのパスを取得取得
    @return     none
    */
    string GetShipPath(EShipType type)
    {
        switch (type)
        {
            case EShipType.Class470:
                return "Ship/Test";

            case EShipType.Class49er:
                return "Ship/Test";

            case EShipType.ClassLaser:
                return "Ship/Test";

            case EShipType.ClassRS_X:
                return "Ship/Test";

            case EShipType.Invalid:
                return "Ship/Test";
        }
        return null;
    }
}
