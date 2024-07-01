﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.Persistence.Configurations
{
	public class GenreConfiguration : IEntityTypeConfiguration<Genre>
	{
		public void Configure(EntityTypeBuilder<Genre> builder)
		{
			builder.Property(x => x.Name).HasMaxLength(100);
			builder.ToTable("Genre", "Musicales");
			builder.HasQueryFilter(x => x.Status);
		}
	}	
}
