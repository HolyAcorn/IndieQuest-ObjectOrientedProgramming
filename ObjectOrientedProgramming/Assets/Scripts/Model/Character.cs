using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace MonsterQuest
{
    public class Character : Creature
    {
        private List<bool> _deathSavingThrows = new List<bool>();
        public override IEnumerable<bool> deathSavingThrows => _deathSavingThrows;

        public WeaponType weaponType { get; private set; }
        public ArmorType armorType { get; private set; }

        public override int armorClass => armorType.armorClass;

        public override AbilityScores abilityScores { get; } = new();

        public Character(string displayName, Sprite bodySprite, int hitPointsMaximum, SizeCategory sizeCategory, WeaponType weaponType, ArmorType armorType) : base(displayName, bodySprite, sizeCategory)
        {
            this.weaponType = weaponType;
            this.armorType = armorType;
            base.hitPointsMaximum = hitPointsMaximum;


            for (int abilityIndex = 1; abilityIndex <= 6; abilityIndex++)
            {
                List<int> rolls = new List<int>();

                for (int j = 0; j < 4; j++)
                {
                    rolls.Add(DiceHelper.Roll("d6"));
                }

                rolls.Sort();
                int attribute = rolls[1] + rolls[2] + rolls[3];
                Ability ability = (Ability)abilityIndex;

                abilityScores[ability].score = attribute;
            }
            Initialize();
        }

        public override IAction TakeTurn(GameState gameState)
        {
            if (lifeStatus != LifeStatus.Conscious) return new BeUnconciousAction(this);

            Creature monster = gameState.combat.monster;

            Ability? ability = null;


            if (weaponType.isFinesse) ability = abilityScores.strength > abilityScores.dexterity ? Ability.Strength : Ability.Dexterity;

            return new AttackAction(this, monster, weaponType, ability);
        }

        protected override IEnumerator TakeDamageAtZeroHitPoints(bool wasCriticalHit)
        {
            if (hitPoints <= -hitPointsMaximum)
            {
                Console.WriteLine($"{displayName.ToUpperFirst()} takes so much damage they immediately die.");
                yield return base.TakeDamageAtZeroHitPoints(wasCriticalHit);
                yield break;
            }

            hitPoints = 0;

            if (lifeStatus == LifeStatus.Conscious)
            {
                Console.WriteLine($"{displayName.ToUpperFirst()} falls uncoscious.");
                lifeStatus = LifeStatus.UnstableUnconcious;
                yield return presenter.TakeDamage();
                yield break;
            }

            Console.WriteLine($"{displayName.ToUpperFirst()} fails a death saving throw.");
            lifeStatus = LifeStatus.UnstableUnconcious;
            yield return presenter.TakeDamage();

            int deathSavingThrowFailureCount = wasCriticalHit ? 2 : 1;

            yield return ApplyDeathSavingThrows(deathSavingThrowFailureCount, false);

            yield return HandleDeathSavingThrows();
        }

        private IEnumerator ApplyDeathSavingThrows(int amount, bool success, int? rollResult = null)
        {
            for (int i = 0; i < amount; i++)
            {
                _deathSavingThrows.Add(success);

                yield return presenter.PerformDeathSavingThrow(success, i == 0 ? rollResult : null);
            }
        }

        private IEnumerator HandleDeathSavingThrows()
        {
            // If the character fails three death saving throws, then they die.
            if (deathSavingThrowFailures >= 3)
            {
                Console.WriteLine($"{displayName.ToUpperFirst()} meets an untimely end.");
                lifeStatus = LifeStatus.Dead;
                yield return presenter.Die();
            }

            // If the character succeeds 3 death saving throws, they stabilize.
            if (deathSavingThrowSuccesses >= 3)
            {
                Console.WriteLine($"{displayName.ToUpperFirst()} stabilizes.");
                lifeStatus = LifeStatus.StableUnconcious;
                ResetDeathSavingThrows();
            }
        }

        private void ResetDeathSavingThrows()
        {
            _deathSavingThrows.Clear();

            if (presenter != null) presenter.ResetDeathSavingThrows();
        }

        public IEnumerator HandleUnconciousState()
        {
            // Unstable Unconcious Characters must make a death saving throw
            if (lifeStatus != LifeStatus.UnstableUnconcious) yield break;

            int deathSavingThrowRollResult = DiceHelper.Roll("d20");

            switch (deathSavingThrowRollResult)
            {
                case 1:
                    Console.WriteLine($"{displayName.ToUpperFirst()} critically fails a death saving throw.");

                    // Critical fails add 2 saving throw failures
                    yield return ApplyDeathSavingThrows(2, false, deathSavingThrowRollResult);

                    break;
                case 20:
                    // Critical Successes regain conciousness with 1 HP
                    Console.WriteLine($"{displayName.ToUpperFirst()} critically succeeds a death saving throw.");
                    yield return Heal(1);
                    yield return ApplyDeathSavingThrows(1, true, deathSavingThrowRollResult);

                    ResetDeathSavingThrows();

                    break;
                case < 10:
                    Console.WriteLine($"{displayName.ToUpperFirst()} fails a death saving throw.");
                    yield return ApplyDeathSavingThrows(1, false, deathSavingThrowRollResult);
                    break;

                default:
                    Console.WriteLine($"{displayName.ToUpperFirst()} succeeds a death saving throw.");

                    yield return ApplyDeathSavingThrows(1, true, deathSavingThrowRollResult);
                    break;

            }

            yield return HandleDeathSavingThrows();
        }

    }
}
