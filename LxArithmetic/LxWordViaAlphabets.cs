using System;
using System.Collections.Generic;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public partial class LxWord /*: ILxWordViaAlphabets*/
	{
		// TODO - or a list/array of alphabets?
		internal List<string> AlphabetSequence { get; set; }

		// TODO - or an array/list of indices?
		internal List<uint> IndexSequence { get; set; }

		internal bool IsSetAsAlphabetSequences { get; set; }

		/// <summary>
		/// Expects both sequences ordered either from the most minor to the most major characters or the other way. 
		/// Requires both sequences nonempty and of the same length.
		/// NOTE: does not check index integrity.
		/// </summary>
		public ILxWord Create (LxElementOrder endian, IEnumerable<string> alphabetSequence, IEnumerable<uint> indexSeqence)
		{
			IsSetAsAlphabetSequences = false;

			if (null == alphabetSequence || null == indexSeqence) {
				this.AlphabetSequence = null;
				this.IndexSequence = null;
				return this;
			}

			AlphabetSequence = new List<string> (alphabetSequence);
			IndexSequence = new List<uint> (indexSeqence);
			if (AlphabetSequence.Count != IndexSequence.Count || AlphabetSequence.Count == 0) {
				AlphabetSequence = null;
				IndexSequence = null;
				return this;
			}

			this.IsSetAsAlphabetSequences = true;
			return this;
		}

		public LxWord (LxElementOrder endian, IEnumerable<string> alphabetSequence, IEnumerable<uint> indexSeqence)
		{
			Create (endian, alphabetSequence, indexSeqence);
		}

		protected bool IsValidAsAlphabets {
			get {
				// This test is superfluous?
				if (null == this.AlphabetSequence || null == this.IndexSequence
					|| this.AlphabetSequence.Count != this.IndexSequence.Count
					|| this.AlphabetSequence.Count == 0) {
					return false;
				}
				// Test index integrity.
				for (int i = 0; i < this.AlphabetSequence.Count; i++) {
					var abc = this.AlphabetSequence [i];
					var index = this.IndexSequence [i];
					if (index >= abc.Length) {
						return false;
					}
				}
				// Passed all tests.
				return true;
			}
		}

		protected int LengthViaAlphabets {
			get {
				return this.AlphabetSequence.Count;
			}
		}

		/// <summary>
		/// Assumes the alphabet and index sequences are set.
		/// Requires (and checks for) internal consistency.
		/// </summary>
		protected string StringValueViaAlphabets {
			get {
				if (!this.IsValidAsAlphabets) {
					return null;
				}

				StringBuilder sb = new StringBuilder (LengthViaAlphabets);
				for (int i = 0; i < this.LengthViaAlphabets; i++) {
					var abc = this.AlphabetSequence [i];
					var index = this.IndexSequence [i];
					if (index >= abc.Length) {
						return null;
					}
					sb.Append (abc [(int)index]);
				}
				return sb.ToString ();
			}
		}

		protected ulong NumericValueViaAlphabets {
			get {
				throw new NotImplementedException ();
			}
		}

		protected ILxCharacter GetAtViaAlphabets (int i)
		{
			throw new NotImplementedException ();
		}

		protected void SetAtViaAlphabets (int i, ILxCharacter value)
		{
			throw new NotImplementedException();
		}

	}
}

