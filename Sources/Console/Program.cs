
using BiblioMilieu;
using BiblioMilieu.Mapper.EnumsMapper;
using Entity_framework;
using Entity_framework.DataBase;
using Model;
using StubLib;

StubData stub = new StubData();

int nbChampions = await stub.ChampionsMgr.GetNbItems();
var championListModel = await stub.ChampionsMgr.GetItems(0, nbChampions);
var championList = championListModel.ToList().Select(e => e.ToDb());

int nbSkins = await stub.SkinsMgr.GetNbItems();
var skinListModel = await stub.SkinsMgr.GetItems(0, nbSkins);
var skinList = skinListModel.ToList().Select(e => e.ToDb());
int nbRunePage = await stub.RunePagesMgr.GetNbItems();
var runePageList = await stub.RunePagesMgr.GetItems(0, nbRunePage);


int nbRunes = await stub.RunesMgr.GetNbItems();
var runesListModel = await stub.RunesMgr.GetItems(0, nbRunes);
var runesList = runesListModel.ToList().Select(e => e.ToDb());
var runesPagesList = runesListModel.ToList().Select(e => e.ToDb());

using (var context = new EntityDbContexte())
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
    runesList.ToList().ForEach(rune => context.RunesSet.Add(rune));
    context.SaveChanges();
    runesList= context.RunesSet;
    foreach (RunePage runePages in runePageList)
    {

        foreach ( var couple in runePages.Runes)
        {
            CategoryDicDB categoryDicDB = new CategoryDicDB();
            categoryDicDB.runePage = runePages.ToDb();
            categoryDicDB.category=couple.Key.ToDb();
            categoryDicDB.rune = runesList.FirstOrDefault(runeDb => runeDb.Name == couple.Value.Name);
            context.CategoryRunePageSet.Add(categoryDicDB);
        }


    }

    /*runePageList.ToList().ForEach(runePage => context.CategoryRunePageSet.);
    foreach (RunePagesDb runePage in runesList)*/

    /* foreach (SkinDB skin in listSkinNew)
     {
         Console.WriteLine($" Result skin { skin.Champion.Name} - {skin.Name} - { skin.ChampionForeignKey}");
     }*/



    listSkinNew.ToList().ForEach(skin => context.SkinsSet.Add(skin));


    //runesList.ToList().ForEach(runes => context.RunesSet.Add(runes));
    context.SaveChanges();

   
}

using (var context = new EntityDbContexte())
{
    foreach (var n in context.ChampionsSet)
    {
       Console.WriteLine($"{n.Id} - {n.Name}");
    }
}