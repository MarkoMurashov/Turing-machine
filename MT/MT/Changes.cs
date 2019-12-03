using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    [DataContract]
    public class Changes
    {
        [DataMember]
        public char CurrentSymbol { get; set; }

        [DataMember]
        public char NewSymbol { get; set; }
        [DataMember]
        public char Shift { get; set; }
        [DataMember]
        public int NewState { get; set; }

        public Changes(char cs, char ns, char shft, int newstate)
        {
            CurrentSymbol = cs;
            NewSymbol = ns;
            Shift = shft;
            NewState = newstate;
        }

    }
}
