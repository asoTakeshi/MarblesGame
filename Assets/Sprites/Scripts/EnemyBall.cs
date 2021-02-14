using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBall : MonoBehaviour
{
    [Header("的球の体力")]
    public int hp;

    private CapsuleCollider2D capsuleCol;

    private int maxHp;              // ゲーム開始時の体力の最大値の保持用。Sliderの計算に使用する

    [SerializeField]
    private Slider hpSlider;        // Sliderコンポーネントとの紐づけ用。インスペクターでアサインする


   

    private BattleManager gameManager;

    private Transform canvasTran;

    ////* ここから追加 *////

    public int money;

    ////* ここまで追加 *////


    /// <summary>
    /// 的球の設定
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="canvasTran"></param>
    public void SetUpEnemyBall(BattleManager gameManager, Transform canvasTran)
    {
        this.gameManager = gameManager;
        this.canvasTran = canvasTran;
    }


    ////* ここまで追加 *////


    void Start()
    {                                                // 変更ありませんが掲載します

        capsuleCol = GetComponent<CapsuleCollider2D>();

        // 最初のスケールを保持
        Vector2 startScale = transform.localScale;

        // 最小化(見えなくなる)
        transform.localScale = Vector2.zero;

        // Sequence初期化
        Sequence sequence = DOTween.Sequence();

        // 敵を回転させながら1.5倍の大きさにし、その後、元の大きさに戻しながら出現させる
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 720, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        sequence.Join(transform.DOScale(startScale * 1.5f, 1.0f).SetEase(Ease.InCirc));
        sequence.AppendInterval(0.15f);

        sequence.Join(transform.DOScale(startScale, 0.15f).SetEase(Ease.InCirc).OnComplete(() => {

            // 体力の最大値を代入
            maxHp = hp;

            // 体力ゲージの表示を更新 => 的球が出現してからゲージが徐々に満タンになるアニメ演出
            UpdateHpGauge();
        }));
    }

    /// <summary>
    /// 体力ゲージの表示を更新                                      // 変更ありませんが掲載します
    /// </summary>
    private void UpdateHpGauge()
    {
        hpSlider.DOValue((float)hp / maxHp, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D col)                // 変更ありませんが掲載します
    {
        // CharaBallのTagを持つゲームオブジェクトに接触したら
        if (col.gameObject.tag == "CharaBall")
        {
            // CharaBallクラスを取得できるか判定
            if (col.gameObject.TryGetComponent(out CharaBall charaBall))
            {
                // Hpを減少させる
                hp -= charaBall.power;

                // 体力の値を体力ゲージに反映
                UpdateHpGauge();

                // Sequence初期化
                Sequence sequence = DOTween.Sequence();

                // 手球と接触すると敵を回転
                sequence.Append(transform.DOLocalRotate(new Vector3(0, 720, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));

                // Hpが0以下になったら
                if (hp <= 0)
                {
                    DestroyEnemy(sequence);
                }
            }
        }
    }

    /// <summary>
    /// 敵の破壊
    /// </summary>
    /// <param name="sequence"></param>
    public void DestroyEnemy(Sequence sequence)
    {
        capsuleCol.enabled = false;

        // 破壊までの時間
        float duration = 0.5f;

        // 内側に小さくする ドロップ内容で消える処理を分岐
        sequence.Join(GetComponent<RectTransform>().DOSizeDelta(new Vector2(0, 100), duration).SetEase(Ease.Linear));




        // 敵の管理リストからこの敵の情報を削除
        //gameManager.RemoveEnemyList(this);　　　

        ////* ここから追加 *////

        // Moneyを加算
        gameManager.AddMoney(money);

        ////* ここまで *////


        // スケールが0になるタイミングで破棄
        Destroy(gameObject, duration);
    }
}
