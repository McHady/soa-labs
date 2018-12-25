using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SerializeTests
{
    [TestClass]
    public class Serialization
    {
        public class Output
        {
            public decimal SumResult;
            public int MulResult;

            public decimal[] SortedInputs;

        }
        [TestMethod]
        public void IsXmlSerializeReturnsRightData()
        {
            string Expected =
                "<Output><SumResult>30.30</SumResult><MulResult>4</MulResult><SortedInputs><decimal>1</decimal><decimal>1.01</decimal><decimal>2.02</decimal><decimal>4</decimal></SortedInputs></Output>";
            var Actual = new Serialize.Serialization<Input>("Xml").Serialize(new Output{SumResult = 30.30M, MulResult = 4, SortedInputs = new[] {1M, 1.01M, 2.02M, 4M}});
            Assert.AreEqual(Expected, Actual);
        }

        public class Input
        {
            public int K;
            public decimal[] Sums = {};
            public int[] Muls = {};

            public override bool Equals(object obj)
            {
                if (obj.GetType() != this.GetType())
                    return false;

                var input = (Input) obj;

                if (this.K != input.K)
                    return false;

                if (this.Sums.Length != input.Sums.Length)
                    return false;

                for (var i = 0; i < this.Sums.Length; i++)
                
                    if (this.Sums[i] != input.Sums[i])
                        return false;
                

                if (this.Muls.Length != input.Muls.Length)
                    return false;

                for (var i = 0; i < this.Muls.Length; i++)

                    if (this.Muls[i] != input.Muls[i])
                        return false;

                return true;

            }
        }

        private string xmlSerStr = @"<Input><K>10</K><Sums><decimal>1.01</decimal><decimal>2.02</decimal></Sums><Muls><int>1</int><int>4</int></Muls></Input>";

        [TestMethod]
        public void IsXmlDeserializeReturnsRightData()
        {
            var Expected = new Input(){K = 10, Sums = new [] {1.01M, 2.02M}, Muls = new []{1,4}};

            var Actual = (Input)new Serialize.Serialization<Input>("Xml").Deserialize(xmlSerStr);

            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void IsJsonSerializationReturnsRightData()
        {
            string Expected = "{\"SumResult\":30.30,\"MulResult\":4,\"SortedInputs\":[1.0,1.01,2.02,4.0]}";

            var Actual = new Serialize.Serialization<Input>("Json").Serialize(new Output { SumResult = 30.30M, MulResult = 4, SortedInputs = new[] { 1M, 1.01M, 2.02M, 4M } });
            Assert.AreEqual(Expected, Actual);
        }

        private string jsonSerStr = "{\"K\":10,\"Sums\":[1.01,2.02],\"Muls\":[1,4]}";
        [TestMethod]
        public void IsJsonDeserializeReturnsRightData()
        {
            var Expected = new Input() { K = 10, Sums = new[] { 1.01M, 2.02M }, Muls = new[] { 1, 4 } };

            var Actual = (Input)new Serialize.Serialization<Input>("Json").Deserialize(jsonSerStr);

            Assert.AreEqual(Expected, Actual);
        }
    }
}
