﻿using Microsoft.EntityFrameworkCore;
using Poster.API.Models;

namespace Poster.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Retweet> Retweets { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;

    }
}