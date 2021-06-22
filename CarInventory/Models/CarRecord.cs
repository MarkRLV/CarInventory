using System;

namespace CarInventory.Models
{
    public class CarRecord
    {
        public int RecordID { get; set; }
        public int ModelYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime DateReceived { get; set; }
        public string Remarks { get; set; }
    }
}
