using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public static class LxWordOrder
	{
		/// <summary>
		/// Return a negative, 0, or positive value if
		/// x&lt;y, x=y, x>y respectively.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static int Compare(LxWord x, LxWord y)
		{
			if (x == null || y == null) { // TODO internal error?
				return 0;
			}

			if (!x.IsCompatible(y)) {
				throw new LxArithmeticException("Incompatible comparison");
			}

			if (x.Length > y.Length) { return -1; }
			if (y.Length > x.Length) { return 1; }

			for (int i = x.Length - 1; i >= 0; i--) {
				var xi = x.Word[i].Index;
				var yi = y.Word[i].Index;
				if (xi > yi) { return 1; }
				if (xi < yi) { return -1; }
			}
			return 0;
		}

		/// <summary>
		/// Number of increments required to reach y from x.
		/// TODO - decide what to do about a word rollover/exhaustion.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static long Distance(LxWord x, LxWord y)
		{
			if (null == x || null == y) {
				throw new LxArithmeticException("Attempted a distance on a null");
			}
			if (!x.IsCompatible(y)) {
				throw new LxArithmeticException("Incompatible distance calculation");
			}

			// TODO - Different lengths

			// Subtract x from y, basically.
			List<long> subtractions = new List<long>(x.Length);
			// Here we assume x and y are of the same length; TODO general case.
			// Note - since ListNumericValue allows negative digit values,
			// there is no need to account for carryover.
			for (int i = 0; i < x.Length; i++) {
				subtractions.Add(y.Word[i].Index - x.Word[i].Index);
			}

			long retval;
			retval = ListNumericValue(subtractions, x.CalculateWeights());

			// If retval < 0, this implies x > y. 
			// TODO decide what to do in this case: roll over?

			return retval;
		}

		#region Helpers - operations with numeric lists

		/// <summary>
		/// First list represents values of individual digits.
		/// Second list is the multiplicative factor
		/// which applies to the digit value
		/// at the same position.
		/// Note that digits may be negative.
		/// <para></para>
		/// </summary>
		/// <param name="digits"></param>
		/// <param name="weights"></param>
		/// <returns></returns>
		/// <remarks>This is an internal helper function, so some shortcuts are taken.
		/// TODO: make it more robust.</remarks>
		/// <note>Can have more weights than digits,
		/// but not the other way around.</note>
		private static long ListNumericValue(List<long> digits, ulong[] weights)
		{
			if (digits.Count > weights.Length) { // internal error
				return 0;
			}

			long retval = 0L;
			for (int i = 0; i < digits.Count; i++) {
				retval += digits[i] * (long) weights[i];
			}
			return retval;
		}

		#endregion

	}

}
