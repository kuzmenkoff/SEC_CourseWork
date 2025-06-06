using UnityEngine;
using UnityEngine.EventSystems;

public class SpellTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {

        if (!GameManagerScr.Instance.PlayerTurn)
            return;
        CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                       target = GetComponent<CardController>();

        if (spell &&
            spell.Card.IsSpell &&
            spell.IsPlayerCard &&
            target.Card.IsPlaced &&
            GameManagerScr.Instance.Player.Mana >= spell.Card.ManaCost)
        {
            if ((spell.Card.SpellTarget == Card.TargetType.ALLY_CARD_TARGET &&
                target.IsPlayerCard) ||
                (spell.Card.SpellTarget == Card.TargetType.ENEMY_CARD_TARGET &&
                !target.IsPlayerCard))
            {
                GameManagerScr.Instance.ReduceMana(true, spell.Card.ManaCost);
                spell.UseSpell(target);
                GameManagerScr.Instance.CheckCardForManaAvailability();
            }
        }
    }
}
