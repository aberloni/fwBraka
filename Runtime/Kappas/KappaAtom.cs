using System.Text;
using UnityEngine;

namespace fwp.braka
{

	abstract public class KappaAtom : iKappa, iKappaDebug
	{
		public iBrain brain;
		public BrainBase bBrain => brain as BrainBase;

		virtual public Transform pivotBody => bBrain.pivotBody; // appearance tr
		virtual public Transform pivot => bBrain.pivot; // brain tr

		public iKappa assoc(iBrain owner)
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

		virtual public void update(float dt)
		{ }

		public bool isOwned(iBrain other)
		{
			return brain == other;
		}

		virtual public void drawGizmos()
		{ }

		StringBuilder _stringify = new();
		public string stringify()
		{
			_stringify.Clear();
			_stringify.Append(GetIdentity());
			
			doStringify();

			return _stringify.ToString();
		}

		virtual protected void doStringify()
		{
			if (bBrain != null) _stringify.Append(" +brain:" + bBrain.name);
			else _stringify.Append(" -brain");

			if (!bBrain.enabled) _stringify.Append(" -enabled");
		}

		public bool IsVerbose(iLogger.LogLevel lvl) => bBrain.IsVerbose(lvl);
		public string GetIdentity() => GetType() + ":" + bBrain.name;
	}
}