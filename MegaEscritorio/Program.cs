using Newtonsoft.Json.Linq;
using System;
using System.IO;


namespace MegaEscritorio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("Pricing Structure");
            Console.WriteLine("");
            Console.WriteLine("===============================================================================");
            Console.WriteLine("");
            Console.WriteLine(String.Format("{0,35} | {1,-5} | {2,-10}", "Base Desk Price", "$200", "each"));
            Console.WriteLine(String.Format("{0,35} | {1,-5} | {2,-10}", "Desktop Surface Area > 1000 in\xB2", "$5", "per in\xB2"));
            Console.WriteLine(String.Format("{0,35} | {1,-5} | {2,-10}", "Drawers", "$50", "per drawer"));
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Surface Material");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine(String.Format("{0,35} | {1,-5}", "Oak", "$200"));
            Console.WriteLine(String.Format("{0,35} | {1,-5}", "Laminate", "$100"));
            Console.WriteLine(string.Format("{0,35} | {1,-5}", "Pine", "$20"));
            Console.WriteLine("===============================================================================");
            Console.WriteLine("");
            Console.WriteLine(String.Format("{0,-10} | {1,-6}   {2,-15}   {3,-6}", "Rush Order", "", "Size of Desk (in\xB2)", ""));
            Console.WriteLine(String.Format("{0,-10} | {1,-6} | {2,-15} | {3,-6}", "", "< 1000", "1000 to 1999", "2000+"));
            Console.WriteLine(String.Format("{0,-10} | {1,-6} | {2,-15} | {3,-6}", "3 Day", "$ ", "$ ", "$ "));
            Console.WriteLine(String.Format("{0,-10} | {1,-6} | {2,-15} | {3,-6}", "5 Day", "$ ", "$ ", "$ "));
            Console.WriteLine(String.Format("{0,-10} | {1,-6} | {2,-15} | {3,-6}", "7 Day", "$ ", "$ ", "$ "));
            Console.WriteLine("");
            Console.WriteLine("");

            ///Prompt User
            Console.WriteLine("Enter desk length in inches: ");
            int length = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Enter desk width in inches: ");
            int width = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Enter the number of drawers: ");
            int drawerNum = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Enter material:");
            string material = Console.ReadLine();

            Console.WriteLine("Enter Rush Order in days (3,5,7)");
            int rushOrderPrompt = Int32.Parse(Console.ReadLine());

            ///Calculate Surface Area
            int surfaceArea = getDeskSurfaceArea(length, width);

            ///get Rush Order Cost
            int surfaceAreaSelection = getSurfaceRange(surfaceArea);
            int rushOrderSelection = getRushOrder(rushOrderPrompt);
            int rushOrderCost = getRushOrderCost(surfaceAreaSelection, rushOrderSelection);

            ///Get Drawer Cost
            int drawerCost = getDrawerCost(drawerNum);

            ///Get Surface Material Cost
            int surfaceCost = getSurfaceMaterialCost(material);

            ///Get Desk Base Price
            int deskBaseCost = getSufaceAreaCost(surfaceArea);

            ///Calculate Final Cost
            int finalCost = getFinalCost(rushOrderCost, drawerCost, surfaceCost, deskBaseCost);

            Console.WriteLine(String.Format("Here is your order confirmation:"));
            Console.WriteLine(String.Format("Base price of desk = " + deskBaseCost));
            Console.WriteLine(String.Format("Cost of drawers = " + drawerCost));
            Console.WriteLine(String.Format("Suface material cost = " + surfaceCost));
            Console.WriteLine(String.Format("rush order cost = " + rushOrderCost));
            Console.WriteLine(String.Format("Final Price = " + finalCost));

            ///Write Json File
            writeJsonFile(rushOrderCost, drawerCost, surfaceCost, deskBaseCost, finalCost);
            

            Console.WriteLine(String.Format("Press any key to exit."));
            Console.ReadKey();
            
        }

        ///Get the Rush Order
        public static int getSurfaceRange(int surfaceArea)
        {
            int sa;
            if (surfaceArea > 0 && surfaceArea < 1000)
            {
                sa = 0;
            }
            else if (surfaceArea >= 1000 && surfaceArea < 2000)
            {
                sa = 1;
            }
            else
            {
                sa = 2;
            }
            return sa;
        }
        public static int getRushOrder(int rushOrderPrompt)
        {
            int rushOrderDay;

            if (rushOrderPrompt == 3)
            {
                 rushOrderDay = 0;
            }
            else if (rushOrderPrompt == 5)
            {
                 rushOrderDay = 1;
            }
            else
            {
                 rushOrderDay = 2;
            }
            return rushOrderDay;

        }
        public static int getRushOrderCost(int surfaceAreaSelection, int rushOrderSelection)
        {
            string[] rushOrderLine = File.ReadAllLines(@"\\HafenCloud\Ryan\School\CIT 301C\Wk03\rushOrder.txt");
            int line = 0;
            int[,] RushOrderCostArray = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    RushOrderCostArray[i, j] = int.Parse(rushOrderLine[line]);
                    line++;
                }
            }
            return RushOrderCostArray[surfaceAreaSelection, rushOrderSelection];
        }

        ///Calculate the surface area
        public static int getDeskSurfaceArea(int length, int width)
        {
            int surfaceArea = length * width;
            return surfaceArea;
        }

        ///Calculate the drawer Cost
        public static int getDrawerCost(int drawerNum)
        {
            int drawerCost = drawerNum * 50;
            return drawerCost;
        }

        ///Calculate to const of 
        public static int getSurfaceMaterialCost(string material)
        {
            int materialCost;
            if (material == "Oak")
            {
                 materialCost = 200;
            }
            else if (material == "Laminate")
            {
                 materialCost = 100;
            }
            else
            {
                 materialCost = 50;
            }
            return materialCost;

        }
        /// calculate Surface Area Cost
        public static int getSufaceAreaCost(int surfaceArea)
        {
            int surfaceAreaCost;
            if (surfaceArea <= 1000)
            {
                surfaceAreaCost = 200;
            }
            else 
            {
                surfaceAreaCost = ((surfaceArea-1000)*5)+200;
            }
            return surfaceAreaCost;
        }
        /// calculate Final Cost
        public static int getFinalCost(int rushOrderCost, int drawerCost, int surfaceCost, int deskBaseCost)
        {
            int finalCost = rushOrderCost + drawerCost + surfaceCost + deskBaseCost;
            return finalCost;
        }
        static void writeJsonFile(int rushOrderCost, int drawerCost, int surfaceCost, int deskBaseCost, int finalCost)
        {
            string path = @"\\HafenCloud\Ryan\School\CIT 301C\Wk03\orderJson.txt";

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                JArray arrayJson = new JArray();
                arrayJson.Add("deskBaseCost: " + deskBaseCost);
                arrayJson.Add("drawerCost: " + drawerCost);
                arrayJson.Add("SufaceCost: " + surfaceCost);
                arrayJson.Add("rushOrderCost: " + rushOrderCost);
                arrayJson.Add("FinalCost: " + finalCost);

                JObject o = new JObject();
                o["orderArray"] = arrayJson;

                string json = o.ToString();

                File.WriteAllText(path, json);
            }

            // Open the file to read from.
            string readText = File.ReadAllText(path);
            Console.WriteLine(readText);
        }

    }
}