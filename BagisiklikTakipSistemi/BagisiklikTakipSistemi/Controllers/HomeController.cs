using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BagisiklikTakipSistemi.Models;

namespace BagisiklikTakipSistemi.Controllers
{  
    public class HomeController : Controller
    {

        DbBagisiklikEntities db = new DbBagisiklikEntities(); //veri tabanı bağlantısı

        [Authorize]
        public ActionResult Index()
        {

            int id = Convert.ToInt32(User.Identity.Name); // üye girişi yapan kullanıcının id'si

            var uye = db.UyelerTablosu.Where(x => x.ID == id).ToList(); // uye bilgileri çekilir.
            var uye2 = uye.FirstOrDefault(x => x.Yetki == 1); // üyenin admin mi yoksa normal kullanıcı mı olup olmadığına bakılır.

            //eğer üye admin ise ana sayfaya girmesine izin vermeden admin anasayfasına yönlendiriyor.
            if (uye2!=null)
            {
                return Redirect("/Admin/AdminIndex");
            }
            //uyeye ait su tüketim ve günlük adım sayılarının olduğu veriler getirilir.
            List<TuketimTablosu> tablo = db.TuketimTablosu.Where(x => x.UyeID == id && x.Durum==true).ToList();
            //bu veriler içinden son 7 veri getirilir.
            List<TuketimTablosu> tb = tablo.OrderByDescending(x => x.ID).Take(7).ToList();

            var degerler = tb.OrderBy(x => x.ID).ToList();

            return View(degerler);
        }

        [Authorize]
        public ActionResult VeriEkle()
        {

            return View();

        }

        [HttpPost]
        public ActionResult VeriEkle(TuketimTablosu t)
        {
            int id = Convert.ToInt32(User.Identity.Name);
            string gun = DateTime.Now.Day.ToString(); //kullanıcının veri eklediği tarih bilgileri sql veri tabanının anlayacağı formata çevirilir.
            string ay = DateTime.Now.Month.ToString();
            string yil = DateTime.Now.Year.ToString();
            string tarih = "";

            if (int.Parse(ay) < 10)
                tarih = yil + "-0" + ay + "-" + gun;
            else
                tarih = yil + "-" + ay + "-" + gun;

            var r = db.TuketimTablosu.FirstOrDefault(x => x.Tarih.ToString() == tarih && x.UyeID == id); // üyenin bugüne ait verileri varsa getirilir.

            if (r == null) //eğer üyenin bugüne ait verisi yoksa ekleme işlemi yapılır.
            {

                db.TuketimTablosu.Add(t);
                db.SaveChanges();

            }

            return Redirect("/Home/Index");

        }

        [Authorize]
        public ActionResult VeriGetir(int id) //güncelleme sayfası için güncellencek güne ait veriler çekilir.
        {

            var veri = db.TuketimTablosu.Find(id);
            return View("VeriGetir", veri);

        }
        [Authorize]
        public ActionResult ProfilGetir(int id) // üyenin profil bilgileri getirilir.
        {

            var veri = db.UyelerTablosu.Find(id);
            return View("ProfilGetir", veri);

        }
        public ActionResult VeriSil(int id) //seçilen güne ait veriler silinir. (daha doğrusu pasif hale getirilir.)
        {

            var veri = db.TuketimTablosu.Find(id);
            veri.Durum = false;
            db.SaveChanges();

            return Redirect("/Home/Index");

        }
        public ActionResult ProfilGuncelle(UyelerTablosu t) //profile ait bilgiler text alanlarına girilen yeni veriler ile değiştirilir.
        {

            var uye = db.UyelerTablosu.Find(t.ID);

            uye.KulAdi = t.KulAdi;
            uye.KulSoyadi = t.KulSoyadi;
            uye.Mail = t.Mail;
            uye.Boy = t.Boy;
            uye.Kilo = t.Kilo;
            uye.Yas = t.Yas;
            db.SaveChanges();

            return Redirect("/Home/Profil");

        }      
        public ActionResult VeriGuncelle(TuketimTablosu t) // seçilen güne ait tüketim bilgileri text alanlarına girilen yeni veriler ile değiştirilir.
        {

            var tuk = db.TuketimTablosu.Find(t.ID);
            tuk.SuTuketim = t.SuTuketim;
            tuk.ToplamAdim = t.ToplamAdim;
            db.SaveChanges();

            return Redirect("/Home/Index");

        }

