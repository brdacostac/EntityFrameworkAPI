using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace LolApp.ViewModels
{
	[ObservableObject]
	public partial class ChampionClassVM
	{
		[ObservableProperty]
		private ChampionClass model;

		[ObservableProperty]
		private bool isSelected;

		public ChampionClassVM(ChampionClass model)
		{
			Model = model;
		}

		public static IEnumerable<ChampionClassVM> Classes { get; }
			= Enum.GetValues(typeof(ChampionClass)).Cast<ChampionClass>().Except(new ChampionClass[] {ChampionClass.Unknown})
				.Select(cc => new ChampionClassVM(cc));
	}
}

