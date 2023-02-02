using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        private Creature attacker;
        private Creature target;
        private WeaponType weaponType;
        private Ability? _ability = null;

        public AttackAction(Creature attacker, Creature target, WeaponType weaponType, Ability? ability = null)
        {
            this.attacker = attacker;
            this.target = target;
            this.weaponType = weaponType;
            _ability = ability;
        }


        public IEnumerator Execute()
        {
            yield return attacker.presenter.FaceCreature(target);
            yield return attacker.presenter.Attack();
            Ability ability = weaponType.isRanged ? Ability.Dexterity : Ability.Strength;
            if (_ability.HasValue) ability = _ability.Value;

            int attackRoll = DiceHelper.Roll("d20") + attacker.abilityScores[ability].modifier;

            bool wasHit = false;
            bool wasCriticalHit = false;

            if(target.lifeStatus == LifeStatus.Conscious)
            {
                wasHit = attackRoll >= target.armorClass;
                wasCriticalHit = attackRoll == 20;
            }
            else
            {
                wasHit = true;
                wasCriticalHit = true;
            }

            if (!wasHit)
            {
                Console.WriteLine($"{attacker.displayName.ToUpperFirst()} misses their attack.");
                yield break;
            }

            int damage = DiceHelper.Roll(weaponType.damageRoll);
            if (wasCriticalHit) damage += DiceHelper.Roll(weaponType.damageRoll);
            Console.WriteLine($"{attacker.displayName.ToUpperFirst()} hits the {target.displayName.ToUpperFirst()} with their {weaponType.displayName.ToUpperFirst()} for {damage}. The {target.displayName.ToUpperFirst()} has {target.hitPoints} HP left.");
            yield return target.ReactToDamage(damage, wasCriticalHit);


        }
    }
}
