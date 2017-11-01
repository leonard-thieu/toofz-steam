using System;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class CharacterTests
    {
        public class Constructor
        {
            [Fact]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var characterId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new Character(characterId, name, displayName);
                });
            }

            [Fact]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new Character(characterId, name, displayName);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var character = new Character(characterId, name, displayName);

                // Assert
                Assert.IsAssignableFrom<Character>(character);
            }

            [Fact]
            public void SetsCharacterId()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var character = new Character(characterId, name, displayName);

                // Act
                var characterId2 = character.CharacterId;

                // Assert
                Assert.Equal(characterId, characterId2);
            }

            [Fact]
            public void SetsName()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var character = new Character(characterId, name, displayName);

                // Act
                var name2 = character.Name;

                // Assert
                Assert.Equal(name, name2);
            }

            [Fact]
            public void SetsDisplayName()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                var displayName = "myDisplayName";
                var character = new Character(characterId, name, displayName);

                // Act
                var displayName2 = character.DisplayName;

                // Assert
                Assert.Equal(displayName, displayName2);
            }
        }
    }
}
