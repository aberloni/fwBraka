using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	using System;

	/// <summary>
	/// no kappas here
	/// </summary>
	abstract public class BrainBase : MonoPresence, iBrain
	{
		[Header("pivots")]
		public Transform pivot; // local transform
		public Transform pivotEye; // transform looking at POI
		public Transform pivotBody => appearance.transform; // appearance tr

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
		}

		override protected void setup()
		{
			base.setup();

			prime();
		}

		protected override void setupLate()
		{
			base.setupLate();

			materialize();
		}

		protected override void destroy()
		{
			base.destroy();

			dematerialize();
		}

		/// <summary>
		/// entry point to reset element
		/// </summary>
		[ContextMenu("prime")]
		virtual public void prime()
		{
			appearance.prime();
		}

		virtual public bool isSelectable() => false;
		virtual public bool isSelected() => false;

		public void setPosition(Vector3 pos)
		{
			pivot.position = pos;
		}

	}

}