namespace Q2.TeeLab.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    /// <summary>
    ///     Commit changes to the database
    /// </summary>
    Task CompleteAsync();
}