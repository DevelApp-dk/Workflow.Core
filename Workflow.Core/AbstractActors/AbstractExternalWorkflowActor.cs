using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using Akka.Pattern;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;

namespace DevelApp.Workflow.Core.AbstractActors
{
    public abstract class AbstractExternalWorkflowActor : ReceiveActor
    {
        protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);
        protected readonly int _circuitBrakerMaxFailures;
        protected readonly TimeSpan _circuitBrakerCallTimeout;
        protected readonly TimeSpan _circuitBrakerResetTimeout;

        public AbstractExternalWorkflowActor(int actorInstance = 1, int circuitBrakerMaxFailures = 5
            , TimeSpan circuitBrakerCallTimeout = default, TimeSpan circuitBrakerResetTimeout = default)
        {
            ActorInstance = actorInstance;
            _circuitBrakerMaxFailures = circuitBrakerMaxFailures;
            if (circuitBrakerCallTimeout == default)
            {
                _circuitBrakerCallTimeout = TimeSpan.FromSeconds(10);
            }
            else
            {
                _circuitBrakerCallTimeout = circuitBrakerCallTimeout;
            }
            if (circuitBrakerResetTimeout == default)
            {
                _circuitBrakerResetTimeout = TimeSpan.FromMinutes(1);
            }
            else
            {
                _circuitBrakerResetTimeout = circuitBrakerResetTimeout;
            }

            Receive<DeadletterHandlingMessage>(message =>
            {
                Context.IncrementCounter(nameof(DeadletterHandlingMessage));
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                DeadletterHandlingMessageHandler(message);
            });
        }

        #region Actor Idenitifaction

        private string _actorId;

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        public virtual string ActorId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_actorId))
                {
                    _actorId = $"{ActorName}_{ActorVersion}_{ActorInstance}";
                }

                return _actorId;
            }
        }

        private string _actorName;

        /// <summary>
        /// ActorName is typically the Key for the actor. Override if not classname without Actor
        /// </summary>
        public virtual KeyString ActorName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_actorName))
                {
                    _actorName = GetType().Name.Replace("Actor", "");
                }
                return _actorName;
            }
        }

        private SemanticVersionNumber _semanticVersionNumber;

        /// <summary>
        /// Returns the actor version based on the assembly version
        /// </summary>
        public SemanticVersionNumber ActorVersion
        {
            get
            {
                if (_semanticVersionNumber == null)
                {
                    _semanticVersionNumber = GetType().Assembly.GetName().Version;
                }
                return _semanticVersionNumber;
            }
        }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        public int ActorInstance { get; }

        #endregion

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PreStart()
        {
            base.PreStart();
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
            base.PostStop();
        }

        private CircuitBreaker _circuitBreaker;

        /// <summary>
        /// Returns a default curcuitbreaker that opens CurcuitBreaker when 5 failures happen
        /// </summary>
        /// <returns></returns>
        protected CircuitBreaker CircuitBreaker
        {
            get
            {
                if (_circuitBreaker == null)
                {
                    _circuitBreaker = CreateCircuitBreaker()
                        .OnOpen(CircuitBreakerOpen)
                        .OnClose(CircuitBreakerClosed)
                        .OnHalfOpen(CircuitBreakerHalfOpen);
                }
                return _circuitBreaker;
            }
        }

        /// <summary>
        /// Returns a default curcuitbreaker that opens CurcuitBreaker when 5 failures happen
        /// </summary>
        /// <returns></returns>
        protected virtual CircuitBreaker CreateCircuitBreaker()
        {
            return new CircuitBreaker(
                maxFailures: _circuitBrakerMaxFailures,
                callTimeout: _circuitBrakerCallTimeout,
                resetTimeout: _circuitBrakerResetTimeout);
        }

        /// <summary>
        /// CircuitBreaker is closed so messages can be sent
        /// </summary>
        protected void CircuitBreakerClosed()
        {
            Logger.Debug("{0} CircuitBreaker is closed so messages can be sent", ActorId);
        }

        /// <summary>
        /// CircuitBreaker is halfopen so singular message is sent and rest is stored awaiting closed
        /// </summary>
        protected void CircuitBreakerHalfOpen()
        {
            Context.IncrementCounter("CircuitBreakerHalfOpen");
            Logger.Debug("{0} CircuitBreaker is halfopen so singular message is sent and rest is stored awaiting closed", ActorId);
        }

        /// <summary>
        /// CircuitBreaker is open so no messages can be sent until timeout
        /// </summary>
        protected void CircuitBreakerOpen()
        {
            Context.IncrementCounter("CircuitBreakerOpen");
            Logger.Debug("{0} CircuitBreaker is open so no messages can be sent until timeout", ActorId);
        }

        /// <summary>
        /// Handles DeadletterHandlingMessage. Default is to log and ignore
        /// </summary>
        /// <param name="message"></param>
        protected virtual void DeadletterHandlingMessageHandler(DeadletterHandlingMessage message)
        {
            Logger.Debug("{0} received message {1}", ActorId, message.ToString());
        }
    }
}
