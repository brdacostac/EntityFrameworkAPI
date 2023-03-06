using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework.Enums
{

	[Flags]
	public enum SkillTypeSkillDb : int
	{
		Unknown =0,
		Basic =1,
		Passive =2,
		Ultimate =3,
	}
}
