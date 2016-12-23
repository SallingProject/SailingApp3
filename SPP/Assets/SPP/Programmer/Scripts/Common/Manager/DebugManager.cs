//=============================================================================
/**
	@file	DebugManager.cs
	@brief	デバッグ管理クラス
	@author	Ko Hashimoto
*/
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

//=============================================================================

/// ログON/OFF用のカテゴリ指定
public enum ELogCategory
{
    Default,
};

//=============================================================================
/**
* @brief	デバッグ管理クラス
*/
public class DebugManager : BaseObjectSingleton<DebugManager>
{
    // スクリーンショット保存ディレクトリ
    const string kSSDir = "ScreenShot";

    // デバッグテキスト描画用
    private class CDrawTextInfo
    {
        public float _registTime;                // 登録時間
        public Vector2 _screenPos;              // スクリーン描画座標
        public Color _color;                 // 文字描画色
        public string _text;                 // 描画テキスト
        public float _drawTime;              // 描画時間（負の値で1フレーム）
    }
    
    // デバッグコマンド定義
    public delegate void DebugCommandDelegate(List<string> i_Args);

    private TouchScreenKeyboard m_debugCommandKeyboard;                                             // デバッグコマンド入力用ソフトウェアキーボード
    private Dictionary<string, DebugCommandDelegate> m_debugCommands = new Dictionary<string, DebugCommandDelegate>();  // デバッグコマンド群	
    private List<CDrawTextInfo> m_drawTexts = new List<CDrawTextInfo>();                        // 描画テキスト
    private GUIStyle m_drawTextStyle;                                                   // テキスト描画用スタイル


    private static readonly int kConsoleHeight = 20;                // コンソールウィンドウの高さ
    private static readonly string kConsoleUIName = "Console";     // コンソールウィンドウ(TextField)の識別子

    private bool m_isConsoleOpen = false;                      // コンソールが開いてるか否か
    private string m_consoleCommanddInput = "";                 // 現在コンソールに入力されている文字列
    private List<string> m_consoleCommandHistory = new List<string>();  // コンソールコマンドの入力履歴
    private int m_historyIndex = -1;                            // 入力履歴のインデックス

    private bool IsConsoleOpen
    {
        get { return m_isConsoleOpen; }
        set
        {
            m_isConsoleOpen = value;
            m_historyIndex = -1;
        }
    }
    
    public static string mKConsoleUIName
    {
        get { return kConsoleUIName; }
    }


    public readonly KeyCode mkConsoleCommandKey = KeyCode.Tab;
    //-----------------------------------------------------------------------------
    /**
	* @brief	シングルトンインスタンス初期化処理
	*/
    protected override void mOnRegistered()
    {
        base.mOnRegistered();
    
        m_drawTextStyle = new GUIStyle();

        // フォントサイズを適当に調整
        const float SCREENSIZE_MIN = 640.0f;
        const float SCREENSIZE_MAX = 1920.0f;
        float screenSize = (float)((Screen.height < Screen.width) ? Screen.width : Screen.height);
        float sizeRatio = Mathf.Clamp01((screenSize - SCREENSIZE_MIN) / (SCREENSIZE_MAX - SCREENSIZE_MIN));
        m_drawTextStyle.fontSize = (int)Mathf.Lerp(12.0f, 30.0f, sizeRatio);

        // 標準のデバッグコマンド
        AddDebugCommand("shot", DebugCommand_Screenshot);
        AddDebugCommand("slow", DebugCommand_Slomotion);
        AddDebugCommand("fts", DebugCommand_ForceLoadScene);

    }

