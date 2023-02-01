// See https://aka.ms/new-console-template for more information

using ClientApi;
using DTOLol;

Console.WriteLine("Hello, World!");
ChampionHttpManager championHttpManager = new ChampionHttpManager(new HttpClient());
foreach(DTOChampion champ in await championHttpManager.GetChampions())
{
    Console.WriteLine(champ.Name);
}