using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;                            // <= ☆　追加してください

public class BattleManager : MonoBehaviour
{
    public UIManager uiManager;

    [SerializeField]
    private CharaBall charaBallPrefab;

    [SerializeField]
    private Transform startCharaTran;

    private CharaBall charaBall;

    [SerializeField]
    private EnemyBall enemyBallPrefab;                               // 敵のプレファブ

    [SerializeField]
    private List<EnemyBall> enemyBallList = new List<EnemyBall>();   // 生成された敵を管理するためのリスト

    [SerializeField]
    private Transform canvasTran;                                    // CanvasのTransformを登録

    [SerializeField]
    private Transform leftBottomTran;                                // ゲーム画面の左下位置の指定用

    [SerializeField]
    private Transform rightTopTran;                                  // ゲーム画面の右上位置の指定用

    [SerializeField]
    private Transform enemyPlace;                                    // 生成した敵を入れるフォルダの役割

    [Header("バトルの残り時間")]
    public int currentTime;

    private float timer;           　　　　　　　　　　　　　　　　　// 時間計測用

    private int money;                        // ゲーム中のMoney管理用


   

    public enum GameState
    {
        Wait,
        Play,
        Result,  // <= ☆　半角カンマ(コンマ)を忘れずにつけてください。次の列挙子が追加できず、エラーになります

        ////* ここから追加 *////

        GameOver

        ////* ここまで追加 *////

    }


    public GameState gameState = GameState.Wait;

   

    [SerializeField]
    private List<ObstacleBase> obstacleList = new List<ObstacleBase>();  // 障害物ゲームオブジェクトの管理用

   



    IEnumerator Start()
    {


        ////* ここから追加 *////

        gameState = GameState.Wait;
        //Debug.Log(gameState);

        ////* ここまで追加 *////


        // 初期化
        yield return StartCoroutine(Initialize());

        // 残り時間の表示を更新
        uiManager.UpdateDisplayBattleTime(currentTime);

        // Moneyの表示を更新
        uiManager.UpdateDisplayMoney(money);
    }

    /// <summary>
    /// ゲーム設定値の初期化
    /// </summary>
    /// <returns></returns>
    public IEnumerator Initialize()
    {

        currentTime = GameData.instance.battleTime;

        // Moneyの初期値設定
        money = 0;

        // 手球を体力の数だけ生成する
        yield return StartCoroutine(uiManager.GenerateIconRemainingBalls(GameData.instance.charaBallHp));

        // GenerateCharaBallメソッドで手球の生成処理し、戻り値で変数に代入
        charaBall = GenerateCharaBall();

        // 敵を生成
        yield return StartCoroutine(GenerateEnemys());


        ////* ここから追加 *////

        gameState = GameState.Play;
        Debug.Log(gameState);

        ////* ここまで追加 *////

    }

    /// <summary>
    /// 手球の生成                                            // 変更なしですが記載します
    /// </summary>
    /// <returns></returns>
    private CharaBall GenerateCharaBall()
    {
        CharaBall chara = Instantiate(charaBallPrefab, startCharaTran, false);
        chara.SetUpCharaBall(this);
        return chara;
    }

