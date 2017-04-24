using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handyman.Types
{
    /// <summary>
    /// The base of a type containing <see cref="Member"/>s.
    /// </summary>
    public abstract class MemberedBaseType
    {
        public MemberedBaseType(string name, string baseClassName, IEnumerable<Member> members)
        {
            this.Name = name;
            this.Members = new List<Member>(members);
            this.BaseClassName = baseClassName;
        }

        public string Name { get; set; }

        public IList<Member> Members { get; private set; }        

        public string BaseClassName { get; private set; }
    }
}
