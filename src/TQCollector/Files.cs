using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using TQVaultAE.Data.Dto;
using Newtonsoft.Json;
using System;

namespace TQCollector
{
    static class Files
    {
        private static XmlSerializer XMLConfigSerializer;
        private static XmlSerializer XMLItemDBSerializer;
        private static XmlSerializer XMLLanguageSerializer;
        private static Config XMLConfig;
        private static ItemDB XMLItemDB;
        private static OrderedTable XMLText;

        //This function cannot be translated because it loads the language.
        public static bool LoadLanguage()
        {
            StreamReader str = null;
            try
            {
                if (File.Exists("text_" + Files.Configuration.Language + ".xml"))
                {
                    str = new StreamReader("text_" + Files.Configuration.Language + ".xml");
                    XMLLanguageSerializer = new XmlSerializer(typeof(OrderedTable));
                    XMLText = (OrderedTable)XMLLanguageSerializer.Deserialize(str);
                    return true;
                }
                else
                {
                    MessageBox.Show("text_" + Files.Configuration.Language + ".xml was not found.");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("An error with text_" + Files.Configuration.Language + ".xml, has it been illegaly modified?");
                return false;
            }
            finally
            {
                if (str != null) str.Close();
            }
        }

        public static OrderedTable Language
        {
            set { XMLText = value; }
            get { return XMLText; }
        }

        //Combine Set[]s because we don't care about difficulty
        public static Set[] Combine(Set[] set1, Set[] set2)
        {
            Set[] retSet = (Set[])set1.Clone();
            for (int i = 0; i < set1.Length; i++)
            {
                for (int j = 0; j < set1[i].Item.Length; j++)
                {
                    retSet[i].Item[j].count = set1[i].Item[j].count + set2[i].Item[j].count;
                    retSet[i].Item[j].locations.AddRange(set2[i].Item[j].locations);
                }
            }
            return retSet;
        }

        //Hades relics are unique in the fact that the whole named set doesn't get shown.
        //This function makes sure to remove the Set so an empty 'Hades' category doesn't show up.
        public static Set[] removeHades(Set[] relics)
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in relics)
            {
                if (!s.name.Equals(Files.Language["xtagMHades"])) ls.Add(s);
            }

            Set[] ret = new Set[ls.Count];

            for (int i = 0; i < ls.Count; i++)
            {
                ret[i] = ls[i];
            }

            return ret;
        }

        public static int Count(Set s)
        {
            int i = 0;
            foreach (Item x in s.Item)
            {
                if (x.count > 0) i++;
            }

            return i;
        }

        public static int Count(Set[] s)
        {
            int i = 0;
            foreach (Set ss in s)
            {
                foreach (Item x in ss.Item)
                {
                    if (x.count > 0) i++;
                }
            }

            return i;
        }

        public static int Total(Set[] s)
        {
            int i = 0;
            foreach (Set x in s)
            {
                i += x.Item.Length;
            }

            return i;
        }

