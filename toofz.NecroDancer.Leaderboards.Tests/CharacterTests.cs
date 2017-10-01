using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class CharacterTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var characterId = 1;
                string name = null;
                var displayName = "myDisplayName";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Character(characterId, name, displayName);
                });
            }

            [TestMethod]
            public void DisplayNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                string displayName = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new Character(characterId, name, displayName);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var characterId = 1;
                var name = "myName";
                var displayName = "myDisplayName";

                // Act
                var character = new Character(characterId, name, displayName);

                // Assert
                Assert.IsInstanceOfType(character, typeof(Character));
            }

            [TestMethod]
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
                Assert.AreEqual(characterId, characterId2);
            }

            [TestMethod]
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
                Assert.AreEqual(name, name2);
            }

            [TestMethod]
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
                Assert.AreEqual(displayName, displayName2);
            }
        }
    }
}