    //-----------------------------------------------------------------------------
    /**
	* @brief	毎フレーム呼ばれる更新処理
	* @details	MonoBehaviourの実装
	*/
    public override void mOnFastUpdate()
    {
#if SPP_DEBUG
        base.mOnFastUpdate();
        UpdateDebugCommandKeyboard();
        UpdateDrawText();
#endif
    }
    
    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンドを追加する
	* @param	i_CommandName	登録したコマンドデリゲートを呼び出すための文字列。
	*							大文字/小文字は無視され、全て小文字に変換されます。
	* @param	i_Command		デバッグコマンドデリゲート
	* @retval	true			成功
	* @retval	false			失敗（引数エラーor既に同名のコマンドが登録済み）
	*/
    public bool AddDebugCommand(string i_CommandName, DebugCommandDelegate i_Command)
    {
#if SPP_DEBUG
		if( ( i_CommandName == null )
		 || ( i_CommandName.Length == 0 )
		 || ( i_Command == null ) )
		{
			return false;
		}

		// 全て小文字で扱う
		string name		= i_CommandName.ToLower();

		// 既に同じ名前のコマンドが登録済みか
		DebugCommandDelegate	command = null;
		if( m_debugCommands.TryGetValue( name, out command ) )
		{
			m_debugCommands.Remove( name );
		}

		// 新規追加
		m_debugCommands.Add( name, i_Command );
		return true;
#else
        return false;
#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンドを取り除く
	* @param	i_CommandName	取り除く登録済みコマンドデリゲート。
	* @return	取り除かれた数
	*/
    public int RemoveDebugCommand(DebugCommandDelegate i_Command)
    {
#if SPP_DEBUG
		List<string>	removeKeys = new List<string>();

		// 削除するキーの列挙
		foreach( KeyValuePair<string,DebugCommandDelegate> pair in m_debugCommands )
		{
			if( pair.Value == i_Command )
			{
				removeKeys.Add( pair.Key );
			}
		}
		// 削除
		foreach( string key in removeKeys )
		{
			m_debugCommands.Remove( key );
		}

		return removeKeys.Count;
#else
        return 0;
#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンドを取り除く
	* @param	i_CommandName	取り除く登録済みコマンド名。
	*							大文字/小文字は無視され、全て小文字に変換されます。
	* @retval	true			成功
	* @retval	false			失敗
	*/
    public bool RemoveDebugCommand(string i_CommandName)
    {
#if SPP_DEBUG
		return m_debugCommands.Remove( i_CommandName.ToLower() );
#else
        return false;
#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンドを実行する
	* @param	i_Command		実行コマンド文字列
	* @retval	true			成功
	* @retval	false			失敗
	*/
    public bool ExecDebugCommand(string i_CommandStr)
    {
#if SPP_DEBUG
		if( i_CommandStr == null )
			return false;

		// とりあえずパラメータはスペース区切りであるとする
		// @todo ダブルクォーテーションくくりでスペースを含む文字列に対応させる
		string[]		aArgsTemp	= i_CommandStr.Split( ' ' );
		// 空の文字列を取り除いてデリゲートの引数を作成
		List<string>	aArgs		= new List<string>( aArgsTemp.Length );
		foreach( string argc in aArgsTemp )
		{
			if( string.IsNullOrEmpty( argc ) )
				continue;

			aArgs.Add( argc );
		}

		if( aArgs.Count <= 0 )
			return false;
        
		// コマンドが登録されているか
		string					name	= aArgs[0].ToLower();
		DebugCommandDelegate	command	= null;
		if( !m_debugCommands.TryGetValue( name, out command ) )
		{
			Debug.LogWarning( "DebugCommand \"" + aArgs[0] + "\" is not found." );
			return false;
		}

		if( command == null )
		{
			Debug.LogWarning( "DebugCommand \"" + aArgs[0] + "\" is invalid. Delegate is removed." );
			// 無効なのでDictionaryから取り除く
			m_debugCommands.Remove( name );
			return false;
		}

		// 実行
		string cmdAndParam = string.Join( " ", aArgs.ToArray() );
		Debug.Log( "Execute DebugCommand \"" + cmdAndParam + "\"" );
		command( aArgs );
		return true;
#else
        return false;
#endif // #if SPP_DEBUG
    }


    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンド入力用のソフトウェアキーボードを開く
	*/
    public void OpenDebugCommandKeyboard()
    {
#if SPP_DEBUG
		if( m_debugCommandKeyboard != null )
			return;
        DebugManager.mInstance.IsConsoleOpen = true;
        m_debugCommandKeyboard = TouchScreenKeyboard.Open(
										""
										, TouchScreenKeyboardType.ASCIICapable
										, false
										, false
										, false
										, true
										, "Debug command" );

#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンド入力用のソフトウェアキーボードの更新
	*/
    private void UpdateDebugCommandKeyboard()
    {
#if SPP_DEBUG
		if( m_debugCommandKeyboard == null )
			return;

		// まだ表示中
		if( !m_debugCommandKeyboard.done && !m_debugCommandKeyboard.wasCanceled )
			return;

		// 入力がキャンセルされるとdoneと併せてwasCanceledがtrueになる。
		if( !m_debugCommandKeyboard.wasCanceled && ( 0 < m_debugCommandKeyboard.text.Length ) )
		{	// コマンド実行
			ExecDebugCommand( m_debugCommandKeyboard.text );
		}

		m_debugCommandKeyboard = null;
#endif // #if SPP_DEBUG
    }

    //-----------------------------------------------------------------------------
    /**
	* @brief	GUIの描画とイベントハンドリング用に呼ばれる
	* @details	MonoBehaviourの実装
	*/
    void OnGUI()
    {
        DrawDrawText();

        if (m_isConsoleOpen)
        {
            DrawConsole();
        }
    }

    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンド入力用のコンソールコマンドの表示(PC用)
	*/
    void DrawConsole()
    {
        bool Close = false;
        // ↓この部分はUIの登録より前に書かないと動かないみたい
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == mkConsoleCommandKey)
            {
                Close = true;
            }
            else if (Event.current.keyCode == KeyCode.Return)
            {
                if (GUI.GetNameOfFocusedControl() == mKConsoleUIName)
                {
                    ExecDebugCommand(m_consoleCommanddInput);

                    m_consoleCommandHistory.Remove(m_consoleCommanddInput);
                    m_consoleCommandHistory.Add(m_consoleCommanddInput);

                    m_consoleCommanddInput = "";
                    Close = true;
                }
            }
            else if (Event.current.keyCode == KeyCode.UpArrow)
            {
                if (m_consoleCommandHistory.Count > 0)
                {
                    if (m_historyIndex == -1)
                        m_historyIndex = m_consoleCommandHistory.Count - 1;
                    else
                    {
                        m_historyIndex--;

                        if (m_historyIndex < 0)
                            m_historyIndex = m_consoleCommandHistory.Count - 1;
                    }

                    m_consoleCommanddInput = m_consoleCommandHistory[m_historyIndex];
                }
            }
            else if (Event.current.keyCode == KeyCode.DownArrow)
            {
                if (m_consoleCommandHistory.Count > 0)
                {
                    if (m_historyIndex == -1)
                        m_historyIndex = 0;
                    else
                    {
                        m_historyIndex++;

                        if (m_historyIndex > (m_consoleCommandHistory.Count - 1))
                            m_historyIndex = 0;
                    }

                    m_consoleCommanddInput = m_consoleCommandHistory[m_historyIndex];
                }
            }
        }

        if (Close)
        {
            m_isConsoleOpen = false;
            GUI.FocusControl("");
            return;
        }

        GUI.SetNextControlName(kConsoleUIName);
        m_consoleCommanddInput = GUI.TextField(new Rect(0, Screen.height - kConsoleHeight, Screen.width, kConsoleHeight), m_consoleCommanddInput);

        // フォーカスをコンソールにうつす
        if (GUI.GetNameOfFocusedControl() == string.Empty)
        {
            GUI.FocusControl(kConsoleUIName);
        }
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	テキスト描画の更新
	*/
    private void UpdateDrawText()
    {
        float time = Time.time;
        float deltaTime = Time.deltaTime;

        // 要素の削除をする都合で末尾からfor
        CDrawTextInfo info;
        for (int i = m_drawTexts.Count - 1; 0 <= i; --i)
        {
            info = m_drawTexts[i];

            // 登録したてのものは無視
            if (time <= info._registTime)
                continue;

            if (0.0f < info._drawTime)
            {   // 表示時間指定がある
                info._drawTime -= deltaTime;
            }
            if (info._drawTime <= 0.0f)
            {   // 賞味期限切れ
                m_drawTexts.Remove(info);
            }
        }

    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグテキスト描画処理
	*/
    private void DrawDrawText()
    {
        const float LOG_POS_X = 0.0f;
        const float LOG_POS_Y = 0.0f;
        float logSizeH = (float)m_drawTextStyle.fontSize + 2.0f;
        int logCount = 0;
        Rect drawRect = new Rect();

        Color oldColor = GUI.color;

        foreach (CDrawTextInfo info in m_drawTexts)
        {
            if (info._screenPos.x == float.MaxValue)
            {   // ログ
                drawRect.x = LOG_POS_X;
                drawRect.y = LOG_POS_Y + ((float)logCount * logSizeH);
                drawRect.width = Screen.width;
                drawRect.height = logSizeH;
                ++logCount;
            }
            else
            {
                drawRect.x = info._screenPos.x;
                drawRect.y = info._screenPos.y;
                drawRect.width = Screen.width;
                drawRect.height = logSizeH;
            }

#if true   // 影
            //GUI.color = Color.black;
            m_drawTextStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(drawRect.x + 1, drawRect.y + 1, drawRect.width, drawRect.height), info._text, m_drawTextStyle);
#endif

            // 色設定
            //GUI.color = info.color;
            m_drawTextStyle.normal.textColor = info._color;
            // 描画
            GUI.Label(drawRect, info._text, m_drawTextStyle);
        }

        // 色を戻す
        GUI.color = oldColor;

    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	テキストを描画する
	* @param	i_vScreenPos	描画座標
	* @parmm	i_Text			描画テキスト
	* @param	i_Color			描画色
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    public void DrawText(Vector2 screenPos, object text, Color color, float time = -1.0f)
    {
#if SPP_DEBUG
		if( ( text == null )
		 || ( text.ToString().Length == 0 )
		 || ( color.a == 0.0f )
		 || ( time == 0.0f ) )
			return;

		CDrawTextInfo	info = new CDrawTextInfo();
		info._registTime	= Time.time;
		info._screenPos		= screenPos;
		info._text			= text.ToString();
		info._color			= color;
		info._drawTime		= time;
		m_drawTexts.Add( info );
#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	テキストを描画する
	* @param	i_vScreenPos	描画座標
	* @parmm	i_Text			描画テキスト
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    public void DrawText(Vector2 screenPos, object text, float time = -1.0f)
    {
#if SPP_DEBUG
		DrawText( screenPos, text, Color.white, time );
#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	テキストを描画する
	* @parmm	i_Text			描画テキスト
	* @param	i_Color			描画色
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    public void DrawText(object text, Color color, float time = -1.0f)
    {
#if SPP_DEBUG
		if( ( text == null )
		 || ( text.ToString().Length == 0 )
		 || ( color.a == 0.0f )
		 || ( time == 0.0f ) )
			return;

		CDrawTextInfo	info = new CDrawTextInfo();
		info._screenPos		= new Vector2( float.MaxValue, float.MaxValue );
		info._registTime		= Time.time;
		info._text			= text.ToString();
		info._color			= color;
		info._drawTime		= time;
		m_drawTexts.Add( info );
#endif // #if SPP_DEBUG
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	テキストを描画する
	* @parmm	i_Text			描画テキスト
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    public void DrawText(object text, float time = -1.0f)
    {
        DrawText(text, Color.white, time);
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	ログ出力時のカテゴリPrefixを取得する
	* @parmm	i_Category			ログカテゴリ
	*/
    private string GetCategoryPrefix(ELogCategory category)
    {
        string retval = "";

        if (category != ELogCategory.Default)
            retval = "[" + category.ToString() + "]";

        return retval;
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	ログ出力とテキストを描画
	* @parmm	i_Text			描画テキスト
	* @parmm	i_Category		ログカテゴリ
	* @param	i_bDrawScreen	画面に描画するか
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    [System.Diagnostics.Conditional("SPP_DEBUG")]
    public void OutputMsg(object message, ELogCategory category = ELogCategory.Default, bool isDrawScreen = false, float time = 5.0f)
    {
        string msg = GetCategoryPrefix(category);

        msg += " " + message.ToString();

        if (isDrawScreen)
            DrawText(msg, Color.white, time);

        Debug.Log(msg);
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	エラーメッセージを出力する
	* @parmm	i_Msg			描画エラーメッセージ
	* @parmm	i_Category		ログカテゴリ
	* @parmm	i_StackDepth	この関数から見ていくつ前のコールスタックを表示するか
	* @param	i_bDrawScreen	画面に描画するか
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    [System.Diagnostics.Conditional("SPP_DEBUG")]
    public void OutputError(object message, ELogCategory category = ELogCategory.Default, int stackdepth = 1, bool isDrawScreen = true, float time = 10.0f)
    {
        
        string msg =  GetCategoryPrefix(category);

        msg += " " + message.ToString();
        msg += "\n";

        if (isDrawScreen)
            DrawText(msg, Color.red, time);

        Debug.LogError(msg);
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	ワーニングメッセージを出力する
	* @parmm	i_Msg			描画エラーメッセージ
	* @parmm	i_Category		ログカテゴリ
	* @parmm	i_StackDepth	この関数から見ていくつ前のコールスタックを表示するか
	* @param	i_bDrawScreen	画面に描画するか
	* @param	i_Time			描画時間（負の値で1フレーム）
	*/
    [System.Diagnostics.Conditional("SPP_DEBUG")]
    public void OutputWarning(object message, ELogCategory category = ELogCategory.Default, int stackDepth = 1, bool isDrawScreen = true, float time = 7.5f)
    {
        
        string msg =  GetCategoryPrefix(category);

        msg += " " + message.ToString();
        msg += "\n";

        if (isDrawScreen)
            DrawText(msg, Color.yellow, time);

        Debug.LogWarning(msg);
    }



    //-----------------------------------------------------------------------------
    /**
	* @brief	スタック情報から指定された深さの関数の名前だけを抜き出す
	*/
    private string GetStackInfo(int stackDepth = 1)
    {
        string stackInfo = StackTraceUtility.ExtractStackTrace();

        int idxStart = 0;
        int idxEnd = stackInfo.IndexOf('\n', idxStart);

        for (int i = 0; i < stackDepth; ++i)
        {
            idxStart = idxEnd + 1;
            idxEnd = stackInfo.IndexOf('\n', idxStart);
        }

        stackInfo = stackInfo.Substring(idxStart, idxEnd - idxStart).Trim();
        stackInfo = "(" + stackInfo.Substring(stackInfo.LastIndexOf('\\') + 1);

        return stackInfo;
    }



    #region デバッグコマンド

    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンド：スローモーション
	*/
    public void DebugCommand_Slomotion(List<string> args)
    {
#if SPP_DEBUG
        if (args.Count < 2) return;
        float timeScale = args[1].ToFloat();
        Time.timeScale = timeScale;
#endif // #if SPP_DEBUG
    }

    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンド：スクリーンショット
	*/
    public void DebugCommand_Screenshot(List<string> args)
    {
#if SPP_DEBUG && UNITY_EDITOR
		if ( !Directory.Exists( kSSDir ) )
			Directory.CreateDirectory( kSSDir );

		string fileName = "";
		if( args.Count >= 2 )
			fileName = args[1] + "_";

		Application.CaptureScreenshot( kSSDir + "\\" + fileName + System.DateTime.Now.ToString("yyMMdd_HHmmss_fff") + ".png" );
#endif // #if SPP_DEBUG && UNITY_EDITOR
    }

    //-----------------------------------------------------------------------------
    /**
	* @brief	デバッグコマンド：同期シーンロード
	*/
    public void DebugCommand_ForceLoadScene(List<string> args)
    {
#if SPP_DEBUG

        if (args.Count >= 2)
            GameInstance.mInstance.mSceneLoad(new LoadInfo(args[1]));
#endif // #if SPP_DEBUG
    }
    

    #endregion


}
