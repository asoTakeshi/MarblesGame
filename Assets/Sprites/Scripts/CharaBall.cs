using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手球の制御用クラス
/// </summary>
public class CharaBall : MonoBehaviour
{
    [Header("手球の速度")]
    public float speed;

    private Rigidbody2D rb;

    ////* ここから追加 *////
   
    private Vector2 procVelocity = Vector2.zero; // Velocity計算保持用

    ////* ここまで追加 *////


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ShotBall();
    }

    /// <summary>
    /// 手球を打ち出す
    /// </summary>
    public void ShotBall()
    {
        // 角度によって速度が変化してしまうのでnormalizedで正規化して同じ速度ベクトルにする
        Vector2 direction = new Vector2(Random.Range(-2.5f, 2.5f), 1).normalized;

        // 手球を打ち出す
        rb.velocity = direction * speed;

        ////* ここから追加 *////
        // 次の速度の計算用にVelocityの値を保持しておく
        procVelocity = rb.velocity;

        ////* ここまで追加 *////

    }
    ////* 新しいメソッドを１つ追加。ここから追加 *////
    /// <summary>
    /// 他のゲームオブジェクトに接触した際の処理
    /// </summary>
    /// <param name="col">他のゲームオブジェクトのCollider情報</param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        // 壁に接触した場合
        if (col.gameObject.tag == "Wall")
        {
            // 接触したオブジェクトの接触情報を壁に垂直な単位ベクトルとして取得
            Vector2 normalVector = col.contacts[0].normal;
            // 跳ね返り用のベクトル(反射角度)をReflectメソッドを利用して計算(第1引数で手球の方向と速度、第2引数は壁に垂直な単位ベクトル)
            Vector2 reflectVector = Vector2.Reflect(procVelocity, normalVector);

            // 手球の速度を更新
            rb.velocity = reflectVector;

            // 次の速度の計算用にVelocityの値を保持しておく
            procVelocity = rb.velocity;

        }
    }
    ////* ここまで追加 *////
}