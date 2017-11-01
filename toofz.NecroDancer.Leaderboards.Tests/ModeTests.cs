using System;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ModeTests
    {
        public class Constructor
        {
            [Fact]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var modeId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new Mode(modeId, name, displayName);
                });
            }

            [Fact]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new Mode(modeId, name, displayName);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var modeId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var mode = new Mode(modeId, name, displayName);

                // Assert
                Assert.IsAssignableFrom<Mode>(mode);
            }

            [Fact]
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
                Assert.Equal(modeId, modeId2);
            }

            [Fact]
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
                Assert.Equal(name, name2);
            }

            [Fact]
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
                Assert.Equal(displayName, displayName2);
            }
        }
    }
}
