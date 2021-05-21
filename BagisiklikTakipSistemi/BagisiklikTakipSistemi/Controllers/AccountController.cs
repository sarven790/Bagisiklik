using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BagisiklikTakipSistemi.Models;

namespace BagisiklikTakipSistemi.Controllers
{
    public class AccountController : Controller
    {

        DbBagisiklikEntities db = new DbBagisiklikEntities();

        // GET: Account
        [HttpGet]
        public ActionResult UyeOl() //üye ol sayfası
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult UyeOl(UyelerTablosu u) //text alanlarına girilen veriler veri tabanına kaydedilir.
        {

            db.UyelerTablosu.Add(u);
            db.SaveChanges();

            return Redirect("/Account/UyeGiris");
        }

        public ActionResult UyeGiris() //üye giriş sayfası
        {

            return View();

        }

        [HttpPost]
        public ActionResult UyeGiris(UyelerTablosu u)
        {

            var bilgiler = db.UyelerTablosu.Where( x => x.Durum==true).FirstOrDefault(x => x.KulAdi == u.KulAdi && x.Sifre == u.Sifre); //kullanıcı adı ve şifre var mı?            

            if (bilgiler != null)
            {

                FormsAuthentication.SetAuthCookie(bilgiler.ID.ToString(), false); //çerez oluşturulur.

                if (bilgiler.Yetki == 1) //eğer yetki adminse admin sayfasına değilse bağışıklık bilgilendirme sayfasına yönlendirilir.
                {
                    return Redirect("/Admin/AdminIndex");
                }
                else
                {
                    return Redirect("/Home/BagisiklikBilgilendirme");
                }               

            }
            else
            {
                return View();
            }

            

        }
        public ActionResult Logout() //kullanıcı çıkış yapar.
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("UyeGiris", "Account");
        }

    }
}
