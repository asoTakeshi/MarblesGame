using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手球を打ち返すキューラインの生成・制御クラス
/// </summary>
public class CueLiner : MonoBehaviour
{
    [Header("生成するキューラインのプレファブ")]
    public GameObject cuelinePrefab;

    [Header("キューラインを生成するために必要なスワイプの最小の長さ")]
    public float minCuelineLength;

    [Header("キューラインの出現時間")]
    public float duration;

    private Vector2 touchPos; 　// マウスのクリック地点


    private void Update()
    {

        // キューラインを引く
        DrawCueLine();
    }

    /// <summary>
    /// 手球を打ち返すキューラインを生成
    /// </summary>
    private void DrawCueLine()
    {

        if (Input.GetMouseButtonDown(0))
        {

            // マウスでクリックした位置を取得
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {

            // 最初にクリックした位置をスタート地点にする
            Vector2 startPos = touchPos;

            // クリックしている間、その位置を更新する(離した地点を最後の地点にする)
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // // マウスの動いた位置のベクトルの長さ(magnitude)が設定した最小値(minCuelineLength)より大きいなら
            if ((endPos - startPos).magnitude > minCuelineLength)
            {

                // キューラインを生成
                GameObject cueline = Instantiate(cuelinePrefab, transform.localPosition, transform.localRotation);

                // キューラインの位置を設定
                cueline.transform.position = (startPos + endPos) / 2;

                // マウスの位置を更新
                touchPos = endPos;

                // 出現時間が経過したらキューラインを破壊
                Destroy(cueline, duration);
            }
        }
    }
}