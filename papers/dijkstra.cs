// By M. Yas. Davoodeh
// This is my first program written in C# as a highschool project.
// THE QUALITY OF IT IS NOT ACCEPTABLE! NOTICE AGAIN: THIS IS A HIGHSCHOOL LEVEL CODE!
/*
Some notes:

Handwriten/Hardcore Codes: codes which are in noobish style and not flexible.
They are so easy to break. Check every single one of them if you make any change to the program.

Modes: This little program supports two modes of inputs. One (Default) helps you
with creating a graph in peace and breaks relatively hardly (for users). The Second one
helps you to develop and input a graph ASAP.
In the latter mode you have to make a flawless "String" made specifically for this mode
and use it as input. I made this mode for me and it is super buggy!
To make a "Input string" you basically use every input separated with a space, in order.
If you provide the program with such a string a Bot function will Enter
String Mode Syntax:
`[NodesCount] [theCostOfEdge1] [From] [To] [theCostofEdge2] [From] [To] ...
[-3 (just when you are done with entring inputs)] [StartNode] [DestinationNode]`
e.g. The First Graph in my Documentation about Dijkstra:
    `7 2 a b 5 a c 6 b c 1 b d 3 b e 4 d e 8 c f 9 e g 7 f g -3 a g`

Extra: Extra things are just Extra (like Updates) and *I* even don't need them
in anycase BUT I thought it's better to keep them here for further development.
(because back then I didn't know such thing as Git exists.)

This program only supports inputs up to 20 nodes.
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace router
{
    internal class Program
    {
        private const bool  Error           = true,             // When true user receives "Error"s and Feedbacks
                            Reportavailable = true;             // When true you can call table by -1 in most inputs
        private const byte  Lettercode      = 64,               // 64 for CAPs 96 for small-caps letter signs
                            Maxnodetosupport= 26;               // as letters are 26 //TODO extend me
        private const int   Infinite        = int.MaxValue;     // Extend Infinite when extend var type : Handwrite*
        public enum Alerts { Error, Info, Notice, Success, Default, DefaultWithBR }
        private static Dijkstra.Node[]  _dijkstraNodes; 
        private static int[,]           _nodesTableInts;        // The Graph states and Definition is goes in this table
        private static int              _stringNthRequired = 1; // is used in StringMode and makes string into Fragments
        private static byte             _nodesCount;            // Number of Nodes
        private static string           _string;
        private static char             _sourceNode,
                                        _destinationNode;
        private static bool             _stringMode;
        
        private static bool Mapper ()
        {
            //Mapper mode to Input the network you want to find shortest path in it (Mapper Mode)

            //Intro      
            string header_temp = "Mapper Mode";
            Output.Visuals.HeaderFooter.Start(header_temp);

            //Input: Number of Nodes
            string output_temp = "Enter the number of nodes on your network: ";
            if (!_stringMode)
                _nodesCount = (byte)Input.Int32(output_temp, 3, Maxnodetosupport, checkReportPossible: false);
            else
            {
                _string = Input.String(
                    "(If you don't know what you are doing run DefaultMode by entering 1)" + "\n" + 
                    "Input THE string you want to apply:", Alerts.Notice, false);
                _nodesCount = byte.Parse(Job.ExtractNumbers(Input.Bot(output_temp)));
            }

            
            //Initialize Table of Network States or NodeTable
            _nodesTableInts = new int[_nodesCount + 1, _nodesCount + 1]; //+1 because of Table header and labels
            //fill the table/array with headers and labels
            _nodesTableInts[0, 0] = -1;
            for (int i = 1; i <= _nodesCount; i++)
            {
                _nodesTableInts[0, i] = i + Lettercode;
                _nodesTableInts[i, 0] = i + Lettercode;
                _nodesTableInts[i, i] = -1;
            }
            
            
            //Input: Data Entry (for NodeTable)
            Output.Alert("START: Data entry", Alerts.Notice);
            while (true)
            {
                output_temp = "Enter the Cost-Price you want (-3 to break): ";
                int data = 
                    _stringMode? int.Parse(Job.ExtractNumbers(Input.Bot(output_temp))) : Input.Int32(output_temp, -3);          
                
                if (data < 0)
                {
                    if (data == -3)
                    {
                        Output.Alert("END: Data entry", Alerts.Notice);
                        break;
                    }
                    continue;
                }

                char x, y;
                if (_stringMode)
                {
                    x = char.Parse(Input.Bot("From: "));
                    y = char.Parse(Input.Bot("To: "));
                }
                else 
                {
                    char endNodeChar = (char)(_nodesCount + Lettercode);
                    x = Input.Char("From: ", max: endNodeChar);
                    y = Input.Char("To: ", max: endNodeChar);
                }   
                if (x != y)
                {
                    _nodesTableInts[x - Lettercode, y - Lettercode] = data;
                    _nodesTableInts[y - Lettercode, x - Lettercode] = data;
                    continue;
                }
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (Error) Output.Alert("You cannot move from a place to itself. That's non-sense.", Alerts.Error);
                Output.BR();
            }
            
            
            //Outro
            Output.Visuals.HeaderFooter.End(header_temp);
            Output.BR();
            if (_stringMode)
                Output.Alert("There is Your Network! You are all done here.\n" + 
                             "You'll go to next session now." + "\n" + 
                             "Where you can process this network (print after this message) you made.", Alerts.Success);
            else
                Input.String("There is Your Network! You are all done here.\n" + 
                             "Press anykey to go to next part." + "\n" + 
                             "Where you can process this network (print after this message) you made.\n" +
                             "Press any key to Continue... ", Alerts.Success);
            Output.BR();
            return true;
        }

        private static bool Router()
        {
            //Router Mode to find the shortest map you gave before in Mapper Mode (Router Mode)
            //Intro
            string header_temp   = "Router Mode";
            Output.Visuals.HeaderFooter.Start(header_temp);
            
            //Inputs: Start/Source Node and Destination Node
            string output_temp   = "Enter start Node: ";
            if (_stringMode)
            {
                _sourceNode      = char.Parse(Input.Bot( output_temp               ));
                _destinationNode = char.Parse(Input.Bot( "Enter destination node: "));
                if (_sourceNode == _destinationNode)
                {
                    Output.Alert(
                        "FATAL: SOURCE AND DESTINATION CANNOT BE THE SAME. (FATAL ONLY IN STRING MODE)", Alerts.Error);
                    return false;
                }
            }
            else
            {
                char endNodeChar_temp = (char)(_nodesCount + Lettercode);
                _sourceNode           = Input.Char(output_temp, max: endNodeChar_temp);
                while (true)
                {
                    _destinationNode  = Input.Char("Enter destination node: ", max: endNodeChar_temp);
                    if (_sourceNode  == _destinationNode)
                        Output.Alert("Source and Destination cannot be the same.", Alerts.Error         );
                    else
                        break;
                }
            }
            
            //Check possibility
            Output.Alert("Checking Connections...", Alerts.Info);
            if (!Job.CheckNodeConnection(_sourceNode))
            {
                Output.Alert("FATAL: START/SOURCE NODE IS DISCONNECTED FROM THE NETWORK!"     , Alerts.Error  );
                return false;
            }
            if(!Job.CheckNodeConnection(_destinationNode))
            {
                Output.Alert("FATAL: DESTINATION NODE IS DISCONNECTED FROM THE NETWORK!"      , Alerts.Error  );
                return false;
            }
            Output.Alert("Start Node and Destination Node are Connected to the Network...", Alerts.Success);
            Console.WriteLine("Let's see if it's possible to find Shortest Path from Start Node to Destination Node." );
            
            //Dijkstra Algorithm
            Output.Alert("START: Dijkstra's Process", Alerts.Notice);
            Dijkstra.Run();
            
            Output.Visuals.HeaderFooter.End(header_temp);
            return true;
        }
        public static void Main()
        {
            //Intro 
            Output.Alert("Usually, when input available," + "\n" +
                         "\t" + "you can get a Table of Network States (NodeTable) by entering \"-1\".", Alerts.Info  );
            
            //Input: Mode*
            Output.Alert("How you wanna enter output?"                                                 , Alerts.Notice);
            Output.Alert("CAUTION: String Mode (2) Has No Debugger and is NOT stable..." + "\n" +
                         "\t" + "It's generally for test and Develop..." +
                         "\t" + "Do NOT use it if you are not sure about inputs."        , Alerts.Error, false        );      
            int inputMode_temp =
                Input.Int32("Enter Mapper Mode: (Defualt: '1' or String: '2') ", 1, 2, checkReportPossible: false);
            switch (inputMode_temp)
            {
                case 1:
                    _stringMode = false;
                    break;
                case 2:
                    _stringMode = true;
                    break;
            }
            
            //Mapper mode to Input the network you want to find shortest path in it (Mapper Mode)
            if (!Mapper())
                return;
            
            Output.Visuals.Table.Int(_nodesTableInts);
            
            //Router Mode to find the shortest map you gave before in Mapper Mode (Router Mode)
            if (!Router())
                return;
            
            Console.ReadKey();
            
        }
        
        private class Dijkstra
        {
            
            public class Node
            {
                //public string Name; //For Update*
                public char   Status = 't', //Status can be 'p' (Permanent) or 't' (Temporary) TODO explain more
                              ForeNode;
                public int    PathCost;
            }

            //We use a -1 when it comes to _dijkstraNodes Array
            //because array starts at 0 but first array (nodeTable) starts at 1
            
            private static void Init()
            {
                //Initialize
                //Dijkstra: at first we guess all Nodes' PathCost. Source Node = 0 and rest are Infinite
                _dijkstraNodes = new Node[_nodesCount];
                for (int i = 0; i < _nodesCount; i++)
                {
                    _dijkstraNodes[i] = new Node
                    {
                        //Name = (i + Lettercode + 1).ToString(),
                        ForeNode = '-',
                        PathCost = Infinite
                    };
                }
                _dijkstraNodes[_sourceNode - Lettercode -1].Status = 'p';
                _dijkstraNodes[_sourceNode - Lettercode -1].PathCost = 0;
            }

            
            private static char[] ReturnNodesToSelectMinBetween(bool FirstTimeFor_sourceNode)
            {
                string nodesToSelectMinBetween = "";
                char[] tempNodesConnectedTo    = Get.TempNodesConnectedTo(_sourceNode);
                if (FirstTimeFor_sourceNode)
                    nodesToSelectMinBetween    = Get.StringFromCharArray (tempNodesConnectedTo); 
                else
                    for (int i = 0; i < _nodesCount; i++)
                    {
                        char status;
                        try { status = _dijkstraNodes[i].Status; }
                        catch { continue; }
                        if (status == 't')
                            nodesToSelectMinBetween += (char) (i + Lettercode + 1);
                    }
                return Get.CharArrayFromString(nodesToSelectMinBetween);
            }
            
            
            private static char SelectMinChar(string nodesToSelectMinBetween, int shift = 0)
            {
                //Dijkstra: Between Candidate nodes (from first step) we select the one with Lowest Cost (minChar = )
                //We Turn the MinChar (Return of this Function) into next CurrentNode
                //Shift doesn't let program to do any loops if all values are same.
                //If there are two candidates for minChar first time it will select first next time it will select next.
                int  c       =  0 ;
                int  min     =  0 ;
                char minChar = '0';
                foreach (char node in nodesToSelectMinBetween)
                {
                    if (c == shift)
                    {
                        min     = _dijkstraNodes[node - Lettercode - 1].PathCost;
                        minChar = node;
                    }
                    else if (c > shift)
                    {
                        if (_dijkstraNodes[node - Lettercode - 1].PathCost < min)
                        {
                            min     = _dijkstraNodes[node - Lettercode - 1].PathCost;
                            minChar = node;
                        }
                    }
                    c++;
                }
                return minChar;
            }
            
            
            private static char[] FindForeNodes(char node, string path_memory = "")
            {
                //This Function Finds the Path from _destinationNode back to the source/start point
                char foreNode = _dijkstraNodes[node - Lettercode -1].ForeNode;
                if (foreNode != '-')
                {
                    path_memory += foreNode;
                    return FindForeNodes(foreNode, path_memory);
                }
                char[] pathChars = new char[path_memory.Length +1];
                int counter = path_memory.Length -1;
                foreach (char station in path_memory)
                    pathChars[counter--] = station;

                pathChars[path_memory.Length] = _destinationNode;
                return pathChars;
            }
            
            
            public static void Run()
            {
                Init();
                //Dijkstra: Start from _sourceNode (start point). we set it to 
                char   currentNode = _sourceNode;
                Output.Alert("currentNode = " + currentNode, Alerts.Info);
                bool   FirstTimeFor_sourceNode = true;
                string error       = "";
                int    shift       = 0;
                char   minChar     = '-';
                //Dijkstra: Then we select first Node that is connected to _sourceNode (start Point) 
                while (true)
                {
                    if (error == "fragmental Network") break;
                    string nodesToSelectMinBetween = 
                        Get.StringFromCharArray(ReturnNodesToSelectMinBetween(FirstTimeFor_sourceNode));
                    if (nodesToSelectMinBetween != "")
                         Output.Alert(
                             "Nodes To Select a Minimum Between them are: " + 
                              Output.Arrays.Char(Get.CharArrayFromString(nodesToSelectMinBetween), false)
                             , Alerts.Info);
                    else
                    {
                        Output.Alert
                            ("There is No Node Left To Select a Minimum Between them and Continue Process! Breaking..."
                            , Alerts.Notice);
                        break;
                    }
                    char[] tempNodesConnectedTo = Get.TempNodesConnectedTo(currentNode);
                    if (tempNodesConnectedTo != null)
                    {                    
                        //skip and redo current node with a new node in NodesToSelectMinBetween
                        Output.Alert(
                            "Nodes Connected to Node " + currentNode + " are: " + 
                             Output.Arrays.Char(tempNodesConnectedTo, false)
                            , Alerts.Info);
                        foreach (char node in tempNodesConnectedTo)
                        {
                            Output.Alert("chosenNode = " + node, Alerts.Info);
                            //Dijkstra: Here We Calculate the PathCost for each node
                            //PathCost = (CurrentNode's PathCost + the Cost of going to new Node)
                            int pathCost_temp = 
                                _dijkstraNodes[currentNode - Lettercode - 1].PathCost +
                                Get.DataFromArray.NodeTableInts(currentNode, node);
                            if (pathCost_temp < 0)
                            {
                                error = "fragmental Network";
                                break;
                            }
                            Output.Alert("totalPathCost = " + pathCost_temp, Alerts.Info);
                            //Dijkstra: If the calculated PathCost is less than previous PathCost we update it with ...
                                //...new PathCost. Then we set a mark (ForeNode) that reminds us how we got to this node
                            if (pathCost_temp < _dijkstraNodes[node - Lettercode - 1].PathCost)
                            {
                                Output.Alert("last cost = " + _dijkstraNodes[node - Lettercode - 1].PathCost,
                                    Alerts.Info);
                                _dijkstraNodes[node - Lettercode - 1].PathCost = pathCost_temp;
                                _dijkstraNodes[node - Lettercode - 1].ForeNode = currentNode;
                                Output.Alert("UPDATE: value applied", Alerts.Info, false);
                            }
                            //Dijkstra: we keep re-doing these steps till we declare state of all nodes which are ...
                                //...connected to the _sourceNode. then we process the Graph
                            //Dijkstra: There are a bit difference in how Dijkstra proceeds the first time (nodes ...
                                //... Connected to the _sourceNode), how it generally runs in graph and finally how ...
                                //... it does it in last time... PLEASE pay attention to details of these Method (Run())
                        }
                    }
                    else
                    {
                        Output.Alert
                            ("CAUTION: Failed: Could NOT find any node ahead of this node...", Alerts.Error, false);
                        Output.Alert("Retrying other node"                                   , Alerts.Notice      );
                    }
                    
                    //Dijkstra: Select Minimum Node in this graph to Set as new CurrentNode
                    //Shift doesn't let program to do any loops if all values are same.
                    //If there are two candidates for minChar first time it will select first, then next...
                    char lastMinChar = minChar;
                    minChar          = SelectMinChar(nodesToSelectMinBetween, shift  );
                    if (!FirstTimeFor_sourceNode &&
                        lastMinChar == minChar   &&
                        lastMinChar != nodesToSelectMinBetween[nodesToSelectMinBetween.Length -1])
                    {
                        minChar      = SelectMinChar(nodesToSelectMinBetween, ++shift);
                    }
                    else if (FirstTimeFor_sourceNode)
                    {
                        minChar      = SelectMinChar(nodesToSelectMinBetween         );
                        FirstTimeFor_sourceNode = false;
                    }
                    else if (lastMinChar == minChar)
                    {
                        break;
                    }
                    
                    currentNode = minChar;
                    
                    Output.Alert("minchar = " + minChar, Alerts.Info);
                    //Dijkstra: when here: we restart the whole process to find the path to _destinationNode ...
                        //... (when _destinationNode.Status = Permanent)
                    //In my app I process till all nodes go 'p' because that way we have something named 'Shortest ...
                        //... Path Tree' which helps us to find any shortest path from this constant start point. ...
                        //... In other words: My app draws a Shortest Path Tree from Start point to everywhere then ...
                        //... Selects the destination and gets the path via the tree.
        //TODO add a branch which this class is different: so program can draw the tree and print it: no destination!
                    if (_dijkstraNodes[currentNode - Lettercode -1].PathCost != Infinite)
                        _dijkstraNodes[currentNode - Lettercode -1].Status = 'p';
                    Output.Alert("currentNode's (" + currentNode +") status = " + 
                                 _dijkstraNodes[currentNode - Lettercode - 1].Status
                                , Alerts.Info);
                    Output.BR();
                }
                Output.Alert("END: Dijkstra Process", Alerts.Notice);
                Output.BR(5);


                int pathCost = _dijkstraNodes[_destinationNode - Lettercode - 1].PathCost;
                if (pathCost == Infinite || error == "fragmental Network") 
                    Output.Alert("This Destination is not Accessable from this Start Node", Alerts.Error);
                else
                    Output.Alert("This is the cost of shortest path possible: " +
                                 pathCost + " via path between these Nodes: "   + 
                                 Output.Arrays.Char(FindForeNodes(_destinationNode), false)
                        , Alerts.Success);
            }
        }
        
        private class Output
        {
            public static void BR(int number = 1)
            {
                for (int i = 1; i <= number; i++)
                    Console.WriteLine();
            }
            public class Visuals
            {
                [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
                public class HeaderFooter
                {
                    public static string Start(string data, bool print = true)
                    {
                        data = ("\n╔═════════════════════════════════════════════" +
                                "\n╠ >>> START: " + data +
                                "\n╟─────────────────────────────────────────────\n");
                        if (print) Alert(data, Alerts.Notice, false);
                        return data;
                    }
                    public static string End(string data, bool print = true)
                    {
                        data = ("\n╟─────────────────────────────────────────────" + 
                                "\n╠ >>> END: " + data +
                                "\n╚═════════════════════════════════════════════\n");
                        if (print) Alert(data, Alerts.Notice, false);
                        return data;
                    }
                }
                public class Table
                {
                    public static void Int(int[,] array)//prog*
                    {
                        string mode_temp = "Int Table Output";
                        HeaderFooter.Start(mode_temp);
                        Console.Write("║     ");
                        int i;
                        for (i = 1; i < array.GetLength(0); i++)
                            Console.Write("│ [" + (char) array[i, 0] + "] ");
                        for (i = 1; i < array.GetLength(0) ; i++)
                        {
                            Console.Write("\n║ [" + (char) array[i, 0] + "]");
                            for (int j = 1; j < array.GetLength(1); j++)
                                Console.Write(" │ {0, 3}", array[i, j]);
                        }
                        BR();
                        HeaderFooter.End(mode_temp);
                    }
                }
            }
            public class Arrays
                {
                    public static string Char(char[] array, bool print = true, bool error = Error)
                    {
                        if (array == null)
                        {
                            if (error) Alert("Null Array to visualize!", Alerts.Error);
                            return "";
                        }
                        string data = ("{" + array[0]);
                        int i = 1;
                        if (array.Length == 1)
                            data += ("}");
                        else if (array.Length > 1)
                        {
                            data += (", ");
                            if (array.Length >= 3)
                                for (; i < array.GetLength(0) - 1; i++)
                                    data += (array[i] + ", ");
                            data += (array[i] + "}");
                        }
                        if (print) Console.WriteLine(data);
                        return data;
                    }   
                }
            public static void Alert (string message, Alerts type = Alerts.Default, bool badge = true)
            {
                switch (type)
                {
                    case Alerts.Error:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        if (badge) Console.Write("ERROR: ");
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case Alerts.Info:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        if (badge) Console.Write("INFO: ");
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case Alerts.Notice:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        if (badge) Console.Write("NOTICE: ");
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case Alerts.Success:
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (badge) Console.Write("SUCCESS: ");
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case Alerts.DefaultWithBR:
                        Console.WriteLine(message);
                        break;
                    //case Alerts.Default://redundant
                    default:
                        Console.Write(message);
                        break;
                }
            }
        }
        private class Input
        {
            public static string String(string message = "Enter a Text String: "
                , Alerts alertType = Alerts.Default
                , bool reports = Reportavailable)
            {
                Output.Alert(message, alertType);
                string value = Console.ReadLine();
                if (!reports) return value;
                switch (value)
                {
                    case "-1":
                        Output.Visuals.Table.Int(_nodesTableInts);
                        break;
                }
                return value;
            }//Handwrite/Hardcore*
            public static string Bot(string message = "", bool printValue = true, bool addNth = true)
            {
                Console.Write(message);
                string value = Get.StringsNthpart(_stringNthRequired);
                if (addNth) _stringNthRequired++;
                if (printValue) Output.Alert("BOT INPUT: " + value, Alerts.Notice);
                Output.BR();
                return value;
            }
            public static int Int32(string message = "Enter an Integer: "
                , int min = int.MinValue
                , int max = int.MaxValue
                , bool error = Error
                , bool checkReportPossible = Reportavailable)
            {
                if (max < min)
                {
                    int _temp = min;
                    min = max;
                    max = _temp;
                }
                int value;
                do
                {
                    Console.Write(message);
                    string inputString = String("",reports: checkReportPossible);
                    if (!string.IsNullOrWhiteSpace(inputString))
                    {
                        bool negativeFlag = inputString.Contains("-");
                        inputString = Regex.Match(inputString, @"\d+").Value;
                        if (!string.IsNullOrEmpty(inputString))
                        {
                            value = int.Parse(inputString);
                            if (negativeFlag) value = -value;
                            if (min > value && error) 
                                Output.Alert("Your input is so small. Minimum is " + min + ".", Alerts.Error);
                            else 
                            if (max < value && error) 
                                Output.Alert("Your input is so big. Maximum is " + max + ".", Alerts.Error);
                            else break;
                        }
                        if (error) Output.Alert("Invalid input", Alerts.Error);
                    }
                    else if (error) Output.Alert("Empty input", Alerts.Error);
                } while (true);
                return value;
            }
            public static char Char(string message = "Enter a Character: "
                , char min = 'A'
                , char max = 'Z'
                , bool error = Error
                , bool checkReportPossible = Reportavailable)
            {
                char value;
                do
                {
                    Console.Write(message);
                    string inputString = String("", reports: checkReportPossible);
                    if (!string.IsNullOrWhiteSpace(inputString))
                    {
                        if (inputString.Length > 1)
                        {
                            if (error) Output.Alert("Only 1 character input", Alerts.Error);
                        }
                        else
                        {
                            value = char.Parse(inputString.ToUpper()); 
                            if (min > value && error) 
                                Output.Alert("Your input is irregular. start is " + min + ".", Alerts.Error);
                            else if (max < value && error) 
                                Output.Alert("Your input is irregular. end is " + max + ".", Alerts.Error);
                            else break;
                        }
                    }
                    else if (error) Output.Alert("Empty input", Alerts.Error);
                } while (true);
                return value;
            }        
        }
        private class Get
        {
            public class DataFromArray
            {
                public static int NodeTableInts(char x, char y)//prog
                {
                    y = char.Parse(y.ToString().ToUpper());
                    x = char.Parse(x.ToString().ToUpper());
                    return _nodesTableInts[x - Lettercode, y - Lettercode];
                }
            }

            public static string StringsNthpart(int nth) //dev Mode
            {
                string value = "";
                int part = 1;
                foreach (char cha in _string)
                {
                    if (cha != ' ')
                        if (part == nth)
                            value += cha;
                    else
                        part++;
                }
                return value.ToUpper();
            }
            
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            public static int ArrayDimensionsCount(Array arrayName) //Extra*
            {
                int dimensionCount;
                for (int i = 0;; i++)
                    try
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        arrayName.GetLength(i);
                    }
                    catch
                    {
                        dimensionCount = i - 1;
                        break;
                    }
                return dimensionCount;
            }
            
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            public static int NumberOfLetterInString(string data, char letter) //Extra
            {
                int c = 0;
                foreach (char ch in data) 
                    if (ch == letter) c++;
                return c;
            }
            
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            public static bool UniqueCharsState(string data) //Extra
            {
                if(data.Length > 256)
                    return false;
                bool[] char_set = new bool[256];
                foreach (int val in data)
                {
                    if(char_set[val])
                        return false;
                    char_set[val] = true;
                }
                return true;
            }

            public static char[] TempNodesConnectedTo(char thisNode)
            {
                string data = "";
                for (int i = 1; i <= _nodesCount; i++)
                {
                    int nodeTableReturn = DataFromArray.NodeTableInts(thisNode, (char)(i + Lettercode));
                    
                    if (nodeTableReturn > 0 && _dijkstraNodes[i -1].Status == 't')
                        data += (char)(i + Lettercode);
                }

                if (data.Length == 0)
                    return null;
                char[] nodesConnectedToSource = new char[data.Length];
                int c = 0;
                foreach (char cha in data)
                {
                    nodesConnectedToSource[c] = cha;
                    c++;
                }
                return nodesConnectedToSource;
            }

            public static char[] CharArrayFromString(string data)
            {
                if (data.Length == 0)
                    return null;
                char[] CharData = new char[data.Length];
                int c = 0;
                foreach (char cha in data)
                {
                    CharData[c] = cha;
                    c++;
                }
                return CharData;
            }

            public static string StringFromCharArray(char[] array)
            {
                string value = "";
                if (array != null)
                    foreach (char cha in array)
                        value += cha;
                return value;
            }
        }
        private class Job
        {
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            private static string ConvertIntToAnyString(int value, int cap) //Extra / For Update*
            {
                string result = "";
                char[] baseChars = new char[cap];
                for (int i = 0; i < cap; i++)
                    baseChars[i] = (char)(Lettercode + i);
                int targetBase   = baseChars.Length;
                do
                {
                    char letter = baseChars[value % targetBase];
                    if (letter == '@') letter = '0';
                    result = letter + result;
                    value  = value / targetBase;
                } 
                while (value > 0);
                return result;
            }

            public static string ExtractNumbers(string data)
            {
                int value = 0;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    bool negativeFlag = data.Contains("-");
                    data = Regex.Match(data, @"\d+").Value;
                    if (string.IsNullOrEmpty(data)) return value.ToString();
                    value = int.Parse(data);
                    if (negativeFlag) value = -value;
                }
                else return null;
                return value.ToString();
            }

            public static bool CheckNodeConnection(char node)
            {
                for(int i = 1; i <= _nodesCount; i++)
                    if (Get.DataFromArray.NodeTableInts(node, (char) (i + Lettercode)) > 0)
                        return true;
                return false;
            }
        }   
    }
}
