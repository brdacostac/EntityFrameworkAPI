// See https://aka.ms/new-console-template for more information
using ClientApi;
using DTOLol;

Console.WriteLine("Hello, World!");
ChampionHttpManager championHttpManager = new ChampionHttpManager(new HttpClient());
foreach(DTOChampion champ in await championHttpManager.GetItems(1,0))
{
    Console.WriteLine(champ.Name);
}
DTOChampion champ1 = await championHttpManager.GetItemByName("SQD");
Console.WriteLine(champ1.Name);
Console.ReadLine();