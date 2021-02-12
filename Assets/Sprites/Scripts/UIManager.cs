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
}
