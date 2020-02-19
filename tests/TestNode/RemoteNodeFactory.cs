using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NeoFx.P2P.Messages;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NeoFx.TestNode
{
    interface IRemoteNodeFactory
    {
        ValueTask<(IRemoteNode, VersionPayload)> ConnectAsync(IPEndPoint endPoint, uint nonce, uint startHeight, ChannelWriter<Message> writer, CancellationToken token = default);
    }

    class RemoteNodeFactory : IRemoteNodeFactory
    {
        private readonly IServiceProvider provider;

        public RemoteNodeFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask<(IRemoteNode, VersionPayload)> ConnectAsync(IPEndPoint endPoint, uint nonce, uint startHeight, ChannelWriter<Message> writer, CancellationToken token = default)
        {
            var pipelineSocket = provider.GetRequiredService<IPipelineSocket>();
            var networkOptions = provider.GetRequiredService<IOptions<NetworkOptions>>();
            var nodeOptions = provider.GetRequiredService<IOptions<NodeOptions>>();
            var logger = provider.GetService<ILogger<RemoteNode>>();

            var node = new RemoteNode(pipelineSocket, networkOptions, nodeOptions, logger);
            var remoteVersion = await node.ConnectAsync(endPoint, nonce, startHeight, writer, token);
            return (node, remoteVersion);
        }
    }
}
