using Q2.TeeLab.DesignLab.Domain.Model.Entities;
using Q2.TeeLab.DesignLab.Domain.Model.Queries;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface ILayerQueryService
{
    Task<Layer?> Handle(GetLayerByIdQuery query);
}