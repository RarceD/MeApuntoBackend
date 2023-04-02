using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using System.Timers;

namespace MeApuntoBackend.Services;
public class BookerManagementService : IBookerManagementService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    public BookerManagementService(IClientRepository clientRepository, IUrbaRepository urbaRepository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
    }
}
