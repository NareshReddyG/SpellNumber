using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpellNumber.Services;

namespace SpellNumber.Test
{
    [TestClass]
    public class SpellNumberTest
    {
        [TestMethod]
        [DataRow(0, "Zero")]
        [DataRow(10.2, "Ten point two")]
        [DataRow(21.47, "Twenty one point four seven")]
        [DataRow(32.609, "Thirty two point six zero nine")]
        [DataRow(1.23456789, "One point two three four five six seven eight nine")]
        public void TestPostivieNumbers(double before, string expected)
        {
            // To-Do: need to improve
            var result = new InternationalNumbering().SpellNumberInWords(before);
            Assert.AreEqual(expected, result);
        }
    }
}
