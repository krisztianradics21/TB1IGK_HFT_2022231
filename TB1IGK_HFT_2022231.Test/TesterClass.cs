﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TB1IGK_HFT_2022231.Logic;
using TB1IGK_HFT_2022231.Models;
using TB1IGK_HFT_2022231.Repository.Interface;

namespace TB1IGK_HFT_2022231.Test
{
    [TestFixture]
    public class TesterClass
    {
        private Mock<IRepository<Competitor>> mockedCompetitorRepo;
        private Mock<IRepository<Models.Competition>> mockedCompetitionRepo;
        private Mock<IRepository<Category>> mockedCategoryRepo;
        private CompetitorLogic competitorLogic;
        private CompetitionLogic competitionLogic;
        private CategoryLogic categoryLogic;

        [SetUp]
        public void Init()
        {
            this.mockedCompetitorRepo = new Mock<IRepository<Competitor>>(MockBehavior.Loose);
            this.mockedCompetitionRepo = new Mock<IRepository<Competition>>(MockBehavior.Loose);
            this.mockedCategoryRepo = new Mock<IRepository<Category>>(MockBehavior.Loose);

            List<Competitor> competitors = new List<Competitor>()
            {
                new Competitor(1, "Tom", 22,1,1,"GER" ),
                new Competitor(2, "Joe", 21,1,1,"ESP" ),
                new Competitor(3, "Josef", 41,2,2,"HUN" ),
                new Competitor(4, "Adam", 32,2,2,"DEN" ),
            };

            List<Competition> competitions = new List<Competition>()
            {
                new Competition(1, 1, 2, 10, "Szentendre", 1000),
                new Competition(1, 2, 3, 14, "München", 500)
            };

            List<Category> categories = new List<Category>()
            {
                new Category(1, "U23", "Canoe"),
                new Category(2, "U18", "Kayak")
            };


            this.mockedCompetitionRepo.Setup(repo => repo.GetAll()).Returns(competitions.AsQueryable());
            this.mockedCompetitorRepo.Setup(repo => repo.GetAll()).Returns(competitors.AsQueryable());
            this.mockedCategoryRepo.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());

            this.competitorLogic = new CompetitorLogic(mockedCompetitorRepo.Object, mockedCompetitionRepo.Object, mockedCategoryRepo.Object);
            this.competitionLogic = new CompetitionLogic(mockedCompetitionRepo.Object, mockedCategoryRepo.Object, mockedCompetitorRepo.Object);
            this.categoryLogic = new CategoryLogic(mockedCategoryRepo.Object, mockedCompetitorRepo.Object);
        }
        [Test]
        public void CompetitorAddTest()
        {
            Competitor competitor = new Competitor(1, "Jacob", 23, 1, 1, "GER");
            competitorLogic.Create(competitor);
            mockedCompetitorRepo.Verify(x => x.Create(competitor), Times.Once);
        }

        [Test]
        public void CompetitorReadTest()
        {
            Competitor competitor = new Competitor(4, "James", 36, 2, 2, "ARG");
            competitorLogic.GetOne(competitor.ID);
            mockedCompetitorRepo.Verify(x => x.GetOne(competitor.ID), Times.Once);
        }

        [Test]
        public void CompetitorUpdateTest()
        {
            Competitor competitor = new Competitor(7, "Tibor", 29, 3, 3, "POL");
            competitorLogic.Update(competitor);
            mockedCompetitorRepo.Verify(x => x.Update(competitor), Times.Once);
        }
        [Test]
        public void CompetitorDeleteTest()
        {
            Competitor competitor = new Competitor(1, "Max", 30, 2, 2, "UKR");
            competitorLogic.Delete(1);
            mockedCompetitorRepo.Verify(x => x.Delete(1), Times.Once);
        }
        [Test]
        public void CompetitorGetAllTest()
        {
            var q = competitorLogic.GetAll();
            mockedCompetitorRepo.Verify(x => x.GetAll(), Times.Once);
        }
        [Test]
        public void CompetitorAvgAgeTest()
        {
            var q = competitorLogic.AVGAge();

            Assert.That(q, Is.EqualTo(29));
        }
        [Test]
        public void CompetitorWithAllRelevantDataTest()
        {
            var q = competitorLogic.CompetitorWithAllRelevantData().ToList();
            var exc = new
            {
                FighterName = "Test1",
                FighterAge = 22,
                FighterForm = "T1",
                MatchLocation = "TestLoc1",
                NumberOfMatches = 4,
                WeightClass = 60
            };

            Assert.That(q[0].ToString(), Is.EqualTo(exc.ToString()));
        }
    }
}
