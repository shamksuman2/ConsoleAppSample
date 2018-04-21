using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleAppSample.CircuitBreaker
{
    public abstract class CircuitBreakerState
    {
        protected readonly CircuitBreaker circuitBreaker;

        protected CircuitBreakerState(CircuitBreaker circuitBreaker)
        {
            this.circuitBreaker = circuitBreaker;
        }

        public virtual CircuitBreaker ProtectedCodeIsAboutToBeCalled()
        {
            return this.circuitBreaker;
        }

        public virtual void ProtectedCodeHasBeenCalled()
        {

        }

        public virtual void ActUponException(Exception e)
        {
            circuitBreaker.IncreaseFailureCount();
        }

        public virtual CircuitBreakerState Update()
        {
            return this;
        }

    }

    public class OpenState: CircuitBreakerState
    {
        private readonly DateTime openDateTime;

        public OpenState(CircuitBreaker circuitBreaker) : base(circuitBreaker)
        {
            openDateTime = DateTime.UtcNow;
        }

        public override CircuitBreaker ProtectedCodeIsAboutToBeCalled()
        {
            base.ProtectedCodeIsAboutToBeCalled();
            this.Update();
            return base.circuitBreaker;
        }

        public override CircuitBreakerState Update()
        {
            base.Update();
            if (DateTime.UtcNow >= openDateTime + base.circuitBreaker.Timeout)
            {
                return circuitBreaker.MoveToHalfOpenState();
            }

            return this;
        }
    }


    public class HalfOpenState: CircuitBreakerState
    {
        public HalfOpenState(CircuitBreaker circuitBreaker) : base(circuitBreaker)
        {

        }

        public override void ActUponException(Exception e)
        {
            base.ActUponException(e);
            circuitBreaker.MoveToOpenState();
        }

        public override void ProtectedCodeHasBeenCalled()
        {
            base.ProtectedCodeHasBeenCalled();
            circuitBreaker.MoveToCloseState();//  MoveCodeToClosedState();
        }
       
    }

    public class CloseState: CircuitBreakerState
    {
        public CloseState(CircuitBreaker circuitBreaker) : base(circuitBreaker)
        {
            circuitBreaker.ResetFailureCount();
        }

        public override void ActUponException(Exception e)
        {
            base.ActUponException(e);
            if (circuitBreaker.IsThresHoldReached())
            {
                circuitBreaker.MoveToOpenState();
            }
        }
    }

    public class CircuitBreaker
    {
        private readonly object monitor = new object();
        private CircuitBreakerState state;

        public int Failures { get; private set; }
        public int Threshold { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public CircuitBreaker(int threshold, TimeSpan timeout)
        {
            if (threshold < 1)
            {
                throw new ArgumentOutOfRangeException("threshold", "Threshold should be greater then 0.");
            }

            if (timeout.TotalMilliseconds < 1 )
            {
                throw new ArgumentOutOfRangeException("Timeout", "Timeout should be greater then 0.");
            }

            Threshold = threshold;
            Timeout = timeout;
            MoveToCloseState();

        }

        public bool IsClosed
        {
            get { return state.Update() is CloseState; }
        }

        public bool IsOpened
        {
            get { return state.Update() is OpenState; }
        }

        public bool IsHalfOpen
        {
            get { return state.Update() is HalfOpenState; }
        }

        internal CircuitBreakerState MoveToCloseState()
        {
            state = new CloseState(this);
            return state;
        }

        internal CircuitBreakerState MoveToOpenState()
        {
            state = new OpenState(this);
            return state;
        }

        internal CircuitBreakerState MoveToHalfOpenState()
        {
            state = new HalfOpenState(this);
            return state;
        }

        internal void IncreaseFailureCount()
        {
            Failures++;
        }
        internal void ResetFailureCount()
        {
            Failures = 0;
        }

        public bool IsThresHoldReached()
        {
            return Failures >= Threshold;
        }

        private Exception exceptionFromLastAttemptCall = null;

        public Exception GetExceptionFromLastAttemptCall()
        {
            return exceptionFromLastAttemptCall;
        }

        public CircuitBreaker AttemptToCall(Action protectedCall)
        {
            this.exceptionFromLastAttemptCall = null;
            lock (monitor)
            {
                state.ProtectedCodeIsAboutToBeCalled();
                if (state is OpenState)
                {
                    return this;
                }
            }

            try
            {
                protectedCall();
            }
            catch (Exception e)
            {
                this.exceptionFromLastAttemptCall = e;
                lock (monitor)
                {
                    state.ActUponException(e);
                }
                return this;
            }

            lock (monitor)
            {
                state.ProtectedCodeHasBeenCalled();
            }

            return this;
        }

        public void Close()
        {
            lock (monitor)
            {
                MoveToCloseState();
            }
        }

        public void Open()
        {
            lock (monitor)
            {
                MoveToOpenState();
            }
        }

    }


}
