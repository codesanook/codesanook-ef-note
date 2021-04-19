using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Codesanook.EFNote.Models
{
    public class TagComparer : IEqualityComparer<Tag>
    {
        public bool Equals([AllowNull] Tag obj, [AllowNull] Tag other)
        {
            if (obj == null && other == null)
            {
                return true;
            }
            else if (obj == null || other == null)
            {
                return false;
            }
            else if (obj.Name == other.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode([DisallowNull] Tag obj)
        {
            //https://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
            //https://stackoverflow.com/questions/1145217/why-should-hash-functions-use-a-prime-number-modulus
            var hash = 31;
            hash *= obj.Name.GetHashCode();   // 31 = another prime number
            return hash;
        }
    }
}