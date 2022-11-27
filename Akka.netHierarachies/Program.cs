// See https://aka.ms/new-console-template for more information
using Akka.Actor;
using Akka.netHierarachies;

Console.WriteLine("Hello, World!");
ActorSystem system = ActorSystem.Create("my-first-akka");

IActorRef dispatcher =

system.ActorOf<MusicPlayerCoordinatorActor>("player-coordinator");

dispatcher.Tell(new PlaySongMessage("Smoke on the water", "John"));

dispatcher.Tell(new PlaySongMessage("Another brick in the wall", "Mike"));

dispatcher.Tell(new StopPlayingMessage("John"));

dispatcher.Tell(new StopPlayingMessage("Mike"));

dispatcher.Tell(new StopPlayingMessage("Mike"));

Console.Read();

system.Terminate();