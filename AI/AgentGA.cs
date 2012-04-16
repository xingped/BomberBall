using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI.ANN;

namespace Platform
{
    public class AgentGA
    {
        #region Fields/Properties

        protected Random m_Random;
        public Random Random
        {
            get { return m_Random; }
            set { m_Random = value; }
        }

        protected List<Genome> m_Population;
        public List<Genome> Population
        {
            get { return m_Population; }
            set { m_Population = value; }
        }

        protected int m_PopulationSize;
        public int PopulationSize
        {
            get { return m_PopulationSize; }
            set { m_PopulationSize = value; }
        }

        protected int m_ChromosomeLength;
        public int ChromosomeLength
        {
            get { return m_ChromosomeLength; }
            set { m_ChromosomeLength = value; }
        }

        protected List<int> m_SplitPoints;
        public List<int> SplitPoints
        {
            get { return m_SplitPoints; }
            set { m_SplitPoints = value; }
        }

        protected double m_TotalFitness;
        public double TotalFitness
        {
            get { return m_TotalFitness; }
            set { m_TotalFitness = value; }
        }

        protected double m_BestFitness;
        public double BestFitness
        {
            get { return m_BestFitness; }
            set { m_BestFitness = value; }
        }

        protected double m_WorstFitness;
        public double WorstFitness
        {
            get { return m_WorstFitness; }
            set { m_WorstFitness = value; }
        }

        protected double m_AverageFitness;
        public double AverageFitness
        {
            get { return m_AverageFitness; }
            set { m_AverageFitness = value; }
        }

        protected int m_FittestGenome;
        public int FittestGenome
        {
            get { return m_FittestGenome; }
            set { m_FittestGenome = value; }
        }

        protected double m_MutationRate;
        public double MutationRate
        {
            get { return m_MutationRate; }
            set { m_MutationRate = value; }
        }

        protected double m_CrossoverRate;
        public double CrossoverRate
        {
            get { return m_CrossoverRate; }
            set { m_CrossoverRate = value; }
        }

        protected int m_Generation;
        public int Generation
        {
            get { return m_Generation; }
            set { m_Generation = value; }
        }



        #endregion
        public AgentGA(int popSize, double mutRate, double crossRate, int numWeights, List<int> splits)
        {
            m_Random = Params.RANDOM;
            m_Population = new List<Genome>();
            m_PopulationSize = popSize;
            m_MutationRate = mutRate;
            m_CrossoverRate = crossRate;
            m_ChromosomeLength = numWeights;
            m_SplitPoints = splits;
            m_Generation = 0;

            //fitnesses
            m_BestFitness = 0.0;
            m_WorstFitness = double.MaxValue;
            m_AverageFitness = 0.0;
            m_FittestGenome = 0;
            m_TotalFitness = 0.0;

            //create population
            for (int i = 0; i < m_PopulationSize; i++)
            {
                //create a new Genome to add to the population
                Genome newGenome = new Genome();

                //add new genome to the population
                m_Population.Add(newGenome);

                //randomize the weight of the new genome
                for (int j = 0; j < m_ChromosomeLength; j++)
                {
                    //new random weight
                    double weight = Params.RANDOM.NextDouble();

                    //add jth weight to ith genome
                    m_Population.ElementAt(i).Weights.Add(weight);
                }
            }
        }

        /// <summary>
        /// Mutates the chromosomes
        /// </summary>
        /// <param name="chromosomes"></param>
        public List<double> Mutate(List<double> chromosomes)
        {
            //List<double> retChromo = new List<double>();
            //mutate each wate depending on the mutation rate
            for (int i = 0; i < chromosomes.Count; i++)
            {
                //check mutation rate
                if (m_Random.NextDouble() < m_MutationRate)
                {
                    //mutate the weight by adding or subtracting a small value to it
                    chromosomes[i] += (Params.RANDOM.NextDouble() - Params.RANDOM.NextDouble()) * Params.MAX_PERTURBATION_RATE;
                }
            }
            return chromosomes;
        }

