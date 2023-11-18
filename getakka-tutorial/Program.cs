using Akka.Actor;
using getakka_tutorial;

Console.WriteLine("Program starting...");

using (var system = ActorSystem.Create("iot-system"))
{
    // Create the top level supervisor
    var supervisor = system.ActorOf(IotSupervisor.Props(), "iot-supervisor");

    // Used to prevent the prrogram from exiting
    // Exit the system after ENTER is pressed
    Console.ReadLine();
}