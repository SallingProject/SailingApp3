using UnityEngine;
using System.Collections;

public class ClearChecker : BaseObject {

    void OnDisable()
    {
        GameInfo.mInstance.mIsEnd = true;
    }

}
