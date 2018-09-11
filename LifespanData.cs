using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Population
{
    struct LifespanDataPoint
    {
        public int lifespan;
        public int birthDate;
        public int deathDate;
    }

    class LifespanData
    {
        [JsonProperty]
        public List<LifespanDataPoint> data = new List<LifespanDataPoint>();

        [JsonProperty]
        public List<int> population = new List<int>();

        [JsonProperty]
        public int Mean { get; private set; }

        [JsonProperty]
        public int StandardDeviation { get; private set; }

        [JsonProperty]
        public int RangeStart { get; private set; }

        [JsonProperty]
        public int RangeEnd { get; private set; }

        public LifespanData() { }  // Default Constructor for JSON converter

        public LifespanData(int mean, int stdDev, int rangeStart, int rangeEnd, int numOfSamples)
        {
            Mean = mean;
            StandardDeviation = stdDev;
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;

            GenerateNewData(numOfSamples);
        }

        public LifespanData(string filepath)
        {
            LoadData(filepath);
        }

        public void GenerateNewData(int numOfSamples = 1000)
        {
            data.Clear();

            bool bRangeLargeEnough = RangeEnd - RangeStart > Mean + StandardDeviation;
            Debug.Assert(bRangeLargeEnough, "Mean and standard deviation aren't suitable for the range: Please increase the range for your data");

            population.Clear();
            population.AddRange(Enumerable.Repeat(0, RangeEnd - RangeStart + 1));

            for (int i = 0; i < numOfSamples; ++i)
            {
                LifespanDataPoint point;

                point.lifespan = (int)DataUtility.GaussianSample(Mean, StandardDeviation);

                // Only want data from people who fit in the range, exclude the outliers by getting a new point
                while (point.lifespan > RangeEnd - RangeStart)
                {
                    point.lifespan = (int)DataUtility.GaussianSample(Mean, StandardDeviation);
                }

                point.birthDate = (int)Math.Round(DataUtility.GetRandIntervalStartPosition(point.lifespan, RangeEnd - RangeStart));
                point.birthDate += RangeStart;
                point.deathDate = point.birthDate + point.lifespan;

                data.Add(point);

                // When creating data, add to population total for that year
                for (int j = point.birthDate - RangeStart; j <= point.deathDate - RangeStart; ++j)
                {
                    ++population[j];
                }
            }

            SaveData(System.IO.Directory.GetCurrentDirectory() + "PopulationData.txt");
        }

        public bool SaveData(string filepath)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            System.IO.File.WriteAllText(filepath, json);

            return true;
        }

        public bool LoadData(string filepath)
        {
            string str = System.IO.File.ReadAllText(filepath);
            LifespanData tmp = JsonConvert.DeserializeObject<LifespanData>(str);

            data.Clear();
            for(int i = 0; i < tmp.data.Count; ++i)
            {
                data.Add(tmp.data[i]);
            }

            population.Clear();
            for (int i = 0; i < tmp.population.Count; ++i)
            {
                population.Add(tmp.population[i]);
            }

            Mean = tmp.Mean;
            StandardDeviation = tmp.StandardDeviation;
            RangeStart = tmp.RangeStart;
            RangeEnd = tmp.RangeEnd;

            return true;
        }

        public int YearWithLargestPopulation()
        {
            int max = 0;
            int year = 0;
            for(int i = 0; i < population.Count; ++i)
            {
                if (population[i] > max)
                {
                    max = population[i];
                    year = i;
                }
            }

            return year + RangeStart;
        }

        public void DebugPrintData()
        {
            for (int i = 0; i < data.Count; ++i)
            {
                Console.WriteLine("Lifespan " + i + ": " + data[i].birthDate + "-" + data[i].deathDate);
            }

            for(int j = 0; j <= RangeEnd - RangeStart; ++j)
            {
                Console.WriteLine("Population " + (j+RangeStart) + ": " + population[j]);
            }

            Console.WriteLine("Best year: " + YearWithLargestPopulation());

            Console.ReadLine();
        }
    }
}
