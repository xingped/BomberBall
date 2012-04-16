using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Platform;

namespace Bann
{
    public class GameOptions
    {
        #region Fields/Properties
        public static float NEAR = 1.0f;
        public static float FAR = 1000;
        public static float FOV = 45;
        //configuration file containing all file info
        public static String CONFIGURATION_FILE;
        
        //the filename containing the different agent files
        public static String AGENT_FILE_INFO;

        //an array of filenames for agent info
        public static List<String> AGENT_FILES;

        //the filename containing the different Map files
        public static String MAP_FILE_INFO;

        //an array of filenames for map info
        public static List<String> MAP_FILES;

        #region Options
        //the length of each round
        public static int ROUND_LENGTH;
        #endregion

        #region Mode Select

        /// <summary>
        /// List of strings to display as menu options.
        /// </summary>
        public static List<String> MODE_SELECT_OPTIONS;

        /// <summary>
        /// Texture for the background image in Mode Select.
        /// </summary>
        public static Texture2D MODE_SELECT_BACKGROUND_IMAGE;
        #endregion

        #region Team/Map Select

        /// <summary>
        /// Texture for background image in Character Selection.
        /// </summary>
        public static Texture2D TEAM_MAP_SELECT_BACKGROUND_IMAGE;

        /// <summary>
        /// List containing AgentInfo objects.
        /// </summary>
        public static List<AgentInfo> AGENT_INFO_LIST;

        #endregion

        #endregion

        //csotr
        public GameOptions()
        {
            //init lists
            AGENT_FILES = new List<String>();
            MAP_FILES = new List<String>();
            MODE_SELECT_OPTIONS = new List<String>();
            AGENT_INFO_LIST = new List<AgentInfo>();

            //get the name of the configuration file
            CONFIGURATION_FILE = "config.ini";

            //create input file
            System.IO.StreamReader fin = new System.IO.StreamReader(@CONFIGURATION_FILE);

            //start scanning in
            GetNextLine(fin);//[Agent File Info]
            AGENT_FILE_INFO = GetNextLine(fin);//agent file info

            GetNextLine(fin);   //[Map File Info]
            MAP_FILE_INFO = GetNextLine(fin);//map file info

            GetNextLine(fin); //[Options]
            GetNextLine(fin);//[Round Length]
            ROUND_LENGTH = int.Parse(GetNextLine(fin));

            GetNextLine(fin);//[Mode Select]
            ReadModeSelectMenus(fin);

            //close file
            fin.Close();

            //scan in agent files
            ReadAgentFile();

            //scan in map files
            ReadMapFile();

            //load Agent info
            LoadAgentInfo();

        }

        private void ReadModeSelectMenus(StreamReader fin)
        {
            string newMode = "";
            if (!fin.EndOfStream)
                newMode = GetNextLine(fin);
            while (!fin.EndOfStream && newMode != "[/Mode Select]")
            {
                //add new mode
                MODE_SELECT_OPTIONS.Add(newMode);
                newMode = GetNextLine(fin);
            }
        }

        

        /// <summary>
        /// Scan in agent files and add each string to a list
        /// that is representative of file names.
        /// </summary>
        private void ReadAgentFile()
        {
            //create a reader for the main agent file
            System.IO.StreamReader fin = new System.IO.StreamReader(@AGENT_FILE_INFO);
            while (!fin.EndOfStream)
            {
                AGENT_FILES.Add(GetNextLine(fin));
            }

            //close file
            fin.Close();
        }

        /// <summary>
        /// Scan in map files and add each string to a list
        /// that is representative of file names.
        /// </summary>
        private void ReadMapFile()
        {
            //create reader for the main map file
            System.IO.StreamReader fin = new System.IO.StreamReader(@MAP_FILE_INFO);
            while (!fin.EndOfStream)
            {
                MAP_FILES.Add(GetNextLine(fin));
            }

            //close file
            fin.Close();

        }

        protected void LoadAgentInfo()
        {
            for (int i = 0; i < AGENT_FILES.Count; i++)
            {
                AGENT_INFO_LIST.Add(new AgentInfo(AGENT_FILES[i]));
            }
        }

        /// <summary>
        /// Returns the next non blank line
        /// </summary>
        /// <param name="fin"></param>
        /// <returns></returns>
        private String GetNextLine(System.IO.StreamReader fin)
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
