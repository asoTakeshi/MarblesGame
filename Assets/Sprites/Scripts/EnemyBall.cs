using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    private CapsuleCollider2D capsuleCol;


    void Start()
    {
        capsuleCol = GetComponent<CapsuleCollider2D>();
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

                // TODO 敵の破壊する処理を書く

            }

        }
    }

}   
