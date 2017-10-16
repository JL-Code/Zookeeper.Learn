using System;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace Zookeeper.Core
{
    public class ConnectionedWatcher : Watcher
    {

        private readonly Action _connectioned;
        private readonly Action _disconnected;

        public ConnectionedWatcher(Action connectioned, Action disconnected)
        {
            _connectioned = connectioned;
            _disconnected = disconnected;
        }

        /// <summary>
        /// Process the specified event.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="event">Event.</param>
        public override Task process(WatchedEvent @event)
        {
            if (@event.getState() == Event.KeeperState.SyncConnected)
            {
                _connectioned();
            }
            else
            {
                _disconnected();
            }
            return Task.CompletedTask;
        }
    }
}