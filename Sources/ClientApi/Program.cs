// See https://aka.ms/new-console-template for more information

using ClientApi;
using DTOLol;

Console.WriteLine("Hello, World!");
ChampionHttpManager championHttpManager = new ChampionHttpManager(new HttpClient());
foreach(DTOChampion champ in await championHttpManager.GetItems(1,0))
{
    Console.Out.WriteLine(champ.Name);
}