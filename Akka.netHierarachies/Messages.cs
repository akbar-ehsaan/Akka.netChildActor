using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.netHierarachies
{
    public class PlaySongMessage

    {

        public PlaySongMessage(string song, string user)

        {

            Song = song;

            User = user;

        }

        public string Song { get; }

        public string User { get; }

    }

    public class StopPlayingMessage

    {

        public StopPlayingMessage(string user)

        {

            User = user;

        }

        public string User { get; }

    }

    public class MusicPlayerCoordinatorActor : ReceiveActor
    {
        protected Dictionary<string, IActorRef> MusicPlayerActors;


        public MusicPlayerCoordinatorActor()
        {
            MusicPlayerActors = new Dictionary<string, IActorRef>();
            Receive<PlaySongMessage>(message => PlaySong(message));

            Receive<StopPlayingMessage>(message => StopPlaying(message));

        }
        private void StopPlaying(StopPlayingMessage message)

        {

            var musicPlayerActor = GetMusicPlayerActor(message.User);

            if (musicPlayerActor != null)

            {

                musicPlayerActor.Tell(message);

            }

        }

        private void PlaySong(PlaySongMessage message)

        {

            var musicPlayerActor = EnsureMusicPlayerActorExists(message.User);

            musicPlayerActor.Tell(message);

        }
        private IActorRef EnsureMusicPlayerActorExists(string user)

        {

            IActorRef musicPlayerActorReference = GetMusicPlayerActor(user);

            MusicPlayerActors.TryGetValue(user, out musicPlayerActorReference);

            if (musicPlayerActorReference == null)

            {

                //create a new actor's instance.

                musicPlayerActorReference = Context.ActorOf<MusicPlayerActor>(user);

                //add the newly created actor in the dictionary.

                MusicPlayerActors.Add(user, musicPlayerActorReference);

            }

            return musicPlayerActorReference;

        }
        private IActorRef GetMusicPlayerActor(string user)

        {

            IActorRef musicPlayerActorReference;

            MusicPlayerActors.TryGetValue(user, out musicPlayerActorReference);

            return musicPlayerActorReference;

        }
    }
    public class MusicPlayerActor : ReceiveActor

    {

        protected PlaySongMessage CurrentSong;

        public MusicPlayerActor()

        {

            StoppedBehavior();

        }

        private void StoppedBehavior()

        {

            Receive<PlaySongMessage>(m => PlaySong(m));

            Receive<StopPlayingMessage>(m => Console.WriteLine($"{m.User}'s player: Cannot stop, the actor is already stopped"));

        }

        private void PlayingBehavior()

        {

            Receive<PlaySongMessage>(m => Console.WriteLine($"{CurrentSong.User}'s player: Cannot play. Currently playing '{CurrentSong.Song}'"));

            Receive<StopPlayingMessage>(m => StopPlaying());

        }

        private void PlaySong(PlaySongMessage message)

        {

            CurrentSong = message;



            Console.WriteLine(

                     $"{CurrentSong.User} is currently listening to '{CurrentSong.Song}'");

            Become(PlayingBehavior);

        }

        private void StopPlaying()

        {

            Console.WriteLine($"{CurrentSong.User}'s player is currently stopped.");

            CurrentSong = null;

            Become(StoppedBehavior);

        }

    }
}
