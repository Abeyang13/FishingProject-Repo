﻿using FishingProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FishingProject.Controllers
{
    public class OrganizationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Organizations
        public ActionResult Index()
        {
            var organizationId = User.Identity.GetUserId();
            var organization = db.Organizations.Where(o => o.ApplicationId == organizationId).ToList();
            return View(organization);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationId,FirstName,LastName")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                organization.ApplicationId = User.Identity.GetUserId();
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organization);
        }


        // GET: Tournaments
        public ActionResult TournamentIndex()
        {
            var tournaments = db.Tournaments.ToList();
            return View(tournaments);
        }
        public ActionResult CreateTournament()
        {
            Tournament tournament = new Tournament();
            return View(tournament); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTournament([Bind(Include = "TournamentId,TournamentName,TournamentDate,OrganizationId")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                Organization organization = db.Organizations.Where(o => o.ApplicationId == currentUserId).Single();
                tournament.OrganizationId = organization.OrganizationId;
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournament);
        }

        //Get: Details of Tournament
        public ActionResult TournamentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // GET: Tournament/Edit/5
        public ActionResult EditTournament(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournament/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTournament([Bind(Include = "TournamentId,TournamentName,TournamentDate,OrganizationId")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournament);
        }

        //GET: List of tournament teams
        public ActionResult TournamentTable(int id)
        {
            var tournamentId = db.TournamentTeams.Where(t => t.TournamentId == id).Single();
            return View();
        }
        
        // GET: Teams/Delete
        public ActionResult DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentTeam team = db.TournamentTeams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeam(int id)
        {
            TournamentTeam team = db.TournamentTeams.Find(id);
            db.TournamentTeams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
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