using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaPingBotNext.Jobs
{
    interface IJob
    {
        Task Do();
    }
}
