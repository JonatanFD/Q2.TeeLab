﻿using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Commands;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface IProjectCommandService
{
    Task<Project> Handle(CreateProjectCommand command);
    Task<LayerId> Handle(AddImageLayerToProjectCommand command);
    Task<LayerId> Handle(AddTextLayerToProjectCommand command);
    Task<bool> Handle(RemoveLayerFromProjectCommand command);
    Task<bool> Handle(SendLayerToBackInProjectCommand command);
    Task<bool> DeleteAsync(ProjectId projectId);
}