using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObstacleBase : MonoBehaviour
{
    public int power;
    protected RectTransform rectTransform;

    [SerializeField]
    protected BattleManager battleManager;

    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Vector2 startSize = rectTransform.sizeDelta;

        rectTransform.sizeDelta = new Vector2(0, startSize.y);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(rectTransform.DOSizeDelta(startSize, 1.0f).SetEase(Ease.OutCirc));
        sequence.Join(rectTransform.DOShakePosition(1.0f, 3, 20, 180).SetEase(Ease.Linear));

    }
    public virtual void SetUpObstacle(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Liner")
        {
            return;
        }

        if (col.gameObject.tag == "charaBall")
        {
            if(col.gameObject.TryGetComponent(out CharaBall charaBall))
            {
                BeforeTriggerEffect(charaBall);

                Sequence sequence = DOTween.Sequence();

                sequence.Append(charaBall.transform.DOLocalRotate(new Vector3(0, 0, 720), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));

                AfterTriggerEffect(charaBall);
            }
        }

    }
    /// <summary>
    /// 接触判定時に最初に追加する処理
    /// </summary>
    /// <param name="charaBall"></param>
    protected virtual void BeforeTriggerEffect(CharaBall charaBall)
    {

    }
    /// <summary>
    /// 接触判定時に最後に追加する処理
    /// </summary>
    /// <param name="charaBall"></param>
    protected virtual void AfterTriggerEffect(CharaBall charaBall)
    {

    }
}　

