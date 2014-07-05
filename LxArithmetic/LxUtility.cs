using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public enum LxElementOrder
	{
		MinorToMajor, MajorToMinor
	}

	public static class LxUtility
	{
		/// <summary>
		/// Check if the given string could be used as an alphabet.
		/// Basically, repetitions are not allowed.
		/// </summary>
		/// <param name="potentialAlphabet"></param>
		/// <returns></returns>
		public static bool IsValidAlphabet(string potentialAlphabet)
		{
			throw new NotImplementedException("In progress");
		}
	}
}
