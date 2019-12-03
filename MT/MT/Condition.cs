using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    [DataContract]
    public class Condition
    {
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public List<Changes> Changes { get; set; }
        [DataMember]
        public bool IsFinish { get; set; }

        public Condition(int st, List<Changes> ch, bool isf)
        {
            State = st;
            Changes = ch;
            IsFinish = isf;
        }

    }
}
