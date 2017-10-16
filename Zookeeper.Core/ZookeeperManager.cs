using System;
using System.Threading;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace Zookeeper.Core
{
    public class ZookeeperManager
    {
        private string _host;
        private int _timeout;
        protected ZooKeeper _zk;
        private ManualResetEvent _connectWait = new ManualResetEvent(false);

        public ZookeeperManager(string host, int timeout = 5000)
        {
            _host = host;
            _timeout = timeout;
            ConnectZookeeper().Wait();
        }

        /// <summary>
        /// Connects the zookeeper.
        /// </summary>
        /// <returns>The zookeeper.</returns>
        private async Task ConnectZookeeper()
        {
            if (_zk != null)
            {
                await _zk.closeAsync();
            }
            _zk = new ZooKeeper(_host, _timeout, new ConnectionedWatcher(() =>
            {
                _connectWait.Set();
            }, async () =>
            {
                _connectWait.Reset();
                await ConnectZookeeper();
            }));
        }
    }
}