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
        CreateMap<Tyre, Tyre>();
        CreateMap<Tyre, TyreDto>();
        CreateMap<User, User>();
        CreateMap<BusinessUnitLeader, BusinessUnitLeader>();
        CreateMap<ProductionOperator, ProductionOperator>();
        CreateMap<QualitySupervisor, QualitySupervisor>();
        CreateMap<Client, Client>();
        CreateMap<Client, ClientDto>();
        CreateMap<Machine, Machine>();
        CreateMap<Machine, MachineDto>();
        CreateMap<Report, Report>();
        CreateMap<Sale, Sale>();
        CreateMap<Production, Production>();
        CreateMap<ActionLog, ActionLog>();
    
    }
}