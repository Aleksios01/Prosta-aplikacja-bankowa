using System;
using System.Collections.Generic;

namespace AplikacjaBankowa
{

    public abstract class Karta
    {
        public string Numer { get; set; }
        public KontoBankowe PrzypisaneKonto { get; set; }

        public Karta(string numer, KontoBankowe konto)
        {
            Numer = numer;
            PrzypisaneKonto = konto;
        }

        public abstract void Zaplac(decimal kwota);
    }

    public class KartaDebetowa : Karta
    {
        public KartaDebetowa(string numer, KontoBankowe konto) : base(numer, konto) { }

        public override void Zaplac(decimal kwota)
        {
            Console.WriteLine($"\n[Karta Debetowa {Numer}] Próba płatności: {kwota} zł...");
            PrzypisaneKonto.Wyplat(kwota);
        }
    }

    public class KartaKredytowa : Karta
    {
        public decimal LimitKredytowy { get; set; }
        private decimal wykorzystanyLimit = 0;

        public KartaKredytowa(string numer, KontoBankowe konto, decimal limit) : base(numer, konto)
        {
            LimitKredytowy = limit;
        }

        public override void Zaplac(decimal kwota)
        {
            Console.WriteLine($"\n[Karta Kredytowa {Numer}] Próba płatności: {kwota} zł...");
            if (wykorzystanyLimit + kwota > LimitKredytowy)
            {
                Console.WriteLine("BŁĄD: Przekroczono limit karty kredytowej!");
            }
            else
            {
                wykorzystanyLimit += kwota;
                Console.WriteLine($"Płatność udana. Wykorzystany limit: {wykorzystanyLimit}/{LimitKredytowy} zł.");
            }
        }
    }

    public class KontoBankowe
    {
        public string NumerKonta { get; set; }
        public decimal Saldo { get; private set; } 

        public KontoBankowe(string numerKonta, decimal poczatkoweSaldo)
        {
            NumerKonta = numerKonta;
            Saldo = poczatkoweSaldo;
        }

        public void Wplat(decimal kwota)
        {
            if (kwota <= 0)
            {
                Console.WriteLine("BŁĄD: Nie można wpłacić kwoty ujemnej lub zero!");
                return;
            }
            Saldo += kwota;
            Console.WriteLine($"Wpłacono {kwota} zł. Nowe saldo konta {NumerKonta}: {Saldo} zł.");
        }

        public bool Wyplat(decimal kwota)
        {
            if (kwota <= 0)
            {
                Console.WriteLine("BŁĄD: Niepoprawna kwota wypłaty!");
                return false;
            }
            if (kwota > Saldo)
            {
                Console.WriteLine($"BŁĄD: Brak środków na koncie {NumerKonta}. Saldo: {Saldo} zł.");
                return false;
            }
            Saldo -= kwota;
            Console.WriteLine($"Wypłacono {kwota} zł. Nowe saldo konta {NumerKonta}: {Saldo} zł.");
            return true;
        }

        public void Przelew(KontoBankowe kontoDocelowe, decimal kwota)
        {
            if (Wyplat(kwota))
            {
                kontoDocelowe.Wplat(kwota);
                Console.WriteLine("Przelew zakończony sukcesem.");
            }
            else
            {
                Console.WriteLine("BŁĄD: Przelew nie mógł zostać zrealizowany.");
            }
        }
    }

    public class KontoOszczednosciowe : KontoBankowe
    {
        public KontoOszczednosciowe(string numerKonta, decimal poczatkoweSaldo) 
            : base(numerKonta, poczatkoweSaldo) { }
    }

    public class Klient
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public List<KontoBankowe> Konta { get; set; } = new List<KontoBankowe>();
        public List<Karta> Karty { get; set; } = new List<Karta>();

        public Klient(string imie, string nazwisko)
        {
            Imie = imie;
            Nazwisko = nazwisko;
        }

        public void PokazSzczegoly()
        {
            Console.WriteLine($"\n=== KLIENT: {Imie} {Nazwisko} ===");
            Console.WriteLine("Konta:");
            foreach (var k in Konta)
            {
                string typ = k is KontoOszczednosciowe ? "Oszczędnościowe" : "Zwykłe";
                Console.WriteLine($" - [{typ}] Nr: {k.NumerKonta}, Saldo: {k.Saldo} zł");
            }
            Console.WriteLine("Karty:");
            foreach (var karta in Karty)
            {
                string typ = karta is KartaKredytowa ? "Kredytowa" : "Debetowa";
                Console.WriteLine($" - [{typ}] Nr: {karta.Numer}, Przypisana do konta: {karta.PrzypisaneKonto.NumerKonta}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Klient jan = new Klient("Jan", "Kowalski");
            KontoBankowe kontoJana = new KontoBankowe("PL111", 500);
            KontoOszczednosciowe oszczednosciJana = new KontoOszczednosciowe("PL222", 1000);
            KartaDebetowa kartaJana = new KartaDebetowa("CARD-999", kontoJana);
            jan.Konta.Add(kontoJana);
            jan.Konta.Add(oszczednosciJana);
            jan.Karty.Add(kartaJana);

            Klient anna = new Klient("Anna", "Nowak");
            KontoBankowe kontoAnny = new KontoBankowe("PL333", 200);
            anna.Konta.Add(kontoAnny);

            bool uruchomiony = true;

            while (uruchomiony)
            {
                Console.WriteLine("\n=== SYSTEM BANKOWY ===");
                Console.WriteLine("1. Pokaż stan kont i szczegóły klientów");
                Console.WriteLine("2. Wpłać pieniądze (Jan - Konto Zwykłe)");
                Console.WriteLine("3. Wypłać pieniądze (Jan - Konto Zwykłe)");
                Console.WriteLine("4. Wyślij przelew (Jan oszczędnościowe -> Anna)");
                Console.WriteLine("5. Zapłać kartą debetową (Jan)");
                Console.WriteLine("0. Wyjdź z programu");
                Console.Write("Wybierz opcję: ");

                string wybor = Console.ReadLine();

                switch (wybor)
                {
                    case "1":
                        jan.PokazSzczegoly();
                        anna.PokazSzczegoly();
                        break;
                    case "2":
                        Console.Write("Podaj kwotę do wpłaty na konto Jana: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal kwotaWplaty))
                            kontoJana.Wplat(kwotaWplaty);
                        else
                            Console.WriteLine("Błędny format kwoty.");
                        break;
                    case "3":
                        Console.Write("Podaj kwotę do wypłaty z konta Jana: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal kwotaWyplaty))
                            kontoJana.Wyplat(kwotaWyplaty);
                        else
                            Console.WriteLine("Błędny format kwoty.");
                        break;
                    case "4":
                        Console.Write("Podaj kwotę przelewu od Jana (oszcz.) do Anny: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal kwotaPrzelewu))
                            oszczednosciJana.Przelew(kontoAnny, kwotaPrzelewu);
                        else
                            Console.WriteLine("Błędny format kwoty.");
                        break;
                    case "5":
                        Console.Write("Podaj kwotę płatności kartą Jana: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal kwotaKarty))
                            kartaJana.Zaplac(kwotaKarty);
                        else
                            Console.WriteLine("Błędny format kwoty.");
                        break;
                    case "0":
                        uruchomiony = false;
                        Console.WriteLine("Dziękujemy za skorzystanie z systemu.");
                        break;
                    default:
                        Console.WriteLine("Nieznana opcja, spróbuj ponownie.");
                        break;
                }
                
                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
} 