    /// <summary> 
    /// キャラを停止させてスタート位置へ戻す                  // 変更なしですが記載します
    /// </summary> 
    /// <returns></returns>
    public IEnumerator RestartCharaPosition(float waiTime = 1.0f)
    {
        // キャラの動きを止める
        charaBall.StopMoveBall();

        // スタート位置へ戻す
        charaBall.transform.DOMove(startCharaTran.position, waiTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(waiTime);

        // 手球を弾けるようにする
        charaBall.ChangeActivateCollider(true);
    }

    /// <summary>
    /// 敵を生成               　　　　　　　　　　　　   // 変更なしですが記載します
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateEnemys()
    {

        // 生成する敵の数をランダムで設定
        int appearEnemyCount = Random.Range(2, 5);

        // 生成した数をカウント用
        int count = 0;

        // 生成した数が生成数になるまでループする。目標数になったら生成終了し、ループを抜ける
        while (count < appearEnemyCount)
        {

            // Transform情報を代入
            Transform enemyTran = canvasTran;

            // 位置を画面内に収まる範囲でランダムに設定
            enemyTran.position = new Vector2(Random.Range(leftBottomTran.localPosition.x, rightTopTran.localPosition.x), Random.Range(leftBottomTran.localPosition.y, rightTopTran.localPosition.y));

            // 設定した位置に対してRayを発射
            RaycastHit2D hit = Physics2D.Raycast(enemyTran.position, Vector3.forward);

            // Rayに当たったオブジェクトを表示。何もないときは null
            Debug.Log(hit.collider);

            // もしも何もRayに当たらない場合(Rayに何か当たった場合にはその位置には生成しないので、ループの最初からやり直す)
            if (hit.collider == null)
            {

                // 敵を生成
                EnemyBall enemyBall = Instantiate(enemyBallPrefab, canvasTran, false);

                // 親子関係を設定
                enemyBall.transform.SetParent(enemyPlace);

                // 敵の位置をランダムで設定した位置に設定
                enemyBall.transform.localPosition = enemyTran.position;

                // 敵の初期設定
                enemyBall.SetUpEnemyBall(this, canvasTran);

                // 敵の生成カウントを加算
                count++;

                // 敵の管理リストに追加
                enemyBallList.Add(enemyBall);

                // 少し待機して、ループを最初から繰り返す
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    /// <summary>
    /// 敵をリストから削除　　　　　　　                  // 変更なしですが記載します
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyList(GameObject enemy)
    {
        RemoveEnemyList(enemy);
        CheckRemainingEnemies();
    }

    /// <summary>
    /// 敵の残数を確認
    /// </summary>
    public void CheckRemainingEnemies()
    {
        if (enemyBallList.Count == 0)
        {
           
            //Debug.Log("ステージ　クリア");

            // 今回のゲーム内で獲得したMoneyをMoney総数に加算
            GameData.instance.ProcMoney(money);


           

            gameState = GameState.Result;
            Debug.Log(gameState);

            ////* ここから追加 *////

            // 障害物を破棄し、ObstacleListをクリア
            ClearObstacleList();


            // クリア表示
            uiManager.DisplayStageClear();

            ////* ここまで追加 *////


        }
    }

    void Update()
    {

        ////* ここから追加 *////

        if (gameState != GameState.Play)
        {
            return;
        }

        ////* ここまで追加 *////


        // 時間を計測
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            // 1秒経過するごとにcurrentTimeを減らす
            timer = 0;
            currentTime--;

            if (currentTime <= 0)
            {
                currentTime = 0;

                ////* ここから追加 *////


                // TODO ゲームオーバー処理を書く
                //Debug.Log("Time Up!");

                // ゲームオーバー処理
                GameUp();

                ////* ここまで追加 *////
            }
        }

        // バトル時間の表示を更新
        uiManager.UpdateDisplayBattleTime(currentTime);
    }

    /// <summary>
    /// Moneyを加算　　　　　　　　　　　　　　　　　　// 変更なしですが記載します
    /// </summary>
    public void AddMoney(int amount)
    {
        // moneyを加算
        money += amount;

        // money 変数は private 修飾子なので、Debug.Logを表示して値の変化を確認する
        Debug.Log(money);

        // Moneyの表示を更新
        uiManager.UpdateDisplayMoney(money);
    }
    ////* ここからメソッドを１つ追加 *////

    //　この処理についてはコメントを記述していません。
    //　処理を書きながらコメントを書いてください。もしも書けない場合にはListとforeachの処理についての復習をしましょう。

    /// <summary>
    /// 障害物を破棄し、ObstacleListをクリア
    /// </summary>
    private void ClearObstacleList()
    {
        if (obstacleList.Count > 0)
        {
            foreach (ObstacleBase obstacle in obstacleList)
            {
                Destroy(obstacle.gameObject);
            }
            obstacleList.Clear();
        }
    }

    ////* ここからメソッドを１つ追加 *////

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameUp()
    {

        // ゲームの進行状態をゲームオーバー状態に変更
        gameState = GameState.GameOver;
        Debug.Log(gameState);

        // ゲームオーバー表示
        uiManager.DisplayGameOver();
    }

    ////* ここまで追加 *////

}









