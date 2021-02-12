using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public UIManager uiManager;

    IEnumerator Start()
    {
        // 初期化(この処理が終了するまで、次の処理は動かない)
        yield return StartCoroutine(Initialize());
    }
    /// <summary>
    /// ゲーム設定値の初期化
    /// </summary>
    /// <returns></returns>
    public IEnumerator Initialize()
    {
        // 残りの手球の数だけ手球のアイコンの生成する(この処理が終了するまで、次の処理は動かない)
        yield return StartCoroutine(uiManager.GenerateIconRemainingBalls(GameData.instance.charaBallHp));
    }

}


