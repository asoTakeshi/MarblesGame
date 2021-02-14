using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int charaBallHp;

   
    [Header("バトル時間の設定値")]
    public int battleTime;

    ////* ここから追加 *////

    [Header("Money総数")]
    public int totalMoney;

    ////* ここまで追加 *////


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    ////* メソッドを１つ追加する *////

    /// <summary>
    /// Moneyの総額を増減する
    /// </summary>
    public void ProcMoney(int money)
    {
        totalMoney += money;
    }
    ////* ここまで *////

}
