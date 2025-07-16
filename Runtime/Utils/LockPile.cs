using System.Collections.Generic;

namespace fwp.braka.utils
{

	public class LockPile
	{
		private HashSet<object> blockers = new HashSet<object>();

		public bool IsLocking => blockers.Count > 0;

		public object Add(object token = null)
		{
			if (token == null) token = new object();
			blockers.Add(token);
			return token;
		}

		public bool Remove(object blocker)
		{
			blockers.Remove(blocker);
			return IsLocking;
		}

		public void Clear()
		{
			blockers.Clear();
		}

		public string stringify()
		{
			if (blockers.Count <= 0) return GetType() + ":empty";
			string ret = GetType() + " blockers x" + blockers.Count;
			foreach (var b in blockers)
			{
				ret += "\n" + b.GetType();
			}
			return ret;
		}
	}

}