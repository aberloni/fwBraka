using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace fwp.braka
{

	abstract public class BrainKappable : BrainBase, iBrainKappable
	{
		public Transform PivotBody => appearance.transform; // appearance tr

		protected Dictionary<Type, iKappa> kappas = null;

		[Header("params")]
		public KappaAppearance appearance;

		/// <summary>
		/// default atom kappa to instantiate
		/// </summary>
		virtual public List<Type> kappaDependencies() => new List<Type>
		{
			//typeof(KappaMove),
		};

		protected override void build()
		{
			base.build();

			kappas = new Dictionary<Type, IKappa>();
		}

		override protected void setup()
		{
			base.setup();

			setupKaps();
		}

		protected override void setupLate()
		{
			base.setupLate();

			prime();
		}

		/// <summary>
		/// generate all needed kappas
		/// add -> will assoc kappa
		/// </summary>
		virtual protected void setupKaps()
		{
			if (appearance != null)
				addKappa(appearance);

			var deps = kappaDependencies();
			foreach (var d in deps)
			{
				addKappa(d);
			}

			var kms = GetComponentsInChildren<KappaMono>();
			foreach (var k in kms)
			{
				addKappa(k);
			}
		}

		/// <summary>
		/// entry point to reset element
		/// </summary>
		[ContextMenu("prime")]
		virtual public void prime()
		{
			if (kappas.Count <= 0)
			{
				Debug.LogWarning("no kappas to prime ?");
				return;
			}

			//Debug.Log(" !!! prime x" + kappas.Count);

			foreach (var k in kappas)
			{
				//Debug.Log(k.GetType());
				k.Value.prime();
			}
		}

		public iKappa addKappa(Type t)
		{
			iKappa ret = default(iKappa);

			if (!kappas.ContainsKey(t))
			{
				ret = Activator.CreateInstance(t) as iKappa;

				kappas.Add(t, ret);

				ret.assoc(this);
			}

			return ret;
		}

		public iKappa addKappa(iKappa k)
		{
			Debug.Assert(k != null, "no kappa given to add ?");

			if (!kappas.ContainsKey(k.GetType()))
			{
				kappas.Add(k.GetType(), k);

				k.assoc(this);
			}

			return k;
		}

		public TKappa getKappa<TKappa>(bool verbose = false) where TKappa : iKappa
		{
			if (kappas != null)
			{
				var t = typeof(TKappa);
				foreach (var k in kappas)
				{
					if (t.IsAssignableFrom(k.Key))
						return (TKappa)k.Value;
				}
			}

			if (verbose)
			{
				Debug.LogWarning("@" + this + " couldn't locate <" + typeof(TKappa) + ">", this);
				if(kappas != null)
				{
					foreach (var k in kappas) Debug.Log(k.Key + " => " + k.Value);
				}
			}

			return default(TKappa);
		}

		protected override void update(float dt)
		{
			updateKappas(Time.deltaTime);
		}

		protected override void updateLate()
		{
			base.updateLate();

			updateLateKappas(Time.deltaTime);
		}

		/// <summary>
		/// is public for interface compat
		/// called localy
		/// </summary>
		public void updateKappas(float dt)
		{
			foreach (var kp in kappas)
			{
				if (kp.Value is IKappaUpdate iku) iku.update(dt);
			}
		}

		public void updateLateKappas(float dt)
		{
			foreach (var kp in kappas)
			{
				if (kp.Value is IKappaUpdate iku) iku.updateLate(dt);
			}
		}

		public iKappaUpdate setKappaActivity<T>(bool whitelist) where T : iKappaUpdate
		{
			return getKappa<T>().setActivity(whitelist);
		}

		public void setKappasActivity(Type[] whitelist, Type[] blacklist)
		{
			foreach (var k in kappas)
			{
				foreach (var t in whitelist)
				{
					if (t.GetType().IsAssignableFrom(k.GetType()))
					{
						k.Value.setActivity(true);
					}
				}

				foreach (var t in blacklist)
				{
					if (t.GetType().IsAssignableFrom(k.GetType()))
					{
						k.Value.setActivity(false);
					}
				}
			}
		}

		void OnDrawGizmosSelected()
		{
			if (kappas != null)
			{
				// /! some are not kappa but kappamono

				foreach (var kp in kappas)
				{

					KappaBase bwk = kp.Value as KappaBase;
					bwk?.drawGizmos();
				}
			}
		}

		public override string stringify()
		{
			var ret = base.stringify();

			if(kappas != null)
			{
				foreach (var kp in kappas)
				{
					KappaBase kb = kp.Value as KappaBase;

					if (kb != null) ret += "\n" + kb.stringify();
					else ret += "\n" + kp.Key.GetType();
				}
			}

			return ret;
		}
	}
}