using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitachiAssessment
{
    class Competitor
    {
        private string firstName;
        private string lastName;
        private string country;
        private int points;

        public Competitor(string fName, string lName, string ctr, int pts)
        {
            firstName = fName;
            lastName = lName;
            country = ctr;
            points = pts;
        }

        //Getters (and Setters for potential future use)
        private void SetFirstName(string fName)
        {
            firstName = fName;
        }
        
        public string GetFirstName()
        {
            return firstName;
        }
        
        private void SetLastName(string lName)
        {
            lastName = lName;
        }
        
        public string GetLasttName()
        {
            return lastName;
        }
        
        private void SetCountry(string ctr)
        {
            country = ctr;
        }
        
        public string GetCountry()
        {
            return country;
        }
        
        private void SetPoints(int pts)
        {
            points = pts;
        }
        
        public int GetPoints()
        {
            return points;
        }
    }
}
