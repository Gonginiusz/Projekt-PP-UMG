using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Diagnostics;
using static Projekt_PP_UMG.FunkcjeAuto;

namespace Projekt_PP_UMG
{

    public static class FunkcjeAuto
    {
        /// <summary>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci</para>
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
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci</para>
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

            return Directory.GetFiles(ścieżka, "*.bin").Length;
        }

        /// <summary>
        /// <para>Lista ID plików (lub folderów w przypadku opcji "Klienci") w wybranym folderze</para>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta, Klienci</para>
        /// </summary>
        /// <returns></returns>
        public static int[] ListaID(string folder)
        {
            int[] lista;
            string ścieżka = ŚcieżkaFolderu(folder);
            string[] nazwy;

            if (folder != "Klienci")
            {
                nazwy = Directory.GetFiles(ścieżka, "*.bin");
            }
            else
            {
                nazwy = Directory.GetDirectories(ścieżka);
            }

            lista = new int[nazwy.Length];
            int i = 0;

            foreach (string nazwa in nazwy)
            {
                string nazwa2 = nazwa.Replace(@".bin", "");
                string nazwa3 = nazwa2.Replace(ścieżka + @"\", "");
                lista[i++] = int.Parse(nazwa3);
            }

            return lista;
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

        public static int NajwiększeIDKlientów()
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu("Klienci");

            foreach (string kl in Directory.GetDirectories(ścieżka))
            {
                string FN = kl.Replace(".bin", "");
                FN = FN.Replace(ścieżka + "\\", "");

                if (int.Parse(FN) > ID)
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
        public static int NajwiększeIDPrzedmiotuKlienta(string rodzajPrzedmiotu, int IDklienta)
        {
            int ID = 0;       // minimalne ID - 1
            string ścieżka = ŚcieżkaFolderu((rodzajPrzedmiotu + "Klienta"), IDklienta);

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
            string ścieżka = ŚcieżkaFolderu(rodzajPrzedmiotu + "Info");

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

        public static void CzyIstniejąWszystkieFoldery()
        {
            string aktualnaŚcieżka = Directory.GetCurrentDirectory();
            string[] nazwyFolderów = { "DaneLogowania", "UrządzeniaInfo", "AbonamentyInfo", "PakietyInfo", "Klienci" };

            foreach (string nazwaF in nazwyFolderów)
            {
                if (!Directory.Exists(aktualnaŚcieżka + @"\" + nazwaF))
                {
                    Directory.CreateDirectory(aktualnaŚcieżka + @"\" + nazwaF);
                }
            }
        }

        /// <summary>
        /// <para>Tworzy plik z załączonej struktury danych, podać kolejno : Strukturę do zapisania, ID służące jako nazwa, ID klienta jeśli jest potrzebne.   </para>
        /// </summary>
        /// <param name="Iteracja"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static void ZapiszPlik(object Iteracja, int ID, int IDKlienta = -1)
        {
            ZapisywaniePlików zp = new ZapisywaniePlików(Iteracja, ID, IDKlienta);
        }

        /// <summary>
        /// <para>Ładuje plik, podać kolejno : nazwę folderu, nazwę folderu, ID służące jako nazwa, ID klienta jeśli jest potrzebnea(dla 3 ostatnich folderów).   </para>
        /// <para>Dostępne foldery :</para>
        /// <para>DaneLogowania,  UrządzeniaInfo,  AbonamentyInfo,  PakietyInfo,  UrządzeniaKlienta,  AbonamentyKlienta,  PakietyKlienta</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        /// 
        public static object WczytajPlik(string folder, int ID, int IDKlienta = -1)
        {
            ŁadowaniePlików łp = new ŁadowaniePlików(folder, ID, IDKlienta);
            return łp.WczytanyPlik;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static DaneLogowania[] WczytajWszystkiePliki(DaneLogowania[] pustaInstancja)
        {
            string typPliku = "DaneLogowania";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new DaneLogowania[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (DaneLogowania)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static UrządzeniaInfo[] WczytajWszystkiePliki(UrządzeniaInfo[] pustaInstancja)
        {
            string typPliku = "UrządzeniaInfo";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new UrządzeniaInfo[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (UrządzeniaInfo)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static AbonamentyInfo[] WczytajWszystkiePliki(AbonamentyInfo[] pustaInstancja)
        {
            string typPliku = "AbonamentyInfo";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new AbonamentyInfo[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (AbonamentyInfo)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static PakietyInfo[] WczytajWszystkiePliki(PakietyInfo[] pustaInstancja)
        {
            string typPliku = "PakietyInfo";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new PakietyInfo[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (PakietyInfo)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static UrządzenieKlienta[] WczytajWszystkiePliki(UrządzenieKlienta[] pustaInstancja)
        {
            string typPliku = "UrządzeniaKlienta";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new UrządzenieKlienta[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (UrządzenieKlienta)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static AbonamentKlienta[] WczytajWszystkiePliki(AbonamentKlienta[] pustaInstancja)
        {
            string typPliku = "AbonamentyKlienta";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new AbonamentKlienta[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (AbonamentKlienta)o;
                i++;
            }
            return pustaInstancja;
        }

        /// <summary>
        /// <para>Ładuje wszystkie pliki, wystarczy podać pusta tabelą odpowiedniego typu</para>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ID"></param>
        /// <param name="IDKlienta"></param>
        public static PakietKlienta[] WczytajWszystkiePliki(PakietKlienta[] pustaInstancja)
        {
            string typPliku = "PakietyKlienta";
            ŁadowanieWszystkichPlików łwp = new ŁadowanieWszystkichPlików(typPliku);
            pustaInstancja = new PakietKlienta[łwp.ListaDanych.Length];
            int i = 0;

            foreach (object o in łwp.ListaDanych)
            {
                pustaInstancja[i] = (PakietKlienta)o;
                i++;
            }
            return pustaInstancja;
        }
    }

    #region Struktury danych
    [Serializable]
    public class UrządzeniaInfo
    {
        public UrządzeniaInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAuto.NajwiększeIDOferty("Urządzenia") + 1;      // automatycznie przypisuje ID zwiększone o 1 od największego ID z tego typu, jeśli nie sprecyzowano jakie ma być
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
    [Serializable]
    public class AbonamentyInfo
    {
        public AbonamentyInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAuto.NajwiększeIDOferty("Abonamenty") + 1;      // automatycznie przypisuje ID zwiększone o 1 od największego ID z tego typu, jeśli nie sprecyzowano jakie ma być
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
        public double[] LimityInternetu = { 0, 0 };                       // prędkość przed i po wyczerpaniu limitu
    }
    [Serializable]
    public class PakietyInfo
    {
        public PakietyInfo(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAuto.NajwiększeIDOferty("Pakiety") + 1;      // automatycznie przypisuje ID zwiększone o 1 od największego ID z tego typu, jeśli nie sprecyzowano jakie ma być
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
    [Serializable]
    public class DaneLogowania
    {
        public DaneLogowania(int tempID = -1)
        {
            if (tempID == -1)
            {
                tempID = FunkcjeAuto.NajwiększeIDUrzytkowników() + 1;
            }
            this.IDvalue = tempID;
        }

        private int IDvalue = 1;

        public int ID
        {
            get { return IDvalue; }
            set { IDvalue = value; }    // modyfikacja po dodaniu; dodać sprawdzenie czy admin
        }

        public string Login { get; set; }
        public string Hasło { get; set; }
        public string Imię;
        public string Nazwisko;
        public string Email;
        public DateTime DataUrodzenia;
        public bool Admin = false;
    }
    [Serializable]
    public class UrządzenieKlienta
    {
        public UrządzenieKlienta(bool przypiszIDAutomatycznie, int IDKlientaLubPrzedmiotu, int IDOferty)
        {
            if (przypiszIDAutomatycznie)
            {
                IDKlientaLubPrzedmiotu = FunkcjeAuto.NajwiększeIDPrzedmiotuKlienta("Urządzenia", IDKlientaLubPrzedmiotu) + 1;
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
    [Serializable]
    public class AbonamentKlienta
    {
        public AbonamentKlienta(bool przypiszIDAutomatycznie, int IDKlientaLubPrzedmiotu, int IDOferty)
        {
            if (przypiszIDAutomatycznie)
            {
                IDKlientaLubPrzedmiotu = FunkcjeAuto.NajwiększeIDPrzedmiotuKlienta("Abonamenty", IDKlientaLubPrzedmiotu) + 1;
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
    [Serializable]
    public class PakietKlienta
    {
        public PakietKlienta(bool przypiszIDAutomatycznie, int IDKlientaLubPrzedmiotu, int IDOferty)
        {
            if (przypiszIDAutomatycznie)
            {
                IDKlientaLubPrzedmiotu = FunkcjeAuto.NajwiększeIDPrzedmiotuKlienta("Pakiety", IDKlientaLubPrzedmiotu) + 1;
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
    public class ZapisywaniePlików
    {
        public ZapisywaniePlików(object Iteracja, int ID, int IDKlienta = -1)
        {
            Type typ = Iteracja.GetType();
            string folder = "";

            if (typ.Equals(typeof(DaneLogowania)))
            {
                folder = "DaneLogowania";
            }
            else if (typ.Equals(typeof(UrządzeniaInfo)))
            {
                folder = "UrządzeniaInfo";
            }
            else if (typ.Equals(typeof(AbonamentyInfo)))
            {
                folder = "AbonamentyInfo";
            }
            else if (typ.Equals(typeof(PakietyInfo)))
            {
                folder = "PakietyInfo";
            }
            else if (typ.Equals(typeof(UrządzenieKlienta)))
            {
                folder = "UrządzeniaKlienta";
            }
            else if (typ.Equals(typeof(AbonamentKlienta)))
            {
                folder = "AbonamentyKlienta";
            }
            else if (typ.Equals(typeof(PakietKlienta)))
            {
                folder = "PakietyKlienta";
            }

            string ścieżka = FunkcjeAuto.ŚcieżkaFolderu(folder, IDKlienta);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream($@"{ścieżka}\{ID}.bin", FileMode.Create, FileAccess.Write, FileShare.None);

            formatter.Serialize(stream, Iteracja);

            stream.Close();
        }
    }
    public class ŁadowaniePlików
    {
        public object WczytanyPlik;

        public ŁadowaniePlików(string folder, int ID, int IDKlienta = -1)
        {
            string ścieżka = FunkcjeAuto.ŚcieżkaFolderu(folder, IDKlienta);
            IFormatter formatter = new BinaryFormatter();

            try
            {
                Stream stream = new FileStream($@"{ścieżka}\{ID}.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                switch (folder)
                {
                    case "DaneLogowania":
                        this.WczytanyPlik = (DaneLogowania)formatter.Deserialize(stream);
                        break;
                    case "UrządzeniaInfo":
                        this.WczytanyPlik = (UrządzeniaInfo)formatter.Deserialize(stream);
                        break;
                    case "AbonamentyInfo":
                        this.WczytanyPlik = (AbonamentyInfo)formatter.Deserialize(stream);
                        break;
                    case "PakietyInfo":
                        this.WczytanyPlik = (PakietyInfo)formatter.Deserialize(stream);
                        break;
                    case "UrządzeniaKlienta":
                        this.WczytanyPlik = (UrządzenieKlienta)formatter.Deserialize(stream);
                        break;
                    case "AbonamentyKlienta":
                        this.WczytanyPlik = (AbonamentKlienta)formatter.Deserialize(stream);
                        break;
                    case "PakietyKlienta":
                        this.WczytanyPlik = (PakietKlienta)formatter.Deserialize(stream);
                        break;
                }

                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd wczytywania pliku, \n" + e.Message);
            }
        }
    }
    public class ŁadowanieWszystkichPlików
    {
        public object[] ListaDanych;
        public ŁadowanieWszystkichPlików(string typPliku)
        {
            ŁadowaniePlików łp;
            int[] listaID = FunkcjeAuto.ListaID(typPliku);
            this.ListaDanych = new object[listaID.Length];
            int i = 0;

            switch (typPliku)
            {
                case "DaneLogowania":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (DaneLogowania)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                case "UrządzeniaInfo":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (UrządzeniaInfo)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                case "AbonamentyInfo":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (AbonamentyInfo)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                case "PakietyInfo":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (PakietyInfo)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                case "UrządzeniaKlienta":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (UrządzenieKlienta)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                case "AbonamentyKlienta":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (AbonamentKlienta)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                case "PakietyKlienta":
                    {
                        while (i < listaID.Length)
                        {
                            łp = new ŁadowaniePlików(typPliku, listaID[i]);
                            this.ListaDanych[i] = (PakietKlienta)łp.WczytanyPlik;
                            i++;
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("  Nie istnieje taki typ pliku");
                        break;
                    }
            }

        }
    }

    internal class Program
    {
        private bool DostępAdmina = false;
        private static string KodAdminów = "123";
        public static void WyczyścEkran()
        {
            Stopwatch czasomierz = new Stopwatch();

            for (int i = 0; i <= Console.WindowHeight; i++)
            {
                while (true)
                {
                    czasomierz.Start();
                    if (czasomierz.ElapsedMilliseconds >= 10)
                    {
                        Console.WriteLine("");
                        czasomierz.Restart();
                        break;
                    }
                }
            }
        }
        public static string ZobaczKod()
        {
            return KodAdminów;
        }

        public static void TworzenieUrzytkownika(DaneLogowania dane, int ID, bool tworzyćKlienta)
        {
            ZapisywaniePlików zp = new ZapisywaniePlików(dane, ID);

            if (tworzyćKlienta)
            {
                Directory.CreateDirectory(FunkcjeAuto.ŚcieżkaFolderu("UrządzeniaKlienta", ID));
                Directory.CreateDirectory(FunkcjeAuto.ŚcieżkaFolderu("AbonamentyKlienta", ID));
                Directory.CreateDirectory(FunkcjeAuto.ŚcieżkaFolderu("PakietyKlienta", ID));
            }
        }

        public static DaneLogowania? DodajUżytkownika()
        {
            DaneLogowania nowyU = new();
            DaneLogowania[] listaDL = new DaneLogowania[1];
            listaDL = FunkcjeAuto.WczytajWszystkiePliki(listaDL);
            string[] listaLoginów = new string[listaDL.Length];
            string[] listaEmaili = new string[listaDL.Length];
            int i = 0;
            bool powtórka;

            foreach (DaneLogowania dana in listaDL)
            {
                listaLoginów[i] = dana.Login;
                listaEmaili[i] = dana.Email;
                i++;
            }

            i = 0;

            WyczyścEkran();
            Console.WriteLine("Tworzenie nowego urzytkownika : \n");

            do
            {
                powtórka = false;

                Console.Write("  Podaj login : ");
                nowyU.Login = Console.ReadLine();

                foreach (string login in listaLoginów)
                {
                    if (login == nowyU.Login)
                    {
                        powtórka = true;
                        Console.Write("\n    Login zajęty");

                        if ((i++) % 3 == 0)
                        {
                            Console.Write("\n      Wciśnij escape (esc) aby wyjść");
                            ConsoleKeyInfo kij = Console.ReadKey(true);

                            if (kij.Key == ConsoleKey.Escape)
                            {
                                return null;
                            }
                        }
                        break;
                    }
                }
            }
            while (powtórka);

            Console.Write("\n  Podaj hasło : ");
            nowyU.Hasło = Console.ReadLine();

            i = 0;

            Console.Write("\n  Podaj imię : ");
            nowyU.Imię = Console.ReadLine();

            Console.Write("\n  Podaj nazwisko : ");
            nowyU.Nazwisko = Console.ReadLine();

            do
            {
                powtórka = false;

                Console.Write("\n  Podaj email : ");
                nowyU.Email = Console.ReadLine();

                foreach (string mail in listaEmaili)
                {
                    if (mail == nowyU.Email)
                    {
                        powtórka = true;
                        Console.WriteLine("\n    Email zajęty");

                        if ((i++) % 3 == 0)
                        {
                            Console.WriteLine("      Wciśnij escape (esc) aby wyjść");
                            ConsoleKeyInfo kij = Console.ReadKey(true);

                            if (kij.Key == ConsoleKey.Escape)
                            {
                                return null;
                            }
                        }
                        break;
                    }
                }
            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj rok urodzenia : ");
                    nowyU.DataUrodzenia = nowyU.DataUrodzenia.AddYears(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj mieisąc urodzenia : ");
                    nowyU.DataUrodzenia = nowyU.DataUrodzenia.AddMonths(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);

            do
            {
                try
                {
                    powtórka = false;
                    Console.Write("\n  Podaj dzień urodzenia : ");
                    nowyU.DataUrodzenia = nowyU.DataUrodzenia.AddDays(int.Parse(Console.ReadLine()) - 1);
                }
                catch (Exception)
                {
                    powtórka = true;
                }

            }
            while (powtórka);



            Console.Write("\n  Zarejestrować jako admin? Pamiętaj, że admini nie mają folderu Klienta(Y/N) : ");
            ConsoleKeyInfo admin = Console.ReadKey(true);

            if (admin.Key == ConsoleKey.Y)
            {

                while (true)
                {
                    Console.Write("\n  Podaj kod admina : ");
                    string? kod = Console.ReadLine();

                    if (kod == ZobaczKod())
                    {
                        nowyU.Admin = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n    Nieprawidłowy kod admina, kontynuować? (Y/N)");
                        ConsoleKeyInfo wybór = Console.ReadKey(true);

                        if (wybór.Key != ConsoleKey.Y)
                        {
                            break;
                        }
                    }
                }
            }

            TworzenieUrzytkownika(nowyU, nowyU.ID, !nowyU.Admin);

            return nowyU;
        }
        public static DaneLogowania? Login()
        {
            DaneLogowania[] listaDL = null;
            listaDL = FunkcjeAuto.WczytajWszystkiePliki(listaDL);
            DaneLogowania? danaZalogowania = new DaneLogowania();
            string[] listaLoginów = new string[listaDL.Length];
            string[] listaHaseł = new string[listaDL.Length];
            int i = 0;
            bool powtórka;

            WyczyścEkran();
            Console.WriteLine("Wpisz \"utwórz\" w miejsce loginu lub hasła, by stworzyć nowy profil ");

            do
            {
                powtórka = false;

                Console.Write("  Podaj login : ");
                string? login = Console.ReadLine();

                if (login == "utwórz")
                {
                    danaZalogowania = DodajUżytkownika();
                    if (danaZalogowania == null)
                    {
                        WyczyścEkran();
                        Console.Write("    Zrezygnowano z tworzenia urzytkownika, kontynuować logowanie? (Y/N) ");
                        ConsoleKeyInfo kij = Console.ReadKey();
                        Console.WriteLine();

                        if (kij.Key != ConsoleKey.N)
                        {
                            powtórka = true;
                            continue;
                        }
                    }
                    break;
                }

                Console.Write("\n  Podaj hasło : ");
                string? hasło = Console.ReadLine();

                if (hasło == "utwórz")
                {
                    danaZalogowania = DodajUżytkownika();
                    if (danaZalogowania == null)
                    {
                        WyczyścEkran();
                        Console.Write("Zrezygnowano z tworzenia urzytkownika, kontynuować logowanie? (Y/N) ");
                        ConsoleKeyInfo kij = Console.ReadKey();
                        Console.WriteLine();

                        if (kij.Key != ConsoleKey.N)
                        {
                            powtórka = true;
                            continue;
                        }
                    }
                    break;
                }

                Console.WriteLine();

                if (listaDL.Length == 0)
                {
                    Console.WriteLine("Nie istnieją żadni urzytkownicy, utwórz nowego urzytkownika podając \"utwórz\" w miejsce loginu lub hasła ");
                    powtórka = true;
                }
                else if (!powtórka)
                {
                    foreach (DaneLogowania dana in listaDL)
                    {
                        if (dana.Login == login && dana.Hasło == hasło)
                        {
                            danaZalogowania = dana;
                            powtórka = false;
                            return danaZalogowania;
                        }
                        else
                        {
                            powtórka = true;
                        }
                    }

                    Console.WriteLine("Błedny login lub hasło");
                }
            }
            while (powtórka);

            return danaZalogowania;
        }

        static void Main(string[] args)
        {
            FunkcjeAuto.CzyIstniejąWszystkieFoldery();

            DaneLogowania? zalogowanyUrzytkownik = Login();

            if (zalogowanyUrzytkownik == null)
            {
                Console.WriteLine("Anulowano logowanie");
            }
        }
    }
}