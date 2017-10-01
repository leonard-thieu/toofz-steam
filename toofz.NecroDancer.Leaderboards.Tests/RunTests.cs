using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class RunTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var runId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Run(runId, name, displayName);
                });
            }

            [TestMethod]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Run(runId, name, displayName);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var run = new Run(runId, name, displayName);

                // Assert
                Assert.IsInstanceOfType(run, typeof(Run));
            }

            [TestMethod]
            public void SetsRunId()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var run = new Run(runId, name, displayName);

                // Act
                var runId2 = run.RunId;

                // Assert
                Assert.AreEqual(runId, runId2);
            }

            [TestMethod]
            public void SetsName()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var run = new Run(runId, name, displayName);

                // Act
                var name2 = run.Name;

                // Assert
                Assert.AreEqual(name, name2);
            }

            [TestMethod]
            public void SetsDisplayName()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var run = new Run(runId, name, displayName);

                // Act
                var displayName2 = run.DisplayName;

                // Assert
                Assert.AreEqual(displayName, displayName2);
            }
        }
    }
}
