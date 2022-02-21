using System;
using System.Collections.Generic;

namespace Entities.Elevator
{
    class ElevadorFactory
    {
        public byte TotalFloor { get; private set; }
        public byte Capacity { get; private set; }
        public byte Passagers { get; private set; }
        public string Status { get; private set; }
        public byte ActualFloor { get; private set; }
        public bool Door { get; private set; }

        public bool Up { get; private set; }
        public List<byte> Route { get; private set; }

        public ElevadorFactory()
        {
            TotalFloor = 9;
            Capacity = 3;
            ActualFloor = 0;
            Door = false;
            Route = new List<byte>();
            Status = "PARADO";
            Up = true;
        }
        public ElevadorFactory(byte totalFloor, byte capacity)
        {
            TotalFloor = totalFloor;
            Capacity = capacity;
            ActualFloor = 0;
            Door = false;
            Route = new List<byte>();
            Status = "PARADO";
            Up = true;
        }

        private void ToBoard(byte quantity)
        {
            if ((!Door) && Status == "PARADO")
            {
                Console.WriteLine($"-->      {quantity} pessoas entraram no elevador");
                Passagers += quantity;
            }
            else
            {
                Console.WriteLine("O elevador esta em movimento ou com a porta fechada");
            }

        }

        private void Land(byte quantity)
        {
            Console.WriteLine($"-->      {quantity} pessoas sairam do elevador");
            Passagers -= quantity;
        }

        public void Move()
        {
            if (Status == "SUBINDO")
            {
                for (byte controll = ActualFloor; controll < Route[0]; controll++)
                {
                    Console.WriteLine($"______{Status}______");
                }
            }
            else
            {
                for (byte controll = ActualFloor; controll > Route[0]; controll--)
                {
                    Console.WriteLine($"______{Status}______");
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            ActualFloor = Route[0];
            Console.WriteLine("--------------- VOCÊ CHEGOU AO ANDAR " + ActualFloor + "---------------");
            OpenDoor("PARADO");
            Land(CountPassagersActions());
            List<byte> aux = new List<byte>();
            foreach (byte route in Route)
            {
                aux.Add(route);
            }
            foreach (byte route in aux)
            {
                if (route == ActualFloor)
                {
                    Route.Remove(route);
                }
            }
        }
        private void CreateRoute(List<byte> route, byte quantity)
        {
            bool _controll = false;
            route.RemoveAll(x => x == ActualFloor);
            foreach (byte floor in route)
            {
                Route.Add(floor);
            }
            List<byte> aux = new List<byte>();
            foreach (byte floor in Route)
            {
                aux.Add(floor);
            }
            Route.Sort();
            aux.Sort();
            foreach (byte c in Route)
            {
                if (c > ActualFloor && Up)
                {
                    _controll = true;
                    Up = true;
                }
            }
            foreach (byte floor in aux)
            {
                if (floor < ActualFloor)
                {
                    Route.RemoveAt(0);
                    Route.Add(floor);
                }
            }
            if (Route[0] < ActualFloor)
            {
                Up = false;
            }
            if (_controll && Up)
            {
                ToBoard(quantity);
                CloseDoor("SUBINDO");
            }
            else
            {
                Route.Sort();
                aux.Sort();
                Route.Reverse();
                aux.Reverse();
                foreach (byte floor in aux)
                {
                    if (floor > ActualFloor)
                    {
                        Route.RemoveAt(0);
                        Route.Add(floor);
                    }
                }
                if (Route[0] > ActualFloor)
                {
                    Up = true;
                    Route.Sort();
                }
                ToBoard(quantity);
                CloseDoor("DESCENDO");
            }
        }
        private void OpenDoor(string status)
        {
            Door = false;
            Status = status;
            Console.WriteLine("-->      A porta abriu!");
        }

        private void CloseDoor(string status)
        {
            Door = true;
            Status = status;
            Console.WriteLine("-->      A porta se fechou!");
        }

        private byte CountPassagersActions()
        {
            byte quantity = 0;
            foreach (byte index in Route)
            {
                if (index == ActualFloor)
                {
                    quantity++;
                }
            }
            return quantity;
        }

        public byte QuestionToBoard()
        {
            Console.Write("->Quantas Pessoas irão embarcar? ");
            byte quantity = byte.Parse(Console.ReadLine());
            if (quantity == 0 && Route.Count == 0)
            {
                throw new ArgumentNullException("The elevator is empty, at least one person is needed to move");
            }
            return quantity;
        }
        public byte ValidationToBoard(byte quantity)
        {
            if (Passagers + quantity > Capacity)
            {
                throw new ArgumentException("This value exceeds the maximum capacity of the elevator!");
            }

            return quantity;
        }

        public List<byte> SelectFloors(byte quantity, out byte auxQuantity)
        {
            List<byte> _floors = new();
            Console.WriteLine("---------------------SELECIONE O ANDAR------------------");

            for (byte floor = 0; floor < TotalFloor; floor += 3)
            {
                Console.Write($"   [{floor}]");
                if (floor + 1 < TotalFloor)
                {
                    Console.Write($"    [{floor + 1}]");
                }
                if (floor + 2 < TotalFloor)
                {
                    Console.WriteLine($"    [{floor + 2}]");
                }
            }
            Console.WriteLine();

            Console.WriteLine("--------------------------------------------------------");
            auxQuantity = quantity;

            for (byte passager = 0; passager < quantity; passager++)
            {
                Console.Write($"Passageiro{passager + 1}: ");
                byte _floor = byte.Parse(Console.ReadLine());
                if (_floor != ActualFloor)
                {
                    _floors.Add(ValidationFloor(_floor, quantity));
                }
                else
                {
                    auxQuantity--;
                }
            }
            Console.WriteLine();
            return _floors;
        }

        public byte ValidationFloor(byte floor, byte quantity)
        {
            if (floor > TotalFloor - 1)
            {
                throw new ArgumentException("This floor does not exist!");
            }
            return floor;
        }

        public void UpdateRoute(List<byte> aux, byte quantity)
        {
            if (aux.Count != 0)
            {
                CreateRoute(aux, quantity);
            }
            else
            {
                aux.Add(ActualFloor);
                CreateRoute(aux, quantity);
              }
            }
            public void Info()
            {
                Console.WriteLine("-------------------------INFO----------------------");
                Console.WriteLine("Total de Andares: " + TotalFloor);
                Console.WriteLine("Capacidade Total: " + Capacity);
                Console.WriteLine("Passageiros: " + Passagers);
                Console.WriteLine("Porta: " + (Door ? "FECHADA" : "ABERTA"));
                Console.WriteLine("Status: " + Status);
                Console.WriteLine("Andar Atual: " + ActualFloor);
                Console.WriteLine("---------------------------------------------------");

            }
        }
}

