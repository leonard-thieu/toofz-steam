﻿using System;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class ProgressDebugNetworkListenerTests
    {
        public class OnIncomingNetworkMessage
        {
            [Fact]
            public void ProgressIsNull_DoesNotThrowNullReferenceException()
            {
                // Arrange
                var listener = new ProgressDebugNetworkListener();

                // Act -> Assert
                listener.OnIncomingNetworkMessage(EMsg.AdminCmd, new byte[0]);
            }

            [Fact]
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
