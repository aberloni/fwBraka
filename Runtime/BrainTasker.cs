using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	using fwp.gamepad;

	public class BrainTasker : BrainKappable, iTasker
	{
		List<iTask> tasks = new List<iTask>(); // blocker

		/// <summary>
		/// is doing something
		/// </summary>
		public bool IsTasking => tasks.Count > 0;
		public bool IsBlocking
		{
			get
			{
				if (tasks.Count <= 0) return false;
				foreach (var t in tasks)
				{
					if (!t.IsMultitask()) return true;
				}
				return false;
			}

		}

		Coroutine _runner = null;

		public bool absorbAction(InputActions act)
		{
			foreach (var t in tasks)
			{
				iTaskAbsorb ita = t as iTaskAbsorb;
				if (ita == null) continue;
				if (ita.absorbAction(act)) return true;
			}
			return false;
		}

		public bool absorbJoys(InputJoystickSide joy)
		{
			foreach (var t in tasks)
			{
				iTaskAbsorb ita = t as iTaskAbsorb;
				if (ita == null) continue;
				if (ita.absorbJoys(joy)) return true;
			}
			return false;
		}

		public void PileTask(iTask task)
		{
			tasks.Add(task);

			// start the coroutine to track process running
			if (_runner == null)
			{
				_runner = StartCoroutine(process());
			}
		}

		protected override void destroy()
		{
			base.destroy();

			if (_runner != null)
			{
				StopCoroutine(_runner);
				_runner = null;
			}
		}

		IEnumerator process()
		{
			while (IsTasking)
			{
				for (int i = 0; i < tasks.Count; i++)
				{
					if (tasks[i].IsCompleted())
					{
						tasks.RemoveAt(i);
						i--;
					}

					yield return null;
				}

			}

			_runner = null;
		}

	}
}