        [Authorize]
        public ActionResult BagisiklikBilgilendirme() // bağışıklık bilgilendirme sayfası
        {

            return View();

        }
        [Authorize]
        public ActionResult NasilHissediyorsun() //nasıl hissediyorsun sayfası.
        {

            return View();

        }
        [Authorize]
        public ActionResult Koronavirus() // koronavirüs belirtilerinin yazdığı sayfa.
        {

            return View();

        }

        [Authorize]
        public ActionResult Anket() //anket sayfası
        {

            return View();

        }

        [Authorize]
        public ActionResult Post() //post sayfası 
        {

            int id = Convert.ToInt32(User.Identity.Name);

            var uye = db.UyelerTablosu.Where(x => x.ID == id).ToList();
            var uye2 = uye.FirstOrDefault(x => x.Yetki == 1);

            if (uye2 != null)
            {
                return Redirect("/Admin/AdminIndex");
            }

            return View();

        }

        [HttpPost]
        public ActionResult Post(PostTablosu p) // girilen post veri tabanına kaydedilir.
        {

            db.PostTablosu.Add(p);
            db.SaveChanges();

            return Redirect("/Home/Post");
        }

        PostYorum p = new PostYorum();
        public PartialViewResult PostListele() //veri tabanında yer alan tüm postlar ve postlara yapılan yorumlar listelenir.
        {

            p.Deger1 = db.PostTablosu.OrderByDescending(x => x.ID).Where(x => x.Durum == true).ToList();
            p.Deger2 = db.YorumlarTablosu.OrderByDescending(x => x.ID).Where(x => x.Durum == true).ToList();

            return PartialView(p);

        } 
        [Authorize]
        public ActionResult Profil() // profil sayfasına üyenin bilgileri yazar.
        {

            int id = Convert.ToInt32(User.Identity.Name);

            var uye = db.UyelerTablosu.Where(x => x.ID == id).ToList();
            var uye2 = uye.FirstOrDefault(x => x.Yetki == 1);

            if (uye2 != null)
            {
                return Redirect("/Admin/AdminIndex");
            }

            List<UyelerTablosu> tablo = db.UyelerTablosu.Where(x => x.ID == id && x.Durum == true).ToList();

            var degerler = tablo.ToList();

            return View(degerler);
        }

        [Authorize]
        public ActionResult YorumEkle()
        {          

            return View();

        }
       
        [HttpPost]
        public ActionResult YorumEkle(int id, YorumlarTablosu y) //postların altına girilen yorumlar veri tabanına kaydedilir.
        {
            db.YorumlarTablosu.Add(y);
            db.SaveChanges();

            return Redirect("/Home/Post");

        }
        public PartialViewResult PostBilgileri() //veri tabanında yer alan tüm postlar ve postlara yapılan yorumlar listelenir.
        {

            int idm = int.Parse(Request.QueryString["ID"]);

            p.Deger1 = db.PostTablosu.OrderByDescending(x => x.ID).Where(x => x.Durum == true && x.ID == idm).ToList();
            p.Deger2 = db.YorumlarTablosu.OrderByDescending(x => x.ID).Where(x => x.Durum == true).ToList();

            return PartialView(p);

        }
        public ActionResult YorumSil(int id)
        {

            var yorum = db.YorumlarTablosu.Find(id);

            db.YorumlarTablosu.Remove(yorum);
            db.SaveChanges();

            return RedirectToAction("Post");

        }

    }
}