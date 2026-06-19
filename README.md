# Prosta-aplikacja-bankowa
Aplikacja konsolowa w języku C# symulująca uproszczony system bankowy. Projekt skupia się na praktycznym wykorzystaniu fundamentów programowania obiektowego (OOP).

## Wykorzystane mechanizmy OOP:
* **Hermetyzacja** – pole `Saldo` w klasie `KontoBankowe` posiada modyfikator `private set`, co chroni je przed bezpośrednią modyfikacją poza metodami klasy.
* **Dziedziczenie** – podział kont na zwykłe i `KontoOszczednosciowe` oraz kart na `KartaDebetowa` i `KartaKredytowa`.
* **Polimorfizm oraz relacje** – klasa `Klient` zarządza kontami i kartami za pomocą list dynamicznych `List<>`.

## Funkcjonalności (Menu konsolowe):
1. Podgląd stanu kont oraz przypisanych kart dla wszystkich klientów.
2. Interaktywne wpłaty i wypłaty z walidacją kwot ujemnych oraz limitów salda.
3. Realizacja przelewów między kontami różnych klientów.
4. Obsługa płatności kartą powiązaną z kontem.

## Jak uruchomić:
1. Skopiuj kod z pliku `Program.cs`.
2. Wklej go do projektu konsolowego w swoim IDE (np. JetBrains Rider / Visual Studio).
3. Uruchom program i steruj operacjami za pomocą liczb w menu.
