// See https://aka.ms/new-console-template for more information
using ClientApi;
using DTOLol;

Console.WriteLine("Hello, World!");
ChampionHttpManager championHttpManager = new ChampionHttpManager(new HttpClient());
foreach(DTOChampion champ in await championHttpManager.GetItems(1,0))
{
    Console.WriteLine(champ.Name);
}
DTOChampion champ1 = await championHttpManager.GetItemByName("Akali");
champ1.Image = "qsd";
champ1.Bio = "qsd";
champ1.skins = new List<DTOSkin> { new DTOSkin() };

await championHttpManager.DeleteItem(champ1);
await championHttpManager.AddItem(champ1);
//DTOChampion champ2 = await championHttpManager.GetItemByName("Akali");

Console.WriteLine(champ1.Name);

Console.ReadLine();