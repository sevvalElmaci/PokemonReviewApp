using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

public class BaseRepository<T> where T : AuditEntityBase
{
    protected readonly DataContext _context;

    public BaseRepository(DataContext context)
    {
        _context = context;
    }

    public void SoftDelete(T entity)
    {
        entity.IsDeleted = true;
        entity.DeletedDateTime = DateTime.Now;
        _context.Update(entity);
    }

    public bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}
