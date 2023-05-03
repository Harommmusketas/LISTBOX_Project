using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ListBoxConsole
{
    class ListBox
    {
        private string elem;
        public string Elem
        {
            set { elem = value; }
            get { return elem; }
        }

        public ListBox() : base() { }
        public ListBox(string elem)
        {
            Elem = elem;
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //Kezdő értékek definiálása
            List<string> foLista = new List<string>();
            int magassag = foLista.Count;
            int szelesseg = 40;
            
            string valasza = "";
            //------------------------------------
            //Megkérdezzük a felhasználót, hogy szeretné e ő megadni a ListBox magasságát és szélességét, majd ha igen, akkor beállítjuk azt
            Console.WriteLine("----------------\nSzeretné megadni, hogy milyen magas legyen a ListBox magassága és szélessége?: (Y/N)\n----------------");
            Console.Write("Válasza: ");
            valasza = Console.ReadLine().ToUpper();
            if (valasza != "N")
            {
                Console.Clear();
                Console.WriteLine("----------------\nKérem adja meg, hogy hány soros legyen a ListBox!\n----------------");
                Console.Write("Válasza: ");
                magassag = int.Parse(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("----------------\nKérem adja meg, hogy mekkora legyen a szélessége a ListBox-nak!\n----------------");
                Console.Write("Válasza: ");
                szelesseg = int.Parse(Console.ReadLine()); 
            }
            //-------------------------------------------------------------------------------------------------------------------

            #region -->Adat bekéréses rész<--
            AdatBekeres();
            #endregion
            #region -->Adatok beolvasásos rész<--
            string[] bemenet = File.ReadAllLines("listaszoveg.txt");
            ListBox[] mellekLista = new ListBox[bemenet.Length];
            AdatBeolvasás(mellekLista, bemenet);
            #endregion
            #region -->Adatok feltöltése Listába
            for (int i = 0; i < mellekLista.Length; i++)
            {
                foLista.Add(mellekLista[i].Elem);
            }
            #endregion
            #region -->Adatok kiíratásos rész<--
            Kiíratás(foLista,magassag);

            #endregion
        }

        #region  -->Függvények és Eljárások<--
        /// <summary>
        /// Itt olvastatom be az adatokat, hogy a feladatokban tudjak vele dolgozni
        /// </summary>
        static void AdatBeolvasás(ListBox[] mellekLista, string[] bemenet)
        {
            #region Adatok beolvasása, dolgozhatóság érdekében
            for (int i = 0; i < mellekLista.Length; i++)
            {
                mellekLista[i] = new ListBox(bemenet[i]);
            }
            #endregion
        }

        /// <summary>
        /// Kiírja a meglévő tábla adatokat
        /// </summary>
        /// <param name="adatok"></param>
        static void Kiíratás(List<string> foLista,int magassag)
        {
            Console.Clear();

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            List<int> szinezettIndex = new List<int>();
            int kezdomagassag = 0;
            var option = 0;
            var decorator = "   \u001b[32m";
            ConsoleKeyInfo key;
            bool kiválasztva = false;
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            while (true)
            {
                Console.SetCursorPosition(left, top);
                //Itt jelenírjük meg az adatokat a listából
                Console.WriteLine("--------------------------------------------\nElemek: ");
                for (int i = 0; i < foLista.Count; i++)
                {
                    //Itt ellenőrizzük, hogy a kiválasztott elemek lista tartalmazza e a for ciklus elemét
                    if (!szinezettIndex.Contains(i))
                    {
                        //Itt ellenőrizzük, hogy a jelenlegi opció, ahol a felhasználó van, egyenlőe a for ciklussal
                        if (option == i)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{(foLista[i] == string.Empty ? " " : foLista[i])}{JavitSzoveg(foLista, i)}\u001b[0m");
                        }
                        else
                        {
                            //Itt azt nézzük, ha nem egyenlő akkor, milyen legyen a háttér és a szöveg
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{(foLista[i] == string.Empty ? " " : foLista[i])}{JavitSzoveg(foLista, i)}\u001b[0m");
                        }

                    }
                    else
                    {
                        //Itt beszinezzük a hátteret szürkére, ha tartalmazza a <szinezettIndex> azt a menüpontot
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"{decorator}{foLista[i]}{JavitSzoveg(foLista, i)}\u001b[0m");
                    }




                    //Console.WriteLine($"{(option == i || szinezettIndex.Contains(i) ? decorator : "   ")}{foLista[i]} \u001b[0m");
                }
                Console.WriteLine("\nHasználd <fel> és <le> a navigáláshoz, majd nyomj egy <\u001b[32mENTER\u001b[0m>-t A választáshoz.\nKilépéshez nyomja meg a <\u001b[32mESC\u001b[0m>-t\n--------------------------------------------");
                Console.WriteLine("\nKijelölt menüpontok: {0}",KijeloltElem(szinezettIndex, foLista));
                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        option = option == 0 ? foLista.Count - 1 : option - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        option = option == foLista.Count - 1 ? 0 : option + 1;
                        break;

                    case ConsoleKey.Enter:
                        Console.Clear();
                        kiválasztva = true;
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("sSikeresen kilépett az alkalmazásból!");
                        Environment.Exit(0);
                        break;
                }
                if (kiválasztva)
                {

                    if (!szinezettIndex.Contains(option))
                    {
                        szinezettIndex.Add(option);
                        kiválasztva = false;
                    }
                    else
                    {
                        szinezettIndex.Remove(option);
                        kiválasztva = false;
                    }
                }

                //Kiválasztott elemek kiírása
                //Console.WriteLine(KijeloltElem(szinezettIndex, foLista)); 
            }
        }

        /// <summary>
        /// Azokat az értékeket adja vissza melyek, ki vannak választva
        /// </summary>
        /// <param name="szinezettIndex"></param>
        /// <returns></returns>
        static string KijeloltElem(List<int> szinezettIndex, List<string> foLista)
        {
            string visszaAdoSz = "";
            for (int i = 0; i < szinezettIndex.Count; i++)
            {
                visszaAdoSz += $"{foLista[szinezettIndex[i]]}; ";
            }
            return visszaAdoSz;
        }

        /// <summary>
        /// Itt kérjük a felhasználót, hogy adjon meg új adatott a menübe
        /// </summary>
        static void AdatBekeres()
        {
            string ellenorzo = "";
            StreamWriter kimenet;
            do
            {
                Console.Clear();
                Console.Write("Kérem adjon meg egy új menű pontott: ");
                ellenorzo = Console.ReadLine();
                if (ellenorzo != "" && ellenorzo != " ")
                {
                    kimenet = File.AppendText("listaszoveg.txt");
                    kimenet.WriteLine(ellenorzo);
                    kimenet.Close();
                }
            } while (ellenorzo != "" && ellenorzo != " ");
        }

        /// <summary>
        /// Itt egyenlítjük ki az oszlopokat, hogy egyenlő hosszúak legyenek
        /// </summary>
        /// <param name="foLista"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static string JavitSzoveg(List<string> foLista, int index)
        {
            string kimenet = "";
            for (int i = 0; i < LegHosszabbSzoveg(foLista, index) - foLista[index].Length + 1; i++)
            {
                kimenet += " ";
            }
            return kimenet;
        }

        /// <summary>
        /// Megállapítja, hog melyik a leghosszabb szöveg a Listában
        /// </summary>
        /// <param name="foLista"></param>
        /// <returns></returns>
        static int LegHosszabbSzoveg(List<string> foLista, int index)
        {
            int legH = foLista[0].Length;
            for (int i = 0; i < foLista.Count; i++)
            {
                if (foLista[i].Length >= legH)
                {
                    legH = foLista[i].Length;
                }
            }
            if (foLista[index] == "")
            {
                return legH - 1;
            }
            else
            {
                return legH;            
            }

        }
        #endregion
    }
}