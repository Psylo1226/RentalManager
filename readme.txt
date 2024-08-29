Grupa:
Karolina Szot
Kamil Sarzyniak

Istotne informacje na temat projektu:
1. Baza danych SQLite jest automatycznie tworzona przy pierwszym uruchomieniu aplikacji. WebAPI oraz BlazorServer mają osobną bazę danych.
2. Aplikację uruchamia się poprzez uruchomienie albo projektu BlazorServer albo projektów WebAPI i BlazorClient. 
3. Przy uruchomieniu samego projektu WebAPI można skorzystać ze swaggera pod adresem https://localhost:<port>/swagger/
4. Aplikacja nie wymaga logowania.

W przypadku problemów z działaniem swaggera (najprawdopodobniej problemy powoduje przeglądarka) należy zmienić port w pliku RentalManager.BlazorClient/Pages/ProductTypes/ProductTypePage.razor
