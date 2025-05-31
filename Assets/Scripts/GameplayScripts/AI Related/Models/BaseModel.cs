using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public abstract class BaseModel
{
    protected static readonly System.Random random = new System.Random();

    public abstract string Name { get; }

    // === ĳ� � �� ===
    public IEnumerator CastSpell(CardController spell, CardController target = null)
    {

        if (!spell.Card.IsSpell || spell.Card.ManaCost > GameManagerScr.Instance.Enemy.Mana)
            yield break;

        if (!(spell.Card.SpellTarget == Card.TargetType.NO_TARGET) && target == null)
            yield break;

        var game = GameManagerScr.Instance;
        var movement = spell.GetComponent<CardMovementScr>();

        // 1. ��������� � ����
        game.Enemy.HandCards.Remove(spell);

        // 2. �������� ���� �� ������
        spell.Info.ShowCardInfo();

        if (spell.Card.SpellTarget == Card.TargetType.NO_TARGET)
        {
            // 5. ��������� ������� � ����� � ���������
            yield return movement.MoveToCenterAndVanish();

            // 4. ����������� �����
            spell.OnCast();

        }
        else
        {
            // 5. ��������� ������� � ����� � ���������
            yield return movement.MoveToTargetAndVanish(target.transform);

            // 4. ����������� �����
            spell.OnCast(target);
        }

    }

    public IEnumerator CastEntity(CardController entity)
    {
        if (entity.Card.IsSpell)
            yield break;

        var game = GameManagerScr.Instance;
        var movement = entity.GetComponent<CardMovementScr>();

        if (entity.Card.ManaCost > game.Enemy.Mana)
        {
            UnityEngine.Debug.Log("Not enough ehough mana");
            yield break;
        }

        if (game.Enemy.FieldCards.Count >= 6)
        {
            UnityEngine.Debug.Log($"Too much cards on field. Current field count: {game.Enemy.FieldCards.Count}"); ;
            yield break;
        }


        entity.Info.ShowCardInfo();

        yield return movement.MoveToField(game.EnemyField);

        entity.OnCast();
    }

    public IEnumerator AttackCard(CardController attacker, CardController target)
    {
        if (!attacker.Card.CanAttack)
            yield break;

        var enemyField = GameManagerScr.Instance.Player.FieldCards;
        bool provokerExists = enemyField.Exists(c => c.Card.Abilities.Contains(Card.AbilityType.PROVOCATION));

        if (provokerExists && !target.Card.Abilities.Contains(Card.AbilityType.PROVOCATION))
            yield break;

        var game = GameManagerScr.Instance;
        var movement = attacker.GetComponent<CardMovementScr>();

        yield return movement.MoveToTargetCor(target.transform);

        game.CardsFight(attacker, target);
    }

    public IEnumerator AttackHero(CardController attacker)
    {
        if (!attacker.Card.CanAttack)
            yield break;

        var enemyField = GameManagerScr.Instance.Player.FieldCards;
        bool provokerExists = enemyField.Exists(c => c.Card.Abilities.Contains(Card.AbilityType.PROVOCATION));

        if (provokerExists)
            yield break;

        var game = GameManagerScr.Instance;
        var movement = attacker.GetComponent<CardMovementScr>();

        yield return movement.MoveToTargetCor(GameManagerScr.Instance.PlayerHero.transform);

        game.DamageHero(attacker, game.Player);
    }
}
