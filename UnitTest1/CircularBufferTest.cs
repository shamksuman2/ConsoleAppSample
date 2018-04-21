using System;
using System.Collections.Generic;
using System.Text;
using ConsoleAppSample.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest1
{
    [TestClass]
    public class CircularBufferTest
    {
        [TestMethod]
        public void New_Buffer_Is_Empty()
        {
            var buffer = new CircularBuffer<double>();
            Assert.IsTrue(buffer.IsEmpty);
        }

        [TestMethod]
        public void Overwrite_When_More_Then_Capacity()
        {
            var buffer = new CircularBuffer<double>(capacity: 3);
            var values = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

            foreach (var value in values)
            {
                buffer.Write(value);
            }

            Assert.IsTrue(buffer.IsFull);
            Assert.AreEqual(values[2], buffer.Read());
            Assert.AreEqual(values[3], buffer.Read());
            Assert.AreEqual(values[4], buffer.Read());
            Assert.IsTrue(buffer.IsEmpty);
        }
    }

}
