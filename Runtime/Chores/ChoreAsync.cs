using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
    using System;
	using System.Threading;
	using System.Threading.Tasks;

    abstract public class ChoreAsync : Chore
    {
        CancellationTokenSource tokenSource;
        CancellationToken token;

        public Chore Execute(Action onDone = null)
        {
            ExecuteAsync(onDone);
            return this;
        }

        public async void ExecuteAsync(System.Action onDone = null)
        {
            tokenSource = new CancellationTokenSource();
            await RunAsync();
            OnCompletion();
            onDone?.Invoke();
        }

        virtual protected bool AllowedToRun()
        {
            if (!Application.isPlaying) return false;
            if (token != null && token.IsCancellationRequested) return false;
            return true;
        }

		public override void Cancel()
		{
			base.Cancel();
			tokenSource?.Cancel();
		}

		protected override void Clear()
		{
			base.Clear();
            
            if(tokenSource != null)
            {
                tokenSource.Dispose();
                tokenSource = null;
            }

		}

		/// <summary>
		/// OnCompletion is auto called at end of routine
		/// </summary>
		abstract protected Task RunAsync();
    }
}