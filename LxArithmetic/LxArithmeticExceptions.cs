using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procinto // Or should it be Procinto.LexicographicalArithmetic instead?
{
	/// <summary>
	/// An all-encompassing exception for operations on "lx" objects.
	/// </summary>
	public class LxArithmeticException : Exception
	{
		public LxArithmeticException() { }
		public LxArithmeticException(string message) : base(message) { }
		public LxArithmeticException(string message, Exception inner) : base(message, inner) { }
	}
}
