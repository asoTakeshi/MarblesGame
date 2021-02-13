using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;                            // <= ☆　追加してください

public class BattleManager : MonoBehaviour
{
    public UIManager uiManager;

    ////* ここから追加 *////
    [SerializeField]
    private CharaBall charaBallPrefab;

    [SerializeField]
    private Transform startCharaTran;

    private CharaBall charaBall;
    ////* ここまで追加 *////

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

        ////* ここから追加 *////

        // GenerateCharaBallメソッドで手球の生成処理し、戻り値で変数に代入
        charaBall = GenerateCharaBall();

        ////* ここまで追加 *////
    }
    ////* メソッドを新しく２つ追加。ここから *////

    /// <summary>
    /// 手球の生成
    /// </summary>
    /// <returns></returns>
    private CharaBall GenerateCharaBall()
    {
        CharaBall chara = Instantiate(charaBallPrefab, startCharaTran, false);
        chara.SetUpCharaBall(this);
        return chara;
    }
    /// <summary>
    /// キャラを停止させてスタート位置へ戻す
    /// </summary>
    /// <returns></returns>
    public IEnumerator RestartCharaPosition(float waiTime = 1.0f)
    {
        // キャラの動きを止める
        charaBall.StopMoveBall();

        // スタート位置へ戻す
        charaBall.transform.DOMove(startCharaTran.position,waiTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(waiTime);

        // 手球を弾けるようにする
        charaBall.ChangeActivateCollider(true);


    }


}


