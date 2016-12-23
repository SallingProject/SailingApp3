using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneBase : BaseObject {
    static bool m_isFirst = false;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);
        if (!m_isFirst)
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name != "Setup")
            {
                SceneManager.LoadScene("Setup");

            }
            m_isFirst = true;
        }
    }
}
