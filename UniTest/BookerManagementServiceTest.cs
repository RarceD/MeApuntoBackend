using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Configuration;

namespace UniTest
{
    public class BookerManagementServiceTest
    {
        //private static BookerManagementService GetBookerManagementServiceToTest()
        //{
        //    var clientRepository = new Mock<IClientRepository>();
        //    var urbaRepository = new Mock<IUrbaRepository>();
        //    var schedulerRepository = new Mock<ISchedulerRepository>();
        //    var configurationRepository = new Mock<IConfigurationRepository>();
        //    var courtRepository = new Mock<ICourtRepository>();
        //    var logger = new Mock<ILogger<BookerManagementService>>();
        //    var mailService = new Mock<IMailService>();

        //    var bookerManagement = new BookerManagementService(
        //        clientRepository.Object,
        //        urbaRepository.Object,
        //        schedulerRepository.Object,
        //        configurationRepository.Object,
        //        mailService.Object,
        //        courtRepository.Object,
        //        logger.Object);
        //    return bookerManagement;
        //}

        //[Fact]
        //public void MakeBookNotValid()
        //{

        //    var clientRepository = new Mock<IClientRepository>();
        //    var urbaRepository = new Mock<IUrbaRepository>();
        //    var schedulerRepository = new Mock<ISchedulerRepository>();
        //    var configurationRepository = new Mock<IConfigurationRepository>();
        //    var courtRepository = new Mock<ICourtRepository>();
        //    var logger = new Mock<ILogger<BookerManagementService>>();
        //    var mailService = new Mock<IMailService>();

        //    var bookerManagement = new BookerManagementService(
        //        clientRepository.Object,
        //        urbaRepository.Object,
        //        schedulerRepository.Object,
        //        configurationRepository.Object,
        //        mailService.Object,
        //        courtRepository.Object,
        //        logger.Object);

        //    // Preprare client who book:
        //    clientRepository.Setup(_ => _.GetById(It.IsAny<int>()))
        //        .Returns<ClientDb>(c => new() { username = string.Empty, urba_id = 0 });
        //    urbaRepository.Setup(_ => _.GetById(It.IsAny<int>()))
        //        .Returns<UrbaDb>(c => new() { advance_book = 2 });
        //    configurationRepository.Setup(_ => _.GetAllFromCourtId(It.IsAny<int>()))
        //        .Returns<List<ConfigurationDb>>(c => new() {
        //            new ConfigurationDb() {ValidHour = "12:00"},
        //            new ConfigurationDb() {},
        //        });

        //    var newBook = new BookerDto();
        //    bookerManagement.MakeABook(newBook);
        //}

        //[Fact]
        //public void MakeBookValid()
        //{
        //    BookerManagementService bookerManagement = GetBookerManagementServiceToTest();
        //    var newBook = new BookerDto();
        //    bookerManagement.MakeABook(newBook);
        //}
    }
}