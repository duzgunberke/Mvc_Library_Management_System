﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KütüphanePaneli.Models.Entity;

namespace KütüphanePaneli.Controllers
{
    [Authorize(Roles = "A,B")]
    public class KitapController : Controller
    {
        // GET: Kitap
        DboKütüphaneYönetimEntities db = new DboKütüphaneYönetimEntities();
        public ActionResult Index(string p)
        {
            var kitaplar = from k in db.TBL_KITAP select k;
            if (!string.IsNullOrEmpty(p))
            {
                kitaplar = kitaplar.Where(x => x.AD.Contains(p));
            }
           // var kitaplar = db.TBL_KITAP.ToList();

            return View(kitaplar.ToList());
        }
        [HttpGet]
        public ActionResult KitapEkle()
        {
            List<SelectListItem> deger1 = (from i in db.TBL_KATEGORI.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.AD,
                                               Value = i.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;

            List<SelectListItem> deger2 = (from y in db.TBL_YAZAR.ToList()
                                           select new SelectListItem
                                           {
                                               Text = y.AD + ' ' + y.SOYAD,
                                               Value = y.ID.ToString()

                                           }).ToList();
            ViewBag.dgr2 = deger2;
            return View(); 
        }
        [HttpPost]
        public ActionResult KitapEkle(TBL_KITAP p)
        {
            var ktg = db.TBL_KATEGORI.Where(k => k.ID == p.TBL_KATEGORI.ID).FirstOrDefault();
            var yzr = db.TBL_YAZAR.Where(y => y.ID == p.TBL_YAZAR.ID).FirstOrDefault();
            p.TBL_KATEGORI = ktg;
            p.TBL_YAZAR = yzr;
            db.TBL_KITAP.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }
        public ActionResult KitapSil(int id)
        {
            var kitap = db.TBL_KITAP.Find(id);
            db.TBL_KITAP.Remove(kitap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KitapGetir(int id)
        {
            var ktp = db.TBL_KITAP.Find(id);
            List<SelectListItem> deger1 = (from i in db.TBL_KATEGORI.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.AD,
                                               Value = i.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;

            List<SelectListItem> deger2 = (from y in db.TBL_YAZAR.ToList()
                                           select new SelectListItem
                                           {
                                               Text = y.AD + ' ' + y.SOYAD,
                                               Value = y.ID.ToString()

                                           }).ToList();
            ViewBag.dgr2 = deger2;
            return View("KitapGetir", ktp);
        }

        public ActionResult KitapGuncelle(TBL_KITAP p)
        {
            var kitap = db.TBL_KITAP.Find(p.ID);
            kitap.AD = p.AD;
            kitap.BASIMYIL = p.BASIMYIL;
            kitap.SAYFASAYISI = p.SAYFASAYISI;
            kitap.YAYINEVI = p.YAYINEVI;
            kitap.DURUM = true;
            var ktg = db.TBL_KATEGORI.Where(k => k.ID == p.TBL_KATEGORI.ID).FirstOrDefault();
            var yzr = db.TBL_YAZAR.Where(y => y.ID == p.TBL_YAZAR.ID).FirstOrDefault();
            kitap.KATEGORI = ktg.ID;
            kitap.YAZAR = yzr.ID;
            kitap.KITAPRESIM = p.KITAPRESIM;
            db.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}