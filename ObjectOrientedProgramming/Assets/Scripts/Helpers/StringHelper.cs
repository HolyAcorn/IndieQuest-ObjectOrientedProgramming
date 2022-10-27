using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public static class StringHelper
    {
        public static string JoinWithAnd(IEnumerable items, bool useSerialComma = true)
        {

            string[] itemList = (from object item in items select item.ToString()).ToArray();
            int count = itemList.Length;
            
            string finalOutput = "";

            if (count == 0)
            {
                return "";
            }
            if (count == 1)
            {
                return itemList[0];
            }
            if (count >= 2)
            {
                for (int x = 0; x < count; x++)
                {

                    if (useSerialComma)
                    {
                        finalOutput += itemList[x] + ", ";
                    }
                    else
                    {


                        if (x < count - 2)
                        {
                            finalOutput += itemList[x] + ", ";
                        }
                        else
                        {
                            if (x != count - 1)
                            {
                                finalOutput += itemList[x] + " and ";

                            }
                            else
                            {
                                finalOutput += itemList[x];
                            }

                        }
                    }

                }

            }
            return finalOutput;
        }
    }
}
