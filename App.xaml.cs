using FlamyPOS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FlamyPOS.ViewModels;
using System.Collections.ObjectModel;
using FlamyPOS.Models;
using System.Windows.Media.Animation;

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Database.Initialize();
            Mains.Initialize();
            Mains.LoginVM.Initialize();
            Mains.AdminVM.Initialize();
            Mains.OrderVM.Initialize();
            Mains.TableOverviewVM.Initialize();
            Mains.TableChangeVM.Initialize();
        }
    }
}
