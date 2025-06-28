using Q2.TeeLab.DesignLab.Domain.Model.Commands;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Transform;

public static class CreateProjectCommandFromResourceAssembler
{
    public static CreateProjectCommand ToCommand(CreateProjectResource resource)
    {
        return new CreateProjectCommand(
            new UserId(resource.UserId),
            resource.Title,
            Enum.Parse<EGarmentColor>(resource.GarmentColor, true),
            Enum.Parse<EGarmentGender>(resource.GarmentGender, true),
            Enum.Parse<EGarmentSize>(resource.GarmentSize, true)
        );
    }
}

