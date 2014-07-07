using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public enum LxElementOrder
	{
		MinorToMajor,
		MajorToMinor
	}

	public static class LxUtility
	{
		/// <summary>
		/// Check if the given string could be used as an alphabet.
		/// Basically, repetitions are not allowed.
		/// </summary>
		/// <param name="potentialAlphabet"></param>
		/// <returns></returns>
		public static bool IsValidAlphabet (string potentialAlphabet)
		{
			if (string.IsNullOrEmpty (potentialAlphabet)) {
				return false;
			}

			HashSet<char> characterInPotentialAlphabet = new HashSet<char> ();
			foreach (var ch in potentialAlphabet) {
				if (characterInPotentialAlphabet.Contains (ch)) {
					// Found repetition.
					return false;
				}
				characterInPotentialAlphabet.Add (ch);
			}

			return true;
		}

		/// <summary>
		/// Convert a "via characters" representation 
		/// to the "via alphabets" representation.
		/// </summary>
		public static LxWord WordRepresentationFromCharactersToAlphabets (LxWord word)
		{
			if (null == word || !word.IsValid) {
				return word;
			}

			int theLength = word.Length;
			word.AlphabetSequence = new List<string> (theLength);
			word.IndexSequence = new List<uint> (theLength);
			for (int i = 0; i < theLength; i++) {
				word.AlphabetSequence.Add ((word [i] as LxCharacter).Alphabet);
				word.IndexSequence.Add ((word [i] as LxCharacter).Index);
			}

			word.IsSetAsAlphabetSequences = true;
			return word;
		}

	}
}
