using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Commands;

public record CreateProjectCommand(UserId userId, string title, EGarmentColor garmentColor, EGarmentGender garmentGender, EGarmentSize garmentSize);
