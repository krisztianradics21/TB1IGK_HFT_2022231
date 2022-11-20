using ConsoleTools;
using System;
using System.Linq;
using TB1IGK_HFT_2022231.Models;

namespace TB1IGK_HFT_2022231.Client
{
    class Program
    {
        private static RestService service = new RestService("http://localhost:55475/", "competition");
        static void Main(string[] args)
        {
            var competitorMenu = new ConsoleMenu()
               .Add("-LIST ALL COMPETITORS", () => ListCompetitors())
               .Add("-SELECT ONE COMPETITOR", () => OneCompetitor())
               .Add("-UPDATE COMPETITOR", () => UpdateCompetitor())
               .Add("-DELETE COMPETITOR", () => DeleteCompetitor())
               .Add("-CREATE COMPETITOR", () => CreateCompetitor())
               .Add("-BACK TO THE MAIN MENU", ConsoleMenu.Close);

            var categoryMenu = new ConsoleMenu()
                .Add("-LIST ALL CATEGORIES", () => ListCategories())
                .Add("-SELECT ONE CATEGORY", () => OneCategory())
                .Add("-UPDATE CATEGORY", () => UpdateCategory())
                .Add("-DELETE CATEGORY", () => DeleteCategory())
                .Add("-CREATE CATEGORY", () => CreateCategory())
                .Add("-BACK TO THE MAIN MENU", ConsoleMenu.Close);

            var competitionMenu = new ConsoleMenu()
                .Add("-LIST ALL COMPETITIONS", () => ListCompetitions())
                .Add("-SELECT ONE COMPETITION", () => OneCompetition())
                .Add("-UPDATE COMPETITION", () => UpdateCompetition())
                .Add("-DELETE COMPETITION", () => DeleteCompetition())
                .Add("-CREATE COMPETITION", () => CreateCompetition())
                .Add("-BACK TO THE MAIN MENU", ConsoleMenu.Close);

            var nonCRUDS = new ConsoleMenu()
                .Add("-COMPETITORS NAME BY BOAT CATEGORY", () => CompetitorsByBoatCategory())
                .Add("-COMPETITOR WITH ALL RELEVANT DATA", () => CompetitorWithAllRelevantData())
                .Add("-COMPETITORS AVERAGE AGE", () => AVGAge())
                .Add("-COMPETITION WITH COMPETITORS NAME AND NATION", () => Competition_BasedOnCompetitorsNameAndNation())
                .Add("-COMPETITIONS BY DISTANCE", () => CompetitionBasedOnDistance())
                .Add("-OPPONENTS BY NAME", () => OpponentsByName())
                .Add("-BACK TO THE MAIN MENU", ConsoleMenu.Close);

            var mainMenu = new ConsoleMenu()
                .Add("COMPETITOR MENU", () => competitorMenu.Show())
                .Add("CATEGORY MENU", () => categoryMenu.Show())
                .Add("COMPETITION MENU", () => competitionMenu.Show())
                .Add("NON CRUDS", () => nonCRUDS.Show())
                .Add("EXIT", ConsoleMenu.Close);

            mainMenu.Show();
        }
        private static void OpponentsByName()
        {
            Console.Clear();
            try
            {
                var q = service.Get<object>("stat/OpponentsByName");
               
                if (q == null || q.Count == 0)
                    Console.WriteLine("NO DATA");
                else
                {
                    foreach (var item in q)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void CompetitionBasedOnDistance()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write a CompetitionID!");
                int input = int.Parse(Console.ReadLine());
                //inp, "competitor"
                var q = service.Get<object>(input, "competition");
                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    Console.WriteLine(q.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void Competition_BasedOnCompetitorsNameAndNation()
        {
            Console.Clear();
            try
            {
                var q = service.Get<object>("stat/Competition_BasedOnCompetitorsNameAndNation");

                if (q == null || q.Count == 0)
                    Console.WriteLine("NO DATA");
                else
                {
                    foreach (var item in q)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void AVGAge()
        {
            Console.Clear();
            try
            {
                double q = service.GetSingle<double>("stat/AVGAge");

                if (q == null)
                    Console.WriteLine("NO DATA");
                else
                {
                    Console.WriteLine("THE AVG COMPETITOR AGE: " + q);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void CompetitorWithAllRelevantData()
        {
            Console.Clear();
            try
            {
                var q = service.Get<object>("stat/CompetitorWithAllRelevantData");

                if (q == null || q.Count == 0)
                    Console.WriteLine("NO DATA");
                else
                {
                    foreach (var item in q)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void CompetitorsByBoatCategory()
        {
            Console.Clear();
            try
            {
                var q = service.Get<object>("stat/CompetitorsByBoatCategory");

                if (q == null || q.Count == 0)
                    Console.WriteLine("NO DATA");
                else
                {
                    foreach (var item in q)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void CreateCompetitor()
        {
            Console.Clear();
            try
            {
                Competitor newCompetitor = new Competitor();

                string inpS;
                int inpI;

                Console.WriteLine("You need to make all property, else it will be the default value");

                ConsoleMenu create = new ConsoleMenu()
                    .Add("-competitionid", () =>
                    {
                        Console.WriteLine("write the CompetitionID");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetitor.CompetitonID = inpI;
                    })
                    .Add("-name", () =>
                    {
                        Console.WriteLine("write the name");
                        inpS = Console.ReadLine();
                        newCompetitor.Name = inpS;
                    })
                    .Add("-nation", () =>
                    {
                        Console.WriteLine("write the nation");
                        inpS = Console.ReadLine();
                        newCompetitor.Nation = inpS;
                    })
                    .Add("-categoryid", () =>
                    {
                        Console.WriteLine("write the categoryID");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetitor.CategoryID = inpI;
                    })
                    .Add("-age", () =>
                    {
                        Console.WriteLine("write the competitor's age");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetitor.Age = inpI;
                    })
                    .Add("-BACK", ConsoleMenu.Close);


                create.Show();

                service.Post<Competitor>(newCompetitor, "competitor");

            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void CreateCategory()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("You need to make all property, else it will be the default value");

                Category newCategory = new Category();

                string inpS;
            
                ConsoleMenu create = new ConsoleMenu()
                    .Add("-agegroup", () =>
                    {
                        Console.WriteLine("write the age group");
                        inpS = Console.ReadLine();
                        newCategory.AgeGroup = inpS;
                    })
                    .Add("-boatcategory", () =>
                    {
                        Console.WriteLine("write the boatcategory");
                        inpS = Console.ReadLine();
                        newCategory.BoatCategory = inpS;
                    })
                    .Add("-BACK", ConsoleMenu.Close);

                create.Show();

                service.Post<Category>(newCategory, "category");
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void CreateCompetition()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("You need to make all property, else it will be the default value");

                Competition newCompetition = new Competition();

                string inpS;
                int inpI;

                ConsoleMenu update = new ConsoleMenu()
                    .Add("-competitorID", () =>
                    {
                        Console.WriteLine("write the competitorID");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetition.CompetitorID = inpI;
                    })
                    .Add("-opponentID", () =>
                    {
                        Console.WriteLine("write the opponentID");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetition.OpponentID = inpI;
                    })
                    .Add("-numberOfRacesAgainstEachOther", () =>
                    {
                        Console.WriteLine("write the number of races against each other");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetition.NumberOfRacesAgainstEachOther = inpI;
                    })
                    .Add("-distance", () =>
                    {
                        Console.WriteLine("write the Distance");
                        inpI = int.Parse(Console.ReadLine());
                        newCompetition.Distance = inpI;
                    })
                    .Add("-location", () =>
                    {
                        Console.WriteLine("write the Location");
                        inpS = Console.ReadLine();
                        newCompetition.Location = inpS;
                    })
                    .Add("-BACK", ConsoleMenu.Close);


                update.Show();

                service.Post<Competition>(newCompetition, "competition");
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void DeleteCompetitor()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Competitor>(inp, "competitor");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    service.Delete(inp, "competitor");
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void DeleteCompetition()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Competition>(inp, "competition");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    service.Delete(inp, "competition");
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void DeleteCategory()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write a CategoryNumber");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Category>(inp, "category");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    service.Delete(inp, "category");
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void UpdateCompetitor()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Competitor>(inp, "competitor");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                {
                    string inpS;
                    int inpI;

                    ConsoleMenu update = new ConsoleMenu()
                        .Add("-competitionid", () =>
                        {
                            Console.WriteLine("write the CompetitionID");
                            inpI = int.Parse(Console.ReadLine());
                            q.CompetitonID = inpI;
                        })
                        .Add("-name", () =>
                        {
                            Console.WriteLine("write the Name");
                            inpS = Console.ReadLine();
                            q.Name = inpS;
                        })
                        .Add("-nation", () =>
                        {
                            Console.WriteLine("write the Nation");
                            inpS = Console.ReadLine();
                            q.Nation = inpS;
                        })
                        .Add("-categoryid", () =>
                        {
                            Console.WriteLine("write the CategoryID");
                            inpI = int.Parse(Console.ReadLine());
                            q.CategoryID = inpI;
                        })
                        .Add("-age", () =>
                        {
                            Console.WriteLine("write the competitor's age");
                            inpI = int.Parse(Console.ReadLine());
                            q.Age = inpI;
                        })
                        .Add("-BACK", ConsoleMenu.Close);


                    update.Show();

                    service.Put<Competitor>(q, "competitor");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void UpdateCategory()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write a CategoryNumber");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Category>(inp, "category");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                {

                    string inpS;
     
                    ConsoleMenu update = new ConsoleMenu()
                        .Add("-agegroup", () =>
                        {
                            Console.WriteLine("write the Age group");
                            inpS = Console.ReadLine();
                            q.AgeGroup = inpS;
                        })
                        .Add("-boatcategory", () =>
                        {
                            Console.WriteLine("write the Boat category");
                            inpS = Console.ReadLine();
                            q.BoatCategory = inpS;
                        })
                        .Add("-BACK", ConsoleMenu.Close);


                    update.Show();

                    service.Put<Category>(q, "category");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void UpdateCompetition()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Competition>(inp, "competition");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                {
                    string inpS;
                    int inpI;

                    ConsoleMenu update = new ConsoleMenu()
                        .Add("-competitorID", () =>
                        {
                            Console.WriteLine("write the competitorID");
                            inpI = int.Parse(Console.ReadLine());
                            q.CompetitorID = inpI;
                        })
                        .Add("-opponentID", () =>
                        {
                            Console.WriteLine("write the opponentID");
                            inpI = int.Parse(Console.ReadLine());
                            q.OpponentID = inpI;
                        })
                        .Add("-NumberOfRacesAgainstEachOther", () =>
                        {
                            Console.WriteLine("write the number of races against each other");
                            inpI = int.Parse(Console.ReadLine());
                            q.NumberOfRacesAgainstEachOther = inpI;
                        })
                        .Add("-distance", () =>
                        {
                            Console.WriteLine("write the Distance");
                            inpI = int.Parse(Console.ReadLine());
                            q.Distance = inpI;
                        })
                         .Add("-location", () =>
                         {
                             Console.WriteLine("write the Location");
                             inpS = Console.ReadLine();
                             q.Location = inpS;
                         })
                        .Add("-BACK", ConsoleMenu.Close);


                    update.Show();

                    service.Put<Competition>(q, "competition");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void OneCompetitor()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Competitor>(inp, "competitor");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    Console.WriteLine(q.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void OneCategory()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Category>(inp, "category");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    Console.WriteLine(q.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void OneCompetition()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Write an ID");
                int inp = int.Parse(Console.ReadLine());

                var q = service.Get<Competition>(inp, "competition");

                if (q == null)
                    Console.WriteLine("NO ONE WITH THIS ID");
                else
                    Console.WriteLine(q.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }
            private static void ListCompetitors()
            {
                Console.Clear();
                try
                {
                    var q = service.Get<Competitor>("competitor");

                    if (q == null || q.Count == 0)
                        Console.WriteLine("No data");
                    else
                    {
                        Console.WriteLine("ALL COMPETITOR:\n");
                        foreach (var item in q)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[FAIL] " + e.Message);
                }
                finally
                {
                    PressSMTHToContinue();
                };
            }

        private static void ListCategories()
        {
            Console.Clear();
            try
            {
                var q = service.Get<Category>("category");

                if (q == null || q.Count == 0)
                    Console.WriteLine("No data");
                else
                {
                    Console.WriteLine("ALL CATEGORIES:\n");
                    foreach (var item in q)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void ListCompetitions()
        {
            Console.Clear();
            try
            {
                var q = service.Get<Competition>("competition");

                if (q == null || q.Count == 0)
                    Console.WriteLine("No data");
                else
                {
                    Console.WriteLine("ALL MATCHES:\n");
                    foreach (var item in q)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[FAIL] " + e.Message);
            }
            finally
            {
                PressSMTHToContinue();
            };
        }

        private static void PressSMTHToContinue()
        {
            Console.WriteLine("\nPress something to continue");
            Console.ReadKey();
        }

    }
}

