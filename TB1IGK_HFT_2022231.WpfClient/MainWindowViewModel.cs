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
        public RestCollection<Category> Categories { get; set; }
        public RestCollection<Competition> Competitions { get; set; }
        public RestCollection<Dictionary<string, string>> CompetitorsByBoatCategory { get; set; }

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
                        Id = value.Id,
                        Age = value.Age,
                        CompetitonID = value.CompetitonID,
                        CategoryID = value.CategoryID,
                        Nation = value.Nation

                    };

                    OnPropertyChanged();
                    //SetProperty(ref selectedCompetitor, value);
                    (DeleteCompetiorCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }

        }
        private Category selectedCategory;

        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (value != null)
                {
                    selectedCategory = new Category()
                    {
                        CategoryNumber = value.CategoryNumber,
                        AgeGroup = value.AgeGroup,
                        BoatCategory = value.BoatCategory
                    };
                    OnPropertyChanged();
                    //SetProperty(ref selectedCompetitor, value);
                    (DeleteCompetiorCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }

        }
        private Competition selectedCompetition;

        public Competition SelectedCompetition
        {
            get { return selectedCompetition; }
            set
            {
                if (value != null)
                {
                    selectedCompetition = new Competition()
                    {
                        ID = value.ID,
                        CompetitorID = value.CompetitorID,
                        OpponentID = value.OpponentID,
                        NumberOfRacesAgainstEachOther = value.NumberOfRacesAgainstEachOther,
                        Location = value.Location,
                        Distance = value.Distance
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

        public ICommand CreateCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand UpdateCategoryCommand { get; set; }

        public ICommand CreateCompetitionCommand { get; set; }
        public ICommand DeleteCompetitionCommand { get; set; }
        public ICommand UpdateCompetitionCommand { get; set; }


        public MainWindowViewModel()
        {
            Competitors = new RestCollection<Competitor>("http://localhost:55475/", "competitor", "hub");
            Categories = new RestCollection<Category>("http://localhost:55475/", "category", "hub");
            Competitions = new RestCollection<Competition>("http://localhost:55475/", "competition", "hub");
           // CompetitorsByBoatCategory = new RestCollection<Dictionary<string, string>>("http://localhost:55475/", "Stat/CompetitorsByBoatCategory", "hub");

            CreateCompetiorCommand = new RelayCommand(() =>
            {
                Competitors.Add(new Competitor()
                {
                    Name = SelectedCompetitor.Name,
                    Age = SelectedCompetitor.Age,
                    CompetitonID = SelectedCompetitor.CompetitonID,
                    CategoryID = SelectedCompetitor.CategoryID,
                    Nation = SelectedCompetitor.Nation
                });
            });

            CreateCategoryCommand = new RelayCommand(() =>
            {
                Categories.Add(new Category()
                {
                    CategoryNumber = SelectedCategory.CategoryNumber,
                    AgeGroup = SelectedCategory.AgeGroup,
                    BoatCategory = SelectedCategory.BoatCategory
                });
            });

            CreateCompetitionCommand = new RelayCommand(() =>
            {
                Competitions.Add(new Competition()
                {
                    ID = SelectedCompetition.ID,
                    CompetitorID = SelectedCompetition.CompetitorID,
                    OpponentID = SelectedCompetition.OpponentID,
                    NumberOfRacesAgainstEachOther = SelectedCompetition.NumberOfRacesAgainstEachOther,
                    Location = SelectedCompetition.Location,
                    Distance = SelectedCompetition.Distance
                });
            });

            UpdateCompetiorCommand = new RelayCommand(() =>
            {
                Competitors.Update(SelectedCompetitor);
            });

            UpdateCategoryCommand = new RelayCommand(() =>
            {
                Categories.Update(SelectedCategory);
            });

            UpdateCompetitionCommand = new RelayCommand(() =>
            {
                Competitions.Update(SelectedCompetition);
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

            DeleteCategoryCommand = new RelayCommand(() =>
            {
                Categories.Delete(SelectedCategory.CategoryNumber);
            },
            () =>
            {
                return SelectedCategory != null;
            });
            SelectedCategory = new Category();

            DeleteCompetitionCommand = new RelayCommand(() =>
            {
                Competitions.Delete(SelectedCompetition.ID);
            },
           () =>
           {
               return SelectedCategory != null;
           });
            SelectedCompetition = new Competition();
        }
    }
}
