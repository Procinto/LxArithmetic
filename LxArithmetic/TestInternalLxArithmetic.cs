using Procinto.LexicographicalArithmetic;

using NUnit.Framework;

namespace Procinto.LexicographicalArithmetic.Test
{
	/// <summary>
	/// We test a particular implementation, Lx*,
	/// as part of the same assembly.
	/// Testing parts that are not API.
	/// Note: API is tested separately, in code which is
	/// outside of this assembly.
	/// </summary>
	[TestFixture()]
	public class TestInternalLexicographicalArithmetic
	{
		[Test]
		public static void TestLxUtility_WordRepresentationFromCharactersToAlphabets()
		{
			LxWord w = new LxWord(LxElementOrder.MajorToMinor,
			                      new LxCharacter("cba", 1),
			                      new LxCharacter("321", 2));
			Assert.AreEqual("b1", w.StringValue);
			Assert.IsTrue(w.IsSetAsListOfCharacters);
			Assert.IsFalse(w.IsSetAsAlphabetSequences);

			LxWord w_same = LxUtility.WordRepresentationFromCharactersToAlphabets(w);
			Assert.AreEqual("b1", w_same.StringValue);
			Assert.AreSame(w, w_same);

			Assert.IsTrue(w.IsSetAsAlphabetSequences);
			Assert.IsTrue(w.IsSetAsListOfCharacters);

		}

//		[Test]
//		public static void TestLxWord_EnsureHasAlphabets()
//		{
//			// 1) Created from alphabets.
//			//LxWord w_abc = new LxWord(LxElementOrder.MajorToMinor,
//
//
//			// 2) Created from characters.
//			LxWord w_char = new LxWord(LxElementOrder.MajorToMinor,
//			                      new LxCharacter("cba", 1),
//			                      new LxCharacter("321", 2));
//		}
	
	}
}
