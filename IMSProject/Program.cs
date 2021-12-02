using System;
using System.Linq;

namespace IMSProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int TIME_INTERVAL = 15; // minutes
            String[] WEEKDAYS = {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };
            int week_index = 0;
            int AREA;

            Console.Write("How many airplanes are there: ");
            int number_of_airplanes = Convert.ToInt32(Console.ReadLine());
            String[] airplanes = new String[number_of_airplanes];

            for (int i = 0; i < number_of_airplanes; i++)
            {
                Console.Write("Enter the name of plane " + i + " : ");
                airplanes[i] = Console.ReadLine();
            }

            Console.WriteLine("-----------------------------");

            Console.Write("How many cities are there: ");
            int number_of_cities = Convert.ToInt32(Console.ReadLine());
            String[] cities = new String[number_of_cities];

            for (int i = 0; i < number_of_cities; i++)
            {
                Console.Write("Enter name of city " + i + " : ");
                cities[i] = Console.ReadLine();
            }

            Console.WriteLine("-----------------------------");

            Console.Write("When the first plane starts its flight: ");
            String startTimeForFirstPlane = Console.ReadLine();
            Console.WriteLine("-----------------------------");

            // get data for reaching hours for each city

            // get data for week days
            String[,] weekDays = new String[number_of_airplanes,number_of_cities];

            String[,] arrivingHours = new String[number_of_airplanes,number_of_cities];
            for (int i = 0; i < number_of_airplanes; i++)
            {
                for (int j = 1; j < number_of_cities; j++)
                {
                    Console.Write("Enter arriving hour from " + cities[0] + " to " + cities[j] +
                            " for " + airplanes[i] + ": ");
                    int minutes = Convert.ToInt32(Console.ReadLine()) * 60; // converting hours to minutes automatically

                    if (i % 2 == 0)
                    {
                        if (i == 0)
                        {
                            arrivingHours[i,0] = Time.add(startTimeForFirstPlane, 0);
                        }
                        else
                        {
                            arrivingHours[i,0] = Time.add(arrivingHours[i - 2,0], TIME_INTERVAL);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            arrivingHours[i,0] = Time.add(startTimeForFirstPlane, 0);
                        }
                        else
                        {
                            arrivingHours[i,0] = Time.add(arrivingHours[i - 2,0], TIME_INTERVAL);
                        }
                    }
                    arrivingHours[i,j] = Time.add(arrivingHours[i,j - 1],
                            2 * minutes + GenerateRandom.random()); // random generates between 30 - 90 minutes
                    weekDays[i,0] = WEEKDAYS[0];// Monday 00:00 -> Tuesday 00:00 -> Wednesday

                    if ((minutes / 60) < 12)
                    {
                        if (Time.compare(arrivingHours[i,j - 1], arrivingHours[i,j]))
                        {

                            week_index++;

                            if (week_index >= 7)
                            {
                                week_index = 0;
                                Console.WriteLine(Colors.TEXT_RED +
                                        "It is not possible to make all flights in the same week!"
                                        + Colors.TEXT_RESET);
                            }
                        }
                    }
                    else
                    {
                        week_index += 2;
                        if (week_index >= 7)
                        {
                            week_index = 0;
                            Console.WriteLine(Colors.TEXT_RED +
                                    "It is not possible to make all flights in the same week!"
                                    + Colors.TEXT_RESET);

                        }
                    }


                    weekDays[i,j] = WEEKDAYS[week_index];
                }

                week_index = 0;

            }
            String[,] time_table = new String[number_of_airplanes + 1,number_of_cities + 1];

            for (int row = 0; row < number_of_airplanes + 1; row++)
            {
                for (int column = 0; column < number_of_cities + 1; column++)
                {
                    if (row == 0 && column == 0)
                    {
                        time_table[0,0] = " -- ";
                    }
                    else if (column != 0 && row == 0)
                    {
                        if (column == 1)
                        {
                            time_table[0,column] = cities[column - 1].ToUpper();
                        }
                        else
                        {
                            time_table[0,column] = cities[column - 1].ToUpper() + " to " + cities[0].ToUpper();
                        }
                    }
                    else if (column == 0)
                    {
                        time_table[row,0] = airplanes[row - 1].ToUpper();
                    }
                    else
                    {

                        if ((row + column) % 2 == 0)
                        {
                            AREA = 1;
                        }
                        else
                        {
                            AREA = 2;
                        }

                        if (column == 1)
                        {
                            time_table[row,column] = "Area: "
                                    + AREA
                                    + ", Flight time from Baku: "
                                    + arrivingHours[row - 1,column - 1]
                                    + ", Week Day: "
                                    + weekDays[row - 1,column - 1];
                        }
                        else
                        {
                            time_table[row,column] = "Area: "
                                    + AREA
                                    + ", Arriving time to Baku: "
                                    + arrivingHours[row - 1,column - 1]
                                    + ", Week Day: "
                                    + weekDays[row - 1,column - 1];
                        }

                    }
                }
            }


            String[] headers = new String[time_table.GetLength(0)];

            for (int i = 0; i < time_table.GetLength(0); i++)
            {
                headers[i] = time_table[0, i];
            }

            String[,] data = new String[time_table.GetLength(0)-1, time_table.GetLength(0)];
            for (int i = 1,x=0; i < time_table.GetLength(0); i++)
            {
                for (int j = 0; j < time_table.GetLength(1); j++)
                {
                    data[x, j] = time_table[i, j];
                }
                x++;
            }

            Console.WriteLine(FlipTable.of(headers, data));
        }
    }
}
