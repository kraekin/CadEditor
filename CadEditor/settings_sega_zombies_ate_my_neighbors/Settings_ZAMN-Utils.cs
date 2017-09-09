using CadEditor;
using System;
using System.Collections.Generic;

public static class ZamnUtils 
{
  public static int victimAddrToVictimNo(int addr)
  {
    var victimAddrs = new Dictionary<int,int> {
      { 0x11CF0 , 0x1},// Barbeque Guy
      { 0x11E44 , 0x2},// Baby
      { 0x11F7C , 0x3},// Guy jumping on a trampoline
      { 0x12164 , 0x4},// Soldier
      { 0x1236C , 0x5},// Dog
      { 0x12550 , 0x6},// Son of Dr Tongue
      { 0x12668 , 0x7},// Teacher
      { 0x128EE , 0x8},// Archeologist
      { 0x12748 , 0x9},// Guy in the pool
      { 0x129EC , 0xA},// Cheerleader
      { 0x12B7E , 0xB},// Tourists 
    };
    int victimCode = 0;
    victimAddrs.TryGetValue(addr, out victimCode);
    return victimCode;
  }
  
  
  public static int victimNoToVictimAddr(int victimNo)
  {
    var victimAddrs = new Dictionary<int,int> {
      { 0x1 ,    0x11CF0 },// Barbeque Guy
      { 0x2 ,    0x11E44 },// Baby
      { 0x3 ,    0x11F7C },// Guy jumping on a trampoline
      { 0x4 ,    0x12164 },// Soldier
      { 0x5 ,    0x1236C },// Dog
      { 0x6 ,    0x12550 },// Son of Dr Tongue
      { 0x7 ,    0x12668 },// Teacher
      { 0x8 ,    0x128EE },// Archeologist
      { 0x9 ,    0x12748 },// Guy in the pool
      { 0xA ,    0x129EC },// Cheerleader
      { 0xB ,    0x12B7E },// Tourists
    };
    int victimAddr = 0;
    victimAddrs.TryGetValue(victimNo, out victimAddr);
    return victimAddr;
  }
  
  public static int enemyAddrToEnemyNo(int addr)
  {
    var enemyAddrs = new Dictionary<int,int> {
      { 0x15296 , 0x1},// Zombie
      { 0x1540C , 0x2},// Fast Zombie
      { 0x15872 , 0x3},// Mummy
      { 0x16C86 , 0x4},// Clone
      { 0x16d6C , 0x5},// Fast Clone
      { 0x162BE , 0x6},// Martian 1 
      { 0x16346 , 0x7},// Martian 2
      { 0x1614E , 0x8},// Werewolf
      { 0x1861A , 0x9},// Чаки
      { 0x18B92 , 0xA},// Бегающий огонь
      { 0x19C5E , 0xB},// Синий муравей
      { 0x19E10,  0xC},// Синий муравей 2 
      { 0x174B6 , 0xD},// Регбист
      { 0x17A8E , 0xE},// Слизень
      { 0x1B1A8 , 0xF},// Гриб с ногами
      { 0x1CF32 , 0x10},// Подводный монстр
      { 0x1B698 , 0x11},// Тентакль
      { 0x227B8 , 0x12},// Паучок
    };
    int enemyCode = 0;
    enemyAddrs.TryGetValue(addr, out enemyCode);
    return enemyCode;
  }
  
  public static int enemyNoToEnemyAddr(int addr)
  {
    var enemyAddrs = new Dictionary<int,int> {
      {0x1,0x15296},// Zombie
      {0x2,0x1540C},// Fast Zombie
      {0x3,0x15872},// Mummy
      {0x4,0x16C86},// Clone
      {0x5,0x16d6C},// Fast Clone
      {0x6,0x162BE},// Martian 1 
      {0x7,0x16346},// Martian 2
      {0x8,0x1614E},// Werewolf
      {0x9,0x1861A},// Чаки
      {0xA,0x18B92},// Бегающий огонь
      {0xB,0x19C5E},// Синий муравей
      {0xC,0x19E10},// Синий муравей 2 
      {0xD,0x174B6},// Регбист
      {0xE,0x17A8E},// Слизень
      {0xF,0x1B1A8},// Гриб с ногами
      {0x10, 0x1CF32},// Подводный монстр
      {0x11, 0x1B698},// Тентакль
      {0x12, 0x227B8},// Паучок
    };
    int enemyAddr = 0;
    enemyAddrs.TryGetValue(addr, out enemyAddr);
    return enemyAddr;
  }
  
