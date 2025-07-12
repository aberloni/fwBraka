using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

	abstract public class KappaAtom : iKappa
	{
		public BrainBase brain;

		public Transform pivotBody => brain.pivotBody; // appearance tr
		public Transform pivot => brain.pivot; // brain tr

		public iKappa assoc(Brain owner)
		{
			this.brain = owner as BrainBase;
			reactAssoc();
			return this;
		}

		/// <summary>
		/// assoc to brain
		/// ~constructor
		/// </summary>
		virtual protected void reactAssoc()
		{ }

		/// <summary>
		/// after assoc
		/// and round start
		/// </summary>
		virtual public iKappa prime()
		{
			Debug.AssertFormat(brain != null, "need brain to prime ?", this);
			return this;
		}

		abstract public void update(float dt);

		public bool isOwned(BWBrainBase other)
		{
			return brain == other;
		}

		virtual public void drawGizmos()
		{ }

		public void log(string msg, object tar = null) => brain.log(msg, tar);
		public void logw(string msg, object tar = null) => brain.log(msg, tar);

		virtual public string stringify()
		{
			if (brain != null) ret += " +brain:" + brain.name;
			else ret += " -brain";

			if (!brain.enabled) ret += " -enabled";

			return ret;
		}
	}
}