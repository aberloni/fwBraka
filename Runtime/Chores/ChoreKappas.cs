using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	/// <summary>
	/// chore linked to kappas
	/// this chore will deactivate some kaps while running
	/// </summary>
	abstract public class ChoreKappas : ChoreAsync
	{
		protected BrainKappable owner;

		/// <summary>
		/// kappas blocked by this chore
		/// </summary>
		List<iKappaActivity> blacklist = new List<iKappaActivity>();

		public ChoreKappas(BrainKappable brain) : base()
		{
			owner = brain;

			// auto pile to taskers
			iTasker to = owner as iTasker;
			if (to != null)
			{
				to.PileTask(this);
			}

			setCandidates();
		}

		/// <summary>
		/// list all concerned kappas w/b
		/// whitelist allowed to run~
		/// blacklist disabled during run
		/// </summary>
		abstract public void setCandidates();

		/*
        public void optinWhitelist<T>() where T : Kappa
        {
            whitelist.Add(owner.setKappaActivity<T>(true));
        }
        */

		public T optinBlacklist<T>() where T : iKappaActivity
		{
			T tar = (T)owner.setKappaActivity<T>(false); // enabled = false
			blacklist.Add(tar);
			return tar;
		}

		protected override void Clear()
		{
			base.Clear();

			foreach (var b in blacklist)
			{
				b.setActivity(true);
			}

			blacklist.Clear();
			blacklist = null;
		}

	}
}