        public static Set[] Add(Set[] set1, Set[] set2)
        {
            //This function only works on uniques
            Set[] retSet = new Set[14];

            List<Item> amulet = new List<Item>();
            List<Item> bracer = new List<Item>();
            List<Item> armor = new List<Item>();
            List<Item> greaves = new List<Item>();
            List<Item> helm = new List<Item>();
            List<Item> ring = new List<Item>();
            List<Item> shield = new List<Item>();
            List<Item> axe = new List<Item>();
            List<Item> bow = new List<Item>();
            List<Item> club = new List<Item>();
            List<Item> spear = new List<Item>();
            List<Item> staff = new List<Item>();
            List<Item> sword = new List<Item>();
            List<Item> ohranged = new List<Item>();

            //Find out what Set it should be inserted into
            foreach (Set ss in set1)
            {
                foreach (Item x in ss.Item)
                {
                    if (x.dbr.Contains("xpack"))
                    {
                        if (x.dbr.Contains("armor\\amulet"))
                        {
                            amulet.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\armband"))
                        {
                            bracer.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\torso"))
                        {
                            armor.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\greaves"))
                        {
                            greaves.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\helm"))
                        {
                            helm.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\ring"))
                        {
                            ring.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\shield") || x.dbr.Contains("weapon\\shield")) // Atlantis changed the path yet again, from weapons to weapon
                        {
                            shield.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\axe") || x.dbr.Contains("weapon\\axe"))
                        {
                            axe.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\bow") || x.dbr.Contains("weapon\\bow"))
                        {
                            bow.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\club") || x.dbr.Contains("weapon\\club"))
                        {
                            club.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\spear") || x.dbr.Contains("weapon\\spear"))
                        {
                            spear.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\staff") || x.dbr.Contains("weapon\\staff"))
                        {
                            staff.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\sword") || x.dbr.Contains("weapon\\sword"))
                        {
                            sword.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\1hranged") || x.dbr.Contains("weapon\\1hranged"))
                        {
                            ohranged.Add(x);
                        }
                    }
                    else
                    {
                        if (x.dbr.Contains("equipmentamulet"))
                        {
                            amulet.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentarmband"))
                        {
                            bracer.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentarmor"))
                        {
                            armor.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentgreaves"))
                        {
                            greaves.Add(x);
                        }
                        else if (x.dbr.Contains("equipmenthelm"))
                        {
                            helm.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentring"))
                        {
                            ring.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentshield"))
                        {
                            shield.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\axe"))
                        {
                            axe.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\bow"))
                        {
                            bow.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\club"))
                        {
                            club.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\spear"))
                        {
                            spear.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\staff"))
                        {
                            staff.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\sword"))
                        {
                            sword.Add(x);
                        }
                    }
                }
            }

            foreach (Set ss in set2)
            {
                foreach (Item x in ss.Item)
                {
                    if (x.dbr.Contains("xpack"))
                    {
                        if (x.dbr.Contains("armor\\amulet"))
                        {
                            amulet.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\armband"))
                        {
                            bracer.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\torso"))
                        {
                            armor.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\greaves"))
                        {
                            greaves.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\helm"))
                        {
                            helm.Add(x);
                        }
                        else if (x.dbr.Contains("armor\\ring"))
                        {
                            ring.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\shield") || x.dbr.Contains("weapon\\shield"))
                        {
                            shield.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\axe") || x.dbr.Contains("weapon\\axe"))
                        {
                            axe.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\bow") || x.dbr.Contains("weapon\\bow"))
                        {
                            bow.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\club") || x.dbr.Contains("weapon\\club"))
                        {
                            club.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\spear") || x.dbr.Contains("weapon\\spear"))
                        {
                            spear.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\staff") || x.dbr.Contains("weapon\\staff"))
                        {
                            staff.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\sword") || x.dbr.Contains("weapon\\sword"))
                        {
                            sword.Add(x);
                        }
                        else if (x.dbr.Contains("weapons\\1hranged") || x.dbr.Contains("weapon\\1hranged"))
                        {
                            ohranged.Add(x);
                        }
                    }
                    else
                    {
                        if (x.dbr.Contains("equipmentamulet"))
                        {
                            amulet.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentarmband"))
                        {
                            bracer.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentarmor"))
                        {
                            armor.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentgreaves"))
                        {
                            greaves.Add(x);
                        }
                        else if (x.dbr.Contains("equipmenthelm"))
                        {
                            helm.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentring"))
                        {
                            ring.Add(x);
                        }
                        else if (x.dbr.Contains("equipmentshield"))
                        {
                            shield.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\axe"))
                        {
                            axe.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\bow"))
                        {
                            bow.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\club"))
                        {
                            club.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\spear"))
                        {
                            spear.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\staff"))
                        {
                            staff.Add(x);
                        }
                        else if (x.dbr.Contains("weapon\\sword"))
                        {
                            sword.Add(x);
                        }
                    }
                }
            }

            amulet.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            bracer.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            armor.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            greaves.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            helm.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            ring.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            shield.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            axe.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            bow.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            club.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            spear.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            staff.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            sword.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });
            ohranged.Sort(delegate (Item p1, Item p2) { return p1.name.CompareTo(p2.name); });

            retSet[0] = new Set();
            retSet[0].name = "uheader01";
            retSet[0].Item = new Item[amulet.Count];
            for (int i = 0; i < amulet.Count; i++)
            {
                retSet[0].Item[i] = amulet[i];
            }

            retSet[1] = new Set();
            retSet[1].name = "uheader02";
            retSet[1].Item = new Item[armor.Count];
            for (int i = 0; i < armor.Count; i++)
            {
                retSet[1].Item[i] = armor[i];
            }

            retSet[2] = new Set();
            retSet[2].name = "uheader03";
            retSet[2].Item = new Item[axe.Count];
            for (int i = 0; i < axe.Count; i++)
            {
                retSet[2].Item[i] = axe[i];
            }

            retSet[3] = new Set();
            retSet[3].name = "uheader04";
            retSet[3].Item = new Item[bow.Count];
            for (int i = 0; i < bow.Count; i++)
            {
                retSet[3].Item[i] = bow[i];
            }

            retSet[4] = new Set();
            retSet[4].name = "uheader05";
            retSet[4].Item = new Item[bracer.Count];
            for (int i = 0; i < bracer.Count; i++)
            {
                retSet[4].Item[i] = bracer[i];
            }

            retSet[5] = new Set();
            retSet[5].name = "uheader06";
            retSet[5].Item = new Item[club.Count];
            for (int i = 0; i < club.Count; i++)
            {
                retSet[5].Item[i] = club[i];
            }

            retSet[6] = new Set();
            retSet[6].name = "uheader07";
            retSet[6].Item = new Item[greaves.Count];
            for (int i = 0; i < greaves.Count; i++)
            {
                retSet[6].Item[i] = greaves[i];
            }

            retSet[7] = new Set();
            retSet[7].name = "uheader08";
            retSet[7].Item = new Item[helm.Count];
            for (int i = 0; i < helm.Count; i++)
            {
                retSet[7].Item[i] = helm[i];
            }

            retSet[8] = new Set();
            retSet[8].name = "uheader09";
            retSet[8].Item = new Item[ring.Count];
            for (int i = 0; i < ring.Count; i++)
            {
                retSet[8].Item[i] = ring[i];
            }

            retSet[9] = new Set();
            retSet[9].name = "uheader10";
            retSet[9].Item = new Item[shield.Count];
            for (int i = 0; i < shield.Count; i++)
            {
                retSet[9].Item[i] = shield[i];
            }

            retSet[10] = new Set();
            retSet[10].name = "uheader11";
            retSet[10].Item = new Item[spear.Count];
            for (int i = 0; i < spear.Count; i++)
            {
                retSet[10].Item[i] = spear[i];
            }

            retSet[11] = new Set();
            retSet[11].name = "uheader12";
            retSet[11].Item = new Item[staff.Count];
            for (int i = 0; i < staff.Count; i++)
            {
                retSet[11].Item[i] = staff[i];
            }

            retSet[12] = new Set();
            retSet[12].name = "uheader13";
            retSet[12].Item = new Item[sword.Count];
            for (int i = 0; i < sword.Count; i++)
            {
                retSet[12].Item[i] = sword[i];
            }

            retSet[13] = new Set();
            retSet[13].name = "uheader14";
            retSet[13].Item = new Item[ohranged.Count];
            for (int i = 0; i < ohranged.Count; i++)
            {
                retSet[13].Item[i] = ohranged[i];
            }

            return retSet;
        }

        public static bool LoadFiles()
        {
            //Load config.xml
            //Can't translate error because Language isn't loaded.
            if (!LoadConfig())
            {
                MessageBox.Show("LoadConfig() returned false.");
                return false;
            }
            //Load the proper language
            //Can't translate error because Language isn't loaded.
            if (!LoadLanguage())
            {
                MessageBox.Show("LoadLanguage() returned false.");
                return false;
            }
            //Load itemdb.xml
            if (!LoadItemDB())
            {
                MessageBox.Show(Files.Language["error01"]);
                return false;
            }
            if (Files.Configuration.Directories.TQ == "null" && Files.Configuration.Directories.IT == "null" && Files.Configuration.Directories.AE == "null")
            {
                Directories d = new Directories(true);

                if (!(bool)d.ShowDialog())
                {
                    Application.Current.Shutdown();

                    return false;
                }
            }
            //Load ARC
            if (!Database.LoadDatabase())
            {
                MessageBox.Show(Files.Language["error02"]);
                return false;
            }
            //Load vaults & characters
            if (Files.Configuration.UseVaults)
            {
                Vaults.LoadVaults();
            }
            if (Files.Configuration.UseInventories)
            {
                if (!Vaults.LoadCharacters())
                {
                    MessageBox.Show(Files.Language["error04"]);
                    return false;
                }
            }
            if (Files.Configuration.UseCaravan)
            {
                if (!Vaults.LoadCaravan())
                {
                    MessageBox.Show(Files.Language["error05"]);
                    return false;
                }
            }
            return true;
        }

        public static bool reloadFiles()
        {
            //reload language
            if (!LoadLanguage()) return false;
            //Close ItemDB
            XMLItemDB = null;
            //Load itemdb.xml
            if (!LoadItemDB()) return false;
            //Load ARC
            if (!Database.LoadDatabase()) return false;
            //Load vaults & characters
            if (Files.Configuration.UseVaults)
            {
                Vaults.LoadVaults();
            }
            if (Files.Configuration.UseInventories)
            {
                if (!Vaults.LoadCharacters()) return false;
            }
            if (Files.Configuration.UseCaravan)
            {
                if (!Vaults.LoadCaravan()) return false;
            }
            return true;
        }

        public static Config Configuration
        {
            get
            {
                return XMLConfig;
            }
            set
            {
                XMLConfig = value;
            }
        }

        public static ItemDB ItemDatabase
        {
            get
            {
                return XMLItemDB;
            }
            set
            {
                XMLItemDB = value;
            }
        }

        public static bool LoadConfig()
        {
            StreamReader str = null;
            try
            {
                if (File.Exists("config.xml"))
                {
                    str = new StreamReader("config.xml");
                    XMLConfigSerializer = new XmlSerializer(typeof(Config));
                    XMLConfig = (Config)XMLConfigSerializer.Deserialize(str);
                    //null means it hasn't been set to 'documents' yet.
                    if (XMLConfig.Directories.Vaults.Equals("null"))
                    {
                        //Default path
                        string defaultVaultPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest\\TQVaultData");
                        if (Directory.Exists(defaultVaultPath))
                        {
                            XMLConfig.Directories.Vaults = defaultVaultPath;
                        }
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("config.xml was not found.");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("An error with config.xml, has it been illegally modified?");
                return false;
            }
            finally
            {
                if (str != null) str.Close();
            }
        }

        public static bool SaveConfig()
        {
            Stream stream = null;
            try
            {
                if (XMLConfig != null)
                {
                    stream = File.Open("config.xml", FileMode.Truncate);
                    XMLConfigSerializer.Serialize(stream, XMLConfig);
                    return true;
                }
                return false;
            }
            catch
            {
                MessageBox.Show(Files.Language["error08"]);
                return false;
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        public static bool LoadItemDB()
        {
            StreamReader str = null;
            try
            {
                if (File.Exists("itemdb.xml"))
                {
                    str = new StreamReader("itemdb.xml");
                    XMLItemDBSerializer = new XmlSerializer(typeof(ItemDB));
                    XMLItemDB = (ItemDB)XMLItemDBSerializer.Deserialize(str);
                    return true;
                }
                else
                {
                    MessageBox.Show(Files.Language["error09"]);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show(Files.Language["error10"]);
                return false;
            }
            finally
            {
                if (str != null) str.Close();
            }
        }

        public static class Database
        {
            public class ARCPartEntry
            {
                public int fileOffset;
                public int compressedSize;
                public int realSize;
            }

            public class ARCDirEntry
            {
                public string filename;
                public int storageType;
                public int fileOffset;
                public int compressedSize;
                public int realSize;
                public ARCPartEntry[] parts;

                public bool IsActive
                {
                    get
                    {
                        if (storageType == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return parts != null;
                        }
                    }
                }
            };

            private static Hashtable textDB;

            public static string ReadRegistryKey(Microsoft.Win32.RegistryKey key, string[] path)
            {
                int valueKey = path.Length - 1;
                int lastSubKey = path.Length - 2;

                for (int i = 0; i <= lastSubKey; ++i)
                {
                    key = key.OpenSubKey(path[i]);
                    if (key == null)
                    {
                        return "";
                        //throw new System.ApplicationException (System.string.Format ("Unable to read registry setting '{0}'", string.Join ("\\", path)));
                    }
                }
                return (string)(key.GetValue(path[valueKey]));
            }

            public static bool LoadDatabase()
            {
                // If we're using AE we won't be using TQ
                if (!Files.Configuration.UseAE && Files.Configuration.Directories.TQ.Equals("null"))
                {
                    //Need to set TQ directory
                    try
                    {
                        using (RegistryKey TQKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Iron Lore\\Titan Quest"))
                        {
                            if (TQKey != null)
                            {
                                object TQLoc = TQKey.GetValue("Install Location");
                                if (TQLoc != null)
                                {
                                    Files.Configuration.Directories.TQ = TQLoc.ToString();
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Nothing to do, dir should already be null
                    }
                }

                if (Files.Configuration.UseIT && Files.Configuration.Directories.IT.Equals("null"))
                {
                    //Need to set TQ directory
                    try
                    {
                        using (RegistryKey ITKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Iron Lore\\Titan Quest Immortal Throne"))
                        {
                            if (ITKey != null)
                            {
                                object ITLoc = ITKey.GetValue("Install Location");
                                if (ITLoc != null)
                                {
                                    Files.Configuration.Directories.IT = ITLoc.ToString();
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Nothing to do, dir should already be null
                    }
                }

                textDB = new Hashtable();
                Directories d = null;

                if (Files.Configuration.UseAE)
                {
                    do
                    {
                        if (File.Exists(Files.Configuration.Directories.AE + "\\text\\text_" + Files.Configuration.Language + ".arc"))
                        {
                            string dbFile = Files.Configuration.Directories.AE + "\\text\\text_" + Files.Configuration.Language + ".arc";

                            if (!ParseTextDB(dbFile, "text\\commonequipment.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\uniqueequipment.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\ui.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\quest.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\xcommonequipment.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\xuniqueequipment.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\xui.txt")) return false;
                            if (File.Exists(Files.Configuration.Directories.AE + "\\Resources\\XPack2\\SceneryAsgard.arc")) //condition to determine whether Ragnarök is installed
                            {
                                if (!ParseTextDB(dbFile, "text\\x2commonequipment.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x2uniqueequipment.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x2ui.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x2quest.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x2monsters.txt")) return false;
                            }
                            if (File.Exists(Files.Configuration.Directories.AE + "\\Resources\\XPack3\\Scenery.arc")) //condition to determine whether Atlantis is installed
                            {
                                if (!ParseTextDB(dbFile, "text\\x3basegame_nonvoiced.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x3items_nonvoiced.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x3misctags_nonvoiced.txt")) return false;
                            }
                            if (File.Exists(Files.Configuration.Directories.AE + "\\Resources\\XPack4\\Scenery.arc")) //condition to determine whether Eternal Embers is installed
                            {
                                if (!ParseTextDB(dbFile, "text\\x4basegame_nonvoiced.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x4items_nonvoiced.txt")) return false;
                                if (!ParseTextDB(dbFile, "text\\x4misctags_nonvoiced.txt")) return false;
                            }
                            return true;
                        }
                        MessageBox.Show(Files.Language["error23"]);
                        d = new Directories();
                    } while ((bool)d.ShowDialog());
                    return false;
                }
                else
                {
                    do
                    {
                        if (File.Exists(Files.Configuration.Directories.TQ + "\\text\\text_" + Files.Configuration.Language + ".arc"))
                        {
                            string dbFile = Files.Configuration.Directories.TQ + "\\text\\text_" + Files.Configuration.Language + ".arc";

                            if (!ParseTextDB(dbFile, "text\\commonequipment.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\uniqueequipment.txt")) return false;
                            if (!ParseTextDB(dbFile, "text\\ui.txt")) return false;

                            if (Files.Configuration.UseIT)
                            {
                                do
                                {
                                    if (File.Exists(Files.Configuration.Directories.IT + "\\resources\\text_" + Files.Configuration.Language + ".arc"))
                                    {
                                        dbFile = Files.Configuration.Directories.IT + "\\resources\\text_" + Files.Configuration.Language + ".arc";

                                        //Don't care what they return because non-EN seem to not have them...
                                        ParseTextDB(dbFile, "text\\commonequipment.txt");
                                        ParseTextDB(dbFile, "text\\uniqueequipment.txt");
                                        ParseTextDB(dbFile, "text\\ui.txt");
                                        if (!ParseTextDB(dbFile, "text\\xcommonequipment.txt")) return false;
                                        if (!ParseTextDB(dbFile, "text\\xuniqueequipment.txt")) return false;
                                        if (!ParseTextDB(dbFile, "text\\xui.txt")) return false;
                                        return true;
                                    }
                                    MessageBox.Show(Files.Language["error11"]);
                                    d = new Directories();
                                } while ((bool)d.ShowDialog());
                                return false;
                            }
                            return true;
                        }
                        MessageBox.Show(Files.Language["error12"]);
                        d = new Directories();
                    } while ((bool)d.ShowDialog());
                    return false;
                }
            }

            private static bool ParseTextDB(string file, string filename)
            {
                byte[] data = ReadARCFile(file, filename);
                if (data == null) return false;

                MemoryStream datastream = new MemoryStream(data);
                StreamReader reader = new StreamReader(datastream, Encoding.Default);

                char delim = '=';
                string line;
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length < 2) continue;
                        if (line.StartsWith("//")) continue;

                        string[] fields = line.Split(delim);
                        if (fields.Length < 2) continue;
                        string label = fields[1].Trim();

                        if (label.IndexOf('[') != -1)
                        {
                            int textStart = label.IndexOf(']') + 1;
                            int textEnd = label.IndexOf('[', textStart);
                            if (textEnd == -1)
                            {
                                label = label.Substring(textStart);
                            }
                            else
                            {
                                label = label.Substring(textStart, textEnd - textStart);
                            }
                            label = label.Trim();
                        }

                        // small hack for a set name introduced in patch 2.10 #3 to remove a '{^n}' format string
                        if (filename.Contains("quest") && !filename.Contains("x2"))
                        {
                            if (label.IndexOf('{') != -1)
                            {
                                label = label.Substring(0, label.IndexOf('{'));
                            }
                        }

                        string key = fields[0].Trim();

                        if (!textDB.ContainsKey(key))
                        {
                            textDB.Add(key, label);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(Files.Language["error13"]);
                    return false;
                }
                finally
                {
                    reader.Close();
                }
                return true;
            }

            private static byte[] ReadARCFile(string file, string filename)
            {
                byte[] ans = null;
                Hashtable hash = null;

                try
                {
                    FileStream arcFile = new FileStream(file, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(arcFile);

                    try
                    {
                        if (reader.ReadByte() != 0x41) return null;
                        if (reader.ReadByte() != 0x52) return null;
                        if (reader.ReadByte() != 0x43) return null;

                        if (arcFile.Length < 0x21) return null;

                        reader.BaseStream.Seek(0x08, SeekOrigin.Begin);
                        int numEntries = reader.ReadInt32();
                        int numParts = reader.ReadInt32();

                        ARCPartEntry[] parts = new ARCPartEntry[numParts];
                        ARCDirEntry[] records = new ARCDirEntry[numEntries];

                        reader.BaseStream.Seek(0x18, SeekOrigin.Begin);
                        int tocOffset = reader.ReadInt32();

                        if (arcFile.Length < (tocOffset + 4 * 3)) return null;

                        reader.BaseStream.Seek(tocOffset, SeekOrigin.Begin);
                        int i;
                        for (i = 0; i < numParts; ++i)
                        {
                            parts[i] = new ARCPartEntry();
                            parts[i].fileOffset = reader.ReadInt32();
                            parts[i].compressedSize = reader.ReadInt32();
                            parts[i].realSize = reader.ReadInt32();
                        }

                        int fileNamesOffset = (int)arcFile.Position;
                        int fileRecordOffset = 44 * numEntries;

                        arcFile.Seek(-1 * fileRecordOffset, SeekOrigin.End);
                        for (i = 0; i < numEntries; ++i)
                        {
                            records[i] = new ARCDirEntry();
                            int storageType = reader.ReadInt32(); // storageType = 3 - compressed / 1- non compressed

                            records[i].storageType = storageType;
                            records[i].fileOffset = reader.ReadInt32();
                            records[i].compressedSize = reader.ReadInt32();
                            records[i].realSize = reader.ReadInt32();
                            int crap = reader.ReadInt32(); // crap
                            crap = reader.ReadInt32(); // crap
                            crap = reader.ReadInt32(); // crap

                            int np = reader.ReadInt32();
                            if (np < 1)
                            {
                                records[i].parts = null;
                            }
                            else
                            {
                                records[i].parts = new ARCPartEntry[np];
                            }

                            int firstPart = reader.ReadInt32();
                            crap = reader.ReadInt32(); // filename length

                            crap = reader.ReadInt32(); // filename offset

                            if (storageType != 1 && records[i].IsActive)
                            {
                                for (int ip = 0; ip < records[i].parts.Length; ++ip)
                                {
                                    records[i].parts[ip] = parts[ip + firstPart];
                                }
                            }
                        }

                        arcFile.Seek(fileNamesOffset, SeekOrigin.Begin);
                        byte[] buffer = new byte[2048];
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        for (i = 0; i < numEntries; ++i)
                        {
                            // only Active files have a filename entry
                            if (records[i].IsActive)
                            {
                                int bufSize = 0;

                                while ((buffer[bufSize++] = reader.ReadByte()) != 0x00)
                                {
                                    if (buffer[bufSize - 1] == 0x03)
                                    { // god damn it
                                        arcFile.Seek(-1, SeekOrigin.Current); // backup
                                        bufSize--;
                                        buffer[bufSize] = 0x00;
                                        break;
                                    }
                                }

                                string newfile;
                                if (bufSize >= 1)
                                {
                                    char[] chars = new char[ascii.GetCharCount(buffer, 0, bufSize - 1)];
                                    ascii.GetChars(buffer, 0, bufSize - 1, chars, 0);
                                    newfile = new string(chars);
                                }
                                else
                                {
                                    newfile = string.Format("Null File {0}", i);
                                }
                                newfile.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                                newfile.Replace('/', '\\');
                                records[i].filename = newfile;
                            }
                        }
                        hash = new Hashtable(numEntries);

                        for (i = 0; i < numEntries; ++i)
                        {
                            if (records[i].IsActive)
                            {
                                hash.Add(records[i].filename, records[i]);
                            }
                        }

                        filename.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                        filename.Replace('/', '\\');

                        int firstPathDelim = filename.IndexOf('\\');
                        if (firstPathDelim != -1)
                        {
                            filename = filename.Substring(firstPathDelim + 1);
                        }

                        ARCDirEntry dirEntry = (ARCDirEntry)(hash[filename]);

                        if (dirEntry == null)
                        {
                            if (!file.Contains("Throne")) MessageBox.Show("text_" + Files.Configuration.Language + ".arc:\\" + filename + Files.Language["error14"]);
                            return null;
                        }

                        FileStream arcFile2 = new FileStream(file, FileMode.Open, FileAccess.Read);
                        try
                        {
                            ans = new byte[dirEntry.realSize];

                            int ipos = 0;

                            if ((dirEntry.storageType == 1) && (dirEntry.compressedSize == dirEntry.realSize))
                            {
                                arcFile.Seek(dirEntry.fileOffset, SeekOrigin.Begin);
                                arcFile.Read(ans, 0, dirEntry.realSize);
                            }
                            else
                            {
                                for (int ipart = 0; ipart < dirEntry.parts.Length; ++ipart)
                                {
                                    arcFile.Seek(dirEntry.parts[ipart].fileOffset, SeekOrigin.Begin);

                                    zlib.ZInputStream zinput = new zlib.ZInputStream(arcFile);

                                    int len;
                                    int partLen = 0;
                                    while ((len = zinput.read(ans, ipos, ans.Length - ipos)) > 0)
                                    {
                                        ipos += len;
                                        partLen += len;
                                        if (partLen >= dirEntry.parts[ipart].realSize) break;
                                    }
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            MessageBox.Show(e.ToString(), Files.Language["error13"]);
                            return null;
                        }
                        finally
                        {
                            arcFile2.Close();
                        }
                    }
                    catch (System.Exception e)
                    {
                        MessageBox.Show(e.ToString(), Files.Language["error13"]);
                        return null;
                    }
                    finally
                    {
                        reader.Close();
                        arcFile.Close();
                    }
                }
                catch
                {
                    MessageBox.Show(Files.Language["error15"] + " text_" + Files.Configuration.Language + ".arc.");
                }

                return ans;
            }

            public static string getName(string s)
            {
                return textDB.ContainsKey(s) ? (string)textDB[s] : "...";
            }

            public static bool exists(string s)
            {
                return textDB.ContainsKey(s);
            }
        }

        public static Set[] filterItems(Set[] sets, bool listBG, bool listIT, bool listSP, bool listR, bool listAtl, bool listEE)
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in sets) //create a new set of items according to the input criteria
            {
                List<Item> filteredList = new List<Item>();
                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (listBG && s.Item[i].isIT == false && s.Item[i].isR == false && s.Item[i].isAtl == false && s.Item[i].isEE == false)
                    {
                        filteredList.Add(s.Item[i]); // item is from the base game? add it
                    }
                    if (listIT && s.Item[i].isIT)
                    {
                        filteredList.Add(s.Item[i]); // item is from IT? add it
                    }
                    if (listSP && s.Item[i].dbr.Contains("\\z_"))
                    {
                        filteredList.Add(s.Item[i]); // item is from the secret passage? add it
                    }
                    if (listR && s.Item[i].isR)
                    {
                        filteredList.Add(s.Item[i]); // item is from Ragnarök? add it
                    }
                    if (listAtl && s.Item[i].isAtl)
                    {
                        filteredList.Add(s.Item[i]); // item is from Atlantis? add it
                    }
                    if (listEE && s.Item[i].isEE)
                    {
                        filteredList.Add(s.Item[i]); // item is from Eternal Embers? add it
                    }
                }
                Item[] filteredItem = new Item[filteredList.Count];
                for (int i = 0; i < filteredList.Count; i++) // recreate new Item array from list w/o filtered out items
                {
                    filteredItem[i] = filteredList[i];
                }
                s.Item = filteredItem;
                if (s.Item != null)
                {
                    ls.Add(s);
                }
            }

            Set[] returnedList = new Set[ls.Count];
            for (int i = 0; i < ls.Count; i++)
            {
                returnedList[i] = ls[i];
            }
            return returnedList;
        }

        public static Set[] removeBG(Set[] sets) //removes base game items from display
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in sets) //create a new set of items not from the base game
            {
                List<Item> nobg = new List<Item>();
                for (int i = 0; i < s.Item.Length; i++) //create new item list w/o base game items
                {
                    if (s.Item[i].isR) // add R item to the new list
                    {
                        nobg.Add(s.Item[i]);
                    }
                    else if (s.Item[i].isAtl) // add Atl item to the new list
                    {
                        nobg.Add(s.Item[i]);
                    }
                    else if (s.Item[i].isEE) // add EE item to the new list
                    {
                        nobg.Add(s.Item[i]);
                    }
                }
                Item[] nobg_item = new Item[nobg.Count];
                for (int i = 0; i < nobg.Count; i++) //recreate new Item array from list w/o BG items
                {
                    nobg_item[i] = nobg[i];
                }
                s.Item = nobg_item;
                if (s.Item != null) //only add set if it has non R items
                {
                    ls.Add(s);
                }
            }

            Set[] ret = new Set[ls.Count];

            for (int i = 0; i < ls.Count; i++)
            {
                ret[i] = ls[i];
            }

            return ret;
        }

        public static Set[] removeSP(Set[] sets)
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in sets)
            {
                List<Item> nosp = new List<Item>();
                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (!s.Item[i].dbr.Contains("\\z_"))
                    {
                        nosp.Add(s.Item[i]); //create new item list w/o secret passage items
                    }
                }
                Item[] nosp_item = new Item[nosp.Count];
                for (int i = 0; i < nosp.Count; i++) //recreate new Item array from list w/o SP items
                {
                    nosp_item[i] = nosp[i];
                }
                s.Item = nosp_item;
                if (s.Item != null) //only add set if it has non SP items
                {
                    ls.Add(s);
                }
            }

            Set[] ret = new Set[ls.Count];

            for (int i = 0; i < ls.Count; i++)
            {
                ret[i] = ls[i];
            }

            return ret;
        }

        public static Set[] removeR(Set[] sets) //removes Ragnarök items from display
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in sets) //create a new set of items not from Ragnarök
            {
                List<Item> nor = new List<Item>();
                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (!s.Item[i].isR)
                    {
                        nor.Add(s.Item[i]); //create new item list w/o Ragnarök items
                    }
                }
                Item[] nor_item = new Item[nor.Count];
                for (int i = 0; i < nor.Count; i++) //recreate new Item array from list w/o R items
                {
                    nor_item[i] = nor[i];
                }
                s.Item = nor_item;
                if (s.Item != null) //only add set if it has non R items
                {
                    ls.Add(s);
                }
            }

            Set[] ret = new Set[ls.Count];

            for (int i = 0; i < ls.Count; i++)
            {
                ret[i] = ls[i];
            }

            return ret;
        }

        public static Set[] removeAtl(Set[] sets) //removes Atlantis items from display
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in sets) //create a new set of items not from Atlantis
            {
                List<Item> noatl = new List<Item>();
                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (!s.Item[i].isAtl)
                    {
                        noatl.Add(s.Item[i]); //create new item list w/o Atlantis items
                    }
                }
                Item[] noatl_item = new Item[noatl.Count];
                for (int i = 0; i < noatl.Count; i++) //recreate new Item array from list w/o Atl items
                {
                    noatl_item[i] = noatl[i];
                }
                s.Item = noatl_item;
                if (s.Item != null) //only add set if it has non Atl items
                {
                    ls.Add(s);
                }
            }

            Set[] ret = new Set[ls.Count];

            for (int i = 0; i < ls.Count; i++)
            {
                ret[i] = ls[i];
            }

            return ret;
        }

        public static Set[] removeEE(Set[] sets) //removes Eternal Embers items from display
        {
            List<Set> ls = new List<Set>();

            foreach (Set s in sets) //create a new set of items not from Eternal Embers
            {
                List<Item> noee = new List<Item>();
                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (!s.Item[i].isEE)
                    {
                        noee.Add(s.Item[i]); //create new item list w/o Eternal Embers items
                    }
                }
                Item[] noee_item = new Item[noee.Count];
                for (int i = 0; i < noee.Count; i++) //recreate new Item array from list w/o EE items
                {
                    noee_item[i] = noee[i];
                }
                s.Item = noee_item;
                if (s.Item != null) //only add set if it has non EE items
                {
                    ls.Add(s);
                }
            }

            Set[] ret = new Set[ls.Count];

            for (int i = 0; i < ls.Count; i++)
            {
                ret[i] = ls[i];
            }

            return ret;
        }
    }

    public partial class Item
    {
        private int countField = 0;
        private List<string> locationsField = new List<string>();

        public int count
        {
            set
            {
                countField = value;
            }
            get
            {
                return countField;
            }
        }

        public List<string> locations
        {
            set
            {
                locationsField = value;
            }
            get
            {
                return locationsField;
            }
        }

        public string name
        {
            get
            {
                return Files.Database.getName(this.id);
            }
        }
    }

    public partial class ItemDB
    {
        public void addItem(string BaseID, string location)
        {
            if (addItemSub(monsterInfrequentField.Normal, BaseID, location)) return;
            if (addItemSub(monsterInfrequentField.Epic, BaseID, location)) return;
            if (addItemSub(monsterInfrequentField.Legendary, BaseID, location)) return;
            if (addItemSub(relicsField.Normal, BaseID, location)) return;
            if (addItemSub(relicsField.Epic, BaseID, location)) return;
            if (addItemSub(relicsField.Legendary, BaseID, location)) return;
            if (addItemSub(charmsField.Normal, BaseID, location)) return;
            if (addItemSub(charmsField.Epic, BaseID, location)) return;
            if (addItemSub(charmsField.Legendary, BaseID, location)) return;
            if (addItemSub(artifactsField.Normal, BaseID, location)) return;
            if (addItemSub(artifactsField.Epic, BaseID, location)) return;
            if (addItemSub(artifactsField.Legendary, BaseID, location)) return;
            if (addItemSub(parchmentsField.Normal, BaseID, location)) return;
            if (addItemSub(formulaeField.Normal, BaseID, location)) return;
            if (addItemSub(formulaeField.Epic, BaseID, location)) return;
            if (addItemSub(formulaeField.Legendary, BaseID, location)) return;
            if (addItemSub(scrollsField.Normal, BaseID, location)) return;
            if (addItemSub(scrollsField.Epic, BaseID, location)) return;
            if (addItemSub(scrollsField.Legendary, BaseID, location)) return;
            if (addItemSub(setsField.Epic, BaseID, location)) return;
            if (addItemSub(setsField.Legendary, BaseID, location)) return;
            if (addItemSub(uniquesField.Epic, BaseID, location)) return;
            if (addItemSub(uniquesField.Legendary, BaseID, location)) return;
            //TODO: Need lookup for item prefix/suffix.
        }

        private static bool addItemSub(Set[] set, string BaseID, string location)
        {
            foreach (Set s in set)
            {
                foreach (Item i in s.Item)
                {
                    if (i.dbr == BaseID)
                    {
                        i.locations.Add(location);
                        i.count++;
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public static class Vaults
    {
        public static bool LoadVaults()
        {
            if (!VaultsDirExists())
                return false;

            var di = new DirectoryInfo(Files.Configuration.Directories.Vaults);
            var rgFilesClassic = di.GetFiles("*.vault");
            var rgFilesJson = di.GetFiles("*.vault.json");

            //No need to loop here, the program can load and they can change manually.
            if (rgFilesJson.Length == 0 && rgFilesClassic.Length == 0)
            {
                MessageBox.Show(Files.Language["error17"]);
                return false;
            }

            var classicLoadSuccess = LoadVaultsClassic(rgFilesClassic);
            var jsonLoadSuccess = LoadVaultsJSON(rgFilesJson);

            return classicLoadSuccess || jsonLoadSuccess;
        }

        public static bool VaultsDirExists()
        {
            //Make sure we have a valid directory. Show a dialog box allowing user to change it until we get one.
            if (!Directory.Exists(Files.Configuration.Directories.Vaults))
            {
                Files.Configuration.Directories.Vaults = "null";
                Files.Configuration.UseVaults = false;

                MessageBox.Show(Files.Language["error16"]);
                return false;
            }
            return true;
        }

        public static bool LoadVaultsJSON(IEnumerable<FileInfo> rgFiles)
        {
            foreach (FileInfo fi in rgFiles)
            {
                try
                {
                    ParseJsonData(fi.FullName);
                }
                catch
                {
                    MessageBox.Show(Files.Language["error18"]);
                    return false;
                }
            }

            return true;
        }

        public static bool LoadVaultsClassic(IEnumerable<FileInfo> rgFiles)
        {
            byte[] raw_data; //Vault data
            FileStream file;
            BinaryReader br;
            MemoryStream ms;
            BinaryReader reader;

            foreach (FileInfo fi in rgFiles)
            {
                file = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                br = new System.IO.BinaryReader(file);

                try
                {
                    raw_data = br.ReadBytes((int)file.Length);
                }
                catch
                {
                    MessageBox.Show(Files.Language["error18"]);
                    return false;
                }
                finally
                {
                    br.Close();
                    file.Close();
                }

                ms = new MemoryStream(raw_data, false);
                reader = new BinaryReader(ms);
                try
                {
                    ParseItemBlock(0, reader, fi.Name);
                }
                catch
                {
                    MessageBox.Show(Files.Language["error18"]);
                    return false;
                }
                finally
                {
                    reader.Close();
                    ms.Close();
                }
            }
            return true;
        }

        private static void ParseJsonData(string path)
        {
            var vault = Path.GetFileName(path);
            using (var jsonfile = new StreamReader(path, Encoding.UTF8))
            {
                var vaultDto = new JsonSerializer().Deserialize(jsonfile, typeof(VaultDto)) as VaultDto;

                vaultDto.sacks.SelectMany((Sack, SackIndex) => Sack.items.Select(Item => new { Item, SackIndex })).ToList().ForEach(Infos =>
                {
                    var baseItemID = Infos.Item.baseName;
                    if (baseItemID == null || baseItemID.Trim() == string.Empty)
                        return;

                    int sack = Infos.SackIndex + 1;
                    var var1 = Infos.Item.var1;
                    var relicID = Infos.Item.relicName;
                    var relicID2 = Infos.Item.relicName2;

                    ParseItemLogic(vault, sack, ref baseItemID, var1, ref relicID, ref relicID2);
                });
            }
        }

        private static void ParseItemLogic(string vault, int sack, ref string baseItemID, int var1, ref string relicID, ref string relicID2)
        {
            #region Copy/Paste from old ParseItem()

            int o = baseItemID.LastIndexOf("records");
            if (o < 0) o = 0;
            baseItemID = baseItemID.Substring(o + 8);
            baseItemID = baseItemID.ToLower(System.Globalization.CultureInfo.InvariantCulture);

            if (baseItemID.Contains("\\animalrelics\\") || baseItemID.Contains("\\charms\\")) //A charm
            {
                if (var1 == 5 || baseItemID.Contains("quest_artifice"))
                {
                    Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover04"], vault, sack.ToString()));
                }
            }
            else if (baseItemID.Contains("\\relics\\")) //A relic
            {
                if (var1 == 3 || baseItemID.Contains("nerthusmistletoe"))
                {
                    Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover04"], vault, sack.ToString()));
                }
            }
            else
            {
                Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover04"], vault, sack.ToString()));
            }

            if (Files.Configuration.UseSocketed && !relicID.Equals(""))
            {
                int oo = relicID.LastIndexOf("records");
                if (oo < 0) oo = 0;
                relicID = relicID.Substring(oo + 8);
                relicID = relicID.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                Files.ItemDatabase.addItem(relicID, string.Format(Files.Language["mouseover05"], vault, sack.ToString()));
                if (!relicID2.Equals(""))
                {
                    int ooo = relicID2.LastIndexOf("records");
                    if (ooo < 0) ooo = 0;
                    relicID2 = relicID2.Substring(ooo + 8);
                    relicID2 = relicID2.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                    Files.ItemDatabase.addItem(relicID2, string.Format(Files.Language["mouseover05"], vault, sack.ToString()));
                }
            }

            #endregion
        }

        private static void ParseItemBlock(int loc, BinaryReader reader, string vault)
        {
            reader.BaseStream.Seek(loc, SeekOrigin.Begin);

            ValidateNextString("numberOfSacks", reader);
            int m_numberOfSacks = reader.ReadInt32();

            ValidateNextString("currentlyFocusedSackNumber", reader);
            int m_currentlyFocusedSackNumber = reader.ReadInt32();

            ValidateNextString("currentlySelectedSackNumber", reader);
            int m_currentlySelectedSackNumber = reader.ReadInt32();

            for (int i = 0; i < m_numberOfSacks; ++i)
            {
                ParseSack(reader, vault, i + 1);
            }
        }

        public static bool LoadCharacters()
        {
            string[] players;

            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                if (!Directory.Exists(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest - Immortal Throne\\SaveData\\Main")))
                {
                    players = null;
                }
                else
                {
                    players = Directory.GetDirectories(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest - Immortal Throne\\SaveData\\Main"), "_*");
                }
            }
            else
            {
                if (!Directory.Exists(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest\\SaveData\\Main")))
                {
                    players = null;
                }
                else
                {
                    players = Directory.GetDirectories(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest\\SaveData\\Main"), "_*");
                }
            }
            if (players == null)
            {
                MessageBox.Show(Files.Language["error19"]);
                Files.Configuration.UseInventories = false;
                return true;
            }

            foreach (string s in players)
            {
                if (!LoadCharacter(s + "\\player.chr")) return false;
                if (!LoadStash(s + "\\winsys.dxb")) return false;
            }
            return true;
        }

        private static bool LoadCharacter(string filename)
        {
            byte[] rawData; //Character data
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(file);

            try
            {
                rawData = reader.ReadBytes((int)file.Length);
            }
            catch
            {
                MessageBox.Show(Files.Language["error20"]);
                return false;
            }
            finally
            {
                reader.Close();
            }

            MemoryStream stream = new MemoryStream(rawData, false);
            reader = new BinaryReader(stream);

            try
            {
                // Find the block pairs until we find the block that contains the item data.
                int blockNestLevel = 0;
                int loc = 0;
                int itemLoc = 0;
                int equipmentLoc = 0;
                bool foundItems = false; //m_isVault; // vaults start at the item data with no crap
                bool foundEquipment = false;//m_isVault; // vaults start at the item data with no crap
                while ((!foundItems || !foundEquipment) && (loc = FindNextBlockDelim(loc, rawData)) != -1)
                {
                    if (rawData[loc] == beginBlockPattern[0])
                    {
                        // begin block
                        ++blockNestLevel;
                        loc += beginBlockPattern.Length;

                        // skip past the 4 bytes of noise after begin_block
                        loc += 4;

                        // Seek our stream to the correct position
                        stream.Seek(loc, SeekOrigin.Begin);

                        // Now get the string for this block
                        string blockName = ReadString(reader);
                        // Assign loc to our new stream position
                        loc = (int)stream.Position;

                        // See if we accidentally got a begin_block or end_block
                        if (blockName.Equals("begin_block"))
                        {
                            blockName = "(noname)";
                            loc -= beginBlockPattern.Length;
                        }
                        else if (blockName.Equals("end_block"))
                        {
                            blockName = "(noname)";
                            loc -= endBlockPattern.Length;
                        }
                        else if (blockName.Equals("itemPositionsSavedAsGridCoords"))
                        {
                            loc += 4;
                            itemLoc = loc; // skip value for itemPositionsSavedAsGridCoords
                            foundItems = true;
                        }
                        else if (blockName.Equals("useAlternate"))
                        {
                            loc += 4;
                            equipmentLoc = loc; // skip value for useAlternate
                            foundEquipment = true;
                        }

                    }
                    else
                    {
                        // end block
                        --blockNestLevel;
                        loc += endBlockPattern.Length;
                    }
                }

                string player = filename.Substring(0, filename.Length - 11); //11 for "player.chr"
                int lastDir = player.LastIndexOf("\\");
                player = player.Substring(lastDir + 2); //Remove '_' from start of name

                if (foundItems)
                {
                    ParseItemBlock(itemLoc, reader, player);
                }
                // Process the equipment block
                if (foundEquipment)
                {
                    ParseEquipmentBlock(equipmentLoc, reader, player);
                }
            }
            catch
            {
                MessageBox.Show(Files.Language["error21"]);
                return false;
            }
            finally
            {
                reader.Close();
            }
            return true;
        }

        private static void ParseEquipmentBlock(int loc, BinaryReader reader, string player)
        {

            reader.BaseStream.Seek(loc, SeekOrigin.Begin);

            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                ValidateNextString("equipmentCtrlIOStreamVersion", reader);
                int m_equipmentCtrlIOStreamVersion = reader.ReadInt32();
            }

            ParseEquipSack(reader, player);
        }

        private static byte[] beginBlockPattern = { 0x0B, 0x00, 0x00, 0x00, 0x62, 0x65, 0x67, 0x69, 0x6E, 0x5F, 0x62, 0x6C, 0x6F, 0x63, 0x6B };

        private static byte[] endBlockPattern = { 0x09, 0x00, 0x00, 0x00, 0x65, 0x6E, 0x64, 0x5F, 0x62, 0x6C, 0x6F, 0x63, 0x6B };

        private static int FindNextBlockDelim(int start, byte[] rawData)
        {
            int beginMatch = 0;
            int endMatch = 0;

            for (int i = start; i < rawData.Length; ++i)
            {
                if (rawData[i] == beginBlockPattern[beginMatch])
                {
                    ++beginMatch;
                    if (beginMatch == beginBlockPattern.Length) return i + 1 - beginMatch;
                }
                else if (beginMatch > 0)
                {
                    beginMatch = 0;
                    // Test again to see if we are starting a new match
                    if (rawData[i] == beginBlockPattern[beginMatch])
                    {
                        ++beginMatch;
                    }
                }
                if (rawData[i] == endBlockPattern[endMatch])
                {
                    ++endMatch;
                    if (endMatch == endBlockPattern.Length) return i + 1 - endMatch;
                }
                else if (endMatch > 0)
                {
                    endMatch = 0;
                    // Test again to see if we are starting a new match
                    if (rawData[i] == endBlockPattern[endMatch])
                    {
                        ++endMatch;
                    }
                }
            }
            return -1;
        }

        public static bool LoadCaravan()
        {
            //Make sure they're using IT
            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                string transferStashFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest - Immortal Throne\\SaveData\\Sys\\winsys.dxb");
                try
                {
                    LoadStash(transferStashFile);
                }
                catch
                {
                    MessageBox.Show(Files.Language["error22"]);
                    return false;
                }
                if (Files.Configuration.UseAE)
                {
                    string relicStashFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest - Immortal Throne\\SaveData\\Sys\\miscsys.dxb");
                    try
                    {
                        LoadStash(relicStashFile);
                    }
                    catch
                    {
                        MessageBox.Show(Files.Language["error24"]);
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool ValidateNextString(string s, System.IO.BinaryReader b)
        {
            string ans = ReadString(b);
            if (ans.Equals(s)) return true;

            //MessageBox.Show("Failed to validate string: " + s + ", got: " + ans);
            File.AppendAllText(Directory.GetCurrentDirectory() + "\\errorlog.log", "[" + System.DateTime.Now.ToString() + "]\tFailed to validate string: " + s + ", got: " + ans + System.Environment.NewLine);

            return false;
        }

        private static string ReadString(System.IO.BinaryReader b)
        {
            int len = b.ReadInt32();

            // Convert the next len bytes into a string
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();

            byte[] Data = b.ReadBytes(len);

            char[] chars = new char[(ascii.GetCharCount(Data, 0, len))];
            ascii.GetChars(Data, 0, len, chars, 0);

            return new string(chars);
        }

        private static void ParseSack(System.IO.BinaryReader b, string vault, int sack)
        {
            ValidateNextString("begin_block", b);// make sure we just read a new block
            int beginBlockCrap = b.ReadInt32();

            ValidateNextString("tempBool", b);
            int tempBool = b.ReadInt32();

            ValidateNextString("size", b);
            int size = b.ReadInt32();

            for (int i = 0; i < size; ++i)
            {
                ParseItem(b, vault, sack);
            }

            ValidateNextString("end_block", b);
            int endBlockCrap = b.ReadInt32();
        }

        private static void ParseEquipSack(System.IO.BinaryReader b, string player)
        {
            int size = 0;

            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                size = 12;
            }
            else
            {
                size = 11;
            }

            for (int i = 0; i < size; ++i)
            {
                if (i == 7 || i == 9)
                {
                    ValidateNextString("begin_block", b);
                    int beginBlockCrap = b.ReadInt32();

                    // Eat the alternate tag and flag
                    ValidateNextString("alternate", b);
                    int alternateCrap = b.ReadInt32();
                }
                ParseEquipItem(b, player);
                ValidateNextString("itemAttached", b);
                int itemAttachedCrap = b.ReadInt32();
                if (i == 8 || i == 10)
                {
                    ValidateNextString("end_block", b);
                    int endBlockCrap1 = b.ReadInt32();
                }
            }

            ValidateNextString("end_block", b);
            int endBlockCrap = b.ReadInt32();
        }

        private static void ParseItem(System.IO.BinaryReader b, string vault, int sack)
        {
            ValidateNextString("begin_block", b);
            int beginBlockCrap2 = b.ReadInt32();

            ValidateNextString("begin_block", b);
            int beginBlockCrap3 = b.ReadInt32();

            ValidateNextString("baseName", b);
            string baseItemID = ReadString(b);

            ValidateNextString("prefixName", b);
            string prefixID = ReadString(b);

            ValidateNextString("suffixName", b);
            string suffixID = ReadString(b);

            //Gives a dbr of relic
            ValidateNextString("relicName", b);
            string relicID = ReadString(b);

            ValidateNextString("relicBonus", b);
            string relicBonusID = ReadString(b);

            ValidateNextString("seed", b);
            int seed = b.ReadInt32();

            ValidateNextString("var1", b);
            int var1 = b.ReadInt32();

            //Atlantis added second relic
            string an = ReadString(b);
            string relicID2 = "";
            if (an.Equals("relicName2"))
            {
                relicID2 = ReadString(b);

                ValidateNextString("relicBonus2", b);
                string relicBonusID2 = ReadString(b);

                ValidateNextString("var2", b);
                int var2 = b.ReadInt32();

                ValidateNextString("end_block", b);
                int endBlockCrap2 = b.ReadInt32();
            }
            else if (an.Equals("end_block"))
            {
                int endBlockCrap2 = b.ReadInt32();
            }
            else
            {
                MessageBox.Show("Failed to validate string, got: " + an);
            }

            ValidateNextString("pointX", b);
            int x = b.ReadInt32();

            ValidateNextString("pointY", b);
            int y = b.ReadInt32();

            ValidateNextString("end_block", b);
            int endBlockCrap1 = b.ReadInt32();

            ParseItemLogic(vault, sack, ref baseItemID, var1, ref relicID, ref relicID2);
        }

        private static void ParseEquipItem(System.IO.BinaryReader b, string player)
        {
            ValidateNextString("begin_block", b);
            int beginBlockCrap3 = b.ReadInt32();

            ValidateNextString("baseName", b);
            string baseItemID = ReadString(b);

            ValidateNextString("prefixName", b);
            string prefixID = ReadString(b);

            ValidateNextString("suffixName", b);
            string suffixID = ReadString(b);

            ValidateNextString("relicName", b);
            string relicID = ReadString(b);

            ValidateNextString("relicBonus", b);
            string relicBonusID = ReadString(b);

            ValidateNextString("seed", b);
            int seed = b.ReadInt32();

            ValidateNextString("var1", b);
            int var1 = b.ReadInt32();

            //Atlantis added second relic
            string an = ReadString(b);
            string relicID2 = "";
            if (an.Equals("relicName2"))
            {
                relicID2 = ReadString(b);

                ValidateNextString("relicBonus2", b);
                string relicBonusID2 = ReadString(b);

                ValidateNextString("var2", b);
                int var2 = b.ReadInt32();

                ValidateNextString("end_block", b);
                int endBlockCrap2 = b.ReadInt32();
            }
            else if (an.Equals("end_block"))
            {
                int endBlockCrap2 = b.ReadInt32();
            }
            else
            {
                MessageBox.Show("Failed to validate string, got: " + an);
            }

            if (!baseItemID.Equals("")) //Nothing equipped
            {
                baseItemID = baseItemID.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                int o = baseItemID.LastIndexOf("records");
                if (o < 0) o = 0;
                baseItemID = baseItemID.Substring(o + 8);

                if (baseItemID.Contains("\\animalrelics\\") || baseItemID.Contains("\\charms\\")) //A charm
                {
                    if (var1 == 5 || baseItemID.Contains("quest_artifice"))
                    {
                        Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover06"], player));
                    }
                }
                else if (baseItemID.Contains("\\relics\\")) //A relic
                {
                    if (var1 == 3 || baseItemID.Contains("nerthusmistletoe"))
                    {
                        Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover06"], player));
                    }
                }
                else
                {
                    Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover06"], player));
                }

                if (Files.Configuration.UseSocketed && !relicID.Equals(""))
                {
                    int oo = relicID.LastIndexOf("records");
                    if (oo < 0) oo = 0;
                    relicID = relicID.Substring(oo + 8);
                    relicID = relicID.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                    Files.ItemDatabase.addItem(relicID, string.Format(Files.Language["mouseover07"], player));
                    if (!relicID2.Equals(""))
                    {
                        int ooo = relicID2.LastIndexOf("records");
                        if (ooo < 0) ooo = 0;
                        relicID2 = relicID2.Substring(ooo + 8);
                        relicID2 = relicID2.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                        Files.ItemDatabase.addItem(relicID2, string.Format(Files.Language["mouseover07"], player));
                    }
                }
            }
        }

        private static bool LoadStash(string filename)
        {
            byte[] rData; //Caravan data
            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                try
                {
                    if (File.Exists(filename))
                    {

                        FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        BinaryReader reader = new BinaryReader(file);

                        // Just suck the entire file into memory
                        try
                        {
                            rData = reader.ReadBytes((int)file.Length);
                        }
                        finally
                        {
                            reader.Close();
                        }

                        // Now Parse the file
                        MemoryStream stream = new MemoryStream(rData, false);
                        reader = new BinaryReader(stream);

                        try
                        {
                            string player;
                            if (Path.GetFileName(filename) == "miscsys.dxb") // Stash being loaded is the shared relic stash
                            {
                                player = ":relicstash:";
                            }
                            else if (Path.GetFileName(filename) == "winsys.dxb" && filename.Substring(filename.Length - 14, 3) == "Sys") // Stash being loaded is the general shared stash
                            {
                                player = ":sharedstash:";
                            }
                            else // character exclusive stash
                            {
                                player = filename.Substring(0, filename.Length - 11);
                                int lastDir = player.LastIndexOf("\\");
                                player = player.Substring(lastDir + 2);
                            }
                            ParseStashItemBlock(reader, player);
                        }
                        catch
                        {
                            MessageBox.Show(Files.Language["error22"]);
                            return false;
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(Files.Language["error22"]);
                    return false;
                }
            }
            return true;
        }

        private static void ParseStashItemBlock(BinaryReader reader, string player)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            int m_checksum = reader.ReadInt32();

            ValidateNextString("begin_block", reader);
            int m_beginBlockCrap = reader.ReadInt32();

            ValidateNextString("stashVersion", reader);
            int m_stashVersion = reader.ReadInt32();

            ValidateNextString("fName", reader);
            int len = reader.ReadInt32();
            byte[] m_fName = reader.ReadBytes(len);

            ValidateNextString("sackWidth", reader);
            int m_sackWidth = reader.ReadInt32();

            ValidateNextString("sackHeight", reader);
            int m_sackHeight = reader.ReadInt32();

            ParseStashSack(reader, player);
        }

        private static void ParseStashItem(System.IO.BinaryReader b, string player)
        {
            ValidateNextString("stackCount", b);
            int beginBlockCrap1 = b.ReadInt32();

            ValidateNextString("begin_block", b);
            int beginBlockCrap3 = b.ReadInt32();

            ValidateNextString("baseName", b);
            string baseItemID = ReadString(b);

            ValidateNextString("prefixName", b);
            string prefixID = ReadString(b);

            ValidateNextString("suffixName", b);
            string suffixID = ReadString(b);

            ValidateNextString("relicName", b);
            string relicID = ReadString(b);

            ValidateNextString("relicBonus", b);
            string relicBonusID = ReadString(b);

            ValidateNextString("seed", b);
            int seed = b.ReadInt32();

            ValidateNextString("var1", b);
            int var1 = b.ReadInt32();

            //Atlantis added second relic
            string an = ReadString(b);
            string relicID2 = "";
            if (an.Equals("relicName2"))
            {
                relicID2 = ReadString(b);

                ValidateNextString("relicBonus2", b);
                string relicBonusID2 = ReadString(b);

                ValidateNextString("var2", b);
                int var2 = b.ReadInt32();

                ValidateNextString("end_block", b);
                int endBlockCrap2 = b.ReadInt32();
            }
            else if (an.Equals("end_block"))
            {
                int endBlockCrap2 = b.ReadInt32();
            }
            else
            {
                MessageBox.Show("Failed to validate string, got: " + an);
            }

            ValidateNextString("xOffset", b);
            float x = b.ReadSingle();

            ValidateNextString("yOffset", b);
            float y = b.ReadSingle();

            int o = baseItemID.LastIndexOf("records");
            if (o < 0) o = 0;
            baseItemID = baseItemID.Substring(o + 8);
            baseItemID = baseItemID.ToLower(System.Globalization.CultureInfo.InvariantCulture);

            if (baseItemID.Contains("\\animalrelics\\") || baseItemID.Contains("\\charms\\")) //A charm
            {
                if (var1 == 5 || baseItemID.Contains("quest_artifice"))
                {
                    if (player == ":relicstash:")
                    {
                        Files.ItemDatabase.addItem(baseItemID, Files.Language["mouseover11"]);
                    }
                    else if (player == ":sharedstash:")
                    {
                        Files.ItemDatabase.addItem(baseItemID, Files.Language["mouseover10"]);
                    }
                    else
                    {
                        Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover08"], player));
                    }
                }
            }
            else if (baseItemID.Contains("\\relics\\")) //A relic
            {
                if (var1 == 3 || baseItemID.Contains("nerthusmistletoe"))
                {
                    if (player == ":relicstash:")
                    {
                        Files.ItemDatabase.addItem(baseItemID, Files.Language["mouseover11"]);
                    }
                    else if (player == ":sharedstash:")
                    {
                        Files.ItemDatabase.addItem(baseItemID, Files.Language["mouseover10"]);
                    }
                    else
                    {
                        Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover08"], player));
                    }
                }
            }
            else
            {
                if (player == ":relicstash:")
                {
                    Files.ItemDatabase.addItem(baseItemID, Files.Language["mouseover11"]);
                }
                else if (player == ":sharedstash:")
                {
                    Files.ItemDatabase.addItem(baseItemID, Files.Language["mouseover10"]);
                }
                else
                {
                    Files.ItemDatabase.addItem(baseItemID, string.Format(Files.Language["mouseover08"], player));
                }
            }

            if (Files.Configuration.UseSocketed && !relicID.Equals(""))
            {
                int oo = relicID.LastIndexOf("records");
                if (oo < 0) oo = 0;
                relicID = relicID.Substring(oo + 8);
                relicID = relicID.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                Files.ItemDatabase.addItem(relicID, string.Format(Files.Language["mouseover09"], player));
                if (!relicID2.Equals(""))
                {
                    int ooo = relicID2.LastIndexOf("records");
                    if (ooo < 0) ooo = 0;
                    relicID2 = relicID2.Substring(ooo + 8);
                    relicID2 = relicID2.ToLower(System.Globalization.CultureInfo.InvariantCulture);
                    Files.ItemDatabase.addItem(relicID2, string.Format(Files.Language["mouseover09"], player));
                }
            }
        }

        private static void ParseStashSack(System.IO.BinaryReader b, string player)
        {
            ValidateNextString("numItems", b);
            int size = b.ReadInt32();

            for (int i = 0; i < size; ++i)
            {
                ParseStashItem(b, player);
            }

            ValidateNextString("end_block", b);
            int endBlockCrap = b.ReadInt32();
        }
    }
}