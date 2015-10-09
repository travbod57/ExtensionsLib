using System;
using ExtensionsLib.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensionsLib.Extensions;

namespace UnitTest
{
    [TestClass]
    public class EnumExtensionsTest
    {
        [TestMethod]
        public void StringToEnum()
        {
            ClaimsEnum result = (ClaimsEnum)Enum.Parse(typeof(ClaimsEnum), "AllClaims");
            Assert.IsTrue(result == ClaimsEnum.AllClaims);
        }

        [TestMethod]
        public void StringToValue()
        {
            ClaimsEnum result = (ClaimsEnum)Enum.Parse(typeof(ClaimsEnum), "AllClaims");
            Assert.IsTrue((int)result == 1);
        }

        [TestMethod]
        public void EnumToDescription()
        {
            string result = ClaimsEnum.AllClaims.GetDescription();
            Assert.IsTrue(result == "All Claims");
        }
    }
}
