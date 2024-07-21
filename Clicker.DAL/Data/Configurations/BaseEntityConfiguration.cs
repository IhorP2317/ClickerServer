using Clicker.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Clicker.DAL.Data.Configurations;



public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity: BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();
    }
}

