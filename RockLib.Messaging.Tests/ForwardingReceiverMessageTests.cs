﻿using FluentAssertions;
using NUnit.Framework;
using RockLib.Messaging.Testing;

namespace RockLib.Messaging.Tests
{
    [TestFixture]
    public class ForwardingReceiverMessageTests
    {
        [Test]
        public void AcknowledgeCallsInnerMessageAcknowledgeIfAcknowledgeForwarderIsNull()
        {
            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, acknowledgeForwarder: null);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Acknowledge();

            message.HandledBy.Should().Be(nameof(IReceiverMessage.Acknowledge));
        }

        [Test]
        public void AcknowledgeSendsMessageToAcknowledgeForwarderWhenAcknowledgeForwarderIsNotNull()
        {
            var forwarder = new TestSender();

            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, acknowledgeForwarder: forwarder);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Acknowledge();

            forwarder.SentMessages.Should().ContainSingle();
            forwarder.SentMessages[0].StringPayload.Should().Be("Hello, world!");
        }

        [TestCase(ForwardingOutcome.Acknowledge)]
        [TestCase(ForwardingOutcome.Rollback)]
        [TestCase(ForwardingOutcome.Reject)]
        public void AcknowledgeHandlesInnerMessageAccordingToAcknowledgeOutcomeWhenAcknowledgeForwarderIsNotNull(ForwardingOutcome outcome)
        {
            var forwarder = new TestSender();

            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, acknowledgeForwarder: forwarder, acknowledgeOutcome: outcome);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Acknowledge();

            message.HandledBy.Should().Be(outcome.ToString());
        }

        [Test]
        public void RollbackCallsInnerMessageRollbackIfRollbackForwarderIsNull()
        {
            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, rollbackForwarder: null);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Rollback();

            message.HandledBy.Should().Be(nameof(IReceiverMessage.Rollback));
        }

        [Test]
        public void RollbackSendsMessageToRollbackForwarderWhenRollbackForwarderIsNotNull()
        {
            var forwarder = new TestSender();

            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, rollbackForwarder: forwarder);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Rollback();

            forwarder.SentMessages.Should().ContainSingle();
            forwarder.SentMessages[0].StringPayload.Should().Be("Hello, world!");
        }

        [TestCase(ForwardingOutcome.Acknowledge)]
        [TestCase(ForwardingOutcome.Rollback)]
        [TestCase(ForwardingOutcome.Reject)]
        public void RollbackHandlesInnerMessageAccordingToRollbackOutcomeWhenRollbackForwarderIsNotNull(ForwardingOutcome outcome)
        {
            var forwarder = new TestSender();

            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, rollbackForwarder: forwarder, rollbackOutcome: outcome);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Rollback();

            message.HandledBy.Should().Be(outcome.ToString());
        }

        [Test]
        public void RejectCallsInnerMessageRejectWhenRejectForwarderIsNull()
        {
            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, rejectForwarder: null);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Reject();

            message.HandledBy.Should().Be(nameof(IReceiverMessage.Reject));
        }

        [Test]
        public void RejectSendsMessageToRejectForwarderWhenRejectForwarderIsNotNull()
        {
            var forwarder = new TestSender();

            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, rejectForwarder: forwarder);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Reject();

            forwarder.SentMessages.Should().ContainSingle();
            forwarder.SentMessages[0].StringPayload.Should().Be("Hello, world!");
        }

        [TestCase(ForwardingOutcome.Acknowledge)]
        [TestCase(ForwardingOutcome.Rollback)]
        [TestCase(ForwardingOutcome.Reject)]
        public void RejectHandlesInnerMessageAccordingToRejectOutcomeWhenRejectForwarderIsNotNull(ForwardingOutcome outcome)
        {
            var forwarder = new TestSender();

            var receiver = new TestReceiver();
            var forwardingReceiver = new ForwardingReceiver("foo", receiver, rejectForwarder: forwarder, rejectOutcome: outcome);
            var message = new TestReceiverMessage("Hello, world!");

            var forwardingMessage = new ForwardingReceiverMessage(forwardingReceiver, message);

            forwardingMessage.Reject();

            message.HandledBy.Should().Be(outcome.ToString());
        }
    }
}
