using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonsterQuest
{
    public static class DiceHelper
    {
        private static int Roll(int numberOfRolls, int diceSides, int fixedBonus = 0)
        {
            int result = 0;
            for (int i = 0; i < numberOfRolls; i++)
            {
                result += Random.Range(1, diceSides + 1);
            }
            result += fixedBonus;
            return result;

        }

        public static int Roll(string diceNotation)
        {
            char[] modifier = new char[] { 'd', '+', '-' };
            string[] splitDice = diceNotation.Split(modifier);

            int numberOfRolls = 0;
            int diceSides = 0;
            int fixedBonus = 0;
            int finalScore = 0;
            if (!diceNotation.Contains('d'))
            {
                throw new ArgumentException("You did not follow the correct notation, try again: ");
            }
            if (splitDice[0] != "")
            {
                if (splitDice[0].Contains('*') || splitDice[0].Contains('/'))
                {
                    throw new ArgumentException($"You cannot use a divide or multiply sign. Try again:");
                }
                try
                {

                    numberOfRolls = Convert.ToInt32(splitDice[0]);

                }
                catch (Exception e)
                {

                    throw new ArgumentException($"{splitDice[0]} is not an integer. Try again:");
                }
                if (numberOfRolls == 0)
                {
                    throw new ArgumentException($"You cannot throw 0 dice! Try again:");
                }
            }
            else
            {
                if (diceNotation.StartsWith("-"))
                {
                    throw new ArgumentException($"You can only use positive numbers! ({diceNotation[0]}{splitDice[1]}) is not a positive number. Try again:");
                }
                numberOfRolls = 1;
            }
            if (splitDice[1] != "")
            {
                if (splitDice[1].Contains('*') || splitDice[1].Contains('/'))
                {
                    throw new ArgumentException($"You cannot use a divide or multiply sign. Try again:");
                }
                try
                {

                    diceSides = Convert.ToInt32(splitDice[1]);

                }
                catch (Exception e)
                {
                    throw new ArgumentException($"{splitDice[1]} is not an integer. Try again:");
                }
                if (diceSides == 0)
                {
                    throw new ArgumentException($"A dice cannot have 0 sides! Try again:");
                }
            }



            if (Convert.ToInt32(splitDice[1]) > 0)
            {
                diceSides = Convert.ToInt32(splitDice[1]);
                if (splitDice[0] == "")
                {
                    numberOfRolls = 1;
                }
                else
                {
                    numberOfRolls = Convert.ToInt32(splitDice[0]);
                }
                if (splitDice.Length > 1)
                {
                    foreach (char c in diceNotation)
                    {
                        if (c == '+')
                        {
                            fixedBonus = Convert.ToInt32(splitDice[2]);
                        }
                        else if (c == '-')
                        {
                            fixedBonus = -Convert.ToInt32(splitDice[2]);
                        }
                    }
                }
                else
                {
                    fixedBonus = 0;
                }

            }


            finalScore = Roll(numberOfRolls, diceSides, fixedBonus);



            return finalScore;
        }
    }
}
