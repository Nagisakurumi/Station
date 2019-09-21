using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseController.Interface
{
    public interface IGuidService { Guid NewGuid(); }

    class GuidService : IGuidService
    {
        public Guid NewGuid() { return Guid.NewGuid(); }
    }
}
