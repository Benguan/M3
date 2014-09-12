using System;
using System.Collections.Generic;

namespace M3.Helpers
{
    public class ListHelper
    {
        public static List<T> GetRandomOrder<T>(List<T> list)
        {
            var random = new Random();
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }
            return newList;
        }
    }
}
