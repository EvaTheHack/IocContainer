using System;

namespace IocContainer.Tests.Services
{
    public class GuidProvider : IGuidProvider
    {
        public readonly ILogger _logger;
        private readonly Guid _guid;

        public GuidProvider(ILogger logger)
        {
            _logger = logger;
            _guid = Guid.NewGuid();
        }
        
        public Guid GetGuid()
        {
            _logger.Show(_guid.ToString());
            return _guid;
        }
    }
}
