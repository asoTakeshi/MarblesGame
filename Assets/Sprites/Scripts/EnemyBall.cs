using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;                       // ☆　<=　追加します


public class EnemyBall : MonoBehaviour
{
    private CapsuleCollider2D capsuleCol;

    void Start()
    {
        capsuleCol = GetComponent<CapsuleCollider2D>();

        ////* ここから追加 *////

        // 最初のスケールを保持
        Vector2 startScale = transform.localScale;

        // 最小化(見えなくなる)
        transform.localScale = Vector2.zero;

        // Sequence初期化
        Sequence sequence = DOTween.Sequence();

        // ①敵を回転させながら1.5倍の大きさにし、その後、元の大きさに戻しながら出現させる
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 720, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        sequence.Join(transform.DOScale(startScale * 1.5f, 1.0f).SetEase(Ease.InCirc));
        sequence.AppendInterval(0.15f);
        sequence.Join(transform.DOScale(startScale, 0.15f).SetEase(Ease.InCirc));

        ////* ここまで追加 *////

    }

   

    private void OnCollisionEnter2D(Collision2D col)
    {
        // CharaBallのTagを持つゲームオブジェクトに接触したら
        if (col.gameObject.tag == "CharaBall")
        {
            //CharaBallクラスを取得できるか判定
            if (col.gameObject.TryGetComponent(out CharaBall charaBall))
            {
                // 取得できているか確認
                Debug.Log(charaBall);

                ////* ここから修正 *////
                ///
                // Sequence初期化
                Sequence sequence = DOTween.Sequence();

                // ②手球と接触すると敵を回転(処理の内容は同じ)
                transform.DOLocalRotate(new Vector3(0, 720, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear);


                ////* ここまで追加 *////
                ///
                // TODO 敵の破壊する処理を書く


            }

        }
    }

}   
