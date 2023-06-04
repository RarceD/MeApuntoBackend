using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace UniTest
{
    public class BookerManagementServiceTest
    {
        private static BookerManagementService GetBookerManagementServiceToTest()
        {
            var clientRepository = new Mock<IClientRepository>();
            var urbaRepository = new Mock<IUrbaRepository>();
            var schedulerRepository = new Mock<ISchedulerRepository>();
            var configurationRepository = new Mock<IConfigurationRepository>();
            var courtRepository = new Mock<ICourtRepository>();
            var logger = new Mock<ILogger<BookerManagementService>>();
            var mailService = new Mock<IMailService>();

            var bookerManagement = new BookerManagementService(
                clientRepository.Object,
                urbaRepository.Object,
                schedulerRepository.Object,
                configurationRepository.Object,
                mailService.Object,
                courtRepository.Object,
                logger.Object);
            return bookerManagement;
        }

        [Fact]
        public void MakeBookNotValid()
        {
            BookerManagementService bookerManagement = GetBookerManagementServiceToTest();
            var newBook = new BookerDto();
            bookerManagement.MakeABook(newBook);
        }

        [Fact]
        public void MakeBookValid()
        {
            BookerManagementService bookerManagement = GetBookerManagementServiceToTest();
            var newBook = new BookerDto();
            bookerManagement.MakeABook(newBook);
        }
    }
}