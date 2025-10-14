using Application.Clients;
using Application.Machines;
using Application.Tyres;
using AutoMapper;
using Domain;
//using Microsoft.AspNetCore.Http;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Tyre
        CreateMap<Tyre, Tyre>();
        CreateMap<Tyre, TyreDto>();

        // User
        CreateMap<User, User>();

        // Client
        CreateMap<Client, Client>();
        CreateMap<Client, ClientDto>();

        // Machine
        CreateMap<Machine, Machine>();
        CreateMap<Machine, MachineDto>();

        // Sale, Production, ActionLog
        CreateMap<Sale, Sale>();
        CreateMap<Production, Production>();
        CreateMap<ActionLog, ActionLog>();
    }
}