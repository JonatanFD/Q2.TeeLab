using Q2.TeeLab.DesignLab.Domain.Model.Queries;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface ILayerQueryService
{
    bool Handle(GetLayerByIdQuery query);
}