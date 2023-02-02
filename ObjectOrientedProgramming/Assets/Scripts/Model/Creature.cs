using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public abstract class Creature
    {
        public abstract IEnumerable<bool> deathSavingThrows { get; }

        public string displayName { get; private set; }
        public Sprite bodySprite { get; private set; }
        public int hitPointsMaximum;



        public SizeCategory sizeCategory { get; private set; }

        public readonly float spaceInFeet;
        public CreaturePresenter presenter { get; private set; }

        public int deathSavingThrowSuccesses => deathSavingThrows.Count(result => result);
        public int deathSavingThrowFailures => deathSavingThrows.Count(result => !result);

        public LifeStatus lifeStatus { get; protected set; } = new LifeStatus();

        public int hitPoints { get; protected set; }
        public abstract int armorClass { get; }

        public bool isAlive => lifeStatus is not LifeStatus.Dead;
        public bool isUnconcious => lifeStatus is not LifeStatus.UnstableUnconcious or LifeStatus.StableUnconcious;

        public abstract AbilityScores abilityScores { get; }

        public Creature(string displayName, Sprite bodySprite, SizeCategory sizeCategory)
        {
            this.displayName = displayName;
            this.bodySprite = bodySprite;
            this.sizeCategory = sizeCategory;
            spaceInFeet = SizeHelper.spaceInFeetPerSizeCategory[sizeCategory];
        }

        public abstract IAction TakeTurn(GameState gameState);

        public IEnumerator ReactToDamage(int damageAmount, bool wasCriticalHit)
        {
            hitPoints -= damageAmount;
            if (hitPoints <= 0) 
            {
                hitPoints = 0;
                yield return TakeDamageAtZeroHitPoints(wasCriticalHit);
            }
            else yield return presenter.TakeDamage();


        }

        public IEnumerator Heal(int amount)
        {
            yield return hitPoints += amount;
            if (hitPoints > hitPointsMaximum)
            {
                hitPoints = hitPointsMaximum;
            }
            yield return presenter.RegainConsciousness();
            yield return presenter.Heal();
        }

        protected virtual IEnumerator TakeDamageAtZeroHitPoints(bool wasCriticalHit)
        {
            lifeStatus = LifeStatus.Dead;
            yield return presenter.TakeDamage(hitPoints <= -hitPointsMaximum);
            yield return presenter.Die();
        }

        protected void Initialize()
        {
            hitPoints = hitPointsMaximum;
        }

        public void InitializePresenter(CreaturePresenter _presenter)
        {
            presenter = _presenter;
        }

        public Ability GetHigherAbility(Ability first, Ability second)
        {
            if (abilityScores[first].score > abilityScores[second].score) return first;
            else return second;

        }

    }
}
