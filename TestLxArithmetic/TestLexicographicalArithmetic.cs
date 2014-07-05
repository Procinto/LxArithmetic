using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

using Procinto.LexicographicalArithmetic;
using Procinto;

using NUnit.Framework;

namespace TestLxArithmetic
{
	/// <summary>
	/// We test a particular implementation, Lx*.
	/// </summary>
	[TestFixture()]
	public class TestLexicographicalArithmetic
	{
		string alphabetABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		class PoorImplementation : ILxCharacter
		{
			#region ILxCharacter Members

			public ILxCharacter Create(string alphabet, int index)
			{
				return this;
			}

			public bool IsValid
			{
				get { throw new NotImplementedException(); }
			}

			public bool IsCompatible(ILxCharacter theOtherCharacter)
			{
				throw new NotImplementedException();
			}

			public char? ToCharacter()
			{
				throw new NotImplementedException();
			}

			public LxCharacter Increment()
			{
				throw new NotImplementedException();
			}

			public bool HasRolledOver
			{
				get { throw new NotImplementedException(); }
			}

			#endregion
		}

		[Test]
		public void TestLxCharacter()
		{
			ILxCharacter c = null;

			// Test invalid input.
			c = new LxCharacter(alphabetABC, -1);
			Assert.IsNotNull(c);
			Assert.IsFalse(c.IsValid);

			c = new LxCharacter(null, 0);
			Assert.IsNotNull(c);
			Assert.IsFalse(c.IsValid);

			c = new LxCharacter(alphabetABC, alphabetABC.Count());
			Assert.IsNotNull(c);
			Assert.IsFalse(c.IsValid);

			c = new LxCharacter("", 0);
			Assert.IsNotNull(c);
			Assert.IsFalse(c.IsValid);

			// Test conversion to character.
			c = new LxCharacter(alphabetABC, 0);
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('A', c.ToCharacter());

			// Test increment.
			c.Increment();
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('B', c.ToCharacter());
			Assert.IsFalse(c.HasRolledOver);

			// Test roll over.
			c = new LxCharacter("ab", 1);
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('b', c.ToCharacter());

			c.Increment();
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('a', c.ToCharacter());
			Assert.IsTrue(c.HasRolledOver);

			c.Increment();
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('b', c.ToCharacter());
			Assert.IsFalse(c.HasRolledOver);

			c.Increment();
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('a', c.ToCharacter());
			Assert.IsTrue(c.HasRolledOver);

			// Test roll over "in place".
			c = new LxCharacter("_", 0);
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('_', c.ToCharacter());
			Assert.IsFalse(c.HasRolledOver); // no increment => no roll over

			c.Increment();
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('_', c.ToCharacter());
			Assert.IsTrue(c.HasRolledOver);

			c.Increment();
			Assert.IsNotNull(c);
			Assert.IsTrue(c.IsValid);
			Assert.AreEqual('_', c.ToCharacter());
			Assert.IsTrue(c.HasRolledOver);

			// Test IsCompatible
			c = new LxCharacter(alphabetABC, 4);

			Assert.IsTrue(c.IsCompatible(c));

			LxCharacter aCharacter = new LxCharacter();
			Assert.IsFalse(c.IsCompatible(aCharacter));

			aCharacter.Create(alphabetABC, 3);
			Assert.IsTrue(c.IsCompatible(aCharacter));

			aCharacter.Create(alphabetABC, -2);
			Assert.IsTrue(c.IsCompatible(aCharacter));

			ILxCharacter notACharacter = new PoorImplementation().Create(alphabetABC, 0);
			Assert.IsFalse(c.IsCompatible(notACharacter));
		}

