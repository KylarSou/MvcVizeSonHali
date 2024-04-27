using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vizeproje.Models;
using System.Data.SqlClient;
using System.Data;

namespace Vizeproje.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult idari()
        {
            ViewBag.Message = "İdari Sayfası";
            return View();
        }

        public ActionResult Raporlar()
        {
            ViewBag.Message = "Rapor Sayfası";
            return View();
        }

        public ActionResult Yonetici()
        {
            ViewBag.Message = "Yonetici sayfası";
            return View();
        }

        public ActionResult Anasayfa()
        {
            ViewBag.Message = "Ana Sayfa";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Kullanıcı adı ve şifre kontrolü
            using (var db = new UniversiteEntities())
            {
                // Kullanıcı adı ve şifre kontrolü
                var user = db.LoginTb.FirstOrDefault(u => u.Ad == username && u.Sifre == password);
                if (user != null)
                {
                    // Giriş başarılıysa oturum değişkeni ayarla
                    Session["LoggedInUser"] = user;

                    // Giriş başarılı mesajını göster
                    ViewBag.SuccessMessage = "Giriş başarılı!";

                    // Ana sayfaya yönlendir
                    return RedirectToAction("Index");
                }
                else
                {
                    // Giriş başarısızsa hata mesajını göster
                    ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre";
                }
            }

            // Index sayfasına geri dön
            return View("Index");
        }
        public ActionResult Logout()
        {
            // Oturumu sil
            Session["LoggedInUser"] = null;

            // Çıkış yapıldı mesajını göster
            ViewBag.SuccessMessage = "Çıkış yapıldı.";

            // Index sayfasına yönlendir
            return RedirectToAction("Index");
        }
        private UniversiteEntities db = new UniversiteEntities();

        public SqlDbType Birim { get; private set; }
        public SqlDbType Ad { get; private set; }

        [HttpPost]
        public ActionResult AddStudent(string id, string ad, string sifre)
        {
            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Index");
            }

            string query = "INSERT INTO LoginTb (id, Ad, Sifre) VALUES (@Id, @Ad, @Sifre)";
            db.Database.ExecuteSqlCommand(query, new SqlParameter("@Id", id), new SqlParameter("@Ad", ad), new SqlParameter("@Sifre", sifre));

            ViewBag.SuccessMessage = "Öğrenci başarıyla eklendi.";
            return RedirectToAction("Yonetici");
        }

        [HttpPost]
        public ActionResult AddBolum(string Ogrencid, string Ders, int Kredi)
        {
            // Oturum kontrolü
            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Index");
            }

            // Veritabanına ekleme işlemi
            using (var db = new UniversiteEntities())
            {
                string query = "INSERT INTO BolumTb (Ogrencid, Ders, Kredi) VALUES (@Ogrencid, @Ders, @Kredi)";
                db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("@Ogrencid", Ogrencid),
                    new SqlParameter("@Ders", Ders),
                    new SqlParameter("@Kredi", Kredi));
            }

            ViewBag.SuccessMessage = "Bölüm başarıyla eklendi.";
            return RedirectToAction("Yonetici");
        }

        public ActionResult ShowBolum()
        {
            using (var db = new UniversiteEntities())
            {
                var bolumler = db.BolumTb.ToList(); // Bütün bölümleri getir

                return View(bolumler);
            }
        }
        

        [HttpPost]
        public ActionResult AddAkademik(int id, string Ogretmenad, string BolumDers, DateTime Baslangic, DateTime? Ayrilis = null)
        {
            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Index");
            }

            using (var db = new UniversiteEntities())
            {
                string query;
                if (Ayrilis.HasValue)
                {
                    query = "INSERT INTO Akademik (id, Ogretmenad, BolumDers, Baslangic, Ayrilis) VALUES (@id, @Ogretmenad, @BolumDers, @Baslangic, @Ayrilis)";
                }
                else
                {
                    query = "INSERT INTO Akademik (id, Ogretmenad, BolumDers, Baslangic) VALUES (@id, @Ogretmenad, @BolumDers, @Baslangic)";
                }

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@id", id),
            new SqlParameter("@Ogretmenad", Ogretmenad),
            new SqlParameter("@BolumDers", BolumDers),
            new SqlParameter("@Baslangic", Baslangic)
        };

                if (Ayrilis.HasValue)
                {
                    parameters.Add(new SqlParameter("@Ayrilis", Ayrilis.Value));
                }

                db.Database.ExecuteSqlCommand(query, parameters.ToArray());
            }

            ViewBag.SuccessMessage = "Akademik bilgiler başarıyla eklendi.";
            return RedirectToAction("Contact");
        }

        public ActionResult ShowAkademik()
        {
            using (var db = new UniversiteEntities())
            {
                var akademikBilgiler = db.Akademik.Select(a => new {
                    a.id,
                    a.Ogretmenad,
                    a.Bolumders,
                    a.Baslangic
                }).ToList(); // Ayrilis sütununu almadan Akademik tablosundaki tüm verileri getir

                ViewBag.AkademikBilgiler = akademikBilgiler;
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddIdari(int id, string birim, string ad)
        {
            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Index");
            }

            using (var db = new UniversiteEntities())
            {
                string query = "INSERT INTO Idari (id, Birim, Ad) VALUES (@Id, @Birim, @Ad)";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@Id", id), new SqlParameter("@Birim", birim), new SqlParameter("@Ad", ad));
            }

            ViewBag.SuccessMessage = "İdari bilgi başarıyla eklendi.";
            return RedirectToAction("idari");
        }

        public ActionResult ShowIdari()
        {
            using (var db = new UniversiteEntities())
            {
                var idariVeriler = db.Idari.ToList(); // Idari tablosundaki tüm verileri getir

                return View(idariVeriler);
            }
        }

        [HttpPost]
        public ActionResult AddReport(int id, string mezunlar, string aktif)
        {
            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(mezunlar) || string.IsNullOrEmpty(aktif))
            {
                ViewBag.ErrorMessage = "Lütfen tüm alanları doldurun.";
                return View(); // Hata durumunda aynı sayfaya geri dön
            }

            using (var db = new UniversiteEntities())
            {
                string query = "INSERT INTO Raporlar (id, Mezunlar, Aktif) VALUES (@Id, @Mezunlar, @Aktif)";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@Id", id), new SqlParameter("@Mezunlar", mezunlar), new SqlParameter("@Aktif", aktif));
            }

            ViewBag.SuccessMessage = "Rapor başarıyla eklendi.";
            return RedirectToAction("Raporlar");
        }

        public ActionResult ShowRaporlar()
        {
            using (var db = new UniversiteEntities())
            {
                var raporlar = db.Raporlar.ToList(); // Raporlar tablosundaki tüm verileri getir

                return View(raporlar);
            }
        }

        public ActionResult ShowYonetici()
        {
            using (var db = new UniversiteEntities())
            {
                var yoneticiler = db.LoginTb.ToList(); // LoginTb tablosundaki tüm verileri getir

                return View(yoneticiler);
            }
        }

        [HttpPost]
        public ActionResult EditYonetici(int id, string ad, string sifre)
        {
            // Gerekli güncelleme işlemlerini SQL sorgusuyla gerçekleştirin
            string query = "UPDATE LoginTb SET Ad = @ad, Sifre = @sifre WHERE id = @id";
            using (var db = new UniversiteEntities())
            {
                db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("@ad", ad),
                    new SqlParameter("@sifre", sifre),
                    new SqlParameter("@id", id));
            }

            return RedirectToAction("ShowYonetici");
        }

        [HttpPost]
        public ActionResult DeleteYonetici(int id)
        {
            // Gerekli silme işlemlerini SQL sorgusuyla gerçekleştirin
            string query = "DELETE FROM LoginTb WHERE id = @id";
            using (var db = new UniversiteEntities())
            {
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@id", id));
            }

            return RedirectToAction("ShowYonetici");
        }

        [HttpPost]
        public ActionResult EditRapor(int id, string Mezunlar, string Aktif)
        {
            // Update sorgusu oluştur
            string query = "UPDATE Raporlar SET Mezunlar = @Mezunlar, Aktif = @Aktif WHERE id = @id";

            // Veritabanı bağlantısını aç
            using (var db = new UniversiteEntities())
            {
                // Update sorgusunu çalıştır
                db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("@Mezunlar", Mezunlar),
                    new SqlParameter("@Aktif", Aktif),
                    new SqlParameter("@id", id));
            }

            // ShowRaporlar sayfasına yönlendir
            return RedirectToAction("ShowRaporlar");
        }

        [HttpPost]
        public ActionResult DeleteRapor(int id)
        {
            // Gerekli silme işlemlerini SQL sorgusuyla gerçekleştirin
            string query = "DELETE FROM Raporlar WHERE id = @id";
            using (var db = new UniversiteEntities())
            {
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@id", id));
            }

            return RedirectToAction("ShowRaporlar");
        }

        [HttpPost]
        public ActionResult EditIdari(int id, string Birim, string Ad)
        {
            string query = "UPDATE Idari SET Birim = @Birim, Ad = @Ad WHERE id = @id";
            using (var db = new UniversiteEntities())
            {
                db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("@Birim", Birim),
                    new SqlParameter("@Ad", Ad),
                    new SqlParameter("@id", id));
            }

            return RedirectToAction("ShowIdari");
        }

        [HttpPost]
        public ActionResult DeleteIdari(int id)
        {
            string query = "DELETE FROM Idari WHERE id = @id";
            using (var db = new UniversiteEntities())
            {
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@id", id));
            }

            return RedirectToAction("ShowIdari");
        }










    }
}