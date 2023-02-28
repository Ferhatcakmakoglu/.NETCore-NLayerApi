using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            //Id nin default 1er 1er artmasını saglar
            builder.Property(x => x.Id).UseIdentityColumn();

            //Name IsRequired ile zorunlu tutuldu, HasMaxLength ile max 50 karakter olması dendi
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            //Bu degeri vermesekte oto olarak AppDbContext deki table name i alır (aynı zaten)
            builder.ToTable("Categories");
        }
    }
}
