using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModels
{
	[ObservableObject]
	public partial class SkillVM
	{
		[ObservableProperty]
		private Skill model;

		public SkillVM(Skill model)
		{
			Model = model;
		}

		public string Name => Model.Name;

		public SkillType Type => Model.Type;

		public string Description
		{
			get => Model.Description;
			set
			{
				SetProperty(Model.Description, value, newValue => Model.Description = newValue);
			}
		}
	}
}

