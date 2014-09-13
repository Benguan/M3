
using System.Collections.Generic;
namespace M3.Helpers
{
    public class StringHelper
    {
        public static string GetUriFromPath(string path)
        {
            return path.Replace(@"\", "/");
        }

        public static List<int> GetIntList(string stringNumbers)
        {
            var stringNumbersArray = stringNumbers.Split(',');
            var intList = new List<int>();
            foreach (var stringNumber in stringNumbersArray)
            {
                int intNumber;
                if (!string.IsNullOrEmpty(stringNumber))
                {
                    if (int.TryParse(stringNumber, out intNumber))
                    {
                        intList.Add(intNumber);
                    }
                }
            }
            return intList;
        }
    }
}
