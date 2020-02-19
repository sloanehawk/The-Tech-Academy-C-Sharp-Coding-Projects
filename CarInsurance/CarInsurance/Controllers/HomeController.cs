using CarInsurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetQuote(string firstName, string lastName, string emailAddress, DateTime dateOfBirth,
            int carYear, string carMake, string carModel, string DUI, int speedingTickets, string fullCoverageOrLiability)
        {
            using (CarInsuranceEntities1 db = new CarInsuranceEntities1())
            {
                var quote = new Quote();
                quote.FirstName = firstName;
                quote.LastName = lastName;
                quote.EmailAddress = emailAddress;
                quote.DateOfBirth = dateOfBirth;
                quote.CarYear = carYear;
                quote.CarMake = carMake;
                quote.CarModel = carModel;
                quote.DUI = DUI;
                quote.SpeedingTickets = speedingTickets;
                quote.FullCoverageOrLiability = fullCoverageOrLiability;

                //calculate quote
                quote.QuoteAmount = 50; //Start with a base of $50 / month.

                //get age
                int age = (DateTime.Now.Year - dateOfBirth.Year);
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                    age -= 1;

                //If the user is under 25, add $25 to the monthly total.
                if (age < 25)
                {
                    quote.QuoteAmount += 25;
                }

                //If the user is under 18, add $100 to the monthly total.
                if (age < 18)
                {
                    quote.QuoteAmount += 100;
                }

                //If the user is over 100, add $25 to the monthly total.
                if (age > 100)
                {
                    quote.QuoteAmount += 25;
                }

                //If the car's year is before 2000, add $25 to the monthly total.
                if (carYear < 2000)
                {
                    quote.QuoteAmount += 25;
                }

                //If the car's year is after 2015, add $25 to the monthly total.
                if (carYear > 2015)
                {
                    quote.QuoteAmount += 25;
                }

                //If the car's Make is a Porsche, add $25 to the price.
                if (carMake == "Porsche")
                {
                    quote.QuoteAmount += 25;
                    if (carModel == "911 Carrera")
                    {
                        quote.QuoteAmount += 25;
                    }
                }

                //Add $10 to the monthly total for every speeding ticket the user has.
                quote.QuoteAmount += speedingTickets * 10;

                //If the user has ever had a DUI, add 25% to the total.
                if (DUI == "Yes")
                {
                    quote.QuoteAmount *= 1.25m;
                }

                //If it's full coverage, add 50% to the total.
                if (fullCoverageOrLiability == "Full Coverage")
                {
                    quote.QuoteAmount *= 1.5m;
                }

                //add to database
                db.Quotes.Add(quote);
                db.SaveChanges();
            }
            return View("Success");
        }


       

       
    }
}