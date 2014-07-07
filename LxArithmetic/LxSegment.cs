using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto.LexicographicalArithmetic
{
	public class LxSegment : ILxSegment
	{
		// Data.
		public LxWord Begin { get; set; }
		public LxWord End { get; set; }

		public ILxSegment Create(ILxWord/*ViaAlphabets*/ begin, ILxWord/*ViaAlphabets*/ end)
		{
			if (!begin.IsCompatible(end)) {
				throw new LxArithmeticException("Attempt to create a segment from incompatible words");
			}
			// TODO HERE
			return this;
		}

		public ulong Size { get {
			// TODO
			return 0uL;
		} }

		public List<ILxWordViaAlphabets> GetAll()
		{
			// TODO
			return null;
		}

		public List<string> GetAllAsStrings()
		{
			// TODO
			return null;
		}
	}
}
