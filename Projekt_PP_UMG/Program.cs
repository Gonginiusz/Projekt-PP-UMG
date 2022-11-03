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
        public static int LiczbaUrzytkowników()
        {
            string ścieżka = ŚcieżkaFolderu("DaneLogowania");

            return Directory.GetDirectories(ścieżka).Length;
        }

        public static int NajwiększeIDUrzytkowników()                   // ID z danych logowania; pracownicy i admini nie mają własnego folderu klient
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu("DaneLogowania");

            if (Directory.Exists(ścieżka))
            {
                foreach (string pk in Directory.GetFiles(ścieżka, "*.bin"))
                {
                    string FN = pk.Replace(".bin", "");
                    FN = FN.Replace(ścieżka+"\\", "");

                    Console.WriteLine("Działa ID urz.");

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
                string FN = kl.Replace(".bin", "");
                FN = FN.Replace(ścieżka + "\\", "");

                if(int.Parse(FN) > ID)
                {
                    ID = int.Parse(FN);
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
        public string CzęstotliwośćRozliczania;                         // zakładam możliwości: tyg, msc, rok (każde ma 3 litery dla łatwości póżniejszego sprawdzania)
        public double Cena;
        public double LimitInternetu = 0;                               // 0 dla braku, -1 dla nielimitowanego, >0 dla normalneog limitu
        public double[] LimityInternetu = {0, 0};                       // prędkość przed i po wyczerpaniu limitu
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
        public int[] TelefonyID;                                        // ID brane z oferty urządzeń
        public int AbonamentID = 0;                                     // ID oferyty przypisanego przy zakupie do telefonów abonamentu
        public double CzasTrwania = 0;                                  // Na ile opłaca abonament, wyrażane w ilości "cykli" abonamentu (np. tygodni jeśli opłacany tygodniowo)
        public double Przecena = 0;                                     // przecena na abonament, w ułamku diesiętnym, 0 dla braku
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
        public KlientInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAutomatyczne.NajwiększeIDKlientów() + 1;
            }
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
        public UrządzenieKlienta(bool przypiszIDAutomatycznie, int IDKlientaLubPrzedmiotu, int IDOferty)
        {
            if (przypiszIDAutomatycznie)
            {
                IDKlientaLubPrzedmiotu = FunkcjeAutomatyczne.NajwiększeIDPrzedmiotuKlienta(IDKlientaLubPrzedmiotu, "Urządzenia") + 1;
            }
            this.IDvalue = IDKlientaLubPrzedmiotu;
            this.IDOferty = IDOferty;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public int IDOferty;                                            // ID oferty(info) z której pochodzi ten telefon
        public string Kolor = "czarny";
        public string Wariant = "normalny";  // przykładowo
        public int IDAbonamentu = 0;                                    // jaki ma przypisany abonament (jesli ma)
        public int IDPakietu = 0;                                       // jaki ma przypisany pakiet (jesli ma)
    }
    public class AbonamentKienta
    {
        public AbonamentKienta(bool przypiszIDAutomatycznie, int IDKlientaLubPrzedmiotu, int IDOferty)
        {
            if (przypiszIDAutomatycznie)
            {
                IDKlientaLubPrzedmiotu = FunkcjeAutomatyczne.NajwiększeIDPrzedmiotuKlienta(IDKlientaLubPrzedmiotu, "Abonamenty") + 1;
            }
            this.IDvalue = IDKlientaLubPrzedmiotu;
            this.IDOferty = IDOferty;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin jest zalogowany
        }

        public int IDOferty;                                            // ID oferty(info) z której pochodzi ten abonament
        public int NaIleOpłaconoDoPrzodu = 0;                           // czy i na ile opłacone do przodu - ( <0 = zaległość z zapłatą, 0 = za bierząco z opłatą, >0 = opłacone do przodu, np. przez pakiet)
        public DateTime DataNastępnejOpłaty;                            // jeśli opłacono wynosi 0 lub <0, to normalna data zwględem zakupu; jeśli >0, to o ileś okresów płacenia do przodu
        public int IDPakietu = 0;                                       // jaki ma przypisany pakiet (jesli ma)
    }
    public class PakietKlienta
    {
        public PakietKlienta(bool przypiszIDAutomatycznie,int IDKlientaLubPrzedmiotu, int IDOferty)
        {
            if (przypiszIDAutomatycznie)
            {
                IDKlientaLubPrzedmiotu = FunkcjeAutomatyczne.NajwiększeIDPrzedmiotuKlienta(IDKlientaLubPrzedmiotu, "Pakiety") + 1;
            }
            this.IDvalue = IDKlientaLubPrzedmiotu;
            this.IDOferty = IDOferty;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public int IDOferty;                                            // ID oferty(info) z której pochodzi ten abonament
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
            DaneLogowania loginHaslo = new();
            KlientInfo nowyKlient = new KlientInfo(loginHaslo.ID);

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

            /*
            #region Test1
            DateTime wiek = new DateTime();
            DateTime now = DateTime.Now;

            string rok = "2005";
            string mieisąc = "11";
            string dzień = "6";

            int[] czasy = { int.Parse(rok), int.Parse(mieisąc), int.Parse(dzień) };

            wiek = wiek.AddYears(now.Year - czasy[0]);
            wiek = wiek.AddMonths(now.Month - czasy[1]);
            wiek = wiek.AddDays(now.Day - czasy[2]);
            #endregion
            */

            try
            {
                KlientInfo KI = new KlientInfo();
                Console.WriteLine(KI.ID);
            }
            catch (Exception)
            {
                Console.WriteLine("Chuj");
            }
        }
    }
}