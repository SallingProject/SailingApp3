/**************************************************************************************/
/*! @file   PopupEnum.cs
***************************************************************************************
@brief      PopupWindow関係のenumの定義所
***************************************************************************************
@author     Ko Hashimoto and Kana Yoshidumi
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/

public enum EPopupState
{
    OpenBegin,
    Openning,
    OpenEnd,
    CloseBegin,
    Closing,
    CloseEnd
}

public enum EButtonId
{
    Ok,
    Cancel
}


public enum EButtonSet
{
   SetNone = 0,
   Set1,
   Set2,
}
