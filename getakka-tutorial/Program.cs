// See https://aka.ms/new-console-template for more information
using Akka.Actor;
using getakka_tutorial;

Console.WriteLine("Hello, World!");

using (var system = ActorSystem.Create("iot-system"))
{
    // Create the top level supervisor
    var supervisor = system.ActorOf(IotSupervisor.Props(), "iot-supervisor");

    // Exit the system after ENTER is pressed
    Console.ReadLine();
}