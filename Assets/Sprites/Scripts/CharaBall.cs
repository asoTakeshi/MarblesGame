﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手球の制御用クラス
/// </summary>
public class CharaBall : MonoBehaviour
{
    [Header("手玉の攻撃力")]
    public int power;
    [Header("手球の速度")]
    public float speed;

    private Rigidbody2D rb;

    private Vector2 procVelocity = Vector2.zero;   // Velocity計算保持用
    // いままでの宣言フィールドに変更なし。以下を追加する

    private int hp;     // Hpの現在値

    private BattleManager battleManager;    // BattleManagerへの紐づけ

    [SerializeField]
    private CapsuleCollider2D capsuleCol;    // 手球のコライダーの制御用

    ///* 新しくプロパティを１つ追加 *////

    /// <summary>
    /// hp用プロパティ
    /// </summary>
    /// <returns></returns>
    public int Hp
    {
        set
        {
            hp = value;
        }

        get
        {
            return hp;
        }
    }

    ////* ここまで追加 *////


    void Awake()
    {　　　　　　　　　　　　　　// 変更ありませんが記載します
        rb = GetComponent<Rigidbody2D>();
        //ShotBall();
    }

    /// <summary>
    /// ボールを打ち出す　　　　　　　　　　　// 変更ありませんが記載します
    /// </summary>
    public void ShotBall()
    {

        // 角度によって速度が変化してしまうのでnormalizedで正規化して同じ速度ベクトルにする
        Vector2 direction = new Vector2(Random.Range(-2.5f, 2.5f), 1).normalized;

        // ボールを打ち出す(摩擦や空気抵抗、重力を切ってあるので、ずっと同じ速度で動き続ける)
        rb.velocity = direction * speed;

        // 次の速度の計算用にVelocityの値を保持しておく
        procVelocity = rb.velocity;
    }

    /// <summary>
    /// 他のゲームオブジェクトに接触した際の処理           // 変更ありませんが記載します
    /// </summary>
    /// <param name="col">他のゲームオブジェクトのCollider情報</param>
    private void OnCollisionEnter2D(Collision2D col)
    {

        // 壁と的球に接触した場合
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "EnemyBall")
        {
            // 接触したオブジェクトの接触情報を壁に垂直な単位ベクトルとして取得
            Vector2 normalVector = col.contacts[0].normal;

            // 跳ね返り用のベクトル(反射角度)をReflectメソッドを利用して計算(第1引数でボールの方向と速度、第2引数は壁に垂直な単位ベクトル)
            Vector2 reflectVector = Vector2.Reflect(procVelocity, normalVector);

            // 手球の速度を更新
            rb.velocity = reflectVector;

            // 次の速度の計算用にVelocityの値を保持しておく
            procVelocity = rb.velocity;
        }

        // キューラインで弾いた場合
        if (col.gameObject.tag == "CueLine")
        {

            // ボールの向きをいれる
            Vector2 dir = transform.position - col.gameObject.transform.position;

            // ボールに速度を加える = 弾く
            rb.velocity = dir * speed;

            // 次の計算用にVelocityの値を保持しておく
            procVelocity = rb.velocity;
        }
    }

    /// <summary>
    /// 手球の初期設定。インスタンスした際に呼び出す         // 変更ありませんが記載します
    /// </summary>
    /// <param name="battelManager"></param>
    public void SetUpCharaBall(BattleManager battelManager)
    {

        // BattleManagerを取得
        this.battleManager = battelManager;

        // Hpを代入
        hp = GameData.instance.charaBallHp;
    }

    /// <summary>
    /// Hpを更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateHp(int amount)
    {
        // hpを増減
        hp += amount;

        // UI上にある手球アイコンを更新
        battleManager.uiManager.UpdateDisplayIconRemainingBall(hp);

        // hpが0以下になったら
        if (hp <= 0)
        {
            hp = 0;

            // 手球を停止
            StopMoveBall();


            ///* ここから追加 *////

            Debug.Log("Game Over");

            // ゲームオーバー処理の呼び出し
            battleManager.GameUp();

            ////* ここまで追加 *////

        }
    }

    /// <summary>
    /// 手球を停止                                     // 変更ありませんが記載します
    /// </summary>
    public void StopMoveBall()
    {
        // 手球の速度ベクトルを0にして止める
        rb.velocity = Vector2.zero;

        // 手球のコライダーを切って弾けないようにする
        ChangeActivateCollider(false);
    }

    /// <summary>
    /// 手球のコライダー制御                           // 変更ありませんが記載します
    /// </summary>
    /// <param name="isSwitch"></param>
    public void ChangeActivateCollider(bool isSwitch)
    {
        capsuleCol.enabled = isSwitch;
    }
}