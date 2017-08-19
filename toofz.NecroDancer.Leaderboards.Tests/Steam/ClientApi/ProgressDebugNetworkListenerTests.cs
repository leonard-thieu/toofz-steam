using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    class ProgressDebugNetworkListenerTests
    {
        [TestClass]
        public class OnIncomingNetworkMessage
        {
            [TestMethod]
            public void ProgressIsNull_DoesNotThrowNullReferenceException()
            {
                // Arrange
                var listener = new ProgressDebugNetworkListener();

                // Act -> Assert
                listener.OnIncomingNetworkMessage(EMsg.AdminCmd, new byte[0]);
            }

            [TestMethod]
            public void ProgressIsNotNull_ReportsDataLength()
            {
                // Arrange
                var data = new byte[3];

                var mockProgress = new Mock<IProgress<long>>();
                mockProgress
                    .Setup(p => p.Report(data.Length));

                var listener = new ProgressDebugNetworkListener { Progress = mockProgress.Object };

                // Act
                listener.OnIncomingNetworkMessage(EMsg.AdminCmd, data);

                // Assert
                mockProgress.VerifyAll();
            }
        }
    }
}
