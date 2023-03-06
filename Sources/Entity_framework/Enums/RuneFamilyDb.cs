using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework.Enums
{
	[Flags]
	public enum RuneFamilyDb : int
	{
		Unknown = 0,
		Precision = 1,
		Domination = 2
	}
}
