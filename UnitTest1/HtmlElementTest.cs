using System.Runtime.InteropServices.ComTypes;
using ConsoleAppSample.dynamic;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;

namespace UnitTest1
{
    public class HtmlElementTest
    {
        [Fact]
        public void TestMethod1()
        {
            var img = new HtmlElement("img");
            Assert.Equal("img", img.TagName);
        }

        [Fact]
        public void ShouldAddAttributeNameAndValueDynamically()
        {
            dynamic img = new HtmlElement("img");
            img.src = "car.png";
            Assert.Equal("car.png", img.src);
        }

        [Fact]
        public void ShouldErrorIfAttributeNotSet()
        {
            dynamic image = new HtmlElement("img");

            Assert.Throws<RuntimeBinderException>(() => image.src);
        }
        [Fact]
        public void ShouldReturnDynamicMemberName()
        {
            dynamic img = new HtmlElement("img");
            img.src = "car.png";
            img.alt = "a blue car";

            string[] members = img.GetDynamicMemberNames();

            Assert.Equal(2, members.Length);
            Assert.Equal("src", members[0]);
            Assert.Equal("alt", members[1]);

        }

        [Fact]
        public void ShouldOutputTagHtml()
        {
                dynamic image = new HtmlElement("img");
            image.src = "car.png";
            image.alt = "a blue car";
            Assert.Equal("<img src='car.png' alt='a blue car' />", image.ToString());
        }

        [Fact]
        public void ShouldRenderHtml()
        {
            dynamic image = new HtmlElement("img");
            image.src = "car.png";
            image.alt = "a blue car";
            var html = image.Render();

            Assert.Equal("<img src='car.png' alt='a blue car' />", html);
        }

    }
}
