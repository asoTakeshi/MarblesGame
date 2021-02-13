using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}


