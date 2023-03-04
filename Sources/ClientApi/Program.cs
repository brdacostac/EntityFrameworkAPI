// See https://aka.ms/new-console-template for more information
using ClientApi;
using DTOLol;
using Model;

Console.WriteLine("Hello, World!");
ChampionHttpManager championHttpManager = new ChampionHttpManager(new HttpClient());

var champList = await championHttpManager.GetItems(1, 0);
foreach(Champion champ in champList)
{
    Console.WriteLine(champ.Name);
}
Champion champ1 = await championHttpManager.GetItemByName("Akali");
champ1.Image = new LargeImage("qsd");
champ1.Bio = "qsd";

await championHttpManager.DeleteItem(champ1);
await championHttpManager.AddItem(champ1);
//DTOChampion champ2 = await championHttpManager.GetItemByName("Akali");

Console.WriteLine(champ1.Name);

Console.ReadLine();