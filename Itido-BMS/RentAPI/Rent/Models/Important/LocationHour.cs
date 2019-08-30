using System;
namespace Rent.Models
{
    public class LocationHour
    {
        public int ID { get; set; }
        //public int LocationID { get; set; }
        public bool DifferentWeeks { get; set; }

        public float L_Mon { get; set; }
        public float L_Tue { get; set; }
        public float L_Wed { get; set; }
        public float L_Thu { get; set; }
        public float L_Fri { get; set; }
        public float L_Sat { get; set; }
        public float L_Sun { get; set; }

        public float U_Mon { get; set; }
        public float U_Tue { get; set; }
        public float U_Wed { get; set; }
        public float U_Thu { get; set; }
        public float U_Fri { get; set; }
        public float U_Sat { get; set; }
        public float U_Sun { get; set; }


        //Nytårsdag
        public bool NewyearsDay { get; set; }
        //Palmesøndag
        public bool Palmsunday { get; set; }
        //Skærtorsdag
        public bool MaundyThursday { get; set; }
        //Langfredag
        public bool GoodFriday { get; set; }
        //1. påskedag
        public bool EasterDay { get; set; }
        //2. påskedag
        public bool SecondEasterDay { get; set; }
        //Store bededag
        public bool PrayerDay { get; set; }
        //kristi himmelfartsdag
        public bool ChristAscension { get; set; }
        //Pinse
        public bool WhitSunday { get; set; }
        //2. pinsedag
        public bool SndPentecost { get; set; }
        //Juleaften
        public bool ChristmasEve { get; set; }
        //Juledag
        public bool ChristmasDay { get; set; }
        //2. Juledag
        public bool SndChristmasDay { get; set; }
        //Nytårsaften
        public bool NewyearsEve { get; set; }

        //public virtual Location Location { get; set; }

        public bool Completed()
        {
            var total = L_Mon + L_Tue + L_Wed + L_Thu + L_Fri + L_Sat + L_Sun + U_Mon + U_Tue + U_Wed + U_Thu + U_Fri + U_Sat + U_Sun;
            return total > 0;
        }
    }
}
