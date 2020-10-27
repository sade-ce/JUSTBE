using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    public class EventCalController : Controller
    {
        private EFDBContext db = new EFDBContext();

        // GET: Coordinator/EventCal
        public ActionResult Index( string Email = "", string ClientID = "", string TransactionID = "", string AgentID = "")
        {
            ViewBag.Email = Email;
            ViewBag.ClientID = ClientID;
            ViewBag.TransactionID = TransactionID;
            ViewBag.AgentID = AgentID;
            return View(db.utblMstEventCalMasters.ToList());
        }

        // GET: Coordinator/EventCal/Details/5

        // GET: Coordinator/EventCal/Create
        public ActionResult Create(string Email = "",string ClientID="",string TransactionID="",string AgentID="")
        {
            ViewBag.Email = Email;
            ViewBag.ClientID = ClientID;
            ViewBag.TransactionID = TransactionID;
            ViewBag.AgentID = AgentID;
            return View();
        }

        // POST: Coordinator/EventCal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CalEventID,EventText,UpdatedOn")] utblMstEventCalMaster utblMstEventCalMaster,string Email = "", string ClientID = "", string TransactionID = "", string AgentID = "")
        {
            ViewBag.Email = Email;
            utblMstEventCalMaster.UpdatedOn = System.DateTime.Now;
            if (ModelState.IsValid)
            {
                db.utblMstEventCalMasters.Add(utblMstEventCalMaster);
                db.SaveChanges();
                return RedirectToAction("Index", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
            }

            return View(utblMstEventCalMaster);
        }

        // GET: Coordinator/EventCal/Edit/5
        public ActionResult Edit(int? id, string Email = "", string ClientID = "", string TransactionID = "", string AgentID = "")
        {
            ViewBag.Email = Email;
            ViewBag.ClientID = ClientID;
            ViewBag.TransactionID = TransactionID;
            ViewBag.AgentID = AgentID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            utblMstEventCalMaster utblMstEventCalMaster = db.utblMstEventCalMasters.Find(id);
            if (utblMstEventCalMaster == null)
            {
                return HttpNotFound();
            }
            return View(utblMstEventCalMaster);
        }

        // POST: Coordinator/EventCal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CalEventID,EventText,UpdatedOn")] utblMstEventCalMaster utblMstEventCalMaster, string Email = "", string ClientID = "", string TransactionID = "", string AgentID = "")
        {
            utblMstEventCalMaster.UpdatedOn = System.DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(utblMstEventCalMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
            }
            return View(utblMstEventCalMaster);
        }

        // GET: Coordinator/EventCal/Delete/5
        public ActionResult Delete(int? id, string Email = "", string ClientID = "", string TransactionID = "", string AgentID = "")
        {
            ViewBag.Email = Email;
            ViewBag.ClientID = ClientID;
            ViewBag.TransactionID = TransactionID;
            ViewBag.AgentID = AgentID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            utblMstEventCalMaster utblMstEventCalMaster = db.utblMstEventCalMasters.Find(id);
            if (utblMstEventCalMaster == null)
            {
                return HttpNotFound();
            }
            return View(utblMstEventCalMaster);
        }

        // POST: Coordinator/EventCal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string Email = "", string ClientID = "", string TransactionID = "", string AgentID = "")
        {
            utblMstEventCalMaster utblMstEventCalMaster = db.utblMstEventCalMasters.Find(id);
            db.utblMstEventCalMasters.Remove(utblMstEventCalMaster);
            db.SaveChanges();
            return RedirectToAction("Index",new { Email=Email,ClientID=ClientID,TransactionID=TransactionID,AgentID=AgentID});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
