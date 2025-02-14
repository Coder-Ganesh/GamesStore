
using GamesStore.api.EndPoints;
using GameStore.api.Data;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using GamesStore.api.Data;

var builder = WebApplication.CreateBuilder(args);

        var connString = builder.Configuration.GetConnectionString("GameStore");
        //builder.Services.AddSqlite<GameStoreContext>(connString);
        builder.Services.AddSqlite<GameStoreContext>(connString);

        var app = builder.Build();

        app.MapGamesEndPoints();

        app.MigrateDb();

        app.Run();
