/**************************************************************************************/
/*! @file   SceneHome.cs
***************************************************************************************
@brief      ホームで起こる挙動は全部ここに書いてます
***************************************************************************************
@author     Hiroto Morikawa
***************************************************************************************
* Copyright © 2016 Hiroto Morikawa All Rights Reserved.
***************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneHome : SceneBase
{
    public void mGoToTutorial()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("SelectCourse", LoadInfo.ELoadType.Async, 1f));
    }

    public void mGoToCpuBattle()
    {
        // GameInstance.mInstance.mSceneLoad(new LoadInfo("CpuBattle", LoadInfo.ELoadType.Async, 1f));
    }

    public void mGoToOnlineBattle()
    {
        // GameInstance.mInstance.mSceneLoad(new LoadInfo("OnlineBattle", LoadInfo.ELoadType.Async, 1f));
    }

    public void mGoToLibrary()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Library", LoadInfo.ELoadType.Async, 1f));
    }

    public void mGoToConfig()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Config", LoadInfo.ELoadType.Async, 1f));
    }
}