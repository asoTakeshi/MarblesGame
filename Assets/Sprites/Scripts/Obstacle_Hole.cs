using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle_Hole : ObstacleBase
{
    // オーバーライドして利用する
    protected override void BeforeTriggerEffect(CharaBall charaBall)
    {
        // 手球を１つ減らす
        charaBall.UpdateHp(-power);

        // スタート位置へ戻す
        StartCoroutine(battleManager.RestartCharaPosition(2.0f));
    }
    ////* 以下を追記。親クラスのメソッドをオーバーライドして使用する *////

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        // 親クラスのOnCollisionEnter2Dメソッドの処理を実装する
        base.OnCollisionEnter2D(col);

        // ここから子クラス独自の処理を実装する

        // EnemyBallタグを持つゲームオブジェクトに接触したら
        if (col.gameObject.TryGetComponent(out EnemyBall enemy))
        {
            // Sequence初期化
            Sequence sequence = DOTween.Sequence();

            // 敵を回転
            sequence.Append(enemy.transform.DOLocalRotate(new Vector3(0, 720, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            // 敵を破壊
            enemy.DestroyEnemy(sequence);

        }
    }





}


