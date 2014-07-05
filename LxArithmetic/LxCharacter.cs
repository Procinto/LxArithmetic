using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public class LxCharacter : ILxCharacter
	{
		#region Constituent data
		public string Alphabet { get; private set; } // ref (shared)
		public uint Index { get; private set; } // val (owned)
		#endregion

		#region Create

		ILxCharacter Create(string alphabet, uint index)
		{
			Alphabet = alphabet;
			Index = index;
			return this;
		}

		public ILxCharacter Create(string alphabet, int index)
		{
			return Create(alphabet, (uint) index);
		}

		#endregion

		#region Constructor

		public LxCharacter(string alphabet, int index)
		{
			Create(alphabet, index);
		}

		public LxCharacter()
		{
			Alphabet = null;
			Index = 0u;
		}
		
		#endregion

		#region Tests

		public bool IsValid { get { return Alphabet != null && Index < Alphabet.Count(); } }

		public bool IsCompatible(ILxCharacter theOtherCharacter)
		{
			if (!(theOtherCharacter is LxCharacter)) { return false; }
			
			LxCharacter c = (LxCharacter) theOtherCharacter;
			if (c.Alphabet == null) { return false; }

			return (this.Alphabet == c.Alphabet);
		}

		#endregion

		public char? ToCharacter()
		{
			if (!IsValid) { return null; }
			return Alphabet[(int)Index];
		}

		// Status.
		public bool HasRolledOver { get; private set; }

		internal LxCharacter Rollover()
		{
			Index = 0;
			HasRolledOver = true;
			return this;
		}

		/// <summary>
		/// Increments in place, also returns the new value.
		/// </summary>
		/// <returns></returns>
		public LxCharacter Increment()
		{
			Index++;
			if (!IsValid) { Rollover(); } else { HasRolledOver = false; }
			return this;
		}

	}
}
