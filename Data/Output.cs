using System.Collections.Generic;

namespace Data
{
    public class Output
    {
        public decimal SumResult;
        public int MulResult;

        public decimal[] SortedInputs;

        public override bool Equals(object obj)
        {
            var output = obj as Output;
            return output != null &&
                   SumResult == output.SumResult &&
                   MulResult == output.MulResult &&
                   EqualityComparer<decimal[]>.Default.Equals(SortedInputs, output.SortedInputs);
        }
    }
}
