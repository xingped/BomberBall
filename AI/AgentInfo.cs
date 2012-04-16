using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Platform
{
    public class AgentInfo
    {
        #region Fields/Properties

        protected String m_Name;
        public String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        protected String m_Filename;
        public String Filename
        {
            get { return m_Filename; }
            set { m_Filename = value; }
        }

        protected List<double> m_Weights;
        public List<double> Weights
        {
            get { return m_Weights; }
            set { m_Weights = value; }
        }

        #endregion

        //ctor
        public AgentInfo(String filename)
        {
            //init the name to be an empty string
            m_Name = "";

            //init the list of weights
            m_Weights = new List<double>();

            LoadAgentInfo(filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void LoadAgentInfo(String filename)
        {
            //create a new stream reader
            StreamReader fin = new StreamReader(@filename);

            GetNextLine(fin);   //[Name]
            m_Name = GetNextLine(fin);//assign name
            GetNextLine(fin);   //[/Name]

            LoadAgentWeights(fin);//[/Weights]
            GetNextLine(fin); //[End]

            //close file
            fin.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fin"></param>
        protected void LoadAgentWeights(StreamReader fin)
        {
            String currentString = "";
            if (!fin.EndOfStream)
            {
                //get next line
                currentString = GetNextLine(fin);
            }
            
            while(currentString != "[Weights]")
            {
                currentString = GetNextLine(fin);
            }

            currentString = GetNextLine(fin);   //should be first weight

            while (currentString != "[/Weights]")
            {
                //add to the weights
                m_Weights.Add(double.Parse(currentString));

                //get next line
                currentString = GetNextLine(fin);
            }
        }

        /// <summary>
        /// Returns the next non blank line
        /// </summary>
        /// <param name="fin"></param>
        /// <returns></returns>
        protected String GetNextLine(System.IO.StreamReader fin)
        {
            String retVal = fin.ReadLine();
            while (retVal == " " || retVal == "\n")
            {
                retVal = fin.ReadLine();
            }

            return retVal;
        }
    }
}