  public static List<ObjectList> getVictimsFromArray(byte[] romdata, int baseAddr, int objCount)
  {
    var objects = new List<ObjectRec>();
    for (int i = 0; i < objCount; i++)
    {
        int x           = Utils.readWord(romdata, baseAddr + i * 12 + 0);
        int y           = Utils.readWord(romdata, baseAddr + i * 12 + 2);
        int data        = Utils.readWord(romdata, baseAddr + i * 12 + 6);
        int victimAddr  = Utils.readInt(romdata, baseAddr + i * 12 + 8);
        int victimNo    = victimAddrToVictimNo(victimAddr);
        var dataDict = new Dictionary<string,int>();
        dataDict["no"] = data;
        var obj = new ObjectRec(victimNo, 0, 0, x/2, y/2, dataDict);
        objects.Add(obj);
    }
    return new List<ObjectList> { new ObjectList { objects = objects, name = "Objects" } };
  }

  public static bool setVictimsToArray(List<ObjectList> objLists, byte[] romdata, int baseAddr, int objCount)
  {
    var objects = objLists[0].objects;
    for (int i = 0; i < objects.Count; i++)
    {
        var obj = objects[i];
        int victimAddr = victimNoToVictimAddr(obj.type);
        Utils.writeWord(romdata, baseAddr + i * 12 + 0, obj.x*2);
        Utils.writeWord(romdata, baseAddr + i * 12 + 2, obj.y*2);
        Utils.writeWord(romdata, baseAddr + i * 12 + 4, 0);
        Utils.writeWord(romdata, baseAddr + i * 12 + 6, obj.additionalData["no"]);
        Utils.writeInt (romdata, baseAddr + i * 12 + 8, victimAddr);
    }
    for (int i = objects.Count; i < objCount; i++)
    {
        Utils.writeWord(romdata, baseAddr + i * 12 + 0, 0);
        Utils.writeWord(romdata, baseAddr + i * 12 + 2, 0);
        Utils.writeWord(romdata, baseAddr + i * 12 + 4, 0);
        Utils.writeWord(romdata, baseAddr + i * 12 + 6, 0);
        Utils.writeInt (romdata, baseAddr + i * 12 + 8, 0);
    }
    return true;
  }
  
  public static List<ObjectList> getEnemiesFromArray(byte[] romdata, int baseAddr, int objCount)
  {
    var objects = new List<ObjectRec>();
    const int ENEMY_REC_LEN = 12;
    for (int i = 0; i < objCount; i++)
    {
        int r           = Utils.readWord(romdata, baseAddr + i * ENEMY_REC_LEN + 0);
        int x           = Utils.readWord(romdata, baseAddr + i * ENEMY_REC_LEN + 2);
        int y           = Utils.readWord(romdata, baseAddr + i * ENEMY_REC_LEN + 4);
        int t           = Utils.readWord(romdata, baseAddr + i * ENEMY_REC_LEN + 6);
        int enemyAddr   = Utils.readInt (romdata, baseAddr + i * ENEMY_REC_LEN + 8);
        int victimNo    = enemyAddrToEnemyNo(enemyAddr);
        var dataDict = new Dictionary<string,int>();
        dataDict["R"] = r;
        dataDict["T"] = t;
        var obj = new ObjectRec(victimNo, 0, 0, x/2, y/2, dataDict);
        objects.Add(obj);
    }
    return new List<ObjectList> { new ObjectList { objects = objects, name = "Objects" } };;
  }
  
