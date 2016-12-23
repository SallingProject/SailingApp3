using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButton : BaseObject {

    [SerializeField]
    private GameObject m_menuButton;

    protected override void mOnRegistered()
    {
        mUnregisterList(this);
        mUnregister();
    }

}
