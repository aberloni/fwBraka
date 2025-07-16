using System;
using System.Collections.Generic;

namespace fwp.braka.utils
{

	public class Blocker<TState> where TState : Enum
	{
		public object Owner { get; }
		public TState State { get; set; }

		public Blocker(object owner, TState state)
		{
			Owner = owner ?? throw new ArgumentNullException(nameof(owner));
			State = state;
		}
	}

	public class LockPileEnum<TState> where TState : Enum
	{
		private List<Blocker<TState>> blockers = new List<Blocker<TState>>();

		public bool IsLocking => blockers.Count > 0;

		public Blocker<TState> Add(object owner, TState state)
		{
			// Check if the blocker already exists for the owner
			var existingBlocker = blockers.Find(b => Equals(b.Owner, owner));
			if (existingBlocker != null)
			{
				// Perform bitwise OR on the underlying int values of the Enum
				var newState = (LockPileHelper<TState>.EnumToInt(existingBlocker.State) | LockPileHelper<TState>.EnumToInt(state));
				existingBlocker.State = LockPileHelper<TState>.IntToEnum(newState);
				return existingBlocker;
			}

			var blocker = new Blocker<TState>(owner, state);
			blockers.Add(blocker);
			return blocker;
		}

		public bool Remove(object owner)
		{
			// Remove the blocker associated with the owner
			var blockerToRemove = blockers.Find(b => Equals(b.Owner, owner));
			if (blockerToRemove != null)
			{
				blockers.Remove(blockerToRemove);
				return true;
			}
			return false;
		}

		public bool HasState(TState state)
		{
			foreach (var blocker in blockers)
				if (LockPileHelper<TState>.StateHasBitmask(blocker.State, state))
					return true;
			return false;
		}

		public void Clear()
		{
			blockers.Clear();
		}

		public bool UpdateStateForOwner(object owner, TState newState)
		{
			var existingBlocker = blockers.Find(b => Equals(b.Owner, owner));
			if (existingBlocker != null)
			{
				existingBlocker.State = newState;
				return true;
			}
			return false;
		}
	}

	public static class LockPileHelper<TState> where TState : Enum
	{

		static public bool StateHasBitmask(TState state, TState bitmask)
		{
			var stateVal = Convert.ToInt32(state);
			var bitmaskVal = Convert.ToInt32(bitmask);
			return (stateVal & bitmaskVal) != 0;
		}

		// Helper method to convert Enum to int
		static public int EnumToInt(TState state)
		{
			return Convert.ToInt32(state);
		}

		// Helper method to convert int to Enum
		static public T EnumToEnum<T>(int value) where T : Enum
		{
			return (T)Enum.ToObject(typeof(T), value);
		}

		// Helper method to convert int to TState
		static public TState IntToEnum(int value)
		{
			return (TState)Enum.ToObject(typeof(TState), value);
		}
	}
}