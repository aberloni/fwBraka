using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	using System;
	using fwp.gamepad;

	abstract public class BrainBase : MonoUpdate, Brain
	{
		public GamepadWatcher watcher;

		[Header("pivots")]
		public Transform pivot; // local transform
		public Transform pivotEye; // transform looking at POI
		public Transform pivotBody => appearance.transform; // appearance tr

		/// <summary>
		/// list of all kappa linked to this brain
		/// </summary>
		protected Dictionary<Type, Kappa> kappas = null;

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

			kappas = new Dictionary<Type, Kappa>();
		}

		override protected void setup()
		{
			base.setup();

			setupKaps();

			prime();

			materialize();
		}

		virtual protected void materialize()
		{
		}

		virtual protected void dematerialize()
		{
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

		protected override void update()
		{
			base.update();

			updateKappas(Time.deltaTime);
		}

		public void updateKappas(float dt)
		{
			foreach (var kp in kappas)
			{
				//Debug.Log(kp.Value);
				kp.Value.update(dt);
			}
		}

		public Kappa addKappa(Type t)
		{
			Kappa ret = default(Kappa);

			if (!kappas.ContainsKey(t))
			{
				ret = Activator.CreateInstance(t) as Kappa;

				kappas.Add(t, ret);

				ret.assoc(this);
			}

			return ret;
		}

		public Kappa addKappa(Kappa k)
		{
			if (!kappas.ContainsKey(k.GetType()))
			{
				kappas.Add(k.GetType(), k);

				k.assoc(this);
			}

			return k;
		}

		public TKappa getKappa<TKappa>() where TKappa : Kappa
		{
			var t = typeof(TKappa);
			if (!kappas.ContainsKey(t))
			{
				return default(TKappa);
			}
			return (TKappa)kappas[t];
		}

		virtual public bool isSelectable() => false;

		public bool isSelected() => watcher != null;

		void OnDrawGizmosSelected()
		{
			if (kappas != null)
			{
				// /! some are not kappa but kappamono

				foreach (var kp in kappas)
				{

					BWKappa bwk = kp.Value as BWKappa;
					bwk?.drawGizmos();
				}
			}
		}


	}

}