  public static bool setEnemiesToArray(List<ObjectList> objLists, byte[] romdata, int baseAddr, int objCount)
  {
    const int ENEMY_REC_LEN = 12;
    var objects = objLists[0].objects;
    for (int i = 0; i < objects.Count; i++)
    {
        var obj = objects[i];
        int enemyAddr = enemyNoToEnemyAddr(obj.type);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 0, obj.additionalData["R"]);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 2, obj.x*2);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 4, obj.y*2);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 6, obj.additionalData["T"]);
        Utils.writeInt (romdata, baseAddr + i * ENEMY_REC_LEN + 8, enemyAddr);
    }
    for (int i = objects.Count; i < objCount; i++)
    {
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 0, 0x00);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 2, 0x00);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 4, 0x00);
        Utils.writeWord(romdata, baseAddr + i * ENEMY_REC_LEN + 6, 0x00);
        Utils.writeInt (romdata, baseAddr + i * ENEMY_REC_LEN + 8, 0);
    }
    return true;
  }
  
  public static List<ObjectList> getItemsFromArray(byte[] romdata, int baseAddr, int objCount)
  {
    var objects = new List<ObjectRec>();
    const int ITEM_REC_LEN = 6;
    for (int i = 0; i < objCount; i++)
    {
        int x           = Utils.readWord(romdata, baseAddr + i * ITEM_REC_LEN + 0);
        int y           = Utils.readWord(romdata, baseAddr + i * ITEM_REC_LEN + 2);
        int t           = Utils.readWord(romdata, baseAddr + i * ITEM_REC_LEN + 4);
        var obj = new ObjectRec(t, 0, 0, x/2, y/2);
        objects.Add(obj);
    }
    return new List<ObjectList> { new ObjectList { objects = objects, name = "Objects" } };
  }
  
  public static bool setItemsToArray(List<ObjectList> objLists, byte[] romdata, int baseAddr, int objCount)
  {
    var objects = objLists[0].objects;
    const int ITEM_REC_LEN = 6;
    for (int i = 0; i < objects.Count; i++)
    {
        var obj = objects[i];
        Utils.writeWord(romdata, baseAddr + i * ITEM_REC_LEN + 0, obj.x*2);
        Utils.writeWord(romdata, baseAddr + i * ITEM_REC_LEN + 2, obj.y*2);
        Utils.writeWord(romdata, baseAddr + i * ITEM_REC_LEN + 4, obj.type);
    }
    for (int i = objects.Count; i < objCount; i++)
    {
        Utils.writeWord(romdata, baseAddr + i * ITEM_REC_LEN + 0, 0x00);
        Utils.writeWord(romdata, baseAddr + i * ITEM_REC_LEN + 2, 0x00);
        Utils.writeWord(romdata, baseAddr + i * ITEM_REC_LEN + 4, 0x00);
    }
    return true;
  }
  
  //-----------------------------------------------------------------------------------------------
  public static List<ObjectList> getVictimsFromRom(int levelNo)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = lr.objectsBeginAddr;
    int objCount = lr.objCount;
    return getVictimsFromArray(Globals.romdata, baseAddr, objCount);
  }
  
  public static List<ObjectList> getVictimsFromFile(int levelNo)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    byte[] data = Utils.loadDataFromFile("victims.bin");
    return getVictimsFromArray(data, 0, lr.objCount);
  }
  
  public static bool setVictimsToRom(int levelNo, List<ObjectList> objects)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = lr.objectsBeginAddr;
    int objCount = lr.objCount;
    return setVictimsToArray(objects, Globals.romdata, baseAddr, objCount);
  }
  
  public static bool setVictimsToFile(int levelNo, List<ObjectList> objects)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = 0;
    int objCount = lr.objCount;
    byte[] data = new byte[objCount*12];
    setVictimsToArray(objects, data, baseAddr, objCount);
    Utils.saveDataToFile("victims.bin", data);
    return true;
  }
  
  public static List<ObjectList> getEnemiesFromRom(int levelNo)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = lr.objectsBeginAddr;
    int objCount = lr.objCount;
    return getEnemiesFromArray(Globals.romdata, baseAddr, objCount);
  }
  
  public static List<ObjectList> getEnemiesFromFile(int levelNo)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    byte[] data = Utils.loadDataFromFile("enemies.bin");
    return getEnemiesFromArray(data, 0, lr.objCount);
  }
  
  public static bool setEnemiesToRom(int levelNo, List<ObjectList> objects)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = lr.objectsBeginAddr;
    int objCount = lr.objCount;
    return setEnemiesToArray(objects, Globals.romdata, baseAddr, objCount);
  }
  
  public static bool setEnemiesToFile(int levelNo, List<ObjectList> objects)
  {
    const int ENEMY_REC_LEN = 12;
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = 0;
    int objCount = lr.objCount;
    byte[] data = new byte[objCount * ENEMY_REC_LEN];
    setEnemiesToArray(objects, data, baseAddr, objCount);
    Utils.saveDataToFile("enemies.bin", data);
    return true;
  }
  
  public static List<ObjectList> getItemsFromRom(int levelNo)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = lr.objectsBeginAddr;
    int objCount = lr.objCount;
    return getItemsFromArray(Globals.romdata, baseAddr, objCount);
  }
  
  public static List<ObjectList> getItemsFromFile(int levelNo)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    byte[] data = Utils.loadDataFromFile("items.bin");
    return getItemsFromArray(data, 0, lr.objCount);
  }
  
  public static bool setItemsToRom(int levelNo, List<ObjectList> objects)
  {
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = lr.objectsBeginAddr;
    int objCount = lr.objCount;
    return setItemsToArray(objects, Globals.romdata, baseAddr, objCount);
  }
  
  public static bool setItemsToFile(int levelNo, List<ObjectList> objects)
  {
    const int ITEMS_REC_LEN = 6;
    LevelRec lr = ConfigScript.getLevelRec(levelNo);
    int baseAddr = 0;
    int objCount = lr.objCount;
    byte[] data = new byte[objCount * ITEMS_REC_LEN];
    setItemsToArray(objects, data, baseAddr, objCount);
    Utils.saveDataToFile("items.bin", data);
    return true;
  }
}
