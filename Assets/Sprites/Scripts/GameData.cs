using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int charaBallHp;

    ////* ここから追加 *////

    [Header("バトル時間の設定値")]
    public int battleTime;

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
}
