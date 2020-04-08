using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyService.Lib.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        [Test]
        public void Create_傳入不存在的分類名稱_新增資料成功()
        {
            string categoryName = "BBB";

            ICategoryDAO dao = Substitute.For<ICategoryDAO>();
            dao.IsExist(Arg.Any<string>()).Returns(false);

            var service = new CategoryService(dao);
            service.Create(categoryName);

            dao.Received().Create(categoryName);
        }

        [Test]
        public void Create_傳入已存在的分類名稱_拋出例外()
        {
            string categoryName = "BBB";

            ICategoryDAO dao = Substitute.For<ICategoryDAO>();
            dao.IsExist(Arg.Any<string>()).Returns(true);

            var service = new CategoryService(dao);
            var ex = Assert.Throws<Exception>(() => service.Create(categoryName));

            Assert.That(() => ex.Message.Contains("存在"));
        }
    }
}
