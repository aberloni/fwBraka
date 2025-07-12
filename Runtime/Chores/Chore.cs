using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using fwp.gamepad;

namespace fwp.braka
{
    /// <summary>
    /// object that will manage a list of tasks
    /// </summary>
    public interface iTasker
    {
        /// <summary>
        /// any tasks running
        /// </summary>
        public bool IsTasking { get; }

        /// <summary>
        /// any task NOT multitask
        /// </summary>
		public bool IsBlocking { get; }

        /// <summary>
        /// new task incoming
        /// </summary>
        public void PileTask(iTask task);
	}

    /// <summary>
    /// base structure fort a task
    /// compat with Tasker
    /// </summary>
    public interface iTask
    {
        public void Cancel();
        //public void Execute(System.Action onDone = null);
        public bool IsCompleted();

        public bool IsMultitask();
    }

    /// <summary>
    /// a task that will silence some inputs
    /// for parent Tasker (brain)
    /// </summary>
    public interface iTaskAbsorb
    {
        public bool absorbAction(InputActions act);
		public bool absorbJoys(InputJoystickSide joy);
	}

    /// <summary>
    /// fail-able
    /// pause-able
    /// </summary>
    abstract public class Chore : iTask
    {
        protected enum TaskStage
        {
            init, 
            running, stop,
            complete, cancel, fail,
            destroy
        }

        protected TaskStage state = TaskStage.init;

        virtual public bool IsMultitask() => false;
        public bool IsCompleted() => state == TaskStage.complete;
        public bool Running => state > TaskStage.init && state < TaskStage.complete;

        public Chore()
        { }

        public void Stop()
        {
            state = TaskStage.stop;
        }

        virtual public void Cancel()
        {
            state = TaskStage.cancel;
			Clear();
		}

        virtual protected void OnCompletion()
        {
            state = TaskStage.complete;
			Clear();
		}

        virtual public void Destroy()
        {
            state = TaskStage.destroy;
            Clear();
        }

        virtual protected void Clear()
        { }
    }

}