        /// <summary>
        /// Used to select a genome from the population using probability 
        /// proportional to fitness
        /// </summary>
        /// <returns></returns>
        public Genome RouletteWheel()
        {
            //genome to return 
            Genome genome = m_Population[m_FittestGenome];

            //get the slice to choose from
            double slice = m_Random.NextDouble() * m_TotalFitness;

            //keep a running total of fitness to represent the current slice
            double totalFitness = 0;

            //loop through the population and return once the chosen slice is greater than the running fitness total
            for (int i = 0; i < m_Population.Count; i++)
            {
                Genome currentGenome = m_Population[i];
                totalFitness += currentGenome.Fitness;

                if (totalFitness > slice)
                {
                    genome = currentGenome;
                    break;
                }
            }

            return genome;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1">Parent chromosome 1</param>
        /// <param name="p2">Parent chromosome 2</param>
        /// <param name="b1">Empty baby chromosome 1</param>
        /// <param name="b2">Empty baby chromosome 2</param>
        public Tuple<List<double>, List<double>> Crossover(List<double> p1, List<double> p2, List<double> b1, List<double> b2)
        {
            //returns a tuple if list<dbl>
            Tuple<List<double>, List<double>> retVal;
            //check to see if a crossover is to be performed
            if (m_Random.NextDouble() > m_CrossoverRate || p1 == p2)
            {
                b1 = p1;
                b2 = p2;
                retVal = new Tuple<List<double>, List<double>>(b1, b2);
                return retVal;
            }

            //create crossover point
            int crossPoint = m_Random.Next(0, m_ChromosomeLength);

            for (int i = 0; i < p2.Count; i++)
            {
                if (i < crossPoint)
                {
                    b1.Add(p1[i]);
                    b2.Add(p2[i]);
                }
                else
                {
                    b1.Add(p2[i]);
                    b2.Add(p1[i]);
                }
            }

            retVal = new Tuple<List<double>, List<double>>(b1, b2);
            return retVal;
        }

        public void CalculateFitnessStats()
        {
            //be sure that total = 0
            m_TotalFitness = 0;

            //init best/worst fitnesses
            double bestFitness = 0;
            int bestFit = 0;

            double worstFitness = double.MaxValue;
            int worstFit = 0;

            //loop through each chromosome in the population
            for (int i = 0; i < m_Population.Count; i++)
            {
                //current genome
                Genome currentGenome = m_Population[i];

                //current fitness
                double currentFitness = currentGenome.Fitness;

                //increment total fitness
                m_TotalFitness += currentFitness;

                //update best fitness
                if (currentFitness > bestFitness)
                {
                    //update best fitness
                    bestFitness = currentFitness;
                    bestFit = i;
                }

                //update worst fitness
                if (currentFitness < worstFitness)
                {
                    //update worst fit
                    worstFitness = currentFitness;
                }
            }

            //update all values
            m_WorstFitness = worstFitness;
            m_BestFitness = bestFitness;
            m_FittestGenome = bestFit;

            //calculate avg fitness
            m_AverageFitness = m_TotalFitness / m_PopulationSize;
        }

        /// <summary>
        /// Ties everything together
        /// </summary>
        public List<Genome> Epoch(List<Genome> oldPopulation)
        {
            //assume fitness scores have been updated

            m_Population = oldPopulation;

            //sort population based on fitness
            m_Population.Sort();

            //recalcute fitness stats
            ScaleFitnessRank();

            //reset fitness stats
            //Reset();

            //create a new population
            int babyPopulation = 0;
            List<Genome> newGenomes = new List<Genome>();

            //copy over some elites
            Console.WriteLine("Cloning Initiated...");
            newGenomes = CloneElites(Params.NUM_ELITE, Params.NUM_COPIES_ELITE);
            Console.WriteLine("...Cloning Complete!");
            babyPopulation = newGenomes.Count;

            //loop until we have as many babies as the original population
            while (babyPopulation < m_PopulationSize)
            {
                //select parents
                Genome p1 = RouletteWheel();
                Genome p2 = RouletteWheel();

                //perform crossovers into two new babies
                Genome b1 = new Genome();
                Genome b2 = new Genome();

                Tuple<List<double>, List<double>> babies;
                babies = CrossoverSplitPoints(p1.Weights, p2.Weights, b1.Weights, b2.Weights);

                b1.Weights = babies.Item1;
                b2.Weights = babies.Item2;

                //mutate the babies :D
                b1.Weights = Mutate(b1.Weights);
                b2.Weights = Mutate(b2.Weights);

                //add babies to the population
                newGenomes.Add(b1);
                newGenomes.Add(b2);

                babyPopulation += 2;
            }

            //new population is now baby population
            m_Population = newGenomes;

            //increment the Generation Count
            m_Generation++;

            return m_Population;
        }

        /// <summary>
        /// Clones numCopies of numBest elites
        /// </summary>
        /// <param name="numBest">The top numBest genomes to copy</param>
        /// <param name="numCopies"> How many of the genomes to copy</param>
        /// <param name="pop"></param>
        /// <returns></returns>
        public List<Genome> CloneElites(int numBest, int numCopies)
        {
            Console.WriteLine("NumBest = " + numBest + "\nNumCopies = " + numCopies);
            List<Genome> elites = new List<Genome>();
            //be sure an even number is being copied
            if (((numBest * numCopies) % 2) != 0)
            {
                Console.WriteLine("No Clones made.");
                return elites;
            }
            //find elite copy
            int currentBest = m_PopulationSize - 1;
            while (numBest > 0)
            {
                for (int i = 0; i < numCopies; i++)
                {
                    Genome newGenome = new Genome();
                    for (int j = 0; j < m_ChromosomeLength; j++)
                    {
                        newGenome.Weights.Add(m_Population[currentBest].Weights[j]);
                    }
                    elites.Add(newGenome);
                    Console.WriteLine("Genome " + (currentBest) + " cloned.");
                }
                currentBest--;
                numBest--;
            }
            return elites;
        }

        public void ScaleFitnessRank()
        {
            int multiplier = 3;
            for (int i = 0; i < m_Population.Count; i++)
            {
                m_Population[i].Fitness = i * multiplier;
            }

            CalculateFitnessStats();
            Console.WriteLine("Best: " + m_BestFitness + "\nWorst: " + m_WorstFitness + "\n\n");
        }

        public Tuple<List<double>, List<double>> CrossoverSplitPoints(List<double> parent1, List<double> parent2, List<double> b1, List<double> b2)
        {
            Tuple<List<double>, List<double>> retVal;
            List<double> p1 = new List<double>();
            List<double> p2 = new List<double>();
            for (int i = 0; i < parent2.Count; i++)
            {
                p1.Add(parent1[i]);
                p2.Add(parent2[i]);
            }
            if (Params.RANDOM.NextDouble() > m_CrossoverRate || (p1 == p2))
            {
                b1 = p1;
                b2 = p2;

                retVal = new Tuple<List<double>, List<double>>(b1, b2);
                return retVal;
            }

            //get 2 cross points
            int cross1 = Params.RANDOM.Next(0, m_SplitPoints.Count - 2);
            int cross2 = Params.RANDOM.Next(0, m_SplitPoints.Count - 1);

            int cp1 = m_SplitPoints[cross1];
            int cp2 = m_SplitPoints[cross2];

            for (int i = 0; i < p2.Count; i++)
            {
                if (i < cp1 || i >= cp2)//outside of cross points
                {
                    b1.Add(p1[i]);
                    b2.Add(p2[i]);
                }
                else
                {
                    //switch
                    b1.Add(p2[i]);
                    b2.Add(p1[i]);
                }
            }

            retVal = new Tuple<List<double>, List<double>>(b1, b2);
            return retVal;
        }

        /// <summary>
        /// Resets necessary variables. i.e. those related to fitness
        /// </summary>
        public void Reset()
        {
            m_TotalFitness = 0;
            m_BestFitness = 0;
            m_WorstFitness = double.MaxValue;
            m_AverageFitness = 0;
        }
    }
}
