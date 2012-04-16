using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Platform
{
    public class Square
    {
        public int squareID; // Individual ID for the square
        public int X, Y; // Grid coordinates
        public int roomID; // Room ID
        public int blockType; // Block Type
        public List<Square> adjSquares; // List of adjacent squares
        public List<Square> connSquares; // List of out connected squares

        public Square()
        {

        }

        // Determines if any adjacent squares can be connected to from
        // this square and if no, return false. If yes, return true.
        public bool checkOpen()
        {
            //Console.WriteLine("Checking open Square " + this.squareID);
            bool result = false;
            foreach (Square adj in this.adjSquares)
            {
                if (adj.roomID == 0)
                    result = true;
            }
            //Console.WriteLine("CheckOpen " + this.squareID + " - Result: " + result);
            return(result);
        }
    }

    public class LevelMap
    {
        public List<Square> grid; // List of all squares on grid
        public int gridSize; // Total number of all squares on grid
        public int gridX, gridY; // X and Y dimensions of grid
        public List<Square> seeds; // List of room seeds for grid
        public List<Door> possibleDoors; // List of all possible possible doors
        public List<Door> chosenDoors; // List of all chosen doors
        public int nextRoomID; // Counter to keep track of room IDs
        public List<Square> activeSquares; // Easily keep track of active (room set) squares

        public LevelMap()
        {

        }

        public void addDoor(Door newDoor)
        {
            bool addIt = true;

            foreach (Door mapDoors in this.possibleDoors)
            {
                if ((newDoor.squareA == mapDoors.squareA && newDoor.squareB == mapDoors.squareB)
                    || (newDoor.squareA == mapDoors.squareB && newDoor.squareB == mapDoors.squareA))
                {
                    addIt = false;
                }
            }

            if (addIt)
            {
                this.possibleDoors.Add(newDoor);
            }
        }

        public void updateActiveSquares()
        {
            //Console.WriteLine("Updating Active Squares");
            this.activeSquares.RemoveAll(square => !square.checkOpen());
        }
    }

    public class Door
    {
        public Square squareA; // Door connected square with lower Room ID
        public Square squareB; // Door connected square with higher Room ID

        public Door(Square a, Square b)
        {
            squareA = a;
            squareB = b;
        }
    }

    enum BlockType
    {
        Unset,
        Empty,
        Destroy,
        Wall,
    }

    public class LevelGenerator
    {
        /*
         * Map Generator for BomberANN
         * 
         */

        public LevelGenerator()
        {

        }

        public LevelMap board;

        public void genLevel()
        {
            board = new LevelMap();
            board.grid = new List<Square>();
            board.gridX = 10;
            board.gridY = 10;
            board.gridSize = board.gridX * board.gridY;
            board.seeds = new List<Square>();
            board.possibleDoors = new List<Door>();
            board.chosenDoors = new List<Door>();
            board.nextRoomID = 1;
            board.activeSquares = new List<Square>();
            setUpSquares();
            setUpStartAreas();
            seedRoom();
            growRooms();
            connectivity();
            defineDoors();
            setWalls();
            printMap();
            translateMap();
        }

        private void setUpSquares()
        {
            // Initialize all the squares
            Console.Write("Initializing squares... ");
            int squareCntr = 0;
            for (int y = 0; y < board.gridY; y++)
            {
                for (int x = 0; x < board.gridX; x++)
                {
                    Square tempSquare = new Square();
                    tempSquare.squareID = squareCntr;
                    tempSquare.X = x;
                    tempSquare.Y = y;
                    tempSquare.roomID = 0;
                    tempSquare.blockType = (int)BlockType.Unset;
                    tempSquare.adjSquares = new List<Square>();
                    tempSquare.connSquares = new List<Square>();

                    board.grid.Add(tempSquare);

                    squareCntr++;
                }
            }
            Console.WriteLine("Complete");

            // Connect adjacent squares to each other
            Console.Write("Connecting adjacent squares... ");
            foreach (Square crntSquare in board.grid)
            {
                // Connect the square to the left
                if (crntSquare.X - 1 >= 0)
                {
                    Square adjSquare = board.grid.First<Square>(
                        delegate(Square tempSquare)
                        {
                            return(tempSquare.X == (crntSquare.X - 1)
                                && tempSquare.Y == crntSquare.Y);
                        }
                    );
                    crntSquare.adjSquares.Add(adjSquare);
                }

                // Connect the square to the top
                if (crntSquare.Y - 1 >= 0)
                {
                    Square adjSquare = board.grid.First<Square>(
                        delegate(Square tempSquare)
                        {
                            return(tempSquare.X == crntSquare.X
                                && tempSquare.Y == (crntSquare.Y - 1));
                        }
                    );
                    crntSquare.adjSquares.Add(adjSquare);
                }

                // Connect the square to the right
                if (crntSquare.X + 1 < board.gridX)
                {
                    Square adjSquare = board.grid.First<Square>(
                        delegate(Square tempSquare)
                        {
                            return (tempSquare.X == (crntSquare.X + 1)
                                && tempSquare.Y == crntSquare.Y);
                        }
                    );
                    crntSquare.adjSquares.Add(adjSquare);
                }

                // Connect the square to the bottom
                if (crntSquare.Y + 1 < board.gridY)
                {
                    Square adjSquare = board.grid.First<Square>(
                        delegate(Square tempSquare)
                        {
                            return (tempSquare.X == crntSquare.X
                                && tempSquare.Y == (crntSquare.Y + 1));
                        }
                    );
                    crntSquare.adjSquares.Add(adjSquare);
                }
            }
            Console.WriteLine("Complete");
        }

        private void setUpStartAreas()
        {
            /*
             * First set the starting positions as room seeds
             */

            Console.Write("Adding starting seeds... ");
            // Add the top left corner
            board.seeds.Add
            (
                board.grid.First<Square>
                (
                    delegate(Square tempSquare)
                    {
                        return (tempSquare.X == 0 && tempSquare.Y == 0);
                    }
                )
            );

            // Add the top right corner
            board.seeds.Add
            (
                board.grid.First<Square>
                (
                    delegate(Square tempSquare)
                    {
                        return (tempSquare.X == (board.gridX - 1) && tempSquare.Y == 0);
                    }
                )
            );

            // Add the bottom right corner
            board.seeds.Add
            (
                board.grid.First<Square>
                (
                    delegate(Square tempSquare)
                    {
                        return (tempSquare.X == (board.gridX - 1) && tempSquare.Y == (board.gridY - 1));
                    }
                )
            );

            // Add the bottom left corner
            board.seeds.Add
            (
                board.grid.First<Square>
                (
                    delegate(Square tempSquare)
                    {
                        return (tempSquare.X == 0 && tempSquare.Y == (board.gridY - 1));
                    }
                )
            );
            Console.WriteLine("Complete");

            /*
             * Next, connect the initial seeds with their adjacent squares.
             * Also set the square sets to the same room and make blockType Empty.
             */
            Console.Write("Connecting initial seeds to adjacent tiles... ");
            foreach(Square seedSquare in board.seeds)
            {
                seedSquare.blockType = (int)BlockType.Empty;
                seedSquare.roomID = board.nextRoomID;
                board.nextRoomID++;
                
                foreach (Square seedAdjSquare in seedSquare.adjSquares)
                {
                    seedAdjSquare.blockType = (int)BlockType.Empty;
                    seedAdjSquare.roomID = seedSquare.roomID;
                    seedSquare.connSquares.Add(seedAdjSquare);

                    // Should always return as true and add to active squares
                    //board.addActiveSquare(seedAdjSquare);
                    if (!board.activeSquares.Contains(seedAdjSquare))
                    {
                        board.activeSquares.Add(seedAdjSquare);
                    }
                    board.updateActiveSquares();

                }
                // Should always return as false and not add to active squares
                //board.addActiveSquare(seedSquare);
                if (!board.activeSquares.Contains(seedSquare))
                {
                    board.activeSquares.Add(seedSquare);
                }
                board.updateActiveSquares();

            }
            Console.WriteLine("Complete");
        }

        private void seedRoom()
        {
            /*
             * The initial four map seeds have already been added.
             * An additional 2-4 seeds will be placed across the map.
             */

            //board.grid.ElementAt<Square>(12).roomID = board.nextRoomID;
            //board.activeSquares.Add(board.grid.ElementAt<Square>(12));
            //board.updateActiveSquares();

            
            Console.Write("Adding random room seeds... ");
            // Determine a random number of additional room seeds between 1 and 4
            Random r = new Random();
            int numAddSeeds = r.Next(1,4);

            // Add these room seeds to the list of seeds
            for (int i = 0; i < numAddSeeds; i++)
            {
                int seedPos;

                // Loop until a square that is not already set aside as
                // one of the starting position rooms is chosen.
                do
                {
                    seedPos = r.Next(board.gridSize);
                } while (board.grid.ElementAt<Square>(seedPos).roomID != 0);

                //board.addActiveSquare(board.grid.ElementAt<Square>(seedPos));
                if (!board.activeSquares.Contains(board.grid.ElementAt<Square>(seedPos)))
                {
                    board.activeSquares.Add(board.grid.ElementAt<Square>(seedPos));
                }
                board.updateActiveSquares();

                board.grid.ElementAt<Square>(seedPos).roomID = board.nextRoomID;
                board.seeds.Add(board.grid.ElementAt<Square>(seedPos));
                board.nextRoomID++;
            }
            Console.WriteLine("Complete");
            
        }

        private void growRooms()
        {
            /*
             * The initial four rooms are grown or placed in the corners of the map.
             * The other rooms are grown from the randomly placed seeds.
             * 
             * 1. Select random cell from list of active cells.
             *    
             * 2. If the cell has an empty neighboring cell, that will be activated, 
             *    given the same room id, and added to the active cell list.
             * 
             * 3. If the cell has no available empty neighboring cell, it would be 
             *    removed from the list.
             *  
             * 4. This loops until all of the cells are filled, defining the shapes 
             *    of each of the rooms.
             */

            Console.Write("Growing rooms... ");
            //int i = 0;
            while (board.activeSquares.Count > 0)
            {
                //Console.WriteLine("Iteration " + i);
                //i++;
                Random r = new Random();

                int chosenGrowSquareIndex = r.Next(0, board.activeSquares.Count);
                Square chosenGrowSquare = board.activeSquares.ElementAt<Square>(chosenGrowSquareIndex);

                // Create a list of all possible growth squares from the chosen grow square
                List<Square> possibleGrow = new List<Square>();
                foreach (Square adjSquare in chosenGrowSquare.adjSquares)
                {
                    //Console.WriteLine("Checking square " + adjSquare.squareID);
                    if (adjSquare.roomID == 0)
                    {
                        possibleGrow.Add(adjSquare);
                    }
                }

                // Connect a random square from the list of possibleGrow squares and connect it to the grow Square
                int nextSquarePos = r.Next(0, possibleGrow.Count);
                Square chosenGrowth = possibleGrow.ElementAt<Square>(nextSquarePos);
                chosenGrowSquare.connSquares.Add(chosenGrowth);
                chosenGrowth.roomID = chosenGrowSquare.roomID;
                //board.addActiveSquare(chosenGrowth);
                if (!board.activeSquares.Contains(chosenGrowth))
                {
                    //Console.WriteLine("Adding active square " + chosenGrowth.squareID);
                    board.activeSquares.Add(chosenGrowth);
                }
                board.updateActiveSquares();
            }
            Console.WriteLine("Complete");
        }

        private void connectivity()
        {
            /*
             * 1. The cells of neighboring rooms will be noted and a list of 
             *    possible door locations will be created.
             * 
             * Doors should be standardized on lower room ID first, higher second.
             * 
             * 2. A separate list will be generated noting what rooms are 
             *    accessible to each other room.
             *    
             * I think the accessible room list is only necessary for game flow.
             * 
             * On the other hand, I could use it to easily keep track of which 
             * sets of doors need to have a door chosen from.
             * 
             * I think a double dictionary would work well to keep a list of doors to choose from.
             * room -> connected room -> doors
             */

            Console.Write("Generating all possible door locations... ");
            foreach (Square gridSquare in board.grid)
            {
                foreach (Square adjGridSquare in gridSquare.adjSquares)
                {
                    if (adjGridSquare.roomID != gridSquare.roomID)
                    {
                        if (adjGridSquare.roomID < gridSquare.roomID)
                        {
                            Door tempDoor = new Door(adjGridSquare, gridSquare);
                            board.addDoor(tempDoor);
                        }
                        else if (adjGridSquare.roomID > gridSquare.roomID)
                        {
                            Door tempDoor = new Door(gridSquare, adjGridSquare);
                            board.addDoor(tempDoor);
                        }
                    }
                }
            }
            Console.WriteLine("Complete");
        }

        private void gameFlow()
        {
            /*
             * 1. Starting location will be selected.
             * 
             * 2. Starting from the start location, connecting paths between 
             *    rooms will be defined.
             */
        }

        private void defineDoors()
        {
            /*
             * 1. Doors will be selected from the possible door locations list.
             * 
             * The door locations list is every barrier between two adjacent cells
             * where the room id of the two locations differ.
             * 
             * Pick one location between each set of rooms would be a good door set.
             */

            Console.Write("Choosing doors... ");

            List<Door> DoorsToChoseFrom = new List<Door>();

            // Doors were organized such that the lower room number is always squareA.
            // Thus, it is easy to check moving up with the lower room as the starting 
            // room and moving up through all the higher level rooms. This ensures each
            // connected room pair is only hit once.
            for (int i = 1; i < board.nextRoomID; i++)
            {
                for (int j = i; j < board.nextRoomID; j++)
                {
                    DoorsToChoseFrom = new List<Door>();

                    foreach (Door possDoor in board.possibleDoors)
                    {
                        // First add all doors that meet the room conditions to the list of
                        // doors for the room pair to choose from.
                        if (possDoor.squareA.roomID == i && possDoor.squareB.roomID == j)
                        {
                            DoorsToChoseFrom.Add(possDoor);
                        }
                    }

                    // Next choose a random door from that list and add it to the board's
                    // list of chosen doors.
                    if (DoorsToChoseFrom.Count != 0)
                    {
                        Random r = new Random();
                        int doorChoose = r.Next(DoorsToChoseFrom.Count);
                        board.chosenDoors.Add(DoorsToChoseFrom.ElementAt<Door>(doorChoose));
                    }
                }
            }
            Console.WriteLine("Complete");
        }

        private void setWalls()
        {
            /*
             * Walls are defined to be squares not adjacent to a door that do
             * not lead in connecting out to another square. Block these with
             * Invulnerable bloks. Fill the rest of the empty space with 
             * approximately 2/3rds Destructible blocks.
             */

            Console.Write("Setting walls... ");
            // Set all squares not part of a door and not connected out as walls.
            foreach (Square square in board.grid)
            {
                if (square.blockType == (int)BlockType.Unset)
                {
                    bool doorAdj = false;
                    foreach (Door door in board.chosenDoors)
                    {
                        if (square.squareID == door.squareA.squareID
                            || square.squareID == door.squareB.squareID)
                        {
                            doorAdj = true;
                        }
                    }

                    if (!doorAdj && square.connSquares.Count == 0)
                    {
                        square.blockType = (int)BlockType.Wall;
                    }
                }
            }
            Console.WriteLine("Complete");

            // Set approximately 2/3rds of all other empty blocks as destructible blocks
            // where the blocks are not one of the three squares in each starting position.
            Console.Write("Setting destructible walls... ");
            Random r = new Random();
            foreach (Square square in board.grid)
            {
                if (square.blockType == (int)BlockType.Unset)
                {
                    int newType = r.Next(1, 3);
                    
                    if (newType == 1)
                    {
                        square.blockType = (int)BlockType.Destroy;
                    }
                    else if (newType == 2)
                    {
                        square.blockType = (int)BlockType.Empty;
                    }
                }
            }
            Console.WriteLine("Complete");
        }

        private void printMap()
        {
            Console.WriteLine("Printing Map...");
            Console.WriteLine(board.gridX + " " + board.gridY);
            int x = 0, y = 0;
            foreach (Square square in board.grid)
            {
                Console.Write(square.blockType + " ");
                x = (x + 1) % board.gridX;
                y = (y + 1) % board.gridY;
                if (x == 0)
                    Console.WriteLine();
            }
        }

        private void translateMap()
        {
            String fileName = "Maps\\MapRand.ini";
            //int i = 1;
            //while (File.Exists(fileName))
            //{
            //    i++;
            //    fileName = "Map" + i.ToString() + ".ini";
            //}
            StreamWriter writer = new StreamWriter(fileName);

            int scale = 3;
            int halfX = board.gridX / 2;
            int halfY = board.gridY / 2;

            // Write the outer Walls of the map first
            writer.WriteLine("[Walls]");

            // Top border
            for (int x = -1; x < (board.gridX + 1); x++)
            {
                int y = -1;
                int xPos = (x - halfX) * 2 * scale;
                int yPos = (y - halfY) * 2 * scale;

                writer.WriteLine(xPos + " " + 0 + " " + yPos);
            }

            // Bottom border
            for (int x = -1; x < (board.gridX + 1); x++)
            {
                int y = board.gridY;
                int xPos = (x - halfX) * 2 * scale;
                int yPos = (y - halfY) * 2 * scale;

                writer.WriteLine(xPos + " " + 0 + " " + yPos);
            }

            // Left border
            for (int y = -1; y < (board.gridY + 1); y++)
            {
                int x = -1;
                int xPos = (x - halfX) * 2 * scale;
                int yPos = (y - halfY) * 2 * scale;

                writer.WriteLine(xPos + " " + 0 + " " + yPos);
            }

            // Right border
            for (int y = -1; y < (board.gridY + 1); y++)
            {
                int x = board.gridX;
                int xPos = (x - halfX) * 2 * scale;
                int yPos = (y - halfY) * 2 * scale;

                writer.WriteLine(xPos + " " + 0 + " " + yPos);
            }

            // Write the actual map Walls second
            foreach (Square square in board.grid)
            {
                if (square.blockType == (int)BlockType.Wall)
                {
                    int xPos = (square.X - halfX) * 2 * scale;
                    int yPos = (square.Y - halfY) * 2 * scale;

                    writer.WriteLine(xPos + " " + 0 + " " + yPos);
                }
            }

            // Write the Blocks third
            writer.WriteLine("[Blocks]");
            foreach (Square square in board.grid)
            {
                if (square.blockType == (int)BlockType.Destroy)
                {
                    int xPos = (square.X - halfX) * 2 * scale;
                    int yPos = (square.Y - halfY) * 2 * scale;

                    writer.WriteLine(xPos + " " + 0 + " " + yPos);
                }
            }

            // Next place the Node locations (everywhere except Walls)
            writer.WriteLine("[Nodes]");
            foreach (Square square in board.grid)
            {
                if (square.blockType != (int)BlockType.Wall)
                {
                    int xPos = (square.X - halfX) * 2 * scale;
                    int yPos = (square.Y - halfY) * 2 * scale;

                    writer.WriteLine(xPos + " " + 0 + " " + yPos);
                }
            }
            
            // Write targets
            writer.WriteLine("[Target]");
            // For the intial testing, put in opposite corner from spawn
            writer.WriteLine(((board.gridX - 1 - halfX) * 2 * scale)
                + " " + 0 + " " + ((board.gridY - 1 - halfY) * 2 * scale));

            // Write the Spawns fourth
            writer.WriteLine("[Spawn]");
            // Top Left spawn
            writer.WriteLine(((0 - halfX) * 2 * scale)
                + " " + 0 + " " + ((0 - halfY) * 2 * scale));

            // Top Right spawn
            writer.WriteLine(((board.gridX - 1 - halfX) * 2 * scale)
                + " " + 0 + " " + ((0 - halfY) * 2 * scale));

            // Bottom Right spawn
            writer.WriteLine(((board.gridX - 1 - halfX) * 2 * scale)
                + " " + 0 + " " + ((board.gridY - 1 - halfY) * 2 * scale));

            // Bottom Left spawn
            writer.WriteLine(((0 - halfX) * 2 * scale)
                + " " + 0 + " " + ((board.gridY - 1 - halfY) * 2 * scale));

            // End Map writer
            writer.WriteLine("[End]");
            writer.Close();
        }
    }
}
