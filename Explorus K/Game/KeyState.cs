using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Explorus_K.Game
{
    class Context
    {
        // A reference to the current state of the Context.
        private State _state = null;

        public Context(State state)
        {
            this.TransitionTo(state);
        }

        // The Context allows changing the State object at runtime.
        public void TransitionTo(State state)
        {
            Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            this._state = state;
            this._state.SetContext(this);
        }

        public void RequestChangingState()
        {
            this._state.Handle1();
        }

        public string CurrentState()
        {
            return _state.GetType().Name;
        }
    }

    abstract class State
    {
        protected Context _context;

        public void SetContext(Context context)
        {
            this._context = context;
        }

        public abstract void Handle1();
    }

    class NoKeyState : State
    {
        public override void Handle1()
        {
            Console.WriteLine("WithKeyState handles request1.");
            Console.WriteLine("WithKeyState wants to change the state of the context.");
            this._context.TransitionTo(new WithKeyState());
        }
    }

    class WithKeyState : State
    {
        public override void Handle1()
        {
            Console.WriteLine("NoKeyState handles request1.");
            Console.WriteLine("NoKeyState wants to change the state of the context.");
            this._context.TransitionTo(new NoKeyState());
        }
    }
}
