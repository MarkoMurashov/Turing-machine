using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace MT
{
    [DataContract]
    public class TuringMachine
    {
        [DataMember]
        public List<char> InputTape;
        [DataMember]
        public List<Condition> Conditions;

        [DataMember]
        public int Pointer { get; set; }

        [DataMember]
        public int CurrState = 0;
        [DataMember]
        public char Symbol;

        public int FindIndexOfStartLetters()
        {
            int index = 0;

            for(int i = 0; i < InputTape.Count; i++)
            {
                if(!InputTape[i].Equals('_'))
                {
                    index = i;
                    break;
                }
            }
            
            return index;
        }

        public bool ChangeTape() 
        {

            Symbol = InputTape[Pointer];

            bool flag = true;

            while (flag)
            {
                if (Conditions[CurrState].IsFinish) //является ли это состояние конечным
                {
                    break;
                }

                int counter = 0;
                int pos = 0;

                for (int a = 0; a < Conditions[CurrState].Changes.Count; a++) // проверка если ли переходы по символу. Если нет, то строка не подходит
                {
                    if (Conditions[CurrState].Changes[a].CurrentSymbol.Equals(Symbol))
                    {
                        counter++;
                        pos = a;
                        break;
                    }
                }

                if (counter == 0)
                {
                    return false;
                }


                InputTape[Pointer] = Conditions[CurrState].Changes[pos].NewSymbol;

                if (Conditions[CurrState].Changes[pos].Shift.Equals('R'))
                {
                    Pointer++;
                }
                if (Conditions[CurrState].Changes[pos].Shift.Equals('L'))
                {
                    Pointer--;
                }

                CurrState = Conditions[CurrState].Changes[pos].NewState;

                Symbol = InputTape[Pointer];

            }         
            return true;
        }

        public int ChangeTapeByOneSymbol()
        {
            Symbol = InputTape[Pointer];

            if (Conditions[CurrState].IsFinish) //является ли это состояние конечным
            {
                return 1;
            }

           
            int counter = 0;
            int pos = 0;

            for (int a = 0; a < Conditions[CurrState].Changes.Count; a++) // проверка если ли переходы по символу. Если нет, то строка не подходит
            {
               if (Conditions[CurrState].Changes[a].CurrentSymbol.Equals(Symbol))
               {
                   counter++;
                   pos = a;
                   break;
               }
            }

           if (counter == 0)
           {
              return 0;
           }


           InputTape[Pointer] = Conditions[CurrState].Changes[pos].NewSymbol;

            if (Conditions[CurrState].Changes[pos].Shift.Equals('R'))
            {
                Pointer++;
            }
            if (Conditions[CurrState].Changes[pos].Shift.Equals('L'))
            {
                Pointer--;
            }

            CurrState = Conditions[CurrState].Changes[pos].NewState;

            Symbol = InputTape[Pointer];

            return 2;
        }

       
    }
}
