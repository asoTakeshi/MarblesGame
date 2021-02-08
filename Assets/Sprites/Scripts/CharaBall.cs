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
    }
}
