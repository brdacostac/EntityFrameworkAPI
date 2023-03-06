using System;
namespace Entity_framework
{
	[Flags]
	public enum ClassChampionDb : int
	{
		Unknown = 0,
		Assassin = 1,
		Fighter = 2,
		Mage = 3,
		Marksman = 4,
		Support = 5,
		Tank = 6,
	}
}

