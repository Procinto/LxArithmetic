using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public partial class LxWord : ILxWord
	{
		/// <summary>
		/// Internal representation of the lexicographical word.
		/// Smaller index values correspond to minor characters.
		/// </summary>
		internal List<LxCharacter> Word { get; set; }
		/// <summary>
		/// Flag indicating whether this instance is set as list of characters.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is set as list of characters; otherwise, <c>false</c>.
		/// </value>
		internal bool IsSetAsListOfCharacters { get; set; }

		public ILxWord Create (LxElementOrder endian, IEnumerable<ILxCharacter> enuc)
		{
			if (null == enuc) {
				IsSetAsListOfCharacters = false;
				Word = null;
				return this;
			}

			Word = new List<LxCharacter> (enuc.Count ());
			foreach (var ch in enuc) {
				if (ch is LxCharacter) {
					Word.Add ((LxCharacter)ch);
				} else { /*internal error?*/
				}
			}
			if (endian == LxElementOrder.MajorToMinor) {
				Word.Reverse ();
			}
			IsSetAsListOfCharacters = true;
			return this;
		}

		#region Constructor
		public LxWord ()
		{
			Create (LxElementOrder.MinorToMajor, null);
		}

		public LxWord (LxElementOrder endian, IEnumerable<ILxCharacter> enuc)
		{
			Create (endian, enuc);
		}

		public LxWord (LxElementOrder endian, params ILxCharacter[] args)
		{
			if (args == null) {
				Create (endian, null);
				return;
			}

			List<ILxCharacter> enuc = new List<ILxCharacter> ();
			foreach (ILxCharacter c in args) {
				enuc.Add (c);
			}
			Create (endian, enuc);
		}
		#endregion

		#region Tests

		public bool IsValid { get { return (null != Word) && Word.All (c => c.IsValid); } }

		public bool IsCompatible (ILxWord theOtherWord)
		{
			if (this.Word == null) {
				return false;
			}
			if (!(theOtherWord is LxWord)) {
				return false;
			}

			LxWord that = (LxWord)theOtherWord;
			if (that.Word == null) {
				return false;
			}

			// Need to only compare the existing character positions.
			var stopAt = this.Word.Count;
			if (stopAt > that.Word.Count) {
				stopAt = that.Word.Count;
			}
			for (int i = 0; i < stopAt; i++) {
				LxCharacter thisCharacter = this.Word [i];
				LxCharacter thatCharacter = that.Word [i];
				if (!thisCharacter.IsCompatible (thatCharacter)) {
					return false;
				}
			}

			return true;
		}

		#endregion

		public int Length { get { return (null == Word) ? 0 : Word.Count (); } }

		public ILxCharacter this [int i] {
			get { return (!IsValid || i < 0 || i >= Length) ? null : Word [i]; }
			private set {
				if (IsValid && (i >= 0 || i < Length) && value is LxCharacter) { 
					Word [i] = (LxCharacter)value; 
				}
			}
		}

		/// <summary>
		/// Increments in place, also returns the new value.
		/// </summary>
		/// <returns></returns>
		public ILxWord Increment ()
		{
			for (int i = 0; i < Word.Count; i++) {
				ILxCharacter c = Word [i];
				c.Increment ();
				if (!c.HasRolledOver) {
					break;
				}
				// Rollover implies incrementing the next major character.
				// Just go on with the loop.
				// TODO - may need to handle the situation when
				// the space of word values has been exhausted.
			}

			return (ILxWord)this;
		}

		/// <summary>
		/// Character weight is the number of individual items that correspond to 
		/// a single increment of the character.
		/// So, for index=0 it is 1, for index=1 it is the size of Alphabet 0, 
		/// and in general a product of the alphabet sizes of characters minor to the current one.
		/// </summary>
		protected ulong[] _weight = null;

		/// <summary>
		/// Just an indicator.
		/// Or is it ever used in calculations? Then revisit the value.
		/// </summary>
		public ulong WEIGHT_INVALID = 0uL;

		/// <summary>
		/// Forces the calculation.
		/// Returns the array of weights.
		/// Lower indices correspond to minor positions.
		/// </summary>
		/// <returns></returns>
		internal ulong[] CalculateWeights ()
		{
			if (Word == null) {
				return _weight = null;
			}

			if (_weight == null) {
				_weight = new ulong[Word.Count];
				_weight [0] = 1;
				for (int i = 1; i < _weight.Length; i++) {
					_weight [i] = _weight [i - 1] * (ulong)Word [i - 1].Alphabet.Count ();
				}
			}
			return _weight;
		}

		/// <summary>
		/// Size of the segment that is swept by incrementing a character
		/// at a given position.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ulong Weight (uint position)
		{
			if (Word == null) {
				return WEIGHT_INVALID;
			}
			if (position >= Word.Count) {
				return WEIGHT_INVALID;
			}

			if (_weight == null) {
				CalculateWeights ();
			}
			return _weight [position];
		}

		/// <summary>
		/// Size of the segment that is swept by incrementing a character
		/// at a given position.
		/// </summary>
		public ulong Weight (int position)
		{
			if (position < 0) {
				return WEIGHT_INVALID;
			}
			return Weight ((uint)position);
		}

		/// <summary>
		/// Null if invalid.
		/// </summary>
		/// <returns></returns>
		public string StringValue ()
		{
			if (!IsValid) {
				return null;
			}

			StringBuilder sb = new StringBuilder (Word.Count);
			for (int i = Word.Count - 1; i >= 0; i--) {
				//int sbi = Word.Count - i - 1;
				sb.Append ((char)Word [i].ToCharacter ());
			}
			return sb.ToString ();
		}

		public ulong NumericValue ()
		{
			ulong retval = 0;
			for (int i = 0; i < this.Word.Count; i++) {
				retval += this.Word [i].Index * Weight (i);
			}
			return retval;
		}

		public override string ToString ()
		{
			return IsValid ? StringValue () : base.ToString ();
		}

	}
}
