
using BiblioMilieu;
using Entity_framework;
using StubLib;

StubData stub = new StubData();
int nbChampions = await stub.ChampionsMgr.GetNbItems();
var championListModel = await stub.ChampionsMgr.GetItems(0, nbChampions);
var championList = championListModel.ToList().Select(e => e.ToDb());

int nbSkins = await stub.SkinsMgr.GetNbItems();
var skinListModel = await stub.SkinsMgr.GetItems(0, nbSkins);
var skinList = skinListModel.ToList().Select(e => e.ToDb());


int nbRunes = await stub.RunesMgr.GetNbItems();
var runesListModel = await stub.RunesMgr.GetItems(0, nbRunes);
var runesList = runesListModel.ToList().Select(e => e.ToDb());

using (var context = new ChampionsDbContexte())
{
    context.Database.EnsureCreated();
    // Crée des champions et les insère dans la base
    Console.WriteLine("Creates and inserts new champions");

    championList.ToList().ForEach(champion => context.ChampionsSet.Add(champion));
    context.SaveChanges();
    championList= context.ChampionsSet;
    var listSkinNew = new List<SkinDB>();
    foreach(SkinDB skin in skinList)
    {
        var champ = championList.FirstOrDefault(e => e.Name == skin.Champion.Name);
        skin.Champion = champ;
        listSkinNew.Add(skin);

        Console.WriteLine($" Add skin { champ.Id} - {champ.Name} - { skin.ChampionForeignKey}");
    }

    foreach (SkinDB skin in listSkinNew)
    {
        Console.WriteLine($" Result skin { skin.Champion.Name} - {skin.Name} - { skin.ChampionForeignKey}");
    }


    listSkinNew.ToList().ForEach(skin => context.SkinsSet.Add(skin));
    //runesList.ToList().ForEach(runes => context.RunesSet.Add(runes));
    context.SaveChanges();

   
}

using (var context = new ChampionsDbContexte())
{
    foreach (var n in context.ChampionsSet)
    {
        //Console.WriteLine($"{n.Id} - {n.Name}");
    }
}