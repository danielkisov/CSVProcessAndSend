using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitachiAssessment
{
    class ByCountry
    {
        private string countryName;
        private int avgScore;
        private int medScore;
        private int maxScore;
        private string maxScorePerson;
        private int minScore;
        private string minScorePerson;
        private int recordCount;
        private List<Competitor> competitors;

        public ByCountry(string cName)
        {
            countryName = cName;
            competitors = new List<Competitor>();
        }

        //Getters (and Setters for potential future use)
        public string GetCountryName()
        {
            return countryName;
        }

        private void SetCoutryName(string cName)
        {
            countryName = cName;
        }

        public int GetAvgScore()
        {
            return avgScore;
        }

        private void SetAvgScore(int avgSc)
        {
            avgScore = avgSc;
        }

        public int GetMedScore()
        {
            return medScore;
        }

        private void SetMedScore(int medSc)
        {
            medScore = medSc;
        }

        public int GetMaxScore()
        {
            return maxScore;
        }

        private void SetMaxScore(int maxSc)
        {
            maxScore = maxSc;
        }

        public string GetMaxScorePerson()
        {
            return maxScorePerson;
        }

        private void SetMaxScorePerson(string maxScPers)
        {
            maxScorePerson = maxScPers;
        }

        public int GetMinScore()
        {
            return minScore;
        }

        private void SetMinScore(int minSc)
        {
            minScore = minSc;
        }

        public string GetMinScorePerson()
        {
            return minScorePerson;
        }

        private void SetMinScorePerson(string minScPers)
        {
            minScorePerson = minScPers;
        }

        public int GetRecordCount()
        {
            return recordCount;
        }

        private void SetRecordCount(int recCnt)
        {
            recordCount = recCnt;
        }

        public List<Competitor> GetCompetitors()
        {
            return competitors;
        }

        public void AddCompetitor(Competitor competitor)
        {
            competitors.Add(competitor);
        }

        public void CalcDataForCountry()
        {
            int sumPoints = 0;
            int[] points = new int[competitors.Count()];
            int i = 0; //to fill array of points
            int maxSc = 0;
            string maxScPers = "";
            int minSc = int.MaxValue;
            string minScPers = "";           

            foreach (Competitor competitor in competitors)
            {
                int compPoints = competitor.GetPoints();

                sumPoints += compPoints;
                points[i] = compPoints;
                i++;
                
                if(compPoints > maxSc)
                {
                    maxSc = compPoints;
                    maxScPers = competitor.GetFirstName() + " " + competitor.GetLasttName();
                }

                if(compPoints < minSc)
                {
                    minSc = compPoints;
                    minScPers = competitor.GetFirstName() + " " + competitor.GetLasttName();
                }
            }

            //solving the median
            Array.Sort(points);
            int med = 0;
            if(points.Length % 2 == 0)
            {
                med = (points[points.Length / 2 - 1] + points[points.Length / 2]) / 2;
            }
            else
            {
                med = points[points.Length / 2 ];
            }

            //setting the fields
            avgScore = sumPoints / competitors.Count();
            medScore = med;
            maxScore = maxSc;
            maxScorePerson = maxScPers;
            minScore = minSc;
            minScorePerson = minScPers;
            recordCount = competitors.Count();
        }

    }
}
