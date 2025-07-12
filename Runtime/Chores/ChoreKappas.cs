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

		List<iKappa> blacklist = new List<iKappa>();
		List<iKappa> mastering = new List<iKappa>();

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

		/// <summary>
		/// T target kappa
		/// </summary>
		public T optinSetMaster<T>() where T : iKappa
		{
			T tar = owner.getKappa<T>();
			mastering.Add(tar);

			tar.lockmaster.Add(this);

			return tar;
		}

		public T optinBlacklist<T>() where T : iKappa
		{
			T tar = (T)owner.setKappaActivity<T>(false); // enabled = false
			blacklist.Add(tar);
			return tar;
		}

		protected override void Clear()
		{
			base.Clear();

			foreach (var m in mastering)
			{
				m.lockmaster.Remove(this);
			}

			foreach (var b in blacklist)
			{
				b.setActivity(true);
			}

			blacklist.Clear();
			blacklist = null;
			mastering.Clear();
			mastering = null;
		}

	}
}
