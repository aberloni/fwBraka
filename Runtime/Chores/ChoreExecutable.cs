using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
    public class ChoreExecutable : Chore
    {

        public void Execute(System.Action onDone = null)
        {
            state = TaskStage.running;
            Run(() =>
            {
                OnCompletion();
                onDone?.Invoke();
            });
        }

        virtual protected void Run(System.Action onCompletion)
        {
            onCompletion?.Invoke();
        }

    }
}