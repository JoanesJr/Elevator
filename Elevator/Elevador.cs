using System;
using Entities.Elevator;
using System.Collections.Generic;

namespace Program
{
    class Elevador
    {
        public static void Main(string[] args)
        {
            try
            {
                ElevadorFactory elevador = new ElevadorFactory();
                do
                {
                    elevador.Info();
                    byte auxQuantity;
                    byte quantity = elevador.ValidationToBoard(elevador.QuestionToBoard());
                    List<byte> aux = elevador.SelectFloors(quantity, out auxQuantity);
                    elevador.UpdateRoute(aux, auxQuantity);
                    elevador.Info();
                    elevador.Move();
                }
                while (true);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Empty Elevator: " + e.Message);
            }
            catch (ArgumentException e)
            {
                Console.Write("Invalid Value: " + e.Message);
            }

            catch (FormatException)
            {
                Console.WriteLine("Error: Only numbers are allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("General Error: " + e.Message);
            }

        }
    }
}

