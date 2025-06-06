using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackedHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    {
        ENEMY,
        PLAYER
    }
    public HeroType Type;
    public Color NormalColor, TargetColor;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.PlayerTurn)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card &&
           card.Card.CanAttack &&
           Type == HeroType.ENEMY &&
           !GameManagerScr.Instance.Enemy.FieldCards.Exists(x => x.Card.IsProvocation))
        {
            GameManagerScr.Instance.DamageHero(card, GameManagerScr.Instance.Enemy);
        }
    }

    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetColor : NormalColor;
    }
}
