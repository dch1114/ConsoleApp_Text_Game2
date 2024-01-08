using ConsoleApp1;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    public class Item
    {
        public string Name;
        public int Attack;
        public int Deffense;
        public int Hp;
        public int Type;
        public int Gold;
        public string Explain;
        public bool Equip;

        public static int ItemCnt = 0;

        public Item(string name, string explain, int type, int attack, int deffense, int hp, int gold, bool equip = false)
        {
            Name = name;
            Attack = attack;
            Deffense = deffense;
            Hp = hp;
            Type = type;
            Explain = explain;
            Equip = equip;
            Gold = gold;
        }

        public void PrintItemStatDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");

            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            if (Equip)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
            }
            Console.Write(Name);
            Console.Write(" | ");

            if (Attack != 0) Console.Write($"공격력 {(Attack >= 0 ? "+" : "")} {Attack} ");
            if (Deffense != 0) Console.Write($"방어력 {(Deffense >= 0 ? "+" : "")} {Deffense} ");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")} {Hp} ");

            Console.Write(" | ");
            Console.WriteLine(Explain);
        }
    }

    public class Character
    {
        public string Name;
        public string Job;
        public int Level;
        public int Attack;
        public int Deffense;
        public int Hp;
        public int Gold;

        public Character(string name, string job, int level, int attack, int deffense, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Attack = attack;
            Deffense = deffense;
            Hp = hp;
            Gold = gold;
        }
    }

    internal class Program
    {
        static Character _player;
        static Item[] _items;

        private static void Inventory()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < Item.ItemCnt; i++)
            {
                _items[i].PrintItemStatDescription();
            }
            Console.WriteLine("");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            switch (ChekValidInput(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    IsEquip();
                    break;
            }
        }

        private static void IsEquip()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                _items[i].PrintItemStatDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int keyInput = ChekValidInput(0, Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    Inventory();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    IsEquip();
                    break;
            }
        }

        private static void ToggleEquipStatus(int idx)
        {
            _items[idx].Equip = !_items[idx].Equip;
        }

        private static void State()
        {
            Console.Clear();

            ShowHighlightedText("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표기됩니다.");

            PrintTextWithHighlights("Lv. ", _player.Level.ToString("00"));
            Console.WriteLine("");
            Console.WriteLine("{0} ( {1} ))", _player.Name, _player.Job);

            int bonusAtk = getSumBonusAtk();
            PrintTextWithHighlights("공격력 : ", (_player.Attack + bonusAtk).ToString(), bonusAtk > 0 ? string.Format(" (+{0})", bonusAtk) : "");
            int bonusDef = getSumBonusDef();
            PrintTextWithHighlights("방어력 : ", (_player.Deffense + bonusDef).ToString(), bonusDef > 0 ? string.Format(" (+{0})", bonusDef) : "");
            int bonusHp = getSumBonusHp();
            PrintTextWithHighlights("체력 : ", (_player.Hp + bonusHp).ToString(), bonusHp > 0 ? string.Format(" (+{0})", bonusHp) : "");
            PrintTextWithHighlights("골드 : ", _player.Gold.ToString());
            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");
            switch (ChekValidInput(0, 0))
            {
                case 0:
                    MainMenu();
                    break;
            }
        }

        private static int getSumBonusAtk()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (_items[i].Equip) sum += _items[i].Attack;
            }
            return sum;
        }

        private static int getSumBonusDef()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (_items[i].Equip) sum += _items[i].Deffense;
            }
            return sum;
        }

        private static int getSumBonusHp()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (_items[i].Equip) sum += _items[i].Hp;
            }
            return sum;
        }

        private static void ShowHighlightedText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void PrintTextWithHighlights(string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(s2);
            Console.ResetColor();
            Console.WriteLine(s3);
        }

        private static void store()
        {
            Console.Clear();

            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            PrintTextWithHighlights("[골드]\n", _player.Gold.ToString());
            Console.WriteLine("\n[아이템 목록]");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                Console.WriteLine($"{i + 1}. {_items[i].Name} | {_player.Attack} | {_items[i].Deffense} | {_items[i].Explain} | {_items[i].Gold} G");
            }
            Console.Write("\n");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기\n");
            Console.Write(">>");
            switch (ChekValidInput(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    Buy();
                    break;
            }
        }

        private static void Buy()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            PrintTextWithHighlights("[골드]\n", _player.Gold.ToString());
            Console.WriteLine("\n[아이템 목록]");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                Console.WriteLine($"{i + 1}. {_items[i].Name} | {_player.Attack} | {_items[i].Deffense} | {_items[i].Explain} | {_items[i].Gold} G");
            }
            Console.Write("\n");
            Console.WriteLine("0. 나가기\n");

            int keyInput = ChekValidInput(0, Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    store();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    Buy();
                    break;
            }
        }

        private static int GetItemPrice(Item item)
        {
            return item.Attack + item.Deffense + item.Hp;

        }
        private static int ChekValidInput(int min, int max)
        {
            int keyInput;
            bool result;
            do
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                result = int.TryParse(Console.ReadLine(), out keyInput);
            } while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;
        }

        private static bool CheckIfValid(int keyInput, int min, int max)
        {
            if (min <= keyInput && keyInput <= max) return true;
            return false;
        }

        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            while (true)
            {
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점\n");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                string number = Console.ReadLine();

                switch (number)
                {
                    case "1":
                        State();
                        break;
                    case "2":
                        Inventory();
                        break;
                    case "3":
                        store();
                        break;
                    default:
                        Console.WriteLine("\n잘못된 입력입니다.\n");
                        break;
                }
            }
        }

        private static void PrintStarLogo()
        {
            Console.WriteLine(" __      __         .__   .__   _________                               ________                     ____                                ____                            ");
            Console.WriteLine("/  \\    /  \\  ____  |  |  |  |  \\_ ___ \\   ____ _____    ____      \\______ \\   __ __   ____    / ___\\   ____ ____    ____        / ___\\ _____ _____    ____");
            Console.WriteLine("\\   \\/\\/   /_/ __ \\ |  |  |  |  /    \\  \\/  /  _ \\  /     \\ _/ __ \\      |    |  \\ |  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\      / /_/  >\\__  \\   /     \\ _/ __ \\  ");
            Console.WriteLine(" \\        / \\  ___/ |  |__|  |__\\     \\____(  <_> )|  Y Y  \\\\  ___/      |    `   \\|  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\     \\___  /  / __ \\_|  Y Y  \\\\  ___/ ");
            Console.WriteLine("  \\__/\\  /   \\___  >|____/|____/ \\______  / \\____/ |__|_|  / \\___  >    /_______  /|____/ |___|  //_____/   \\___  >\\____/ |___|  /    /_____/  (____  /|__|_|  / \\___  > ");
            Console.WriteLine("       \\/        \\/                     \\/               \\/      \\/             \\/             \\/               \\/             \\/                   \\/       \\/      \\/ ");
            Console.ReadKey();
        }

        private static void GameDataSetting()
        {
            _player = new Character("chad", "전사", 1, 10, 5, 100, 1500);
            _items = new Item[10];
            AddItem(new Item("수련자의 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 0, 5, 0, 1000));
            AddItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1, 0, 9, 0, 0));
            AddItem(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3, 0, 15, 0, 3500));
            AddItem(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 4, 2, 0, 0, 600));
            AddItem(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 5, 5, 0, 0, 1500));
            AddItem(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다..", 0, 7, 0, 0, 0));
        }

        static void AddItem(Item item)
        {
            if (Item.ItemCnt == 10) return;
            _items[Item.ItemCnt] = item;
            Item.ItemCnt++;
        }

        static void Main(string[] args)
        {
            PrintStarLogo();
            GameDataSetting();
            MainMenu();
        }
    }
}