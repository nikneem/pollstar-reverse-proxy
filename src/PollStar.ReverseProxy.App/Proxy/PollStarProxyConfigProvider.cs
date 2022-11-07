using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Transforms;

namespace PollStar.ReverseProxy.App.Proxy
{
    public class PollStarProxyConfigProvider : IProxyConfigProvider
    {
        private PollStarMemoryConfig _config;

        public IProxyConfig GetConfig() => _config;

        public PollStarProxyConfigProvider()
        {
            // Load a basic configuration
            // Should be based on your application needs.


            var routeConfigs = new[] { GetUsersRoute() };

            var clusterConfigs = new[]
            {
                new ClusterConfig
                {
                    ClusterId = "users-service",
                    LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        { "destination1", new DestinationConfig { Address = "http://localhost:80" } },
                    }
                }
            };

            _config = new PollStarMemoryConfig(routeConfigs, clusterConfigs);
        }

        private RouteConfig GetUsersRoute()
        {
            var route = new RouteConfig
            {
                RouteId = "users-route",
                ClusterId = "users-service",
                CorsPolicy = Constants.DefaultCorsPolicy,
                Match = new RouteMatch
                {
                    Path = "/users/{**catch-all}"
                },
            };
            return route
                .WithTransformPathPrefix("/api")
                .WithTransformRequestHeader("dapr-app-id", "pollstar-users-api");

        }



    }
}
