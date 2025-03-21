using AutoMapper;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Domain.Entities;

namespace TigrinhoGame.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<CreatePlayerDto, Player>();
            CreateMap<UpdatePlayerDto, Player>();

            CreateMap<Spin, SpinResultDto>();

            CreateMap<WinningLine, WinningLineDto>();

            CreateMap<Spin, SpinHistoryDto>();
        }
    }
} 