using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public partial class LxWord : ILxWord
	{
		public bool IsCompatible (ILxWord theOtherWord)
		{
			if (null == theOtherWord) {
				return false;
			}

			// Compatibility is defined as the same potential values.
			// That is, the same lists of alphabets.
			var thisList = this.ListOfAlphabets ().ToList ();
			var theOtherList = theOtherWord.ListOfAlphabets ().ToList ();
			if (null == theOtherList || null == theOtherList) {
				return false;
			}
			if (thisList.Count != theOtherList.Count) {
				// TODO - extension: allow different lengths.
				return false;
			}
			for (int i = 0; i < thisList.Count; i++) {
				if (thisList [i] != theOtherList [i]) {
					return false;
				}
			}

			return true;
		}

		public bool IsValid {
			get {
				if (IsSetAsListOfCharacters && IsValidAsCharacters) {
					return true;
				}
				if (IsSetAsAlphabetSequences && IsValidAsAlphabets) {
					return true;
				}
				return false;
			}
		}

		public ILxWord Increment ()
		{
			if (IsSetAsListOfCharacters) {
				IncrementAsCharacters ();
				if (IsSetAsAlphabetSequences) {
					// TODO - synchronize as alphabet and index sequences
				}
				return this;
			}

			if (IsSetAsAlphabetSequences) {
				throw new NotImplementedException ();
			}

			throw new NotImplementedException ();
		}

		#region Utility - Alphabets

		internal void EnsureHasAlphabets ()
		{
			if (!IsSetAsAlphabetSequences) {
				LxUtility.WordRepresentationFromCharactersToAlphabets (this);
			}
		}

		/// <summary>
		/// Lists the of constituent alphabets which describe the possible values of the word. 
		/// Side effect: calculates the alphabets if needed.
		/// </summary>
		public IEnumerable<string> ListOfAlphabets ()
		{
			if (!this.IsValid) {
				return null;
			}

			if (this.IsSetAsAlphabetSequences) {
				return this.AlphabetSequence;
			}

			if (!this.IsSetAsListOfCharacters) {
				return null; // internal error?
			}

			return LxUtility.WordRepresentationFromCharactersToAlphabets (this).AlphabetSequence;
		}

		#endregion

		#region From Word Via Characters - TODO: account for "via alphabets".

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
		/// TODO HERE
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
		/// TODO HERE
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

		public string StringValue { 
			get {
				if (IsSetAsListOfCharacters) {
					return this.StringValueViaCharacters;
				}
				if (IsSetAsAlphabetSequences) {
					return this.StringValueViaAlphabets;
				}

				return null; // internal error?
			}
		}

		public ulong NumericValue {
			get {
				if (IsSetAsListOfCharacters) {
					return this.NumericValueViaCharacters;
				}
				if (IsSetAsAlphabetSequences) {
					return this.NumericValueViaAlphabets;
				}

				return 0; // internal error?
			}
		}

		public override string ToString ()
		{
			return IsValid ? StringValue : base.ToString ();
		}

		#endregion

		public int Length {
			get {
				if (IsSetAsListOfCharacters) {
					return this.LengthViaCharacters;
				}

				if (IsSetAsAlphabetSequences) {
					return this.LengthViaAlphabets;
				}

				return 0; // internal error?
			}
		}

		public ILxCharacter this [int i] {
			get {
				if (IsSetAsListOfCharacters) {
					return this.GetAtViaCharacters (i);
				}
				if (IsSetAsAlphabetSequences) {
					return this.GetAtViaAlphabets (i);
				}

				return null; // internal error?
			}
			set {
				if (IsSetAsListOfCharacters) {
					this.SetAtViaCharacters (i, value);
					return;
				}
				if (IsSetAsAlphabetSequences) {
					this.SetAtViaAlphabets (i, value);
					return;
				}
			}
		}

	}
}