		[Test]
		public void TestLxWord()
		{
			ILxWord w = null;
			List<ILxCharacter> enuc = null;

			// Create - edge cases.
			w = new LxWord(LxElementOrder.MajorToMinor, null);
			Assert.IsNotNull(w);
			Assert.IsFalse(w.IsValid);
			Assert.AreEqual(0, w.Length);

			enuc = new List<ILxCharacter>();
			w = new LxWord(LxElementOrder.MajorToMinor, enuc);
			Assert.IsNotNull(w);
			Assert.AreEqual(0, w.Length);
			try {
				w.Increment();
			}
			catch (Exception ex) {
				Assert.Fail("Incrementing an empty word threw an exception: " + ex);
			}
			
			// Create - normal case, with a list.
			enuc = new List<ILxCharacter>(2);
			enuc.Add(new LxCharacter("ab", 0)); // major
			enuc.Add(new LxCharacter("012", 1)); // minor
			w = new LxWord(LxElementOrder.MajorToMinor, enuc);
			Assert.IsNotNull(w);
			Assert.AreEqual(2, w.Length);

			Assert.AreEqual("a1", w.StringValue());
			Assert.AreEqual(1uL, w.NumericValue());
			
			// Increment without carryover.
			w.Increment();
			Assert.AreEqual("a2", w.StringValue());
			Assert.AreEqual(2uL, w.NumericValue());

			// Increment with carryover.
			w.Increment();
			Assert.AreEqual("b0", w.StringValue());
			Assert.AreEqual(3uL, w.NumericValue());

			w.Increment();
			Assert.AreEqual("b1", w.StringValue());
			Assert.AreEqual(4uL, w.NumericValue());

			// Create - normal case, with a parameter sequence.
			w = new LxWord(LxElementOrder.MinorToMajor, 
				new LxCharacter("012", 1), new LxCharacter("ab", 0));
			Assert.AreEqual("a1", w.StringValue());

			// Increment - exhausted word space.
			// TODO - need to decide what to do on exhaustion.
			// Options: 
			// - (silent) rollover; 
			// - (silent?) stuck value;
			// - word extended
			//   - how? Which alphabets; finite or infinite;
			// - error.

		}

		[Test]
		public void TestLxWordOrder_Compare()
		{
			// Equal lengths. Compatible words.

			var enuc = new List<ILxCharacter>();
			enuc.Add(new LxCharacter("ab", 0));
			enuc.Add(new LxCharacter("012", 2));
			LxWord w1 = new LxWord(LxElementOrder.MajorToMinor, enuc) as LxWord;
			Assert.AreEqual("a2", w1.StringValue());

			enuc = new List<ILxCharacter>();
			enuc.Add(new LxCharacter("ab",1));
			enuc.Add(new LxCharacter("012", 0));
			LxWord w2 = new LxWord(LxElementOrder.MajorToMinor, enuc) as LxWord;
			Assert.AreEqual("b0", w2.StringValue());

			Assert.IsTrue(w1.IsCompatible(w2));

			Assert.AreEqual(-1, LxWordOrder.Compare(w1, w2));
			Assert.AreEqual(0, LxWordOrder.Compare(w1, w1));
			Assert.AreEqual(1, LxWordOrder.Compare(w2, w1));

			// Equal lengths. Incompatible words.

			LxWord w3 = new LxWord(LxElementOrder.MinorToMajor, 
				new LxCharacter("ab", 1), 
				new LxCharacter("ab", 0));
			Assert.AreEqual("ab", w3.StringValue());

			bool wasThrownCorrectly = false;
			try {
				LxWordOrder.Compare(w1, w3);
			}
			catch (LxArithmeticException) {
				// Correct exception thrown.
				wasThrownCorrectly = true;
			}
			catch (Exception ex) {
				Assert.Fail("Incorrect exception on a comparison of incompatible words: " + ex);
			}
			finally {
				if (!wasThrownCorrectly) {
					Assert.Fail("Exception not thrown on a comparison of incompatible words");
				}
			}

			// TODO - different lengths.
			// What's the requirements?
		}

		[Test]
		public void TestLxWordOrder_Distance()
		{
			LxWord w_b0 = new LxWord(LxElementOrder.MinorToMajor, 
				new LxCharacter("01", 0),
				new LxCharacter("abc", 1));
			Assert.AreEqual("b0", w_b0.StringValue());
			LxWord w_c0 = new LxWord(LxElementOrder.MinorToMajor, 
				new LxCharacter("01", 0),
				new LxCharacter("abc", 2));
			Assert.AreEqual("c0", w_c0.StringValue());
			LxWord w_b0_too = new LxWord(LxElementOrder.MajorToMinor,
				new LxCharacter("abc", 1),
				new LxCharacter("01", 0));
			Assert.AreEqual("b0", w_b0_too.StringValue());
			LxWord w_c0_butDifferent = new LxWord(LxElementOrder.MajorToMinor,
				new LxCharacter("abc", 2),
				new LxCharacter("012", 0));
			Assert.AreEqual("c0", w_c0_butDifferent.StringValue());

			Assert.AreEqual(2L, LxWordOrder.Distance(w_b0, w_c0));
			Assert.AreEqual(0L, LxWordOrder.Distance(w_b0, w_b0_too));
			
			bool wasThrownCorrectly = false;
			try {
				Assert.AreNotEqual(0L, LxWordOrder.Distance(w_c0, w_c0_butDifferent));
			}
			catch (LxArithmeticException) {
				wasThrownCorrectly = true;
			}
			catch (Exception ex) {
				Assert.Fail("Incorrect exception on a comparison of incompatible words: " + ex);
			}
			finally {
				if (!wasThrownCorrectly) {
					Assert.Fail("Exception not thrown on a comparison of incompatible words");
				}
			}

		}

	}
}
