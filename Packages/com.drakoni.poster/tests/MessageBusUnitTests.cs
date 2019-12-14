using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Poster;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MessageBusUnitTests
    {
        [Test]
        public void UnBind_ShouldUnbind()
        {
            // Arrange
            var service = new MessageBus();
            service.OnHandleMessageException += (e) => throw e;
            service.Bind<int>(Handler);
            service.UnBind<int>(Handler);

            // Act/Assert
            service.Send(1);

            void Handler(int _) => Assert.Fail("Not unbinded");
        }

        [Test]
        public void UnBind_ShouldBindAndRecvMessage()
        {
            // Arrange
            var service = new MessageBus();
            service.OnHandleMessageException += (e) => throw e;
            service.Bind<int>(Handler);

            // Act/Assert
            service.Send(1);

            Assert.Fail("Not handled");

            void Handler(int _) => Assert.Pass("message handled");
        }
    }
}
