using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BagisiklikTakipSistemi.Models;

namespace BagisiklikTakipSistemi.Controllers
{
    public class AdminController : Controller
    {

        DbBagisiklikEntities db = new DbBagisiklikEntities();

        // GET: Admin
        [Authorize]
        public ActionResult AdminIndex()
        {

            int id = Convert.ToInt32(User.Identity.Name); // adminin idsi çekilir.

            List<UyelerTablosu> tablo = db.UyelerTablosu.Where(x => x.ID!=id).ToList(); // kendisi hariç tüm üyeleri listele.


            var degerler = tablo;

            return View(degerler);

        }
        public ActionResult UyePasif(int id) //seçilen üyeler pasif hale getirilir.
        {

            var veri = db.UyelerTablosu.Find(id);
            veri.Durum = false;
            db.SaveChanges();

            return Redirect("/Admin/AdminIndex");

        }
        public ActionResult UyeAktif(int id) //seçilen üyeler aktif hale getirilir.
        {

            var veri = db.UyelerTablosu.Find(id);
            veri.Durum = true;
            db.SaveChanges();

            return Redirect("/Admin/AdminIndex");

        }
        [Authorize]
        public ActionResult PostVeYorum() // post ve yorumların listelendiği sayfa
        {
            
            return View();

        }
        [Authorize]
        public PartialViewResult AdminPost() //veritabanındaki tüm postlar çekilir.
        {

            List<PostTablosu> tablo = db.PostTablosu.ToList();


            var degerler = tablo;

            return PartialView(degerler);

        }
        [Authorize]
        public PartialViewResult AdminYorum() //veritabanındaki tüm yorumlar çekilir.
        {

            List<YorumlarTablosu> tablo = db.YorumlarTablosu.ToList();


            var degerler = tablo;

            return PartialView(degerler);

        }
        public ActionResult PostAktif(int id) // seçilen post aktif hale getirilir.
        {

            var veri = db.PostTablosu.Find(id);
            veri.Durum = true;
            db.SaveChanges();

            return Redirect("/Admin/PostVeYorum");

        }
        public ActionResult PostPasif(int id) // seçilen post pasif hale getirilir.
        {

            var veri = db.PostTablosu.Find(id);
            veri.Durum = false;
            db.SaveChanges();

            return Redirect("/Admin/PostVeYorum");

        }
        public ActionResult YorumAktif(int id) // seçilen yorum aktif hale getirilir.
        {

            var veri = db.YorumlarTablosu.Find(id);
            veri.Durum = true;
            db.SaveChanges();

            return Redirect("/Admin/PostVeYorum");

        }
        public ActionResult YorumPasif(int id) // seçilen yorum pasif hale getirilir.
        {

            var veri = db.YorumlarTablosu.Find(id);
            veri.Durum = false;
            db.SaveChanges();

            return Redirect("/Admin/PostVeYorum");

        }

        [Authorize]
        public ActionResult PostDuzgetir(int id) //güncelleme sayfası için güncellencek güne ait veriler çekilir.
        {

            var veri = db.PostTablosu.Find(id);
            return View("PostDuzgetir", veri);

        }

        [Authorize]
        public ActionResult PostDuzenle(PostTablosu p) //profile ait bilgiler text alanlarına girilen yeni veriler ile değiştirilir.
        {

            var post = db.PostTablosu.Find(p.ID);

            post.PostIcerik = p.PostIcerik;

            db.SaveChanges();

            return Redirect("/Admin/PostVeYorum");

        }
        [Authorize]
        public ActionResult YorumDuzgetir(int id) //güncelleme sayfası için güncellencek güne ait veriler çekilir.
        {

            var veri = db.YorumlarTablosu.Find(id);
            return View("YorumDuzgetir", veri);

        }
        [Authorize]
        public ActionResult YorumDuzenle(YorumlarTablosu p) //profile ait bilgiler text alanlarına girilen yeni veriler ile değiştirilir.
        {

            var post = db.YorumlarTablosu.Find(p.ID);

            post.YorumIcerik = p.YorumIcerik;

            db.SaveChanges();

            return Redirect("/Admin/PostVeYorum");

        }
    }
}