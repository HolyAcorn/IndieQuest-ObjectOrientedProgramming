using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        private List<bool> _deathSavingThrows = new List<bool>();

        public WeaponType weaponType { get; private set; }
        public ArmorType armorType { get; private set; }
        public Character(string displayName, Sprite bodySprite, int hitPointsMaximum, SizeCategory sizeCategory, WeaponType weaponType, ArmorType armorType) : base(displayName, bodySprite, sizeCategory)
        {
            this.weaponType = weaponType;
            this.armorType = armorType;
            base.hitPointsMaximum = hitPointsMaximum;
            Initialize();
        }

        public override IEnumerable<bool> deathSavingThrows => _deathSavingThrows;

        protected override IEnumerator TakeDamageAtZeroHitPoints(bool wasCriticalHit)
        {
            if(hitPoints <= -hitPointsMaximum)
            {
                Console.WriteLine($"{displayName} takes so much damage they immediately die.");
                yield return base.TakeDamageAtZeroHitPoints(wasCriticalHit);
                yield break;
            }

            hitPoints = 0;

            if (lifeStatus == LifeStatus.Conscious)
            {
                Console.WriteLine($"{displayName} falls uncoscious.");
                lifeStatus = LifeStatus.UnstableUnconcious;
                yield return presenter.TakeDamage();
                yield break;
            }

            Console.WriteLine($"{displayName} fails a death saving throw.");
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
                Console.WriteLine($"{displayName} meets an untimely end.");
                lifeStatus = LifeStatus.Dead;
                yield return presenter.Die();
            }

            // If the character succeeds 3 death saving throws, they stabilize.
            if (deathSavingThrowSuccesses >= 3)
            {
                Console.WriteLine($"{displayName} stabilizes.");
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
            if(lifeStatus != LifeStatus.StableUnconcious) yield break;

            int deathSavingThrowRollResult = DiceHelper.Roll("d20");

            switch (deathSavingThrowRollResult)
            {
                case 1:
                    Console.WriteLine($"{displayName} critically fails a death saving throw.");

                    // Critical fails add 2 saving throw failures
                    yield return ApplyDeathSavingThrows(2, false, deathSavingThrowRollResult);

                    break;
                case 20:
                    // Critical Successes regain conciousness with 1 HP
                    Console.WriteLine($"{displayName} critically succeeds a death saving throw.");

                    yield return ApplyDeathSavingThrows(1, true, deathSavingThrowRollResult);

                    ResetDeathSavingThrows();

                    break;
                case < 10:
                    Console.WriteLine($"{displayName} fails a death saving throw.");
                    yield return ApplyDeathSavingThrows(1, false, deathSavingThrowRollResult);
                    break;
                
                default:
                    Console.WriteLine($"{displayName} succeeds a death saving throw.");

                    yield return ApplyDeathSavingThrows(1, true, deathSavingThrowRollResult);
                    break;

            }

            yield return HandleDeathSavingThrows();
        }

    }
}
