using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> iconRemainingBallList = new List<GameObject>();  // 生成した手球アイコンを順番に追加

    [SerializeField]
    private GameObject iconRemainingBallPrefab;  // アイコンのプレファブ

    [SerializeField]
    private Transform remainingBallTran;   // アイコンを生成する位置

   

    [SerializeField]
    private Image imgTimeGauge;    // バトル時間のゲージ表示用

    [SerializeField]
    private Text txtBattleTime;  // バトル時間の画面表示用

    ////* ここから追加 *////

    [SerializeField]
    private Text Txtmoney;        // Moneyの画面表示用

    ////* ここまで追加 *////


    /// <summary>
    /// 手球の残数を画面上に生成
    /// </summary>
    /// <param name="ballCount"></param>
    /// <returns></returns>
    public IEnumerator GenerateIconRemainingBalls(int ballCount)
    {
        // 手球の最大値の数だけfor文を実行して手球アイコンを生成する
        for (int i = 0; i < ballCount; i++)
        {
            // 生成までに少し待つ(順番にアイコンが生成されるようにする演出)
            yield return new WaitForSeconds(0.15f);

            // 手球アイコンを１つ生成し、icon変数に代入
            GameObject icon = Instantiate(iconRemainingBallPrefab, remainingBallTran, false);

            // Listに追加
            iconRemainingBallList.Add(icon);

        }

    }
    /// <summary>
    /// 手球の残数の表示を更新
    /// </summary>
    /// <param name="amount">残りの手球数</param>
    
    public void UpdataDisplayIconRemainingBall(int amount)
    {
        // Listにある手球アイコンの数だけfor文を実行する
        for (int i = 0; i < iconRemainingBallList.Count; i++)
        {
            if (i < amount)
            {
                // 残りの手球数より小さい場合には手球アイコンを表示する
                iconRemainingBallList[i].SetActive(true);

            }
            else
            {
                // 大きい場合には手球アイコンを非表示にする
                iconRemainingBallList[i].SetActive(false);
            }
        }
    }
    ////* ここからメソッドを１つ追加 *////

    /// <summary>
    /// バトル時間の表示を更新
    /// </summary>
   
    public void UpdateDisplayBattleTime(int currentTime)
    {
        // 残り時間を計算
        float value = (float)currentTime / GameData.instance.battleTime;

        // 時間ゲージをアニメさせながら減少
        imgTimeGauge.DOFillAmount(value, 1.0f).SetEase(Ease.Linear);

        // バトルの残り時間を更新
        txtBattleTime.text = currentTime.ToString();
    }
    ////* ここからメソッドを１つ追加 *////

    /// <summary>
    /// 現在までに獲得しているMoneyの表示を更新
    /// </summary>

    public void UpdateDisplayMoney(int money)
    {
        Txtmoney.text = money.ToString();
    }
    ////* ここまで追加 *////


}

