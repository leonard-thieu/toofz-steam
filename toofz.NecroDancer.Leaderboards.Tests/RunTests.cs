using System;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class RunTests
    {
        public class Constructor
        {
            [Fact]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var runId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new Run(runId, name, displayName);
                });
            }

            [Fact]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new Run(runId, name, displayName);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var runId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var run = new Run(runId, name, displayName);

                // Assert
                Assert.IsAssignableFrom<Run>(run);
            }

            [Fact]
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
                Assert.Equal(runId, runId2);
            }

            [Fact]
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
                Assert.Equal(name, name2);
            }

            [Fact]
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
                Assert.Equal(displayName, displayName2);
            }
        }
    }
}
