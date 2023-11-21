using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TB1IGK_HFT_2022231.Models;

namespace TB1IGK_HFT_2022231.WpfClient
{
    
    public class MainWindowViewModel : ObservableRecipient
    {
       

        public RestCollection<Competitor> Competitors { get; set; }

        private Competitor selectedCompetitor;

        public Competitor SelectedCompetitor
        {
            get { return selectedCompetitor; }
            set 
            {
                if (value != null)
                {
                    selectedCompetitor = new Competitor()
                    {
                        Name = value.Name,
                        Id = value.Id
                    };

                    OnPropertyChanged();
                    //SetProperty(ref selectedCompetitor, value);
                    (DeleteCompetiorCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }

        }


        public ICommand CreateCompetiorCommand { get; set; }
        public ICommand DeleteCompetiorCommand { get; set; }
        public ICommand UpdateCompetiorCommand { get; set; }

        //public static bool IsInDesignMode
        //{
        //    get
        //    {
        //        var prop = DesignerProperties.IsInDesignMode;
        //        return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata;
        //    }
        //}
        public MainWindowViewModel()
        {
            Competitors = new RestCollection<Competitor>("http://localhost:55475/", "competitor", "hub");

            CreateCompetiorCommand = new RelayCommand(() =>
            {
                Competitors.Add(new Competitor()
                {
                    Name = SelectedCompetitor.Name
                });

            });

            UpdateCompetiorCommand = new RelayCommand(() =>
            {
                Competitors.Update(SelectedCompetitor);
            });


            DeleteCompetiorCommand = new RelayCommand(() =>
            {
                Competitors.Delete(SelectedCompetitor.Id);
            },
            () => 
            {
                return SelectedCompetitor != null;
            });
            SelectedCompetitor = new Competitor();
        }
    }
}
