using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Surveys
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Guid AppId = Guid.NewGuid();

        public static readonly string ClientIdentifier;

        static App()
        {
            //ClientIdentifier = new Services.ClientIdService().GetMacAdress();
            //ClientIdentifier = AppId.ToString();
            ClientIdentifier = new Services.ClientIdService().GetIp();
        }
    }
}
