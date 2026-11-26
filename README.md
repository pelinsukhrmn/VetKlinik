# Veteriner Kliniği Randevu ve Hasta Takip Sistemi

- **Öğrenci No:** B241210104
- **Ad Soyad:** Pelinsu Kahraman
- **Ders Grubu:** 1A
- **GitHub:** https://github.com/pelinsukhrmn/VetKlinik

## Proje Hakkında
ASP.NET Core MVC (.NET 10) ile geliştirilen; klinik hizmetlerini, veteriner hekimleri,
evcil hayvanların muayene ve aşı geçmişini ve randevuları yöneten bir web uygulaması.
REST API ve yapay zekâ asistanlarının kullanabileceği bir MCP sunucusu içerir.

## Veritabanı Modeli

```mermaid
erDiagram
    ApplicationUser ||--o{ EvcilHayvan : "sahibidir"
    ApplicationUser ||--o| Veteriner : "hesabidir"
    Veteriner }o--o{ UzmanlikAlani : "uzmanlik"
    Veteriner }o--o{ Hizmet : "verebilir"
    Veteriner ||--o{ VeterinerMusaitlik : "musaitlik"
    EvcilHayvan ||--o{ AsiKaydi : "asi kaydi"
    EvcilHayvan ||--o{ Randevu : "randevu"
    Veteriner ||--o{ Randevu : "randevu"
    Hizmet ||--o{ Randevu : "randevu"
    Randevu ||--o| MuayeneNotu : "not"
    Veteriner ||--o{ VeterinerIzin : "izin"
    EvcilHayvan ||--o{ KiloOlcumu : "kilo"
    AsiSablonu |o--o{ AsiKaydi : "sablon"
    Randevu ||--o{ RandevuDurumGecmisi : "gecmis"

    ApplicationUser { string Id PK
        string Ad
        string Soyad
        string Email }
    Hizmet { int Id PK
        string Ad
        int SureDakika
        decimal Ucret
        bool AktifMi }
    Veteriner { int Id PK
        string KullaniciId FK
        string Unvan }
    EvcilHayvan { int Id PK
        string SahipId FK
        string Ad
        string Tur
        date DogumTarihi
        decimal Kilo }
    AsiKaydi { int Id PK
        int EvcilHayvanId FK
        string AsiAdi
        date YapilmaTarihi
        date SonrakiDozTarihi }
    Randevu { int Id PK
        int EvcilHayvanId FK
        int VeterinerId FK
        int HizmetId FK
        datetime Baslangic
        datetime Bitis
        decimal Ucret
        string Durum }
    MuayeneNotu { int Id PK
        int RandevuId FK
        string Bulgular }
    VeterinerMusaitlik { int Id PK
        int VeterinerId FK
        int Gun
        time BaslangicSaati
        time BitisSaati }
    KlinikCalismaSaati { int Id PK
        int Gun
        time AcilisSaati
        time KapanisSaati }
    AsiSablonu { int Id PK
        string AsiAdi
        string Tur
        int TekrarAraligiGun }
    VeterinerIzin { int Id PK
        int VeterinerId FK
        date BaslangicTarihi
        date BitisTarihi }
    KiloOlcumu { int Id PK
        int EvcilHayvanId FK
        date OlcumTarihi
        decimal Kilo }
    RandevuDurumGecmisi { int Id PK
        int RandevuId FK
        string EskiDurum
        string YeniDurum
        string DegistirenKullaniciId FK
        datetime Tarih }
```

Tek klinik üzerinde çalışılmaktadır. Randevu, oluşturulduğu andaki ücret ve süreyi kendi içinde saklar; böylece hizmet fiyatı değişse de geçmiş kayıtlar ve ciro raporu doğru kalır.