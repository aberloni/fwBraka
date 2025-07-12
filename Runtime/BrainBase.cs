using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	using System;
	

	abstract public class BrainBase : MonoPresence, iBrain
	{
		[Header("pivots")]
		public Transform pivot; // local transform
		public Transform pivotEye; // transform looking at POI
		public Transform pivotBody => appearance.transform; // appearance tr

		/// <summary>
		/// list of all kappa linked to this brain
		/// </summary>
		protected Dictionary<Type, iKappa> kappas = null;

		/// <summary>
		/// wrapper to manage anything visual
		/// </summary>
		[Header("params")]
		public KappaAppearance appearance;

		virtual public List<Type> kappaDependencies() => new List<Type>
		{
			//typeof(KappaMove),
		};

		protected override void build()
		{
			base.build();

			pivot = transform;

			kappas = new Dictionary<Type, iKappa>();
		}

		override protected void setup()
		{
			base.setup();

			setupKaps();

			prime();

			materialize();
		}

		protected override void destroy()
		{
			base.destroy();
			dematerialize();
		}

		public void setPosition(Vector3 pos)
		{
			pivot.position = pos;
		}

		/// <summary>
		/// generate all needed kappas
		/// </summary>
		virtual protected void setupKaps()
		{
			addKappa(appearance);

			var deps = kappaDependencies();
			foreach (var d in deps)
			{
				addKappa(d);
			}

		}

		/// <summary>
		/// entry point to reset element
		/// </summary>
		[ContextMenu("prime")]
		virtual public void prime()
		{
			appearance.prime();
			foreach (var k in kappas)
			{
				k.Value.prime();
			}
		}

		protected override void update(float dt)
		{
			updateKappas(dt);
		}

		public void updateKappas(float dt)
		{
			foreach (var kp in kappas)
			{
				//Debug.Log(kp.Value);
				kp.Value.update(dt);
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
			if (!kappas.ContainsKey(k.GetType()))
			{
				kappas.Add(k.GetType(), k);

				k.assoc(this);
			}

			return k;
		}

		public TKappa getKappa<TKappa>() where TKappa : iKappa
		{
			var t = typeof(TKappa);
			if (!kappas.ContainsKey(t))
			{
				return default(TKappa);
			}
			return (TKappa)kappas[t];
		}

		virtual public bool isSelectable() => false;
		virtual public bool isSelected() => false;

	}

}