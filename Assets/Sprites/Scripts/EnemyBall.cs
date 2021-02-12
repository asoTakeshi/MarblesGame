using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;                       // ☆　<=　追加します


public class EnemyBall : MonoBehaviour
{
    [Header("的球の体力")]
    public int hp;
    

    private CapsuleCollider2D capsuleCol;


    ////* ここから追加 *////

    private int maxHp;              // ゲーム開始時の体力の最大値の保持用。Sliderの計算に使用する

    [SerializeField]
    private Slider hpSlider;        // Sliderコンポーネントとの紐づけ用。インスペクターでアサインする

    ////* ここまで追加 *////


    void Start()
    {
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


        ////* ここから修正・追加 *////

        sequence.Join(transform.DOScale(startScale, 0.15f).SetEase(Ease.InCirc).OnComplete(() => {

            // 体力の最大値を代入
            maxHp = hp;

            // 体力ゲージの表示を更新 => 的球が出現してからゲージが徐々に満タンになるアニメ演出
            UpdateHpGauge();
        }));

        ////* ここまで *////

    }


    ////* ここから新しくメソッドを１つ追加。ここから追加 *////


    /// <summary>
    /// 体力ゲージの表示を更新
    /// </summary>
    private void UpdateHpGauge()
    {
        hpSlider.DOValue((float)hp / maxHp, 0.5f);
    }


    ////* ここまで追加 *////


    private void OnCollisionEnter2D(Collision2D col)
    {
        // CharaBallのTagを持つゲームオブジェクトに接触したら
        if (col.gameObject.tag == "CharaBall")
        {
            // CharaBallクラスを取得できるか判定
            if (col.gameObject.TryGetComponent(out CharaBall charaBall))
            {
                // Hpを減少させる
                hp -= charaBall.power;


                ////* ここから修正・追加 *////

                //Debug.Log("的球の残り体力値 : " + hp);//　<=　体力値の減少処理が動いているのであれば、ここでの確認は不要になりますので、削除してください

                // 体力の値を体力ゲージに反映
                UpdateHpGauge();

                ////* ここまで *////


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

    ////* 新しくメソッドを１つ追加します。ここから追加 *////

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

        // スケールが 0 になるタイミング(DoTweenの時間と合わせる)で破棄
        Destroy(gameObject, duration);



    }



}
