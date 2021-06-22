using CarInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace CarInventory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ViewResult Index(CarRecord CarRec)
        {
            MySqlConnection myConnection = new();
            MySqlCommand myCommand = new();
            MySqlDataAdapter myAdapter = new();

            string ErrorMessage = SQLdbOpen(myConnection, myCommand);
            if (ErrorMessage != string.Empty)
            {
                ModelState.AddModelError("VMessage", ErrorMessage);
                return View("Index");
            }

            DataTable CarInfo = new();
            myCommand.CommandText = "SELECT * FROM sandbox.cars WHERE sqlID = " + CarRec.RecordID.ToString();
            myAdapter.SelectCommand = myCommand;
            myAdapter.Fill(CarInfo);

            if (CarInfo.Rows.Count != 1)
            {
                ModelState.AddModelError("VMessage", "Invalid Record ID #");
                return View("Index");
            }

            CarRec.ModelYear = Convert.ToInt16(CarInfo.Rows[0]["ModelYear"]);
            CarRec.Make = Convert.ToString(CarInfo.Rows[0]["Make"]);
            CarRec.Model = Convert.ToString(CarInfo.Rows[0]["Model"]);
            CarRec.Color = Convert.ToString(CarInfo.Rows[0]["Color"]);
            CarRec.DateReceived = Convert.ToDateTime(CarInfo.Rows[0]["DateReceived"]);
            CarRec.Remarks = Convert.ToString(CarInfo.Rows[0]["Remarks"]);

            return View("EditCarInventory", CarRec);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string SQLdbOpen(MySqlConnection myConnection, MySqlCommand myCommand)
        {
            string ConnectionString = string.Empty;
            string CSfilename = @"C:\users\mark\documents\mysql\ConnectionString.txt";
            try
            {
                using (StreamReader cs = new StreamReader(CSfilename))
                {
                    while (!cs.EndOfStream) { ConnectionString += cs.ReadLine() + "; "; }
                }
                myConnection.ConnectionString = ConnectionString;
                myConnection.Open();
                myCommand.Connection = myConnection;
            }
            catch (MySqlException E)
            {
                return E.Message;
            }

            return string.Empty;
        }
    }
}
