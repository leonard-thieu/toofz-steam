using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ModeTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var modeId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Mode(modeId, name, displayName);
                });
            }

            [TestMethod]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Mode(modeId, name, displayName);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var mode = new Mode(modeId, name, displayName);

                // Assert
                Assert.IsInstanceOfType(mode, typeof(Mode));
            }

            [TestMethod]
            public void SetsModeId()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var mode = new Mode(modeId, name, displayName);

                // Act
                var modeId2 = mode.ModeId;

                // Assert
                Assert.AreEqual(modeId, modeId2);
            }

            [TestMethod]
            public void SetsName()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var mode = new Mode(modeId, name, displayName);

                // Act
                var name2 = mode.Name;

                // Assert
                Assert.AreEqual(name, name2);
            }

            [TestMethod]
            public void SetsDisplayName()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var mode = new Mode(modeId, name, displayName);

                // Act
                var displayName2 = mode.DisplayName;

                // Assert
                Assert.AreEqual(displayName, displayName2);
            }
        }
    }
}
