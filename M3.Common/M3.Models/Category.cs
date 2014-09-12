using System;
using System.Collections;
using System.Collections.Generic;

namespace M3.Models
{
    public class Category : IComparable
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Name { get; set; }

        public int Page { get; set; }

        public Photo Cover { get; set; }

        public List<Photo> Photos { get; set; }

        public int CompareTo(object obj)
        {
            var compareCategory = (Category)obj;
            if (this.Year > compareCategory.Year)
            {
                return 1;
            }
            else if (this.Year < compareCategory.Year)
            {
                return -1;
            }

            return new CaseInsensitiveComparer().Compare(this.Name, compareCategory.Name);
        }
    }
}