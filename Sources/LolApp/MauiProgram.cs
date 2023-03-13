using CommunityToolkit.Maui;
using LolApp.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Model;
using StubLib;
using ViewModels;

namespace LolApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
			});
		builder.Services.AddSingleton<IDataManager, StubData>()
						.AddSingleton<ChampionsMgrVM>()
						.AddSingleton<SkinsMgrVM>()
						.AddSingleton<ApplicationVM>()
						.AddSingleton<ChampionsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

