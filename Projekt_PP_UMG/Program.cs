using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Projekt_PP_UMG
{
    /*
               WAŻNE !!!
              
       forma nazw plików : (ID).bin

       nazwa folderu klienta - ID klienta
    */

    public static class FunkcjeAutomatyczne
    {
        /// <summary>
        /// Dostępne foldery :
        ///  DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci
        /// </summary>
        /// <param name="nazwaFolderu"></param>
        /// <param name="IDklienta"></param>
        /// <param name="rozszerzenie"></param>
        /// <returns></returns>
        public static string ŚcieżkaFolderu(string nazwaFolderu, int IDklienta = 1)
        {
            string ścieżka = Directory.GetCurrentDirectory();

            switch (nazwaFolderu)
            {
                case "DaneLogowania":
                case "UrządzeniaInfo":
                case "AbonamentyInfo":
                case "PakietyInfo":
                case "Klienci":
                    ścieżka = Path.Combine(ścieżka, nazwaFolderu);
                    break;

                case "UrządzeniaKlienta":
                case "AbonamentyKlienta":
                case "PakietyKlienta":
                    ścieżka = Path.Combine(ścieżka, ("Klienci\\" + IDklienta + "\\" + nazwaFolderu));
                    break;
            }

            return ścieżka;
        }

        /// <summary>
        /// Dostępne foldery :
        ///  DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci
        /// </summary>
        /// <param name="nazwaFolderu"></param>
        /// <param name="IDklienta"></param>
        /// <param name="rozszerzenie"></param>
        /// <returns></returns>
        public static int LiczbaPlików(string nazwaFolderu, int IDklienta = 1, string rozszerzenie = "*.bin")
        {
            string ścieżka = ŚcieżkaFolderu(nazwaFolderu, IDklienta);
            return Directory.GetFiles(ścieżka, rozszerzenie).Length; 
        }
        
        public static int LiczbaKlientów()
        {
            string ścieżka = ŚcieżkaFolderu("Klienci");

            return Directory.GetDirectories(ścieżka).Length;
        }
        
        public static int NajwiększeIDUrzytkowników()           // ID z danych logowania; pracownicy i admini nie mają własnego folderu klient
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu("DaneLogowania");

            if (Directory.Exists(ścieżka))
            {
                foreach (string pk in Directory.GetFiles(ścieżka, "*.bin"))
                {
                    string FN = pk.Replace(".bin", "");
                    FN = FN.Replace(ścieżka+"\\", "");

                    int id1 = int.Parse(FN);
                    if (ID < id1)
                    {
                        ID = id1;
                    }
                }
            }

            return ID;
        }

        public static int NajwiększeIDKlientów()
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu("Klienci");

            foreach (string kl in Directory.GetDirectories(ścieżka))
            {
                if(int.Parse(kl) > ID)
                {
                    ID = int.Parse(kl);
                }
            }
            return ID;
        }

        /// <summary>
        /// Rodzaje przedmiotów : Urządzenia, Abonamenty, Pakiety
        /// </summary>
        /// <param name="IDklienta"></param>
        /// <param name="rodzajPrzedmiodu"></param>
        /// <returns></returns>
        public static int NajwiększeIDPrzedmiotuKlienta(int IDklienta, string rodzajPrzedmiotu)
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu((rodzajPrzedmiotu+"Klienta"), IDklienta);

            if (Directory.Exists(ścieżka))
            {
                foreach (string pk in Directory.GetFiles(ścieżka, "*.bin"))
                {
                    string FN = pk.Replace(".bin", "");
                    FN = FN.Replace(ścieżka + "\\", "");

                    int id1 = int.Parse(FN);
                    if (ID < id1)
                    {
                        ID = id1;
                    }
                }
            }

            return ID;
        }
        
        /// <summary>
        /// Rodzaje przedmiotów : Urządzenia, Abonamenty, Pakiety
        /// </summary>
        /// <param name="IDklienta"></param>
        /// <param name="rodzajPrzedmiodu"></param>
        /// <returns></returns>
        public static int NajwiększeIDOferty(string rodzajPrzedmiotu)
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu(rodzajPrzedmiotu+"Info");

            if (Directory.Exists(ścieżka))
            {
                foreach (string pk in Directory.GetFiles(ścieżka, "*.bin"))
                {
                    string FN = pk.Replace(".bin", "");
                    FN = FN.Replace(ścieżka+"\\", "");

                    int id1 = int.Parse(FN);
                    if (ID < id1)
                    {
                        ID = id1;
                    }
                }
            }

            return ID;
        }

    }

    #region Struktury danych
    public class UrządzeniaInfo
    {
        public UrządzeniaInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAutomatyczne.NajwiększeIDOferty("Urządzenia") + 1;      // automatycznie przypisuje ID zwiększone o 1 od największego ID z tego typu, jeśli nie sprecyzowano jakie ma być
            }
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public double Cena;
        public string Nazwa;
        public string Wytwórca;
        public string[]? Kolory;                                        // niewymagane
        public string[]? Warianty;                                      // niewymagane
    }
    public class AbonamentyInfo
    {
        public AbonamentyInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAutomatyczne.NajwiększeIDOferty("Abonamenty") + 1;      // automatycznie przypisuje ID zwiększone o 1 od największego ID z tego typu, jeśli nie sprecyzowano jakie ma być
            }
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public string Nazwa;
        public string CzęstotliwośćRozliczania;                       // zakładam możliwości: tyg, msc, rok (każde ma 3 litery dla łatwości póżniejszego sprawdzania)
        public double Cena;
        public double LimitInternetu = 0;                             // 0 dla braku, -1 dla nielimitowanego, >0 dla normalneog limitu
        public double[] LimityInternetu = {0, 0};                    // prędkość przed i po wyczerpaniu limitu
    }
    public class PakietyInfo
    {
        public PakietyInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAutomatyczne.NajwiększeIDOferty("Pakiety") + 1;      // automatycznie przypisuje ID zwiększone o 1 od największego ID z tego typu, jeśli nie sprecyzowano jakie ma być
            }
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public double Cena;
        public string Nazwa;
        public int[] TelefonyID;                                             // ID brane z oferty
        public int AbonamentID;
        public double CzasTrwania;                                           // Na ile opłaca abonament, wyrażane w ilości "cykli" abonamentu (np. tygodni jeśli opłacany tygodniowo)
        public double Przecena = 0;                                          // przecena na abonament, w ułamku diesiętnym, 0 dla braku
    }
    public class DaneLogowania
    {
        public DaneLogowania(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAutomatyczne.NajwiększeIDUrzytkowników() + 1;
            }
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public string Imię;
        public string Nazwisko;
        public DateTime DataUrodzenia;
        public bool Admin = false;
    }
    public class KlientInfo
    {
        public KlientInfo()
        {
            int tempID = 1;
            // automatycznie przypisuje ID (pierwsze 3 liczby zależne od klasy, reszta zwiększona o 1 od największego ID z tego typu)
            this.IDvalue = tempID;
        }

        private int IDvalue = 0;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public string Imię;
        public string Nazwisko;
        public DateTime DataUrodzenia;
        public string Email;
    }
    public class UrządzenieKlienta
    {
        public UrządzenieKlienta()
        {
            int tempID = 1;
            // automatycznie przypisuje ID (pierwsze 3 liczby zależne od klasy, reszta zwiększona o 1 od największego ID z tego typu)
            this.IDvalue = tempID;
        }

        private int IDvalue = 0;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public double Cena;
        public string Nazwa;
        public string Wytwórca;
        public string?[] Kolory;                                        // niewymagane
        public string?[] Warianty;                                      // niewymagane
    }
    public class AbonamentKienta
    {
        public AbonamentKienta()
        {
            int tempID = 1;
            // automatycznie przypisuje ID (pierwsze 3 liczby zależne od klasy, reszta zwiększona o 1 od największego ID z tego typu)
            this.IDvalue = tempID;
        }

        private int IDvalue = 0;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public double Cena;
        public string Nazwa;
        public string Wytwórca;
        public string?[] Kolory;                                        // niewymagane
        public string?[] Warianty;                                      // niewymagane
    }
    public class PakietKlienta
    {
        public PakietKlienta()
        {
            int tempID = 1;
            // automatycznie przypisuje ID (pierwsze 3 liczby zależne od klasy, reszta zwiększona o 1 od największego ID z tego typu)
            this.IDvalue = tempID;
        }

        private int IDvalue = 0;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public double Cena;
        public string Nazwa;
        public string Wytwórca;
        public string?[] Kolory;                                        // niewymagane
        public string?[] Warianty;                                      // niewymagane
    }
    #endregion

    internal class Program
    {
        private bool DostępAdmina = false;
        private static int KodAdminów = 123;

        public static int ZobaczKod()
        {
            return KodAdminów;
        }

        public static void DodajUżytkownika()
        {
            int ostatnieID = FunkcjeAutomatyczne.NajwiększeIDKlientów();
            KlientInfo nowyKlient = new();

            Console.Write("  Podaj login : ");
            Console.ReadLine();
            Console.Write("\n  Podaj hasło : ");
            Console.ReadLine();
            Console.Write("\n  Podaj email : ");
            Console.ReadLine();
            Console.Write("\n  Zarejestrować jako admin? (Y/N) : ");
            string? admin = Console.ReadLine();

            if (admin == "Y" || admin == "y")
            {
                Console.Write("\n  Podaj kod admina : ");
                string? kod = Console.ReadLine();

                if (kod == Convert.ToString(ZobaczKod()))
                {

                }
            }
        }
        public static void Login()
        {
            /*
                Tu dodać kod który liczy ile użytkowników i tworzy ich tablicę 
            */
            Console.Write("  Podaj login : ");
            Console.Write("\n  Podaj hasło : ");
            /*
                Sprawdza czy użytkownik istnieje, czy hasło i login sie zgadzają i sprawdza uprawnienia
            */
        }

        static void Main(string[] args)
        {
            //Console.WriteLine(FunkcjeAutomatyczne.NajwiększeIDOferty("Abonamenty"));

            //Console.WriteLine(FunkcjeAutomatyczne.NajwiększeIDPrzedmiotuKlienta(112,"Urządzenia"));

            //Console.WriteLine(Directory.GetCurrentDirectory());

            KlientInfo KI = new KlientInfo();
            Console.WriteLine(KI.ID);
        }
    }
}