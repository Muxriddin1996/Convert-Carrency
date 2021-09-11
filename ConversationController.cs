using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Konveter_valyut.Models;

namespace Konveter_valyut.Controllers
{
    public class ConversationController : Controller
    {
        // GET: Konveter
        public ActionResult Index()
        {
            using(testConvert db=new testConvert())
            {
                return View(db.Conversations.ToList());
            }
            
        }
        public ActionResult Conversation()
        {
            using (testConvert db = new testConvert())
            {
                ViewBag.CurrencyList = new SelectList(db.CurrencyNames.ToList(), "ID", "Name");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Conversation(ConversationViewModel model)
        {
            try
            {
                using(testConvert db=new testConvert())
                {
                    var result = model.ValueOne / (db.CurrencyNames.Where(m => m.ID == (int)model.NameOne)).FirstOrDefault().Unity * (db.CurrencyNames.Where(m => m.ID == (int)model.NameTwo)).FirstOrDefault().Unity;

                    Conversation list = new Conversation();
                    list.CurrencyNameOne = (db.CurrencyNames.Where(m => m.ID == (int)model.NameOne)).FirstOrDefault().Name;
                    list.CurrencyNameTwo = (db.CurrencyNames.Where(m => m.ID == (int)model.NameTwo)).FirstOrDefault().Name;
                    list.CurrencyValueOne = model.ValueOne;
                    list.CurrencyValueTwo = (double)result;
                    Session["result"] = list.CurrencyNameOne + " => to " + list.CurrencyNameTwo+ "           " + list.CurrencyValueOne + " = " + list.CurrencyValueTwo;
                    list.SaveDate = DateTime.Now;
                    db.Conversations.Add(list);
                    db.SaveChanges();
                }
                return RedirectToAction("Conversation");

            }
            catch
            {
                return View();
            }
        }
    }
}