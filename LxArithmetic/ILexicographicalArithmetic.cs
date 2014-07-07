using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public interface ILxCharacter
	{
		/// <summary>
		/// The value of index should be within the range 
		/// defined by the size of alphabet.
		/// </summary>
		/// <param name="alphabet"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		ILxCharacter Create (string alphabet, int index);
		/// <summary>
		/// Valid if the combination of the alphabet and the index makes sense.
		/// Checks if the alphabet exists; does not check if the alphabet itself makes sense.
		/// </summary>
		bool IsValid { get; }
		/// <summary>
		/// Test if this character can be compared with the other.
		/// </summary>
		/// <param name="theOtherCharacter"></param>
		/// <returns></returns>
		bool IsCompatible (ILxCharacter theOtherCharacter);
		/// <summary>
		/// Null if cannot convert.
		/// </summary>
		/// <returns></returns>
		char? ToCharacter ();
		/// <summary>
		/// May roll over.
		/// </summary>
		/// <returns></returns>
		LxCharacter Increment ();
		/// <summary>
		/// As a result of the most recent increment.
		/// </summary>
		bool HasRolledOver { get; }
	}

	public interface ILxWordViaCharacters
	{
		/// <summary>
		/// Checks that it has been created and that
		/// all its constituent characters are valid.
		/// </summary>
		//bool IsValid { get; }
		/// <summary>
		/// Test if this word can be compared with the other.
		/// </summary>
		/// <param name="theOtherCharacter"></param>
		/// <returns></returns>
		//bool IsCompatible (ILxWordViaCharacters theOtherWord);
	}

	public interface ILxWordViaAlphabets
	{
		/// <summary>
		/// Test if this word can be compared with the other.
		/// </summary>
		/// <param name="theOtherCharacter"></param>
		/// <returns></returns>
		//bool IsCompatible (ILxWordViaAlphabets theOtherWord);
	}

	public interface ILxWord
	{
		// TODO - Most of the lx word interface should go here.
		// Only the creation-dependent parts should be left in
		// the implementation-specific interfaces.
		// TODO HERE
		// OR, perhaps, just place everything here?
		// Decided: PLACE EVERYTHING HERE.

		/// <summary>
		/// Expects characters ordered either from the most minor to the most major characters
		/// or the other way.
		/// </summary>
		/// <param name="enuc"></param>
		/// <returns></returns>
		ILxWord Create (LxElementOrder endian, IEnumerable<ILxCharacter> enuc);

		/// <summary>
		/// Expects characters ordered either from the most minor to the most major characters
		/// or the other way.
		/// </summary>
		/// <param name="enuc"></param>
		/// <returns></returns>
		ILxWord Create (LxElementOrder endian, IEnumerable<string> alphabetSequence, IEnumerable<uint> indexSeqence);

		/// <summary>
		/// Test if this word can be compared with the other.
		/// </summary>
		bool IsCompatible (ILxWord theOtherWord);

		/// <summary>
		/// Checks that it has been created and is internally consistent.
		/// </summary>
		bool IsValid { get; }

		/// <summary>
		/// The number of constituent characters.
		/// </summary>
		int Length { get; }

		/// <summary>
		/// Lists the of constituent alphabets which describe the possibel values of the word.
		/// </summary>
		IEnumerable<string> ListOfAlphabets ();

		/// <summary>
		/// Increment in place, also return the new value.
		/// No effect if the word is (effectively) empty.
		/// </summary>
		/// <returns></returns>
		ILxWord Increment ();

		/// <summary>
		/// Access the character at the given position
		/// (null if the position is out of range).
		/// TODO - need a public set, too, or not?
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ILxCharacter this [int position] { get;
		}
		/// <summary>
		/// Most major to most minor characters.
		/// </summary>
		/// <returns></returns>
		string StringValue { get; }
		/// <summary>
		/// The value is based on the index values of the constituent characters.
		/// </summary>
		/// <returns></returns>
		ulong NumericValue { get; }

	}

	public interface ILxSegment
	{
	}

	public interface ILxSegmentSet
	{
	}

	/*
	 * Alternative implementation of an Lx word.
	 * Or, rather, alternative way of looking at it
	 * (not through the explicit individual characters
	 * but as sequences of alphabets and indices).
	 */
	public interface ILxAlphabet
	{
		/// <summary>
		/// Maps an index to the character.
		/// </summary>
		string Alphabet { get; }

		// Likely extension:
		// Map a character to the index.

		/// <summary>
		/// TODO - needed?
		/// </summary>
		bool IsValid { get; }
	}

	public interface ILxAlphabetSequence
	{
		// TODO - create, get length, is valid, etc.
		// Perhaps ILxSequence?

		// is compatible, etc

		int Length { get; }
	}

	public interface ILxIndexSequence
	{
	}

}
