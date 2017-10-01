using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ProductTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var productId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Product(productId, name, displayName);
                });
            }

            [TestMethod]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var productId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Product(productId, name, displayName);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var productId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var product = new Product(productId, name, displayName);

                // Assert
                Assert.IsInstanceOfType(product, typeof(Product));
            }

            [TestMethod]
            public void SetsProductId()
            {
                // Arrange
                var productId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var product = new Product(productId, name, displayName);

                // Act
                var productId2 = product.ProductId;

                // Assert
                Assert.AreEqual(productId, productId2);
            }

            [TestMethod]
            public void SetsName()
            {
                // Arrange
                var productId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var product = new Product(productId, name, displayName);

                // Act
                var name2 = product.Name;

                // Assert
                Assert.AreEqual(name, name2);
            }

            [TestMethod]
            public void SetsDisplayName()
            {
                // Arrange
                var productId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var product = new Product(productId, name, displayName);

                // Act
                var displayName2 = product.DisplayName;

                // Assert
                Assert.AreEqual(displayName, displayName2);
            }
        }
    }
}
