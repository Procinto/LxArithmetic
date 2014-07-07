using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public partial class LxWord //: ILxWordViaCharacters
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

		public ILxWord/*ViaCharacters*/ Create (LxElementOrder endian, IEnumerable<ILxCharacter> enuc)
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

		protected bool IsValidAsCharacters { get { return (null != Word) && Word.All (c => c.IsValid); } }

		[Obsolete("Unused?")]
		protected bool IsCompatibleAsCharacters (ILxWord/*ViaCharacters*/ theOtherWord)
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

		/// <summary>
		/// Null if invalid.
		/// </summary>
		/// <returns></returns>
		public string StringValueViaCharacters {
			get {
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
		}

		protected int LengthViaCharacters { get { return (null == Word) ? 0 : Word.Count (); } }

		public ILxCharacter GetAtViaCharacters (int i)
		{
			return (!IsValid || i < 0 || i >= Length) ? null : Word [i];
		}

		protected void SetAtViaCharacters (int i, ILxCharacter value)
		{
			if (IsValid && (i >= 0 || i < Length) && value is LxCharacter) { 
				Word [i] = (LxCharacter)value; 
			}
		}

		public ulong NumericValueViaCharacters {
			get {
				ulong retval = 0;
				for (int i = 0; i < this.Word.Count; i++) {
					retval += this.Word [i].Index * Weight (i);
				}
				return retval;
			}
		}

		/// <summary>
		/// Increments in place, also returns the new value.
		/// </summary>
		/// <returns></returns>
		protected ILxWord IncrementAsCharacters ()
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

			return this;
		}

	}
}
